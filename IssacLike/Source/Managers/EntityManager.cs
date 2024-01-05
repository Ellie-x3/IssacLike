using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.ZeldaLikeImGui;

namespace ProjectMystic.Source.Managers
{
    public static class EntityManager
    {
        private static List<Entity> Entities = new List<Entity>();

        private static List<Entity> EntityQueue = new List<Entity>();

        public static void Add(Entity entity) {
            EntityQueue.Add(entity);
            entity.Start();
        }

        public static void Remove(Entity entity) {
            if (Entities.Contains(entity)) {
                Entities.Remove(entity);
            }
        }

        public static void Update(GameTime gameTime) {
            foreach(Entity queued in EntityQueue) {
                Entities.Add(queued);            
            }            

            foreach (Entity ent in Entities) {
                if(EntityQueue.Contains(ent))
                    EntityQueue.Remove(ent);

                ent.Update(gameTime);
            }
        }
     
        public static void Draw(SpriteBatch batch, GameTime gameTime) {
            foreach (Entity ent in Entities) {
                ent.Draw(batch, gameTime);
            }
        }

        public static Entity Find(string name) {
            var ent = Entities.Find(x => x.Name.Equals(name));

            if(ent == null)
                ent = EntityQueue.Find(x => x.Name.Equals(name));

            if(ent == null)
                throw new NullReferenceException($"No entity with the name: {name}");

            return ent;
        }
    }
}
