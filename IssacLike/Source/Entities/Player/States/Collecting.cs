using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Entities.Player;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Managers.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Entities.StateMachine;
using Microsoft.Xna.Framework.Input;
using ProjectMystic.Source.Components;
using Microsoft.Xna.Framework;

namespace ZeldaLike.Source.Entities.Player.States {
    internal class Collecting : IState {

        public Entity Owner => player;

        APlayer player;
        Entity m_Owner;

        public Collecting(Entity owner) { 
            m_Owner = owner;
            player = m_Owner as APlayer;

            TextureLoader.LoadTexture("ItemShow", "Graphics/Player/Pickup");
            player.Animation.Create("ShowItem", TextureLoader.Texture("ItemShow"));
        }

        public void OnEnter(Dictionary<string, object> param = null) {            
            player.PlayerCollider.Location = player.Collider;
            player.Body.Velocity = Vector2.Zero;            
        }

        public void OnInput() {
           
        }

        public void OnUpdate() {

            player.Animation.Play("ShowItem");

            if (Input.IsKeyPressed(Keys.F5)) {
                player.StateMachine.Transition("Idle");
            }
        }

        public void OnExit() {
            player.Inventory.SignificantItemCollected = false;
            player.Animation.Disable = false;
        }        
    }
}
