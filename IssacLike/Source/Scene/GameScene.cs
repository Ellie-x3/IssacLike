using ProjectMystic.Source.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProjectMystic.Source.Util;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Entities.Player;
using ProjectMystic.Source.Managers.Resources;
using LDtk.Renderer;
using Microsoft.Xna.Framework.Graphics;
using ProjectMystic.Source.ZeldaLikeImGui;
using System.Collections;
using ProjectMystic.Source.Entities.Enemies;
using LDtk;
using LDtkTypes.TestHome;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;

namespace ProjectMystic.Source.Scene
{
    public class GameScene : IScene {
        public string Name => "GameScene";

        private Player m_Player;

        private Matrix m_ScaleMatrix;

        public static IScene Instance {
            get { return m_Instance ??= new GameScene(); } 
        }
        
        private static IScene m_Instance;

        private GameScene() {
            Globals.Camera = new Camera(Globals.s_GraphicsDevice);
            m_ScaleMatrix = Matrix.CreateScale(2);

            Globals.Camera.Position = new Vector2(640 - 74, 360-4);

            //m_Player = new Player(LevelLoader.GetEntityData());
           


            Coroutine.StartCoroutine(() => CheckCollisions());
        }

        public void AddEntityToScene(Entity entity) {
            EntityManager.Add(entity);
        }

        public void Update(GameTime gameTime) {
            LevelLoader.Update(gameTime);
            Globals.Camera.Update();
            Globals.Camera.Zoom = Math.Max(Globals.s_GraphicsDevice.Viewport.Height / 720f, 1);
            EntityManager.Update(gameTime);  
        }

        public void SceneContent(SpriteBatch batch, ContentManager content) {
            //LevelLoader.m_Renderer = new LDtkRenderer(batch);
            //LevelLoader.Init(batch);

            LevelLoader.Init(batch, content);


            m_Player = new Player();

            AddEntityToScene(m_Player);
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: Globals.Camera.Transform * m_ScaleMatrix);
                LevelLoader.Draw(batch);
                EntityManager.Draw(batch, gameTime);                
                //batch.DrawString(Globals.font, Globals.fps.ToString(), new Vector2(5, 5), Color.White);
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
