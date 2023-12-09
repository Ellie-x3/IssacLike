using IssacLike.Source.Entities;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Components {
    internal class Collider : IDraw {

        public string name { get; set; }
        public Entity Owner { get; set; }

        private Rectangle m_Bound;
        public Rectangle Bound { get => m_Bound; set => m_Bound = value; }
        public Point Location { get => m_Bound.Location; set => m_Bound.Location = value; }

        private Texture2D m_Debug;

        private Vector2 m_Position;

        public Collider(Rectangle bound) {
            m_Bound = bound;
            m_Debug = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_Debug.SetData(new Color[] { new Color(255, 0, 0, 50) });
        }

        public void Initialize() { throw new NotImplementedException(); }

        public void Update(GameTime gameTime) { 

        }

        public void Draw(SpriteBatch batch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layer) {
            batch.Draw(m_Debug, m_Bound, Color.White);
        }
    }
}
