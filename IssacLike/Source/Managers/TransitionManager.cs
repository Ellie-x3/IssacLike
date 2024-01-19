using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMystic.Source.Util;
using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Transitions;

namespace ZeldaLike.Source.Managers {
    public static class TransitionManager {

        private static ITransition? m_ActiveTransition;

        public static void SetTransition(string key) {

            m_ActiveTransition = TransitionFactory(key);

            if(m_ActiveTransition == null) {
                Logger.Log("Transition: {0} is null", key);
                return;
            }

            m_ActiveTransition.IsFinished = false;
        }

        public static void Draw(SpriteBatch batch) {
            m_ActiveTransition?.Draw(batch);
        }

        public static void Update(GameTime gameTime) {
            m_ActiveTransition?.Update(gameTime);

            if (m_ActiveTransition != null && m_ActiveTransition.IsFinished) {
                
            }
        }

        private static ITransition? TransitionFactory(string key) {
            switch (key) {
                case "FullDown": 
                    return new FullBlock(new Vector2(CameraManager.CurrentCamera.Position.X - Globals.CameraSize.X, CameraManager.CurrentCamera.Position.Y - Globals.CameraSize.Y));
                case "FullUp":
                    return new FullBlock(new Vector2(CameraManager.CurrentCamera.Position.X - Globals.CameraSize.X, CameraManager.CurrentCamera.Position.Y), -1f, true);
                default:
                    Logger.Log("Transition: {0} doesnt exist", key);
                    break;
            }

            return null;
        }
    }
}
