using IssacLike.Source.Components;
using IssacLike.Source.Entities;
using IssacLike.Source.Managers;
using IssacLike.Source.Managers.Resources;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace IssacLike.Source.Scene {
    public class MainMenuScene : IScene {
        public string name { get => "MainMenuScene"; }

        public static IScene Instance { get {
                if(m_Instance == null){
                    m_Instance = new MainMenuScene();
                }

                return m_Instance;
            } 
        }

        private static IScene m_Instance;

        public MainMenuScene() {
            TextureLoader.AddTexture("MainMenu", "mainmenu");
        }

        public void AddEntityToScene(Entity entity) {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime) {
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {            
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                batch.Draw(TextureLoader.Texture("MainMenu"), new Rectangle(0, 0, 640, 360), Color.White);
            batch.End();
        }

        public void SceneContent(SpriteBatch batch) {
            Logger.Log("Main Menu Loading Content!");
        }
    }
}
