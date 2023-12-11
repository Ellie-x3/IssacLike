using IssacLike.Source.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IssacLike.Source.Util;
using IssacLike.Source.Managers;
using IssacLike.Source.Entities.Player;
using IssacLike.Source.Managers.Resources;
using LDtk.Renderer;
using Microsoft.Xna.Framework.Graphics;
using IssacLike.Source.Rooms;
using IssacLike.Source.RogueLikeImGui;

namespace IssacLike.Source.Scene
{
    public class GameScene : IScene {
        public string name { get => "GameScene"; }

        private Player player;

        public static IScene Instance { get {
                if(m_Instance == null){
                    m_Instance = new GameScene();
                }

                return m_Instance;
            } 
        }
        
        private static IScene m_Instance;

        public GameScene() {
            Globals.Camera = new Camera(Globals.s_GraphicsDevice);

            FloorManager.Create();

            player = new Player();
            AddEntityToScene(player);           
        }

        public void AddEntityToScene(Entity entity) {
            EntityManager.Add(entity);
        }

        public void Update(GameTime gameTime) {
            //LevelLoader.Update(gameTime);
            Globals.Camera.Update();
            //Globals.Camera.Position = new Vector2(0,0);
            Globals.Camera.Zoom = Math.Max(Globals.s_GraphicsDevice.Viewport.Height / 720f, 1);
            EntityManager.Update(gameTime);
            FloorManager.PlayerPosition = player.PlayerPosition;
            FloorManager.Update(gameTime);   
        }

        public void SceneContent(SpriteBatch batch) {
           // LevelLoader.m_Renderer = new LDtkRenderer(batch);
           // LevelLoader.Init(batch);
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: Globals.Camera.Transform);
                FloorManager.Draw(batch, gameTime);
                EntityManager.Draw(batch, gameTime);
            batch.End();
        }

        private void UpdateCameraPosition() {

        }
    }
}
