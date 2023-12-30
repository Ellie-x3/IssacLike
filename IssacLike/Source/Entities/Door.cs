using ProjectMystic.Source.Components;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.ZeldaLikeImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDtkTypes.TestHome;

namespace ProjectMystic.Source.Entities
{
    public class Door : Entity
    {
        //Components

        [Component(typeof(Collider))]
        private Collider m_Collider;

        private Vector2 m_Size;
        private Vector2 m_Position;
        private Rectangle m_Door;

        public Rectangle DoorBound { get => m_Collider.Bound; }

        private DoorEnt data;

        public Door(Rectangle door, DoorEnt data) {
            this.data = data;
            name = "Door";
            m_Size = new Vector2(door.Width, door.Height);
            m_Position = new Vector2(door.X, door.Y);
            m_Door = door;
        }

        public override void Start() {
            m_Collider = new Collider(m_Door) { CanCollide = true, Tag = "Door", Color = new Color(255, 0, 0, 125) };
            Scale = new Vector2(m_Collider.Bound.Width / 16);
            Position = m_Position;
            base.Start();
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime) {
            base.Draw(batch, gameTime);       
        }

        public override void Update(GameTime gameTime) {      
            base.Update(gameTime);
        }
    }
}
