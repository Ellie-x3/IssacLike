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
using ZeldaLike.Source.Transitions;

namespace ZeldaLike.Source.Managers {
    public static class TransitionManager {

        public enum TransitionTypes {
            SCREENCOVER,
        }

        public enum TransitionStates {
            NORMAL,
            BEFORETRANSITION,
            TRANSITION,
            AFTERTRANSITION,
            TRANSITIONFINISHED,
        }

        private static ITransition? m_ActiveTransition;
        private static TransitionStates m_CurrentState = TransitionStates.NORMAL;

        public static void SetTransition(TransitionTypes type) {

            m_ActiveTransition = TransitionFactory(type);

            if(m_ActiveTransition == null) {
                Logger.Log("Transition: {0} is null", type.ToString());
                return;
            }            

            ChangeState(TransitionStates.BEFORETRANSITION);
        }

        public static void Draw(SpriteBatch batch) {
            if(m_CurrentState == TransitionStates.NORMAL || m_ActiveTransition == null)
                return;

            switch(m_CurrentState) {
                case TransitionStates.BEFORETRANSITION:
                    m_ActiveTransition?.FirstDrawPass(batch);
                    break;
                case TransitionStates.TRANSITION:
                    m_ActiveTransition?.Draw(batch);
                    break;
                case TransitionStates.AFTERTRANSITION:
                    m_ActiveTransition?.SecondDrawPass(batch);
                    break;
                default:
                    m_CurrentState = TransitionStates.NORMAL;
                    break;
            }
            //m_ActiveTransition?.Draw(batch);
        }

        public static void Update(GameTime gameTime) {
            //m_ActiveTransition?.Update(gameTime);

            //if (m_ActiveTransition != null && m_ActiveTransition.IsFinished) {                
            //}

            if(m_CurrentState == TransitionStates.NORMAL || m_ActiveTransition == null)
                return;

            switch(m_CurrentState) {
                case TransitionStates.BEFORETRANSITION:
                    m_ActiveTransition?.FirstPassUpdate(gameTime);
                    break;
                case TransitionStates.TRANSITION:
                    m_ActiveTransition?.Update(gameTime);
                    break;
                case TransitionStates.AFTERTRANSITION:
                    m_ActiveTransition?.SecondPassUpdate(gameTime);
                    break;
                default:
                    m_CurrentState = TransitionStates.NORMAL;
                    break;
            }

            if(m_ActiveTransition.FirstPassIsFinished && m_CurrentState == TransitionStates.BEFORETRANSITION)
                ChangeState(TransitionStates.TRANSITION);
        }

        private static ITransition? TransitionFactory(TransitionTypes type) {
            switch (type) {
                case TransitionTypes.SCREENCOVER: 
                    return new FullBlock(new Vector2(CameraManager.CurrentCamera.Position.X - Globals.CameraSize.X, CameraManager.CurrentCamera.Position.Y), -3f, true);
                default:
                    Logger.Log("Transition: {0} doesnt exist", type.ToString());
                    break;
            }

            return null;
        }

        public static void ChangeState(TransitionStates state){
            if(m_CurrentState != state){
                m_CurrentState = state;
                switch(m_CurrentState) {
                    case TransitionStates.NORMAL:                 
                        EventManager.OnTransitionNormal();
                        break;
                    case TransitionStates.BEFORETRANSITION:
                        if(m_ActiveTransition == null) {
                            Logger.Log("No Transition was set");
                            break;
                        }

                        EventManager.OnTransitionBefore();
                        break;
                    case TransitionStates.TRANSITION:
                        EventManager.OnTransitionDuring();
                        break;
                    case TransitionStates.AFTERTRANSITION:
                        EventManager.OnTransitionAfter();
                        break;
                    case TransitionStates.TRANSITIONFINISHED:
                        EventManager.OnTransitionFinished();
                        break;
                    default:
                        Logger.Log("Transition: {0} does not exist changing back to normal state", state);
                        m_CurrentState = TransitionStates.NORMAL;
                        break;
                }
            }
        }
    }
}
