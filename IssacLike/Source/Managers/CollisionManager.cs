using IssacLike.Source.Entities;
using IssacLike.Source.Managers.Events;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Managers {
    public static class CollisionManager {
        public static List<ICollidable> Colliders = new List<ICollidable>();

        public static void CheckCollisions() {  
            for(int i = 0; i < Colliders.Count; i++) {
                var colliderA = Colliders[i];
                for (int j = i + 1; j < Colliders.Count; j++) {
                    var colliderB = Colliders[j];
                    
                    if((colliderA == null || colliderB == null))
                        return;

                    if(colliderA.Entity == null || colliderB.Entity == null)
                        return;

                    if (colliderA.Bound.Intersects(colliderB.Bound)) {
                        colliderA.Entity.OnCollisionEvent(colliderB);
                        colliderB.Entity.OnCollisionEvent(colliderA);
                    }
                }
            }
        }
    }
}
