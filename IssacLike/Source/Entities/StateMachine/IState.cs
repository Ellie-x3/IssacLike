using ProjectMystic.Source.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaLike.Source.Entities.StateMachine {
    public interface IState {
        Entity Owner { get; }
        void OnEnter(Dictionary<string, object> param = null);
        void OnUpdate();
        void OnInput();
        void OnExit();
    }
}
