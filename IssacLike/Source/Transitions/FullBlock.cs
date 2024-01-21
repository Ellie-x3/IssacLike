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
        public bool FirstPassIsFinished { get; private set; } = false;
        public bool SecondPassIsFinished { get; private set; } = false;

        public FullBlock(Vector2 position, float speed = 1.0f, bool reverse = false) {            
            IsFinished = false;
            Speed = speed;

            m_Position = position;
            m_InitialPosition = position;
            m_IsReversed = reverse;

            m_Texture = new Texture2D(Globals.s_GraphicsDevice, 1, 1);

            m_Texture.SetData(new Color[] {Color.Black});
            m_Rectangle = new Rectangle(0, 0, (int)Globals.CameraSize.X, (int)Globals.CameraSize.Y + 20);
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(m_Texture, m_Position, m_Rectangle, Color.White);
        }

        public void FirstDrawPass(SpriteBatch batch) {
            batch.Draw(m_Texture, m_Position, m_Rectangle, Color.White);
        }

        public void SecondDrawPass(SpriteBatch batch) {
            batch.Draw(m_Texture, m_Position, m_Rectangle, Color.White);
        }

        public void FirstPassUpdate(GameTime gameTime) {
            if(!FirstPassIsFinished)
                m_Position = new Vector2(CameraManager.CurrentCamera.Position.X - Globals.CameraSize.X, m_Position.Y + Speed);

            if(m_Position.Y < CameraManager.CurrentCamera.Position.Y - Globals.CameraSize.Y && m_IsReversed) {
                IsFinished = true;
                FirstPassIsFinished = true;  
                Speed *= -1;              
            }
        }

        public void Update(GameTime gameTime) {
            //
        }

        public void SecondPassUpdate(GameTime gameTime) {
            if(!SecondPassIsFinished)
                m_Position = new Vector2(CameraManager.CurrentCamera.Position.X - Globals.CameraSize.X, m_Position.Y + Speed);

            if(m_Position.Y > CameraManager.CurrentCamera.Position.Y) {
                SecondPassIsFinished = true;
                TransitionManager.ChangeState(TransitionManager.TransitionStates.TRANSITIONFINISHED);               
            }
        }

        public void Dispose() { }

        
    }
}
