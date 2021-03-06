/**
 * Autogenerated by Thrift
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;
namespace Apache.Cassandra
{

  [Serializable]
  public partial class IndexClause : TBase
  {
    private List<IndexExpression> _expressions;
    private byte[] _start_key;
    private int _count;

    public List<IndexExpression> Expressions
    {
      get
      {
        return _expressions;
      }
      set
      {
        __isset.expressions = true;
        this._expressions = value;
      }
    }

    public byte[] Start_key
    {
      get
      {
        return _start_key;
      }
      set
      {
        __isset.start_key = true;
        this._start_key = value;
      }
    }

    public int Count
    {
      get
      {
        return _count;
      }
      set
      {
        __isset.count = true;
        this._count = value;
      }
    }


    public Isset __isset;
    [Serializable]
    public struct Isset {
      public bool expressions;
      public bool start_key;
      public bool count;
    }

    public IndexClause() {
      this._count = 100;
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.List) {
              {
                Expressions = new List<IndexExpression>();
                TList _list8 = iprot.ReadListBegin();
                for( int _i9 = 0; _i9 < _list8.Count; ++_i9)
                {
                  IndexExpression _elem10 = new IndexExpression();
                  _elem10 = new IndexExpression();
                  _elem10.Read(iprot);
                  Expressions.Add(_elem10);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Start_key = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Count = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("IndexClause");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Expressions != null && __isset.expressions) {
        field.Name = "expressions";
        field.Type = TType.List;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, Expressions.Count));
          foreach (IndexExpression _iter11 in Expressions)
          {
            _iter11.Write(oprot);
            oprot.WriteListEnd();
          }
        }
        oprot.WriteFieldEnd();
      }
      if (Start_key != null && __isset.start_key) {
        field.Name = "start_key";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Start_key);
        oprot.WriteFieldEnd();
      }
      if (__isset.count) {
        field.Name = "count";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Count);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("IndexClause(");
      sb.Append("Expressions: ");
      sb.Append(Expressions);
      sb.Append(",Start_key: ");
      sb.Append(Start_key);
      sb.Append(",Count: ");
      sb.Append(Count);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
