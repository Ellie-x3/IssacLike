using ProjectMystic.Source.Entities;
using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaLike.Source.Entities.StateMachine {
    public delegate T StateFactory<out T>(Entity owner) where T : IState;

    public class StateMachine {

        public IState State { get => m_State; }

        private IState m_State = null;
        private Dictionary<string, IState> m_States = new Dictionary<string, IState>();

        public virtual void Update() { 
            m_State.OnUpdate();
        }
        public virtual void OnInput() { 
            m_State.OnInput();
        }

        public void Transition(string name, Dictionary<string, object> param = null) { 

            m_States.TryGetValue(name, out IState state);

            if(state != m_State && m_States.ContainsKey(name)) {
                if(m_State != null)
                    m_State.OnExit();

                m_State = state;
                m_State.OnEnter(param);
            }
        }

        public void RegisterState<T>(string name, StateFactory<T> factory, Entity owner) where T : IState {
            if (!m_States.ContainsKey(name)) {
                T state = factory(owner);
                m_States.Add(name, state);
            }
        }

        public void UnRegisterState<T>(string name) where T : IState {
            if (m_States.ContainsKey(name)) {
                m_States.Remove(name);
            }
        }
    }
}
