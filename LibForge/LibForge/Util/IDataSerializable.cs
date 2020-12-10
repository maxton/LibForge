using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using DtxCS;
using DtxCS.DataTypes;

namespace LibForge.Util
{
  public interface IDataSerializable
  {
  }
  public class AnonymousListAttribute : Attribute
  { }
  public static class IDataSerializableExtensions
  {
    
    public static DataArray Serialize(this IDataSerializable obj)
    {
      var array = new DataArray();
      foreach(var field in obj.GetType().GetProperties())
      {
        var name = CamelToSnake(field.Name);
        if (field.PropertyType.IsGenericType)
        {
          var gtd = field.PropertyType.GetGenericTypeDefinition();
          var args = field.PropertyType.GetGenericArguments();
          if (gtd == typeof(List<>))
          {
            // Add list elements
            if (!typeof(IDataSerializable).IsAssignableFrom(args[0]))
            {
              throw new NotImplementedException("Cannot handle non-serializable types in lists yet");
            }
            var listArray = new DataArray();
            var value = (IList)field.GetValue(obj, null);
            var typename = CamelToSnake(args[0].Name);
            var namedValues = field.GetCustomAttributes(typeof(AnonymousListAttribute), true).Length == 0;
            foreach (var f in value)
            {
              var valueNode = SerializeObject(f);
              listArray.AddNode(namedValues ? NamedItem(typename, valueNode) : valueNode);
            }
            array.AddNode(NamedItem(name, listArray));
          }
          else if (gtd == typeof(Dictionary<,>))
          {
            // Add dictionary elements
            if (args[0] != typeof(string))
            {
              throw new NotImplementedException("Cannot serialize dictionaries with non-string keys");
            }
            if (!typeof(IDataSerializable).IsAssignableFrom(args[1]))
            {
              throw new NotImplementedException("Cannot handle non-serializable types in dictionaries yet");
            }
            var dictArray = new DataArray();
            var value = (IDictionary)field.GetValue(obj, null);
            foreach (var k in value.Keys)
            {
              dictArray.AddNode(NamedItem((string)k, SerializeObject(value[k])));
            }
            array.AddNode(NamedItem(name, dictArray));
          }
          else if (gtd == typeof(Tuple<,>))
          {
            var tupleArray = new DataArray();
            tupleArray.AddNode(new DataAtom(name));
            var tuple = field.GetValue(obj, null) as ITuple;
            for(var i = 0; i < tuple.Length; i++)
            {
              tupleArray.AddNode(SerializeObject(tuple[i]));
            }
            array.AddNode(tupleArray);
          }
          else if (gtd == typeof(Nullable<>))
          {
            var value = field.GetValue(obj, null);
            if (value != null)
            {
              array.AddNode(NamedItem(name, SerializeObject(value)));
            }
          }
          else
          {
            throw new NotImplementedException("Generic type support for " + field.PropertyType.FullName + " not implemented");
          }
        }
        else
        {
          array.AddNode(NamedItem(name, SerializeObject(field.GetValue(obj, null))));
        }
      }
      return array;
    }
    private static DataNode SerializeObject(object value)
    {
      switch (value)
      {
        case IDataSerializable ids:
          return ids.Serialize();
        case int i:
          return new DataAtom(i);
        case float f:
          return new DataAtom(f);
        case string s:
          return new DataAtom(s);
        case Enum e:
          return new DataAtom(CamelToSnake(Enum.GetName(e.GetType(), value)));
        default:
          throw new NotImplementedException("Cannot serialize type " + value.GetType().Name);
      }
    }
    static bool IsNullable(this Type type)
    {
      return Nullable.GetUnderlyingType(type) != null;
    }
    public static T Deserialize<T>(this DataArray array) where T : IDataSerializable, new()
    {
      var obj = new T();
      var properties = obj.GetType().GetProperties();
      foreach (var field in properties)
      {
        var name = CamelToSnake(field.Name);
        var value = array.Array(name);
        if (value == null && !IsNullable(field.PropertyType))
        {
          throw new InvalidDataException("Data array is missing field: " + name);
        }
        if (field.PropertyType.IsGenericType)
        {
          var gtd = field.PropertyType.GetGenericTypeDefinition();
          var args = field.PropertyType.GetGenericArguments();
          if (gtd == typeof(List<>))
          {
            // Add list elements
            if (!typeof(IDataSerializable).IsAssignableFrom(args[0]))
            {
              throw new NotImplementedException("Cannot handle non-serializable types in lists yet");
            }
            object list = Activator.CreateInstance(field.PropertyType);
            field.SetValue(obj, list, null);
            MethodInfo method = typeof(IDataSerializableExtensions).GetMethod(nameof(Deserialize));
            MethodInfo generic = method.MakeGenericMethod(args[0]);
            foreach (var c in value.Children)
            {
              if (c is DataArray da)
              {
                ((IList)list).Add(generic.Invoke(null, new[] { da }));
              }
            }
          }
          else if (gtd == typeof(Dictionary<,>))
          {
            // Add dictionary elements
            if (args[0] != typeof(string))
            {
              throw new NotImplementedException("Cannot serialize dictionaries with non-string keys");
            }
            if (!typeof(IDataSerializable).IsAssignableFrom(args[1]))
            {
              throw new NotImplementedException("Cannot handle non-serializable types in dictionaries yet");
            }
            object list = Activator.CreateInstance(field.PropertyType);
            field.SetValue(obj, list, null);
            MethodInfo method = typeof(IDataSerializableExtensions).GetMethod(nameof(Deserialize));
            MethodInfo generic = method.MakeGenericMethod(args[1]);
            foreach (var c in value.Children)
            {
              if (c is DataArray da)
              {
                ((IDictionary)list).Add(da.Any(0), generic.Invoke(null, new[] { da }));
              }
            }
          }
          else if (gtd == typeof(Tuple<,>))
          {
            MethodInfo method = typeof(Tuple).GetMethods().First(x => x.Name == nameof(Tuple.Create) && x.GetParameters().Length == 2);
            MethodInfo generic = method.MakeGenericMethod(args[0], args[1]);
            field.SetValue(obj, generic.Invoke(null, new[] { DeserializeNode(value.Node(1)), DeserializeNode(value.Node(2)) }), null);
          }
          else if (gtd == typeof(Nullable<>))
          {
            if (value != null)
            {
              field.SetValue(obj, DeserializeNode(value.Node(1)), null);
            }
          }
          else
          {
            throw new NotImplementedException($"Generic type deserialization for {field.PropertyType.FullName} not yet implemented");
          }
        }
        else
        {
          if (typeof(IDataSerializable).IsAssignableFrom(field.PropertyType))
          {
            MethodInfo method = typeof(IDataSerializableExtensions).GetMethod(nameof(Deserialize));
            MethodInfo generic = method.MakeGenericMethod(field.PropertyType);
            field.SetValue(obj, generic.Invoke(null, new[] { value }), null);
          }
          else if (field.PropertyType.IsEnum)
          {
            var enumName = SnakeToCamel(value.String(1));
            field.SetValue(obj, Enum.Parse(field.PropertyType, enumName, true), null);
          }
          else
          {
            field.SetValue(obj, DeserializeNode(value.Node(1)), null);
          }
        }
      }
      return obj;
    }
    private static object DeserializeNode(DataNode n)
    {
      if (n is DataAtom d)
      {
        switch (d.Type)
        {
          case DataType.INT:
            return d.Int;
          case DataType.FLOAT:
            return d.Float;
          case DataType.STRING:
            return d.String;
        }
      }
      return null;
    }
    private static DataArray NamedArray(string name, DataArray siblings)
    {
      var array = new DataArray();
      array.AddNode(DataSymbol.Symbol(name));
      foreach (var n in siblings.Children)
      {
        array.AddNode(n);
      }
      return array;
    }
    private static DataArray NamedItem(string name, DataNode value)
    {
      if (value is DataArray da)
      {
        return NamedArray(name, da);
      }
      var array = new DataArray();
      array.AddNode(DataSymbol.Symbol(name));
      array.AddNode(value);
      return array;
    }
    private static string CamelToSnake(string s)
    {
      var sb = new StringBuilder();
      bool start = true;
      foreach (char c in s)
      {
        if (c >= 'A' && c <= 'Z')
        {
          if (start)
          {
            start = false;
          }
          else
          {
            sb.Append('_');
          }
          sb.Append(char.ToLowerInvariant(c));
        }
        else
        {
          sb.Append(c);
        }
      }
      return sb.ToString();
    }
    private static string SnakeToCamel(string s)
    {
      var sb = new StringBuilder();
      bool start = true;
      foreach (char c in s)
      {
        if (c == '_')
        {
          start = true;
          continue;
        }
        if (start)
        {
          sb.Append(char.ToUpperInvariant(c));
          start = false;
        }
        else
        {
          sb.Append(c);
        }
      }
      return sb.ToString();
    }
  }
}
