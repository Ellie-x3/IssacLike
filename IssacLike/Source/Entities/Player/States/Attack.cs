using ProjectMystic.Source.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Entities.StateMachine;

namespace ZeldaLike.Source.Entities.Player.States {
    internal class Attack : IState {
        public Entity Owner => throw new NotImplementedException();

        public Attack(Entity owner) {

        }

        public void OnEnter(Dictionary<string, object> param = null) {
            throw new NotImplementedException();
        }

        public void OnExit() {
            throw new NotImplementedException();
        }

        public void OnInput() {
            throw new NotImplementedException();
        }

        public void OnUpdate() {
            throw new NotImplementedException();
        }
    }
}
