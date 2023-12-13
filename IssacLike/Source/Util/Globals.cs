using IssacLike.Source.Managers;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Util
{
    public static class Globals {
        public static GraphicsDevice s_GraphicsDevice;
        public static GraphicsDeviceManager s_Graphics;
        public static SpriteBatch s_SpriteBatch;      
        public static float Delta;

        public static SpriteFont font;
        public static float fps;

        public static LDtkLevel CurrentLevel { get; set; }

        //Debug
        public static Vector2 RoomSize { get => new Vector2(640, 360); }

        public static Vector2 ScreenSize {
            get {
                return new Vector2(s_Graphics.PreferredBackBufferWidth, s_Graphics.PreferredBackBufferHeight);
            }

            set {
                s_Graphics.PreferredBackBufferWidth = (int)value.X;
                s_Graphics.PreferredBackBufferHeight = (int)value.Y;
            }
        }

        public static Camera Camera { get; set; }

        public static int GetHash(string input) {
            using (SHA256 sha = SHA256.Create()) {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                int seed = BitConverter.ToInt32(hashBytes, 0);

                if(seed < 0) {
                    seed *= -1;
                }
                return seed;
            }
        }
        
    }
}
