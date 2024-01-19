using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMystic.Source.Managers.Events;
using ProjectMystic.Source.Util;
using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Managers;

namespace ZeldaLike.Source.Transitions {
    internal class FullBlock : ITransition {

        public float Speed = 10.0f;

        private Texture2D m_Texture;
        private Rectangle m_Rectangle;
        private Vector2 m_Position;
        private Vector2 m_InitialPosition;
        private Vector2 m_Velocity;
        private bool m_IsReversed;

        public bool IsFinished { get; set; } = false;

        public FullBlock(Vector2 position, float speed = 1.0f, bool reverse = false) {            
            IsFinished = false;
            Speed = speed;

            m_Position = position;
            m_InitialPosition = position;
            m_IsReversed = reverse;

            m_Texture = new Texture2D(Globals.s_GraphicsDevice, 1, 1);

            m_Texture.SetData(new Color[] {Color.Black});
            m_Rectangle = new Rectangle(0, 0, (int)Globals.CameraSize.X, (int)Globals.CameraSize.Y + 20);

            EventManager.E_RoomChanged += RoomChanged;
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(m_Texture, m_Position, m_Rectangle, Color.White);
        }

        public void Update(GameTime gameTime) {
            if(!IsFinished)
                m_Position = new Vector2(m_Position.X, m_Position.Y + Speed);

            if(m_Position.Y > CameraManager.CurrentCamera.Position.Y && !m_IsReversed) {
                IsFinished = true;
               // m_Position = m_InitialPosition;
            } else if(m_Position.Y < CameraManager.CurrentCamera.Position.Y - Globals.CameraSize.Y && m_IsReversed) {
                IsFinished = true;
               // m_Position = m_InitialPosition;
            }
        }

        public void Dispose() { }

        private void RoomChanged() {
            Logger.Log("CHANGING ROOOOMS");
        }

    }
}
