using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace EntityTests
{
    static class DataBlobIndex
    {
        public const int RuinsDB = 0;
        public const int TemperatureDB = 1;
    }


    class DataBlob : ISerializable
    {
        public bool Active
        {
            get;
            set;
        }

        public DataBlob()
        {

        }

        public DataBlob(SerializationInfo info, StreamingContext context)
        {
            Active = (bool)info.GetValue("Active", typeof(bool));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Active", Active);
        }
    }


    class RuinsDB : DataBlob
    {
        private uint _number;
        public uint Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public RuinsDB()
            : base()
        {
            Active = true;
        }

        public RuinsDB(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _number = (uint)info.GetValue("Number", typeof(uint));
        }

        // called on serialize:
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Number", Number);
        }
    }

    class TemperatureDB : DataBlob
    {
        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public TemperatureDB()
            : base()
        {
            Active = true;
        }

        public TemperatureDB(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _value = (double)info.GetValue("Temp", typeof(double));
        }

        // called on serialize:
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Temp", Value);
        }
    }
}
