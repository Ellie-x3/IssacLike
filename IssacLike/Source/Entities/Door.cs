using IssacLike.Source.Components;
using IssacLike.Source.Managers.Resources;
using IssacLike.Source.RogueLikeImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Entities
{
    public class Door : Entity
    {
        //Components
        [Component(typeof(Sprite))]
        private Sprite m_Sprite;

        [Component(typeof(Collider))]
        private Collider m_Collider;

        private Vector2 m_Size;
        private Vector2 m_Position;
        private Rectangle m_Door;

        public Rectangle DoorBound { get => m_Collider.Bound; }

        public Door(Rectangle door) {
            m_Size = new Vector2(door.Width, door.Height);
            m_Position = new Vector2(door.X, door.Y);
            m_Door = door;
        }

        public override void Start() {
            m_Sprite = new Sprite(TextureLoader.Texture("Textures/TEXTURE_default"));    
            
            m_Collider = new Collider(m_Door) { CanCollide = true, Tag = "Door" };
            m_Collider.Color = new Color(255,0,0,125);
            Scale = new Vector2(m_Collider.Bound.Width / 64);
            Position = m_Position;
            Texture = m_Sprite.Texture;
            base.Start();
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime) {
            batch.Draw(Texture, m_Collider.Bound, Color.White);
            base.Draw(batch, gameTime);       
        }

        public override void Update(GameTime gameTime) {           
            base.Update(gameTime);
        }

        //public override void OnCollisionEvent(ICollidable other) {
            //Logger.Log("{0} is Colliding with Door", other.Tag);

          //  base.OnCollisionEvent(other);
        //}
    }
}
