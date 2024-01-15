using Microsoft.Xna.Framework;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Entities.Player;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Entities.StateMachine;

namespace ZeldaLike.Source.Entities.Player.States {
    internal class Idle : IState {

        Entity IState.Owner { get => m_Owner; }

        private Entity m_Owner; 
        private APlayer player;

        public Idle(Entity owner) {
            m_Owner = owner;
            player = m_Owner as APlayer;
            TextureLoader.LoadTexture("Idle", "Graphics/Player/Idle");   

            player.Animation.Create("Idle", TextureLoader.Texture("Idle"), new Vector2(16, 16), 4, 16, 0.4f);
            player.PlayerCollider.Location = player.Collider;
            
        }

        public void OnEnter(Dictionary<string, object> param = null) {
            player.Body.Velocity = Vector2.Zero;
        }

        public void OnUpdate() {
            player.Animation.Play("Idle", player.Facing);

            if (player.PlayerMoveInput != Vector2.Zero) {
                player.StateMachine.Transition("Walk");
            }
        }

        public void OnInput() {
            throw new NotImplementedException();
        }

        public void OnExit() {
            
        }      
    }
}
