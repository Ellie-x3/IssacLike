﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssacLike.Source.Entities;
using IssacLike.Source.RogueLikeImGui;

namespace IssacLike.Source.Managers
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
    }
}
