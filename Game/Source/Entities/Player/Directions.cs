using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Entities.Player {
    internal static class Directions {
        internal static readonly Vector2 Zero  = Vector2.Zero;
        internal static readonly Vector2 North = new Vector2(0,-1);
        internal static readonly Vector2 West  = new Vector2(-1,0);
        internal static readonly Vector2 South = new Vector2(0,1);
        internal static readonly Vector2 East  = new Vector2(1,0);

        internal static Vector2 Normalize(Vector2 vector) {          
            float x = vector.X;
            float y = vector.Y;

            if(vector == Vector2.Zero)
                return vector;

            double sqrtroot = Math.Sqrt((x * x) + (y * y));     

            return new Vector2((float)(x / sqrtroot), (float)(y / sqrtroot));
        }
    }
}
