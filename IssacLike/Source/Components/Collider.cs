using IssacLike.Source.Entities;
using IssacLike.Source.Managers.Resources;
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
    public class Collider : IDraw {

        public string name { get; set; }
        public Entity Owner { get; set; }

        private Rectangle m_Bound;
        public Rectangle Bound { get => m_Bound; set => m_Bound = value; }
        public Point Location { get => m_Bound.Location; set => m_Bound.Location = value; }

        private Texture2D m_Debug;

        private int m_width;
        private float radians;
        private Rectangle m_RaycastRect;

        public Collider(Rectangle bound) {
            m_Bound = bound;
            m_Debug = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_Debug.SetData(new Color[] { new Color(255, 0, 0, 50) });

            TextureLoader.AddTexture("Raycast", "line");
        }

        public void Initialize() { throw new NotImplementedException(); }

        public void Update(GameTime gameTime) { 

        }

        public void Draw(SpriteBatch batch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layer) {
            if(Debug.DrawDebug) {
                batch.Draw(m_Debug, m_Bound, Color.White);
               //batch.Draw(TextureLoader.Texture("Raycast"), m_RaycastRect, null, Color.White, radians, new Vector2(0,0), effects, layer);
            }
                
        }

        public void Raycast(Point start, Point end, out bool hit, Rectangle collision) {
            float length = 0.0f;
            int startX = start.X;
            int startY = start.Y;

            int endX = end.X;
            int endY = end.Y;

            //Magnitude between vectors
            // d = Sqrt((X2 - X1)^2 + (Y2 - Y1)^2)
            length = (float)Math.Sqrt(((endX - startX) * (endX - startX)) + ((endY - startY) * (endY - startY)));
            m_width = (int)length;

            radians = (float)Math.Atan2((endY - startY), (endX  - startX));
            radians = MathHelper.ToRadians(0f);

            m_RaycastRect = new Rectangle(startX, startY, 1, m_width);

            hit = m_RaycastRect.Intersects(collision);
        }

        public void Raycast(Point start, Point end, out bool hit) {
            float length = 0.0f;
            int startX = start.X;
            int startY = start.Y;

            int endX = end.X;
            int endY = end.Y;

            //Magnitude between vectors
            // d = Sqrt((X2 - X1)^2 + (Y2 - Y1)^2)
            length = (float)Math.Sqrt(((endX - startX) * (endX - startX)) + ((endY - startY) * (endY - startY)));
            m_width = (int)length;

            radians = (float)Math.Atan2((endY - startY), (endX - startX));
            radians = MathHelper.ToRadians(0f);

            m_RaycastRect = new Rectangle(startX, startY, 1, m_width);

            hit = false;
        }
    }
}
