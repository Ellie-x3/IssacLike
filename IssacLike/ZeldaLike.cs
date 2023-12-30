using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using ProjectMystic.Source.ZeldaLikeImGui;
using ProjectMystic.Source.Util;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Scene;

using LDtk;
using LDtk.Renderer;
using LDtkTypes.TestHome;

namespace ProjectMystic
{
    public class ZeldaLike : Game {
        private SpriteBatch m_SpriteBatch;

        private SceneManager m_SceneManager;

        private Color m_BackgroundColor = new Color(19,0,13,255);

        private RenderTarget2D m_RenderTarget;
        private float m_RenderScale;

        public ZeldaLike() {
            Globals.s_Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void PreInit() {
            Globals.s_GraphicsDevice = Globals.s_Graphics.GraphicsDevice;
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 240.0f);

            IsMouseVisible = true;

            Globals.ScreenSize = new Vector2(1280, 720);
            Globals.s_Graphics.ApplyChanges();
        }

        protected override void Initialize() {
            PreInit();
            Input.Initialize();
            ResourceLoader.Initialize(Content);
            Logger.Log("Initializing");
            m_SceneManager = new SceneManager(GameScene.Instance);

            GameManager.Start(m_SpriteBatch, Content);

            UIRenderer.Init(this);

            base.Initialize();
        }

        protected override void LoadContent() {
            Logger.Log("LOAD CONTENT");
            m_RenderTarget = new RenderTarget2D(Globals.s_GraphicsDevice, 1280, 720);
            Globals.font = Content.Load<SpriteFont>("Fonts/Font");
            GameManager.LoadContent(m_SpriteBatch, Content);            

            UIRenderer.s_GuiRenderer.RebuildFontAtlas();
        }

        protected override void Update(GameTime gameTime) {
            Globals.Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update(gameTime);
            UIRenderer.Update(gameTime);

            GameManager.Update(gameTime);

            Globals.fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
 
        }

        protected override void Draw(GameTime gameTime) {

            m_RenderScale = 1f / (360f / Globals.s_Graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(m_RenderTarget);
            GraphicsDevice.Clear(m_BackgroundColor);   

            GameManager.Draw(m_SpriteBatch, gameTime);         
            
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(m_BackgroundColor);

            m_SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            m_SpriteBatch.Draw(m_RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, m_RenderScale, SpriteEffects.None, 0f);
            m_SpriteBatch.End();
            
            base.Draw(gameTime);
            Logger.Draw(gameTime);
        }
    }
}