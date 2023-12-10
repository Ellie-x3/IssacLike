using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using IssacLike.Source.Managers;
using IssacLike.Source.Managers.Resources;
using IssacLike.Source.Entities;
using IssacLike.Source.Scene;

using LDtk;
using LDtk.Renderer;

namespace IssacLike
{
    public class RogueLike : Game {
        private SpriteBatch _spriteBatch;

        private SceneManager SceneManager;

        private RenderTarget2D renderTarget;
        private float renderScale;

        public RogueLike() {
            Globals.s_Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void PreInit() {
            Globals.s_GraphicsDevice = Globals.s_Graphics.GraphicsDevice;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0f);

            IsMouseVisible = true;

            Globals.ScreenSize = new Vector2(1280, 720);
            Globals.s_Graphics.ApplyChanges();
        }

        protected override void Initialize() {
            
            PreInit();
            Input.Initialize();
            ResourceLoader.Initialize(Content);
            Logger.Log("Initializing");
            SceneManager = new SceneManager(GameScene.Instance);

            UIRenderer.Init(this);

            base.Initialize();
        }

        protected override void LoadContent() {
            renderTarget = new RenderTarget2D(Globals.s_GraphicsDevice, 1920, 1080);

            GameManager.LoadContent(_spriteBatch);            

            UIRenderer.s_GuiRenderer.RebuildFontAtlas();
        }

        protected override void Update(GameTime gameTime) {
            Globals.Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update(gameTime);
            UIRenderer.Update(gameTime);

            GameManager.Update(gameTime);

            if (Input.IsKeyPressed(Keys.F3)) {
                Logger.Log("F3 Pressed");
            }

            base.Update(gameTime);
 
        }

        protected override void Draw(GameTime gameTime) {

            renderScale = 1f / (1080f / Globals.s_Graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                GameManager.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);
            Logger.Draw(gameTime);
        }
    }
}