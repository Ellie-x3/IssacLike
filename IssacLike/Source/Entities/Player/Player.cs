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

        [Component(typeof(Collider))]
        private Collider m_ScreenCollider;
        #endregion

        //Animation
        private Animation.Direction m_Facing = 0;
        private readonly Vector2 m_SpriteHalf = new Vector2(32, 32);

        //Collision
        private Point m_DoorColliderPoint;
        private Point DoorColliderLocation {
            get {
                m_DoorColliderPoint = new Point((int)(Position.X - m_SpriteHalf.X / 2), (int)(Position.Y - m_SpriteHalf.Y / 2));
                return m_DoorColliderPoint;
            }
        }
        private Point m_ScreenColliderPoint;
        private Point ScreenColliderLocation {
            get {
                m_ScreenColliderPoint = new Point((int)(Position.X - m_SpriteHalf.X / 2), (int)(Position.Y - m_SpriteHalf.Y));
                return m_ScreenColliderPoint;
            }
        }

        private Vector2 m_PrevPosition;
        private Vector2 m_NextPosition;

        //DEBUG / TESTING
        public Vector2 PlayerPosition => Position;


        public Player()
        {
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
                Position = new Vector2(10,10),
                Speed = 60.0f,
                Velocity = new Vector2()
            };

            Position = body.Position;
            //Origin = Position;
            m_DoorCollider = new Collider(new Rectangle(DoorColliderLocation.X, DoorColliderLocation.Y, 32,32)) { 
                CanCollide = true,
                Tag = "Player"
            };
            m_ScreenCollider = new Collider(new Rectangle(ScreenColliderLocation.X, ScreenColliderLocation.Y, 32,64)) { 
                CanCollide = false, 
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
            var roomWidth = Globals.RoomSize.X;
            var roomHeight = Globals.RoomSize.Y;

            var playerNextPosition = m_NextPosition;

            var playerScreenColliderX = m_ScreenCollider.Bound.Width / 2;
            var playerScreenColliderY = m_ScreenCollider.Bound.Height / 2;

            if (playerNextPosition.X + playerScreenColliderX >= roomWidth || playerNextPosition.X - playerScreenColliderX <= 0) { //WILL HAVE TO BE CHANGED FOR ROOM POS - WOOM WIDTH
                body.Position = m_PrevPosition;
                body.Velocity = Vector2.Zero;
            }

            if(playerNextPosition.Y + playerScreenColliderY >= roomHeight || playerNextPosition.Y - playerScreenColliderY <= 0) {
                body.Position = m_PrevPosition;
                body.Velocity = Vector2.Zero;
            }
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
            m_ScreenCollider.Location = ScreenColliderLocation;
            //OnCollision();

            Position = body.Move();
        }

        public override void OnCollisionEvent(ICollidable other) {
            Logger.Log("Player is Colliding with {0}", other.Tag);

            base.OnCollisionEvent(other);
        }
    }
}
