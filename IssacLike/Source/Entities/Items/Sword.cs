using LDtkTypes.TestHome;
using Microsoft.Xna.Framework.Graphics;
using ProjectMystic.Source.Components;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Managers.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMystic.Source.ZeldaLikeImGui;
using ProjectMystic.Source.Util;
using ProjectMystic.Source.Entities.Player;

namespace ZeldaLike.Source.Entities.Items {
    internal class Sword : Entity, ISignificantItem {

        public Entity Item => this; 
        public string ItemName => "Sword"; 
        public bool HasBeenCollected { get => false; }
        Rectangle ISignificantItem.CollectionArea => m_CollectionArea;

        public Texture2D Image { get; }

        private Rectangle m_CollectionArea;

        [Component(typeof(Sprite))]
        private Sprite m_Sprite;

        [Component(typeof(Collider))]
        private Collider m_Collider;

        private Texture2D m_CollectionAreaTexture;

        public Sword(Pickup data) {
            Name = "Sword";
            Position = new Vector2(data.Position.X, data.Position.Y);
            Origin = new Vector2(3f);
            TextureLoader.AddTexture("Sword", "Items/StaticSword");
            Image = TextureLoader.Texture("Sword");

            m_Sprite = new Sprite(Image);
            m_Sprite.Source = new Rectangle(0,0,22,22);
            
            m_CollectionArea = new Rectangle((int)Position.X, (int)Position.Y, 16, 16);

            m_CollectionAreaTexture = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_CollectionAreaTexture.SetData(new Color[] { Color.Red }); 

            m_Collider = new Collider(m_CollectionArea) { 
                CanCollide = true,
                Tag = "Sword"
            };
        }        

        public override void Draw(SpriteBatch batch, GameTime gameTime) {
            //batch.Draw(m_CollectionAreaTexture, m_CollectionArea, Color.White);
            base.Draw(batch, gameTime);
        }

        public override void OnCollisionEvent(ICollidable other) {
            Logger.Log("Sword is collected");

            if(other.Entity.Name == "Player") {
                APlayer player = other.Entity as APlayer;

                player.Inventory.BasicSwordCollected = true;
                player.Inventory.CollectedItems.Add(ItemName, true);
                player.Inventory.SignificantItemCollected = true;
            }

            Dispose(this);
        }
    }
}
