using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaLike.Source.Entities {
    public interface IMediator {
        void Notify(string message, params object[] args);
    }

    public class Mediator : IMediator {

        private Dictionary<string, List<Action<object[]>>> m_Handlers;

        public Mediator() {
            m_Handlers = new Dictionary<string, List<Action<object[]>>>();
        }

        public void RegisterHandler(string message, Action<object[]> handler) {
            if (!m_Handlers.ContainsKey(message)) {
                m_Handlers[message] = new List<Action<object[]>>();
            }

            m_Handlers[message].Add(handler);
        }

        public void Notify(string message, params object[] args) { 
            if(m_Handlers.TryGetValue(message, out var handlers)) {
                foreach(var handler in handlers) {
                    handler(args);
                }
            }
        }
    }
}
