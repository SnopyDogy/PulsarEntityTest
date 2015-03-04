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
            { new List<RuinsDB>() },
            { new List<TemperatureDB>() }
        };

        /// <summary>
        /// This function will Return a List containing all datablob in this system of the type provided.
        /// </summary>
        /// <typeparam name="T">DataBlobType</typeparam>
        /// <param name="dataBlobIndex">The index of the Datablob (the datablobs index specifies its type).</param>
        /// <returns>A list of DataBlobs</returns>
        public T GetDataBlobList<T>(int dataBlobIndex)
        {
            return (T)_dataBlobLists[dataBlobIndex];
        }

        // Same as GetDataBlobList<T>() but it returns a IList interface. used when iterating over the _dataBlobLists
        // so we dont have to cast to IList all the time.
        private IList GetDataBlobIList(int dataBlobIndex)
        {
            return (IList)_dataBlobLists[dataBlobIndex];
        }

        // master list of entites, mainly used to controll assignment of Ids.
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

            // first lets find an index for it:
            int newIndex = FindFreeIndex();
            if (newIndex < 0)
            {
                // no free space, allocate new:
                AllEntities.Add(entity);
                entity.DataBlobsIndex = AllEntities.Count - 1;

                // we need to allocate space in the datablobs lists for the componetents:
                for (int i = 0; i < _dataBlobLists.Count; ++i)
                    GetDataBlobIList(i).Add(null);              // set to null as we don't know which to init.
            }
            else
            {
                AllEntities[newIndex] = entity;
                entity.DataBlobsIndex = newIndex;
            }

            entity.Owner = this;
        }

        public void MoveEntity(DataEntity entity)
        {
            // first lets find an index for it:
            int newIndex = FindFreeIndex();
            if (newIndex < 0)  // if no free index then we will need to add it to then end of the list.
            {
                for (int i = 0; i < _dataBlobLists.Count; ++i)
                {
                    GetDataBlobIList(i).Add(entity.Owner.GetDataBlobIList(i)[entity.DataBlobsIndex]);
                }

                entity.DataBlobsIndex = AllEntities.Count - 1;
            }
            else
            {
                for (int i = 0; i < _dataBlobLists.Count; ++i)
                {
                    GetDataBlobIList(i)[newIndex] = entity.Owner.GetDataBlobIList(i)[entity.DataBlobsIndex];
                    entity.Owner.GetDataBlobIList(i)[entity.DataBlobsIndex] = null; // signal unused
                }

                entity.DataBlobsIndex = newIndex;
            }

            entity.Owner.RemoveEntity(entity);
            entity.Owner = this;
        }

        public void RemoveEntity(DataEntity entity)
        {
            int index = AllEntities.IndexOf(entity);
            AllEntities[index] = null;      // this has the effect of removing the item but leaving its place to be resuesd.
        }

        private int FindFreeIndex()
        {
            for (int i = 0; i < AllEntities.Count; ++i)
            {
                if (AllEntities[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
