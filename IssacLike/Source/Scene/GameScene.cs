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
using ProjectMystic.Source.ZeldaLikeImGui;
using ZeldaLike.Source.GUI;

namespace ProjectMystic.Source.Scene
{
    public class GameScene : IScene {
        public string Name => "GameScene";

        public Effect overlayEffect;
        Vector4 overlayColor = new Vector4(0.0f,0.1f,0.5f, 1f);

        public static IScene Instance {
            get { return m_Instance ??= new GameScene(); } 
        }
        
        private static IScene m_Instance;

        private GameScene() {        
            MediatorHandler.RegisterPlayerNotifications();

            

            Coroutine.StartCoroutine(() => CheckCollisions());
        }

        public void Initialize(SpriteBatch batch, ContentManager content) {
            LevelLoader.Init(batch, content);

            EntityManager.SpawnEntitiesInLevel<PlayerEnt>(LevelLoader.CurrentLevel);
            EntityManager.SpawnEntitiesInLevel<DoorEnt>(LevelLoader.CurrentLevel);
            EntityManager.SpawnEntitiesInLevel<Pickup>(LevelLoader.CurrentLevel);

            EntityManager.Initialize();

            CameraManager.CreateCamera("Main", EntityManager.Find("Player"));
            CameraManager.CurrentCamera.Position = new Vector2(320 - 72, 180 - 4);

            GuiManager.Add("Hud", new PlayerHud());

            GuiManager.SetHud = GuiManager.Guis["Hud"];
        }

        public void Update(GameTime gameTime) {
            LevelLoader.Update(gameTime);         

            EntityManager.Update(gameTime);  
            GuiManager.Update(gameTime);
            TransitionManager.Update(gameTime);
            CameraManager.Update();
        }

        public void SceneContent(SpriteBatch batch, ContentManager content) {
            overlayEffect = content.Load<Effect>("Effects\\File");
            overlayEffect.Parameters["OverlayColor"].SetValue(overlayColor);
            GuiManager.LoadContent();
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: CameraManager.CurrentCameraMatrices);
                LevelLoader.Draw(batch);
                EntityManager.Draw(batch, gameTime);
                GuiManager.Draw(batch);
                TransitionManager.Draw(batch);
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
