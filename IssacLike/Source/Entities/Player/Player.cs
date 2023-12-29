using ProjectMystic.Source.Components;
using ProjectMystic.Source.Managers;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.ZeldaLikeImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMystic.Source.Util;
using LDtk;
using ProjectMystic.Source.Managers.Events;
using LDtkTypes.TestHome;

namespace ProjectMystic.Source.Entities.Player
{
    public class Player : Entity {
        #region Components
        //Components
        [Component(typeof(CharacterBody))]
        private CharacterBody body;
     
        [Component(typeof(Animation))]
        private Animation AnimationController;

        [Component(typeof(Collider))]
        private Collider m_DoorCollider;

        #endregion

        //Animation
        private Animation.Direction m_Facing = 0;
        private readonly Vector2 m_SpriteHalf = new Vector2(8, 8);

        //Collision
        private Point m_DoorColliderPoint;
        private Point DoorColliderLocation {
            get {
                m_DoorColliderPoint = new Point((int)(Position.X - m_SpriteHalf.X), (int)(Position.Y - m_SpriteHalf.Y));
                return m_DoorColliderPoint;
            }
        }

        private Vector2 m_PrevPosition;
        private Vector2 m_NextPosition;

        //DEBUG / TESTING
        public Vector2 PlayerPosition => Position;

        //LDtk
        private PlayerEnt data;

        public Player()//PlayerEnt data)
        {
            this.data = LevelLoader.GetEntityData();
            name = "Player";
            Scale = new Vector2(1f);
            Origin = new Vector2(8f, 8f);
            TextureLoader.AddTexture("Walk", "Player/Zink_Walk");
            TextureLoader.AddTexture("Idle", "Player/Zink_Idle");
     
        }

        public override void Start()
        {
           
            AnimationController = new Animation();
         
            AnimationController.Create("Walk", TextureLoader.Texture("Walk"), new Vector2(16,16), 2, 8, 0.17f);
            AnimationController.Create("Idle", TextureLoader.Texture("Idle"), new Vector2(16,16), 1, 4, 0f);

            body = new CharacterBody
            {                
                Position = new Vector2(data.Position.X + m_SpriteHalf.X, data.Position.Y + m_SpriteHalf.Y),
                Speed = 60.0f,
                Velocity = new Vector2()
            };

            Position = body.Position;
            //Origin = Position;
            m_DoorCollider = new Collider(new Rectangle(DoorColliderLocation.X, DoorColliderLocation.Y, 16,16)) { 
                CanCollide = true,
                Tag = "Player",
                Color = new Color(140, 238, 100, 10)
            };

            base.Start();
        } 

        public override void Update(GameTime gameTime)
        {
            PlayerMove(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {            
            base.Draw(batch, gameTime);              
        }

        private void OnCollision() {

        }  
        
        private void PlayerMove(GameTime gameTime) {
            body.Magnitude = Directions.Zero;

            if (Input.IsKeyDown(Keys.W)) {
                body.Magnitude += Directions.North;
                m_Facing = Animation.Direction.NORTH;
            }

            if (Input.IsKeyDown(Keys.A)) {
                body.Magnitude += Directions.West;
                m_Facing = Animation.Direction.WEST;
            }

            if (Input.IsKeyDown(Keys.S)) {
                body.Magnitude += Directions.South;
                m_Facing = Animation.Direction.SOUTH;
            }

            if (Input.IsKeyDown(Keys.D)) {
                body.Magnitude += Directions.East;
                m_Facing = Animation.Direction.EAST;
            }            

            if (body.Magnitude != Directions.Zero) {
                AnimationController.Play("Walk", m_Facing);
            } else {
                AnimationController.Play("Idle", m_Facing);
            }

           
            body.Magnitude.Normalize(); //Directions.Normalize(body.Magnitude);
            body.Magnitude = Directions.Normalize(body.Magnitude);
            body.Velocity = body.Magnitude * body.Speed * Globals.Delta;

            m_PrevPosition = Position;
            m_NextPosition = Position += body.Velocity;
            
            m_DoorCollider.Location = DoorColliderLocation;
            //OnCollision();

            Position = body.Move();
        }

        public override void OnCollisionEvent(ICollidable other) {
            Logger.Log("Player is Colliding with {0}", other.Tag);

            base.OnCollisionEvent(other);
        }
    }
}
