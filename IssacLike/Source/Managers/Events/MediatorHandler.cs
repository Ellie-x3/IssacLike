using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Entities;

namespace ZeldaLike.Source.Managers.Events {
    internal static class MediatorHandler {

        public static Mediator PlayerMediator;

        public static void Initialize() { }

        public static void RegisterPlayerNotifications() {
            PlayerMediator = new Mediator();

            PlayerMediator.RegisterHandler("INTERACTION_Player_Door", args => { 
                Logger.Log("Meow from player - door");
            });
        }
    }
}
