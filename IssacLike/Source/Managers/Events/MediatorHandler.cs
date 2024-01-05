using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Entities;
using Microsoft.Xna.Framework;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Entities.Player;

namespace ZeldaLike.Source.Managers.Events {
    internal static class MediatorHandler {

        public static Mediator PlayerMediator;

        public static void Initialize() { }

        public static void RegisterPlayerNotifications() {
            PlayerMediator = new Mediator();

            PlayerMediator.RegisterHandler("INTERACTION_Player_Door", args => {
                Player player = args[0] as Player;
                Door door = args[1] as Door;

                player.PlayerPosition = new Vector2(door.LinkedDoorLocation.X + 8, door.LinkedDoorLocation.Y + 8);

                LevelLoader.ChangeLevel(door.LinkedDoorLevel);
                //CameraManager.ChangeCameraToLevel();
                
            });
        }
    }
}
