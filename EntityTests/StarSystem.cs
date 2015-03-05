using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.Design;
using System.Collections;

namespace EntityTests
{
    class StarSystem : ISerializable
    {
        // This list stores one list for each type of data blob:
        private ArrayList _dataBlobLists = new ArrayList()
        {
            { new Dictionary<DataEntity, RuinsDB>() },
            { new Dictionary<DataEntity, TemperatureDB>() }
        };

        /// <summary>
        /// This function will Return a List containing all datablob in this system of the type provided.
        /// </summary>
        /// <typeparam name="T">DataBlobType</typeparam>
        /// <param name="dataBlobIndex">The index of the Datablob (the datablobs index specifies its type).</param>
        /// <returns>A list of DataBlobs</returns>
        public T GetDataBlobDict<T>(int dataBlobIndex)
        {
            return (T)_dataBlobLists[dataBlobIndex];
        }

        // Same as GetDataBlobList<T>() but it returns a IList interface. used when iterating over the _dataBlobLists
        // so we dont have to cast to IDictionary all the time.
        private IDictionary GetDataBlobIDictionary(int dataBlobIndex)
        {
            return (IDictionary)_dataBlobLists[dataBlobIndex];
        }

        // master list of entites
        public List<DataEntity> AllEntities = new List<DataEntity>();      

        public Guid Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public StarSystem()
            : base()
        {
            Name = "";
            Id = Guid.NewGuid();
        }

        public StarSystem(SerializationInfo info, StreamingContext context)
        {
            Id = (Guid)info.GetValue("ID", typeof(Guid));
            Name = (string)info.GetValue("Name", typeof(string));

            // when we are de-serialized in we want to add ourselves to the game state.
            GameState.StarSystems.Add(this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", Id, typeof(Guid));
            info.AddValue("Name", Name, typeof(RuinsDB));
        }

        public void AddNewEntity(DataEntity entity)
        {
            if (entity == null)
                throw new System.NullReferenceException("Cannot add new null entity to a Star System.");

            AllEntities.Add(entity);
            entity.Owner = this;
        }

        public void MoveEntity(DataEntity entity)
        {
            entity.Owner.RemoveEntity(entity);
            AllEntities.Add(entity);
            entity.Owner = this;
        }

        public void RemoveEntity(DataEntity entity)
        {
            AllEntities.Remove(entity);
        }
    }
}
