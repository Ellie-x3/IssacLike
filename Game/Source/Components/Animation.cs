using IssacLike.Source.Entities;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Components {
    internal class Animation : IDraw {

        public enum Direction {
            EAST  = 0,
            NORTH = 1,
            WEST  = 2,
            SOUTH = 3,
            NONE
        }
        
        private State m_State;
        public enum State {
            ONCE,
            LOOP
        }

        public Entity Owner { get; set; }
        public string name { get; set; }

        public Flipbook CurrentAnimation { get => m_CurrentAnimation; }
        private Flipbook m_CurrentAnimation;

        private Direction m_Facing; //Should be between 0 and 3

        public Dictionary<string, Flipbook> Animations = new Dictionary<string, Flipbook>();

        public Animation() { }

        public void Initialize() {
            name = "Animation Component";
        }

        public void Update(GameTime gameTime) {
            if(m_CurrentAnimation != null) {
                m_CurrentAnimation.Update(gameTime, m_Facing, m_State);

                if (m_CurrentAnimation.IsFinished) {
                    m_CurrentAnimation = null;
                }
            }          
        }

        public void Draw(SpriteBatch batch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layer) {
            if(m_CurrentAnimation != null) {
                batch.Draw(m_CurrentAnimation.Texture, position, m_CurrentAnimation.UV[m_CurrentAnimation.CurrentFrame], Color.White, 0f, origin, scale, effects, layer);      
            }  
        }

        public void Create(string key, Texture2D texture, Vector2 size, int frameCount, int totalFrameCount, float frameTime) {
            if (Animations.ContainsKey(key)) {
                Logger.Log("Key: {0} already exists", key);
                return;
            }

            Animations.Add(key, new Flipbook(texture, size, frameCount, totalFrameCount, frameTime));
        }

        public void Play(string key, Direction direction = Direction.NONE, State state = State.LOOP) {
            if (!Animations.ContainsKey(key)) {
                Logger.Log("No Animation for: {0}", key);
                return;
            }

            if(m_CurrentAnimation == null || m_CurrentAnimation.Key != key) {
                m_CurrentAnimation = Animations[key];
                m_CurrentAnimation.Key = key;
                m_CurrentAnimation.Stop();
                m_CurrentAnimation.Start();
            }
                
            m_Facing = direction;
            m_State = state;
        }
    }

    internal class Flipbook {
        private Texture2D m_Texture; // Texture to read from
        private int m_FrameCount; // How many frames to split it
        private int m_TotalFrameCount; // How many frames to split it
        private float m_FrameTime; // How long inbetween frames
        private float m_FrameTimeLeft; 
        private Vector2 m_SpriteSize; // Size of sprite to chop. 
        private bool m_IsFinished = false;

        private int m_StartFrame;
        private int m_EndFrame;
        private Animation.Direction m_Direction;

        private bool m_Enable = true;

        private int m_CurrentFrame = 0;

        public int CurrentFrame { get => m_CurrentFrame; }
        public Texture2D Texture { get => m_Texture; }
        public Rectangle[] UV { get; set; }
        public string Key { get; set; }
        public bool IsFinished { get => m_IsFinished; }


        public Flipbook(Texture2D texture, Vector2 size, int frameCount, int totalFrameCount, float frameTime) {

            m_Texture = texture;
            m_FrameCount = frameCount;
            m_TotalFrameCount = totalFrameCount;
            m_SpriteSize = size;
            m_FrameTime = frameTime;
            m_FrameTimeLeft = frameTime;

            UV = new Rectangle[totalFrameCount];
            for(int i = 0; i < totalFrameCount; i++) {
                UV[i] = new Rectangle((int)m_SpriteSize.X * i, 0, (int)m_SpriteSize.X, (int)m_SpriteSize.Y);
            }
        }

        public void Start() => m_Enable = true;
        public void Stop() => m_Enable = false;

        public void Update(GameTime gameTime, Animation.Direction facing, Animation.State state) { 
            if(!m_Enable)
                return;

            float totalSeconds = Globals.Delta;

            m_FrameTimeLeft -= totalSeconds;

            if(m_Direction != facing) {
                m_CurrentFrame = DetermineDirection(facing);
                m_FrameTimeLeft = 0;
                m_Direction = facing;
            }

            if (m_FrameTimeLeft <= 0) {
                switch (state) {
                    case Animation.State.LOOP:
                        m_CurrentFrame++;
                        m_CurrentFrame = DetermineDirection(facing);
                        m_FrameTimeLeft = m_FrameTime;
                        break;
                    case Animation.State.ONCE:
                        if (m_CurrentFrame >= m_FrameCount - 1) {
                            m_IsFinished = true;
                            Stop();
                        }

                        m_CurrentFrame++;
                        m_CurrentFrame = DetermineDirection(facing);
                        m_FrameTimeLeft = m_FrameTime;
                  
                        break;
                    default:
                        break;
                }
            }          
        }

        private int DetermineDirection(Animation.Direction facing) {
            if(facing == Animation.Direction.NONE)
                return m_CurrentFrame;

            int frames = m_FrameCount;
            int direction = (int)facing;
            m_StartFrame = direction * frames;
            m_EndFrame = m_StartFrame + frames;

            if(m_CurrentFrame >= m_EndFrame || m_StartFrame > m_CurrentFrame) {
                return m_StartFrame;
            }

            return m_CurrentFrame;
        }
    }
}
