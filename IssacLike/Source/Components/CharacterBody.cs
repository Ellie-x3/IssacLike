using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Entities.Player;
using ProjectMystic.Source.ZeldaLikeImGui;
using ProjectMystic.Source.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMystic.Source.Components {
    public class CharacterBody : IComponent {
        public Entity Owner { get; set; }

        public string name { get; set; }

        private Vector2 m_Velocity;
        private Vector2 m_Position;
        private Vector2 m_Magnitude;
        private float m_Speed;
        private float m_Acceleration;

        public Vector2 Velocity { get => m_Velocity; set => m_Velocity = value; }
        public Vector2 Position { get => m_Position; set => m_Position = value; }

        public float PositionX {
            get => m_Position.X;
            set {
                m_Position = new Vector2(value, m_Position.Y);
            }
        }

        public float PositionY {
            get => m_Position.Y;
            set {
                m_Position = new Vector2(m_Position.X, value);
            }
        }

        public Vector2 Magnitude { get => m_Magnitude; set => m_Magnitude = value; }

        public float Speed { get => m_Speed; set => m_Speed = value; }
        public float Acceleration { get => m_Acceleration; set => m_Acceleration = value; }

        public void Initialize() {
            Logger.Log("Initializing {0}'s Characterbody", Owner.Name);
        }

        public void Update(GameTime gameTime) {

        }

        public Vector2 Move() {
            m_Position += m_Velocity;
            return m_Position;
        }
    }
}
