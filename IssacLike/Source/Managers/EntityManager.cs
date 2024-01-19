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
using ProjectMystic.Source.Managers.Resources;
using System.Collections;

namespace ProjectMystic.Source.Managers
{
    public static class EntityManager
    {
        private static List<Entity> Entities = new List<Entity>();
        private static List<Entity> EntitiesToAdd = new List<Entity>();
        private static List<Entity> EntitiesToRemove = new List<Entity>();

        public static void Add(Entity entity) {
            EntitiesToAdd.Add(entity);
            entity.Start();           
        }

        public static void Remove(Entity entity) {
            if (Entities.Contains(entity)) {
                EntitiesToRemove.Add(entity);
            }
        }

        public static void Initialize() {
            foreach (Entity ent in EntitiesToAdd) {
                Entities.Add(ent);
            }

            EntitiesToAdd.Clear();

            foreach (Entity ent in EntitiesToRemove) {
                Entities.Remove(ent);
            }

            EntitiesToRemove.Clear();
        }

        public static void Update(GameTime gameTime) {       
            foreach (Entity ent in Entities) {
                ent.Update(gameTime);
            }

            foreach (Entity ent in EntitiesToAdd) {
                Entities.Add(ent);
            }

            EntitiesToAdd.Clear();

            foreach (Entity ent in EntitiesToRemove) {
                Entities.Remove(ent);
            }

            EntitiesToRemove.Clear();
        }
     
        public static void Draw(SpriteBatch batch, GameTime gameTime) {
            for(int i = Entities.Count - 1; i >= 0; i--) {
                Entities[i].Draw(batch, gameTime);
            }
        }

        public static Entity? Find(string name) {
            return Entities.Find(x => x.Name.Equals(name));
        }

        public static bool Exists(Entity entity) {
            if (!Entities.Contains(entity)) {
                return false;
            }

            return true;
        }

        public static void SpawnEntitiesInLevel<T>(LDtkLevel level) where T : ILDtkEntity, new() {
            foreach (T entity in level.GetEntities<T>()) {
                Entity child = CreateGameEntityFactory(entity);
                Add(child);
                LevelLoader.LDtkLevelEntities.Add(child);
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
