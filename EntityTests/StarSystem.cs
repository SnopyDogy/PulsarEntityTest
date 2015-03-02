using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace EntityTests
{
    class StarSystem : DataEntity
    {
        public List<DataEntity> AllEntities = new List<DataEntity>();      // master list of entites, mainly used to controll assignment of Ids.
        public List<RuinsComponent> Ruins = new List<RuinsComponent>();
        public List<TemperatureComponent> Temperatures = new List<TemperatureComponent>();

        public StarSystem()
            : base()
        {

        }

        public StarSystem(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // when we are serialized in we want to add ourselves to the game state.
            GameState.StarSystems.Add(this);
        }

        public void AddNewEntity(DataEntity entity)
        {
            // first lets find an index for it:
            int newIndex = FindFreeIndex();
            if (newIndex < 0)
            {
                // no free space, allocate new:
                AllEntities.Add(entity);
                entity.ComponentIndex = AllEntities.Count - 1;

                // allocate new components:
                Ruins.Add(new RuinsComponent());
                Temperatures.Add(new TemperatureComponent());
            }
            else
            {
                entity.ComponentIndex = newIndex;
                AllEntities[newIndex] = entity;
            }
        }

        public void MoveEntity(DataEntity entity)
        {
            // first lets find an index for it:
            int newIndex = FindFreeIndex();
            if (newIndex < 0)  // if no free index then we will need to add it to then end of the list.
            {
                // there is no free space, add to the end:
                Ruins.Add(entity.Owner.Ruins[entity.ComponentIndex]);
                Temperatures.Add(entity.Owner.Temperatures[entity.ComponentIndex]);
                //... and so on for all component types???

                entity.ComponentIndex = AllEntities.Count - 1;
            }
            else
            {
                // add to specified index:
                Ruins[newIndex] = entity.Owner.Ruins[entity.ComponentIndex];
                Temperatures[newIndex] = entity.Owner.Temperatures[entity.ComponentIndex];

                entity.ComponentIndex = newIndex;
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
