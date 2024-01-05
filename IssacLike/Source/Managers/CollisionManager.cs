using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Managers.Events;
using ProjectMystic.Source.ZeldaLikeImGui;
using ProjectMystic.Source.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMystic.Source.Managers {
    public static class CollisionManager {
        public static List<ICollidable> Colliders = new List<ICollidable>();
        public static Dictionary<Tuple<string, string>, bool> CollidingPairs = new Dictionary<Tuple<string, string>, bool>();

        public static void CheckCollisions() {  

            var collidingPairs = new Dictionary<Tuple<string, string>, bool>();

            for (int i = 0; i < Colliders.Count; i++) {
                var colliderA = Colliders[i];
                for (int j = i + 1; j < Colliders.Count; j++) {
                    var colliderB = Colliders[j];
                    
                    if(colliderA == null || colliderB == null)
                        return;

                    if(colliderA.Entity == null || colliderB.Entity == null)
                        return;

                    if (colliderA.Bound.Intersects(colliderB.Bound)) {
                        colliderA.Entity.OnCollisionEvent(colliderB);
                        colliderB.Entity.OnCollisionEvent(colliderA);

                        var pair = new Tuple<string, string>(colliderA.Tag, colliderB.Tag);
                        collidingPairs[pair] = true;
                    }                    
                }
            }

            foreach(var pair in CollidingPairs.Keys) {
                if (!collidingPairs.ContainsKey(pair)) {
                    EntityManager.Find(pair.Item1).OnExitCollisionEvent();
                    EntityManager.Find(pair.Item2).OnExitCollisionEvent();
                }
            }

            CollidingPairs = collidingPairs;
        }
    }
}
