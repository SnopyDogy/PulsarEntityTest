using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace EntityTests
{
    enum ComponentFields : int
    {
        RuinsComponent = 0,
        TemperatureComponent = 1
    }


    interface IComponent : ISerializable
    {
        bool Active
        {
            get;
            set;
        }
    }


    struct RuinsComponent : IComponent
    {
        bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        private uint _number;
        public uint Number
        {
            get { return _number; }
            set { _number = value; }
        }

        // called on serialize:
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Number", Number);
        }

        public RuinsComponent(SerializationInfo info, StreamingContext context)
        {
            _active = true;
            _number = (uint)info.GetValue("Number", typeof(uint)); 
        }
    }

    struct TemperatureComponent : IComponent
    {
        bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        // called on serialize:
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Temp", Value);
        }

        public TemperatureComponent(SerializationInfo info, StreamingContext context)
        {
            _active = true;
            _value = (double)info.GetValue("Temp", typeof(double));
        }
    }
}
