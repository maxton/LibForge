using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibForge.RBSong
{
  public class RBSongReader : ReaderBase<RBSongResource>
  {
    public RBSongReader(Stream s) : base(s)
    {  }

    public override RBSongResource Read()
    {
      var ret = new RBSongResource();
      LoadResource(ret);
      return ret;
    }
    private void LoadResource(Resource r)
    {
      if(r is EntityResource er)
      {
        er.Version = Int();
        if (er.Version < 0xC || er.Version > EntityResource.MaxVersion)
        {
          throw new InvalidDataException("Can't handle EntityResource version " + er.Version);
        }
        er.InlineLayerNames = Arr(String);
        er.InlineResourceLayers = new Resource[er.InlineLayerNames.Length][];
        if(er.Version >= 0xF)
        {
          var unk0 = Int();
          if (er.Version >= 0x11)
          {
            var unk1 = Int();
          }

        }
        er.Entity = ReadEntity(er);
      }
    }
    private Entity ReadEntity(EntityResource rsrc)
    {
      var ent = new Entity();
      ent.Version = Int();
      if (ent.Version > Entity.MaxVersion)
      {
        throw new Exception("Can't handle entity version " + ent.Version);
      }
      if (ent.Version <= 1)
      {
        throw new InvalidDataException("Entity version must be > 1");
      }
      if (ent.Version <= 4)
      {
        String();
      }
      int numLayers = 1;
      if (ent.Version >= 9)
      {
        numLayers = Int();
      }
      else
      {
        int rootId = Int();
        if(ent.Version > 6)
        {
          numLayers = Int();
        }
        else
        {
          short arraySize = Short();
        }
      }
      for (int li = 0; li < numLayers; li++)
      {
        if(ent.Version < 8)
        {
          if (ent.Version >= 7)
            Short(); // layer_field_28
          if (numLayers != 1)
            throw new Exception("Num layers should be 1 for version <8");
          var propArrayBaseSize = Int();
          if(propArrayBaseSize > 0)
          {
            throw new NotImplementedException("Version < 8 should load objs here");
          }
        }
        else if (ent.Version <= 11)
        {
          String(); // layer_name
        }
      }
      if (ent.Version < 8)
      {
        throw new NotImplementedException("Version < 8 not implemented");
      }
      ent.Layers = new EntityLayer[numLayers];
      LoadEntityLayer(0, rsrc, ent);
      for(int i = 1; i < numLayers; i++)
      {
        LoadEntityLayer(i, rsrc, ent);
      }
      return ent;
    }
    private void LoadEntityLayer(int layerIndex, EntityResource rsrc, Entity ent)
    {
      ent.Layers[layerIndex] = ReadEntityLayer(layerIndex, ent.Version);
      if (ent.Version >= 0xC)
      {
        LoadInlineResources(layerIndex, rsrc);
      }
    }
    private void LoadInlineResources(int layerIndex, EntityResource rsrc)
    {
      if (layerIndex > rsrc.InlineResourceLayers.Length)
        throw new InvalidDataException("More inline resource layers than entity resource layers");
      int version = Int();
      int count = Int();
      var layer = rsrc.InlineResourceLayers[layerIndex] = new Resource[count];
      for (int i = 0; i < count; i++)
      {
        var type = String();
        var inline = layer[i] = Resource.Create(type);
        inline.Path = String();
        LoadResource(inline);
      }
    }
    private EntityLayer ReadEntityLayer(int index, int version)
    {
      var ent = new EntityLayer();
      ent.Version = Int();
      if (ent.Version < 8)
        throw new InvalidDataException("Entity layer version should be > 8");
      if (ent.Version >= 0x17)
      {
        Int(); // unknown
      }
      ent.fileSlotIndex = Int();
      if(ent.fileSlotIndex != index)
      {
        //throw new InvalidDataException("Entity layer failed to load (serialized to different slot)");
      }
      ent.TotalObjectLayers = Short();
      ent.Objects = new GameObject[Int()];
      for(int i = 0; i < ent.Objects.Length; i++)
      {
        ent.Objects[i] = ReadGameObject(ent.Version);
      }
      return ent;
    }
    private GameObject ReadGameObject(int layerVersion)
    {
      var id = Int();
      if (id == -1) return null;
      var obj = new GameObject();
      obj.Id = new GameObjectId
      {
        Index = id & 0xFFF,
        Layer = (short)(id >> 16), // Some places say this should be >> 12, but that doesn't make sense with any files
      };
      obj.Rev = Int();
      if(obj.Rev < 0)
      {
        obj.Name = new string((char)Byte(),1);
      }
      else
      {
        obj.Name = String();
      }

      if(obj.Rev >= 4 && obj.Name.Length == 0)
      {
        // TODO: Newer GameObject unknown stuff
        Int();
        Int();
        Int();
        Int();
        obj.Components = new Component[] { };
        return obj;
      }
      var numChildren = Int();
      obj.Components = new Component[numChildren];
      for(int i = 0; i < numChildren; i++)
      {
        obj.Components[i] = ReadComponent(obj.Rev, layerVersion);
      }
      return obj;
    }
    private Component ReadComponent(int objRev, int layerVersion)
    {
      var com = new Component();
      com.Name1 = String();
      if (objRev >= 2)
        com.Name2 = String();
      com.Rev = Int();
      com.Unknown2 = Long();
      if(layerVersion >= 0xE)
      {
        com.Props = new Property[Int()];
        for(int i = 0; i < com.Props.Length; i++)
        {
          com.Props[i] = ReadPropDef();
        }
        foreach(var prop in com.Props)
        {
          prop.Value = ReadValue(prop.Type);
        }
      }
      return com;
    }
    private Type ReadType()
    {
      var type = (DataType)Int();
      if (type.HasFlag(DataType.Array))
      {
        return new ArrayType
        {
          InternalType = type,
          ElementType = ReadType()
        };
      }
      else if(type.HasFlag(DataType.Struct))
      {
        return new StructType
        {
          InternalType = type,
          Refcount = Long(),
          Properties = Arr(ReadPropDef)
        };
      }
      switch (type)
      {
        case DataType.Float:
          return PrimitiveType.Float;
        case DataType.Int32:
          return PrimitiveType.Int;
        case DataType.Uint8:
          return PrimitiveType.Byte;
        case DataType.Uint32:
          return PrimitiveType.UInt;
        case DataType.Uint64:
          return PrimitiveType.Long;
        case DataType.Bool:
          return PrimitiveType.Bool;
        case DataType.Symbol:
          return PrimitiveType.Symbol;
        case DataType.ResourcePath:
          return PrimitiveType.ResourcePath;
        case DataType.DrivenProp:
          return PrimitiveType.DrivenProp;
        case DataType.GameObjectId:
          return PrimitiveType.GameObjectId;
        default:
          return new PrimitiveType(type);
      }
    }
    private Property ReadPropDef()
      => new Property
      {
        Name = String(),
        Type = ReadType()
      };
    private Value ReadValue(Type t)
    {
      switch (t)
      {
        case ArrayType at:
          return new ArrayValue(at, Arr(() => ReadValue(at.ElementType)));
        case StructType st:
          var props = new List<Property>();
          foreach(var p in st.Properties)
          {
            props.Add(new Property
            {
              Name = p.Name,
              Type = p.Type,
              Value = ReadValue(p.Type)
            });
          }
          return new StructValue(st, props.ToArray());
        case PrimitiveType pt:
          switch (pt.InternalType)
          {
            case DataType.Float:
              return new FloatValue(Float());
            case DataType.Int32:
              return new IntValue(Int());
            case DataType.Uint8:
              return new ByteValue(Byte());
            case DataType.Uint32:
              return new UIntValue(UInt());
            case DataType.Uint64:
              return new LongValue(Long());
            case DataType.Bool:
              return new BoolValue(Byte() != 0);
            case DataType.Symbol:
              return new SymbolValue(String());
            case DataType.ResourcePath:
              var prefix = Byte();
              return new ResourcePathValue(String(), prefix);
            case DataType.DrivenProp:
              int unk_driven_prop_1 = Int();
              int unk_driven_prop_2 = Int();
              if (unk_driven_prop_2 == 0)
              {
                return new DrivenProp
                {
                  Unknown1 = unk_driven_prop_1,
                  Unknown2 = unk_driven_prop_2,
                  ClassName = String(),
                  Unknown3 = Int(),
                  Unknown4 = Long(),
                  PropertyName = String()
                };
              }
              else
              {
                return new DrivenProp
                {
                  Unknown1 = unk_driven_prop_1,
                  Unknown2 = unk_driven_prop_2,
                  ClassName = null,
                  Unknown3 = Int(),
                  Unknown4 = Long(),
                  PropertyName = null
                };
              }
            case DataType.GameObjectId:
              return new GameObjectIdValue
              {
                Unknown1 = Int(),
                Unknown2 = Int(),
                Unknown3 = Int(),
                Unknown4 = Int(),
                Unknown5 = Int(),
                Unknown6 = Int(),
              };
            case DataType.Color:
              return new ColorValue
              {
                R = Float(),
                G = Float(),
                B = Float(),
                A = Float(),
                Unk1 = Int(),
                Unk2 = Int(),
                Unk3 = Int(),
                Unk4 = Int(),
              };
            default:
              throw new InvalidDataException("Unknown type");
          }
        default:
          throw new Exception("This switch is exhaustive but C# doesn't have sum types");
      }
    }
  }
}
