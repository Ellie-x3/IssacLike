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
using LDtk;

namespace ProjectMystic.Source.Entities
{
    public class Door : Entity
    {
        public Vector2 LinkedDoorLocation { get => m_LinkedDoorLocation; }

        [Component(typeof(Collider))]
        private Collider m_Collider;

        private Vector2 m_Position;
        private Rectangle m_Door;

        public Rectangle DoorBound { get => m_Collider.Bound; }

        private DoorEnt data;
        private DoorEnt m_LinkedDoor;
        private Vector2 m_LinkedDoorLocation;
        public LDtkLevel LinkedDoorLevel { get; private set; }

        public Door(Rectangle door, DoorEnt data) {
            this.data = data;

            LinkedDoorLevel = LevelLoader.EntityLevel("DoorEnt", "DoorRef", data.Iid);
            m_LinkedDoor = LinkedDoorLevel.GetEntityRef<DoorEnt>(data.DoorRef);

            m_LinkedDoorLocation = m_LinkedDoor.Position;

            Name = "Door";
            m_Position = new Vector2(door.X, door.Y);
            m_Door = door;
        }

        public override void Start() {
            m_Collider = new Collider(m_Door) { CanCollide = true, Tag = "Door", Color = new Color(255, 0, 0, 125) };
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
