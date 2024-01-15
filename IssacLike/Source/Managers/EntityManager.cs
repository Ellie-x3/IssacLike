using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.ZeldaLikeImGui;
using LDtk;
using LDtkTypes.TestHome;
using ProjectMystic.Source.Entities.Player;
using ZeldaLike.Source.Entities.Items;

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

            if (EntityQueue.Contains(entity)) {
                EntityQueue.Remove(entity);
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

            for(int i = Entities.Count - 1; i >= 0; i--) {
                Entities[i].Draw(batch, gameTime);
            }
        }

        public static Entity? Find(string name) {
            var ent = Entities.Find(x => x.Name.Equals(name));

            if(ent == null)
                ent = EntityQueue.Find(x => x.Name.Equals(name));

            return ent;
        }

        public static bool Exists(Entity entity) {
            if (!Entities.Contains(entity) && !EntityQueue.Contains(entity)) {
                return false;
            }

            return true;
        }

        public static void SpawnEntitiesInLevel<T>(LDtkLevel level) where T : ILDtkEntity, new() {
            foreach (T entity in level.GetEntities<T>()) {
                Entity child = CreateGameEntityFactory(entity);
                Add(child);
            }
        }

        private static Entity CreateGameEntityFactory(ILDtkEntity entity) {
            switch (entity) {
                case PlayerEnt playerEntity:
                    return new APlayer(playerEntity);
                case DoorEnt doorEntity:
                    var size = doorEntity.Size;
                    var Position = doorEntity.Position;
                    Door door = new Door(new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y), doorEntity);

                    return door;
                case Pickup swordData:
                    Sword sword = new Sword(swordData);
                    return sword;

                default:
                    throw new ArgumentException("Unsupported entity type: " + entity.GetType());
            }
        }
    }
}
