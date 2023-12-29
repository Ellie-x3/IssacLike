using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.ZeldaLikeImGui;
using ProjectMystic.Source.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ProjectMystic.Source.Components {
    public class Collider : IDraw, ICollidable {

        public string Tag { get; set; } = "default";
        public string name { get; set; }
        public Entity Owner { get; set; }
        public Entity Entity { get => Owner; }

        private Rectangle m_Bound;
        public Rectangle Bound {
            get => m_Bound;
            set {
                if (!ColliderActive) { Logger.Log("Collider is not allowed on this Entity, Use ColliderActive = true to activate it"); return; }
                m_Bound = value;
            } 
        }
        
        public Point Location { get => m_Bound.Location; set => m_Bound.Location = value; }
        public bool ColliderActive { get; set; } = true;
        public Color Color { get => m_Color; set { 
                m_Debug.SetData(new Color[] { value });
                m_Color = value;
            }
        } 

        private bool m_CanCollide = true;
        public bool CanCollide { get => m_CanCollide; set {
                if(value) {
                    CollisionManager.Colliders.Add(this);
                    m_CanCollide = true;
                } else m_CanCollide = false;                    
            } 
        }

        private readonly Texture2D m_Debug;

        private Color m_Color;

        public Collider(Rectangle bound) {
            m_Bound = bound;
            m_Debug = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_Debug.SetData(new Color[] { Color });

            TextureLoader.AddTexture("Raycast", "line");
        }

        public void Initialize() { throw new NotImplementedException(); }

        public void Update(GameTime gameTime) { 

        }

        public void Draw(SpriteBatch batch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layer) {
            if(Debug.DrawDebug && ColliderActive) {
                batch.Draw(m_Debug, m_Bound, Color.White);
            }                
        }

        /*public void Raycast(Point start, Point end, out bool hit, Rectangle collision) {
            var length = 0.0f;
            var startX = start.X;
            var startY = start.Y;

            var endX = end.X;
            var endY = end.Y;

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
            var length = 0.0f;
            var startX = start.X;
            var startY = start.Y;

            var endX = end.X;
            var endY = end.Y;

            //Magnitude between vectors
            // d = Sqrt((X2 - X1)^2 + (Y2 - Y1)^2)
            length = (float)Math.Sqrt(((endX - startX) * (endX - startX)) + ((endY - startY) * (endY - startY)));
            m_width = (int)length;

            //radians = (float)Math.Atan2((endY - startY), (endX - startX));
            //radians = MathHelper.ToRadians(0f);
            
            m_RaycastRect = new Rectangle(startX, startY, m_width, 1);

            hit = false;
        }
        
        public Point Raycast(Point start, Point end) {
            var point = new Point();
            
            var startX = start.X;
            var startY = start.Y;

            var endX = end.X;
            var endY = end.Y;

            var height = 1;
            var width = 1;

            if (endY - startY > 0 || endY - startY < 0) {
                height = endY - startY;
            }
            else {
                width = endX - startX;
            }
       
            radians = MathHelper.ToRadians(0f);
            m_RaycastRect = new Rectangle(startX, startY, -70, 1);
            return point;
        }*/

        public void Dispose() { }

    }
}
