using IssacLike.Source.Components;
using IssacLike.Source.Managers.Resources;
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
        private static Collider m_Collider;

        private Vector2 m_Size;
        private Vector2 m_Position;

        public static Rectangle DoorBound { get => m_Collider.Bound; }


        public Door(Rectangle door) {
            m_Size = new Vector2(door.Width, door.Height);
            m_Position = new Vector2(door.X, door.Y);
        }

        public override void Start() {
            m_Sprite = new Sprite(TextureLoader.Texture("TEXTURE_default"));
            m_Collider = new Collider(new Rectangle(0, 0, (int)m_Size.X, (int)m_Size.Y));
            Position = m_Position;
            base.Start();
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime) {
            base.Draw(batch, gameTime);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void OnCollision(Rectangle hit) {

        }
    }
}
