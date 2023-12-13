using IssacLike.Source.Entities;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Managers {
    public class CollisionManager {
        public static List<ICollisionDetection> Colliders = new List<ICollisionDetection>();

        public static void CheckCollisions() {  
            System.Diagnostics.Debug.WriteLine(Colliders.Count);
        }

    }
}
