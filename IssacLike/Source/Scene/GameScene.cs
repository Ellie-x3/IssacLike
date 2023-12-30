using ProjectMystic.Source.Entities;
using Microsoft.Xna.Framework;

using ProjectMystic.Source.Util;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Entities.Player;
using ProjectMystic.Source.Managers.Resources;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using ZeldaLike.Source.Managers;
using ZeldaLike.Source.Entities;
using ZeldaLike.Source.Managers.Events;
using LDtkTypes.TestHome;

namespace ProjectMystic.Source.Scene
{
    public class GameScene : IScene {
        public string Name => "GameScene";

        public static IScene Instance {
            get { return m_Instance ??= new GameScene(); } 
        }
        
        private static IScene m_Instance;

        private GameScene() {
            CameraManager.CreateCamera("Main");
            CameraManager.CurrentCamera.Position = new Vector2(320 - 74, 180 - 4);

            MediatorHandler.RegisterPlayerNotifications();

            Coroutine.StartCoroutine(() => CheckCollisions());
        }

        public void Initialize(SpriteBatch batch, ContentManager content) {
            LevelLoader.Init(batch, content);

            LevelLoader.SpawnEntitiesInLevel<PlayerEnt>();
            LevelLoader.SpawnEntitiesInLevel<DoorEnt>();
        }

        public void Update(GameTime gameTime) {
            LevelLoader.Update(gameTime);         
            CameraManager.Update();
            
            EntityManager.Update(gameTime);  
        }

        public void SceneContent(SpriteBatch batch, ContentManager content) {
            
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: CameraManager.CurrentCameraMatrices);
                LevelLoader.Draw(batch);
                EntityManager.Draw(batch, gameTime);                
                //batch.DrawString(Globals.font, Globals.fps.ToString(), new Vector2(5, 5), Color.White);
            batch.End();
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
