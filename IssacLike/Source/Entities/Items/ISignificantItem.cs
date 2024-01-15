using ProjectMystic.Source.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaLike.Source.Entities.Items {
    public interface ISignificantItem {
        string ItemName { get; }
        Entity Item { get; }
        Texture2D Image { get; }

        bool HasBeenCollected { get => false; }
        Rectangle CollectionArea { get; }
    }
}
