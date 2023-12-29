using ProjectMystic.Source.Managers.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMystic.Source.Entities {
    public interface ICollidable : IDisposable {

        string Tag { get; set; }
        Rectangle Bound { get; set; }
        Entity Entity { get; }

    }
}
