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
        public BitArray DataBlobs = new BitArray(2);

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

        public int DataBlobsIndex
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
            if (ownerID != Guid.Empty)  // only look if we have a vlaid owner:
            {
                foreach(var starSystem in GameState.StarSystems)
                {
                    if (starSystem.Id == ownerID)
                    {
                        Owner = starSystem;
                        break;
                    }
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", Id, typeof(Guid));
            info.AddValue("Name", Name, typeof(RuinsDB));

            if (Owner != null)
                info.AddValue("OwnerID", Owner.Id, typeof(Guid));
            else
                info.AddValue("OwnerID", Guid.Empty, typeof(Guid));
        }
    }




    class TestEntity : DataEntity
    {
        // so it will compile:
        public TestEntity() 
            : base()
        {
            // init components:
            DataBlobs.Set((int)DataBlobIndex.RuinsDB, true);


            DataBlobs.Set((int)DataBlobIndex.TemperatureDB, false);
        }

        public TestEntity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            try 
            { 
                Owner.GetDataBlobList<List<RuinsDB>>(DataBlobIndex.RuinsDB)[DataBlobsIndex] = (RuinsDB)info.GetValue("Ruins", typeof(RuinsDB));
            }
            catch (System.NullReferenceException e)
            {
                throw new System.NullReferenceException("Cannot load Data Blob, no valid star system, or some other value was null!");
            }
            catch(System.Runtime.Serialization.SerializationException e)
            {
                // one or more values faild to load, log it here.
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (Owner != null)
            {
                RuinsDB ruins = Owner.GetDataBlobList<List<RuinsDB>>(DataBlobIndex.RuinsDB)[DataBlobsIndex];
                if (ruins != null)
                    info.AddValue("Ruins", ruins, typeof(RuinsDB));
            }
        }
    }
}
