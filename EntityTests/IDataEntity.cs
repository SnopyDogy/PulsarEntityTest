using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections;

namespace EntityTests
{
    class DataEntity : ISerializable
    {
        public BitArray Componments = new BitArray(2);

        public string Name
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public int ComponentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Basically which StarSystem this entity is curently in...
        /// </summary>
        public StarSystem Owner
        {
            get;
            set;
        }

        public DataEntity()
        {
            Id = Guid.NewGuid();
        }

        public DataEntity(SerializationInfo info, StreamingContext context)
        {
            Id = (Guid)info.GetValue("ID", typeof(Guid));
            Name = (string)info.GetValue("Name", typeof(string));

            // Use the Id of the star system to lookup our owner:
            Guid ownerID = (Guid)info.GetValue("OwnerID", typeof(Guid));
            foreach(var starSystem in GameState.StarSystems)
            {
                if (starSystem.Id == ownerID)
                {
                    Owner = starSystem;
                    break;
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", Id, typeof(Guid));
            info.AddValue("Name", Name, typeof(RuinsComponent));
            info.AddValue("OwnerID", Owner.Id, typeof(Guid));
        }
    }




    class TestEntity : DataEntity
    {
        //public RuinsComponent _ruins;
        //public RuinsComponent Ruins
        //{
         //   get { return _ruins; }
         //   set { _ruins = value; }
        //}

        // so it will compile:
        public TestEntity() 
            : base()
        {
            // init components:
            Componments.Set((int)ComponentFields.RuinsComponent, true);
            Componments.Set((int)ComponentFields.TemperatureComponent, false);
        }

        public TestEntity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            //_ruins = (RuinsComponent)info.GetValue("Ruins", typeof(RuinsComponent));
            Owner.Ruins[ComponentIndex] = (RuinsComponent)info.GetValue("Ruins", typeof(RuinsComponent));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Ruins", Owner.Ruins[ComponentIndex], typeof(RuinsComponent));
        }
    }
}
