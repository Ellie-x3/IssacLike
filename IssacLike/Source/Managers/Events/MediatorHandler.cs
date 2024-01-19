using ProjectMystic.Source.Managers.Resources;
using ZeldaLike.Source.Entities;
using Microsoft.Xna.Framework;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Entities.Player;
using ZeldaLike.Source.GUI;
using ZeldaLike.Source.Entities.Player.States;
using ZeldaLike.Source.Entities.StateMachine;
using ZeldaLike.Source.Entities.Items;

namespace ZeldaLike.Source.Managers.Events {
    internal static class MediatorHandler {

        public static Mediator PlayerMediator;

        public static void Initialize() { }

        public static void RegisterPlayerNotifications() {
            PlayerMediator = new Mediator();

            PlayerMediator.RegisterHandler("INTERACTION_APlayer_Door", args => {
                APlayer player = args[0] as APlayer;
                Door door = args[1] as Door;

                player.PlayerPosition = new Vector2(door.LinkedDoorLocation.X + 8, door.LinkedDoorLocation.Y + 8);

                LevelLoader.ChangeLevel(door.LinkedDoorLevel);                
            });

            PlayerMediator.RegisterHandler("INTERACTION_APlayer_Sword", args => {
                APlayer player = args[0] as APlayer;
                Sword sword = args[1] as Sword;

                StateFactory<Attack> attackFactory = owner => new Attack(player);
                player.StateMachine.RegisterState("Attack", attackFactory, player);
                
                PlayerHud.ChangeWeaponSelected(sword.Image);
            });
        }
    }
}
