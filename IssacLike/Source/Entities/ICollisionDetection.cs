using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Entities {
    public interface ICollisionDetection {
        bool CollidesWith(Rectangle other);
    }
}
