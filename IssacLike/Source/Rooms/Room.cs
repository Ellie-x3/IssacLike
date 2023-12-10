using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Rooms {
    internal class Room { //Create and defines a room
        //Size
        //Enemies
        //Difficuly
        //Boss room
        //Item Drop Room
        //Key Room
        //Secret door
        //Door Side
        //Background color (debug purposes)
        //Room Collission
        
        private Vector2 m_Size;
        //private EnemySpawner m_Spawner;
        private int m_Difficulty;
        private bool m_BossRoom = false;
        private bool m_ItemSelectRoom = false;
        private bool m_HasKeyDrop = false;
        private Color m_BackgroundColor;
        private Texture2D m_BackgroundTexture;

        private Point m_Position;

        internal Room(Point pos, Vector2 size) {
            m_Size = size;
            m_Position = pos;

            DateTime time = DateTime.Now; //debug purposes

            string hash = $"{time.Second} + {time.Millisecond} + some more time stuff {time.Minute * 60} + {m_Position.X} + {m_Position.Y} + {m_Position}";

            int seed = Globals.GetHash(hash);

            FastRandom ran = new FastRandom(seed);

            float r = ran.Next(0, 255);
            float g = ran.Next(0, 255);
            float b = ran.Next(0, 255);

            m_BackgroundColor = new Color(r / 255, g / 255, b / 255, 1.0f);

            m_BackgroundTexture = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_BackgroundTexture.SetData(new Color[] { m_BackgroundColor });
        }

        internal void Update(GameTime gameTime) {

        }

        internal void Draw(SpriteBatch batch, GameTime gameTime) {
            batch.Draw(m_BackgroundTexture, new Rectangle(m_Position.X, m_Position.Y, (int)m_Size.X, (int)m_Size.Y), Color.White);
        }
    }
}
