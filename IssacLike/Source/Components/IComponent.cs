using IssacLike.Source.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Components {
    public interface IComponent {
        string name { get; set; }
        Entity Owner { get; set; }

        void Initialize();
        void Update(GameTime gameTime);
    }
}
