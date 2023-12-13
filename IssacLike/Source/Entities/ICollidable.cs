using IssacLike.Source.Managers.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Entities {
    public interface ICollidable : IDisposable {

        string Tag { get; set; }
        Rectangle Bound { get; set; }
        Entity Entity { get; }

    }
}
