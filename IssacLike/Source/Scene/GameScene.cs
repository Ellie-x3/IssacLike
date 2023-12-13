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
using System.Collections;

namespace IssacLike.Source.Scene
{
    public class GameScene : IScene {
        public string Name => "GameScene";

        private readonly Player m_Player;
        private Door m_Door;

        public static IScene Instance {
            get { return m_Instance ??= new GameScene(); } 
        }
        
        private static IScene m_Instance;

        private GameScene() {
            Globals.Camera = new Camera(Globals.s_GraphicsDevice);

            FloorManager.Create();

            m_Player = new Player();
            AddEntityToScene(m_Player);

            //Coroutine.StartCoroutine(() => CheckCollisions());
            CollisionManager.CheckCollisions();
            
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
            FloorManager.PlayerPosition = m_Player.PlayerPosition;
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

        private static IEnumerator CheckCollisions() {
            WaitForSeconds wait = new WaitForSeconds(Globals.Delta * 3);
            while (!wait.IsWaitFinished()) {
                yield return null;
            }

            CollisionManager.CheckCollisions();
        }
    }
}
