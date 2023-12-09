using IssacLike.Source.Managers;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Util
{
    internal static class Globals {
        internal static GraphicsDevice s_GraphicsDevice;
        internal static GraphicsDeviceManager s_Graphics;
        internal static SpriteBatch s_SpriteBatch;
        internal static float Delta;

        public static LDtkLevel CurrentLevel { get; set; }

        internal static Vector2 ScreenSize {
            get {
                return new Vector2(s_Graphics.PreferredBackBufferWidth, s_Graphics.PreferredBackBufferHeight);
            }

            set {
                s_Graphics.PreferredBackBufferWidth = (int)value.X;
                s_Graphics.PreferredBackBufferHeight = (int)value.Y;
            }
        }

        
    }
}
