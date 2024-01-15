using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Entities.Player;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Entities.StateMachine;
using ProjectMystic.Source.Components;
using System.Reflection.Emit;

namespace ZeldaLike.Source.Entities.Player.States {
    internal class Walk : IState {
        public Entity Owner { get => m_Entity; }

        private Entity m_Entity;
        private APlayer player;
        private float m_WalkSpeed = 0.11f;

        public Walk(Entity owner) {
            m_Entity = owner;
            player = m_Entity as APlayer;

            TextureLoader.LoadTexture("Walk", "Graphics/Player/Walk");
            player.Animation.Create("Walk", TextureLoader.Texture("Walk"), new Vector2(16, 16), 4, 16, m_WalkSpeed);
        }

        public void OnEnter(Dictionary<string, object> param = null) {
            
        }

        public void OnExit() {

        }

        public void OnInput() {
            throw new NotImplementedException();
        }

        public void OnUpdate() {
            PlayerMove();
            player.Animation.Play("Walk", player.Facing);

            if(player.PlayerMoveInput == Vector2.Zero) {
                player.StateMachine.Transition("Idle");
            }
        }

        private void PlayerMove() {   

            player.Body.Magnitude = Directions.Normalize(player.PlayerMoveInput);
            player.Body.Velocity = player.Body.Magnitude * player.Body.Speed * Globals.Delta;
            player.Body.Position = player.BodyPosition;

            player.NextBodyPosition = player.Body.Position + player.Body.Velocity;

            player.BodyOverridePosition = player.OnWallCollision(LevelLoader.LevelCollisionTiles);

            player.BodyPosition = player.BodyOverridePosition ?? player.NextBodyPosition;
            player.PlayerCollider.Location = player.ColliderLocation;
        }
    }
}
