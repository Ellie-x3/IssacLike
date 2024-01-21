using ProjectMystic.Source.Components;
using ProjectMystic.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using LDtkTypes.TestHome;
using ZeldaLike.Source.Managers.Events;
using ZeldaLike.Source.Entities.StateMachine;
using ZeldaLike.Source.Entities.Player.States;
using ZeldaLike.Source.Entities.Player;
using ZeldaLike.Source.Managers;
using ProjectMystic.Source.Managers.Events;

namespace ProjectMystic.Source.Entities.Player
{
    public class APlayer : Entity {

        public Animation.Direction Facing { get => m_Facing; set => m_Facing = value; }
        public Vector2 HalfSprite { get => m_SpriteHalf; }
        public Point Collider { get => m_DoorColliderPoint; }
        public Vector2 Velocity { get => body.Velocity; }
        public Vector2 BodyPosition { get => Position; set => Position = value; }
        public Vector2 NextBodyPosition { get => m_NextPosition; set => m_NextPosition = value; }
        public Vector2 PlayerTransitionPosition { get; set; }
        public Vector2? BodyOverridePosition { get => m_OvrridePosition; set => m_OvrridePosition = value; }
        public StateMachine StateMachine { get => m_StateMatchine; }
        public Inventory Inventory { get => m_Inventory; }
        public bool OverrideDraw = false;

        public CharacterBody Body { get => body; }
        public Animation Animation { get => AnimationController; }
        public Collider PlayerCollider { get => m_WallCollider; }

        public Vector2 PlayerMoveInput { get {
                body.Magnitude = Directions.Zero;

                if (Input.IsKeyDown(Keys.W)) {
                    body.Magnitude += Directions.North;
                    m_Facing = Animation.Direction.NORTH;
                }

                if (Input.IsKeyDown(Keys.S)) {
                    body.Magnitude += Directions.South;
                    m_Facing = Animation.Direction.SOUTH;
                }

                if (Input.IsKeyDown(Keys.A)) {
                    body.Magnitude += Directions.West;
                    m_Facing = Animation.Direction.WEST;
                }

                if (Input.IsKeyDown(Keys.D)) {
                    body.Magnitude += Directions.East;
                    m_Facing = Animation.Direction.EAST;
                }

                return body.Magnitude;
            }
        }

        #region Components
        //Components
        [Component(typeof(CharacterBody))]
        private CharacterBody body;

        [Component(typeof(Animation))]
        private Animation AnimationController;

        [Component(typeof(Collider))]
        private Collider m_WallCollider;

        #endregion

        //Animation
        private Animation.Direction m_Facing = 0;
        private readonly Vector2 m_SpriteHalf = new Vector2(8, 8);

        //Collision
        private Point m_DoorColliderPoint;
        public Point ColliderLocation {
            get {
                m_DoorColliderPoint = new Point((int)(Position.X - m_SpriteHalf.X), (int)(Position.Y - m_SpriteHalf.Y));
                return m_DoorColliderPoint;
            }
        }

        private bool m_CollidedWithDoor = false;

        private Rectangle horizontalRect;
        private Rectangle verticalRect;

        private Vector2 m_NextPosition;
        private Vector2? m_OvrridePosition;

        //DEBUG / TESTING
        public Vector2 PlayerPosition { get => Position; set => Position = value; }

        private StateMachine m_StateMatchine;
        private Inventory m_Inventory;
        private bool m_ShowInventory = false;

        //LDtk
        private PlayerEnt data;

        public APlayer(PlayerEnt data)
        {         
            this.data = data;
            Name = "Player";
            Scale = new Vector2(1f);
            Origin = new Vector2(8f, 8f);
            m_Inventory = new Inventory();  

            SubscribeToGameEvents();            
        }

        public override void Start()
        {
            var mediator = MediatorHandler.PlayerMediator;
            SetMediator(mediator);

            AnimationController = new Animation();                        

            body = new CharacterBody
            {
                Position = new Vector2(data.Position.X, data.Position.Y),
                Speed = 60.0f,
                Velocity = new Vector2()
            };

            Position = body.Position;

            m_WallCollider = new Collider(new Rectangle(ColliderLocation.X, ColliderLocation.Y, 16,16)) {
                CanCollide = true,
                Tag = "Player",
                Color = new Color(255, 0, 0, 125)
            };

            m_StateMatchine = new StateMachine();
            StateFactory<Idle> idleFactory = owner => new Idle(this);
            StateFactory<Walk> walkFactory = owner => new Walk(this);
            StateFactory<Collecting> collectFactory = owner => new Collecting(this);
            m_StateMatchine.RegisterState("Idle", idleFactory, this);
            m_StateMatchine.RegisterState("Walk", walkFactory, this);
            m_StateMatchine.RegisterState("Collecting", collectFactory, this);
            m_StateMatchine.Transition("Idle");

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            m_StateMatchine.Update();

            if (Inventory.SignificantItemCollected) {
                m_StateMatchine.Transition("Collecting");
            }

            if (Input.IsKeyPressed(Keys.F6)) {
                
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime) {            
            base.Draw(batch, gameTime);          
        }

        public Vector2? OnWallCollision(List<Rectangle> walls) {

            List<Rectangle> wallsCollection = new List<Rectangle>(walls);

            // Create two rectangles for potential new positions along X and Y axes
            horizontalRect = new Rectangle(
                (int)(m_NextPosition.X - m_SpriteHalf.X),
                (int)(body.Position.Y - m_SpriteHalf.Y),
                m_WallCollider.Bound.Width,
                m_WallCollider.Bound.Height
            );

            verticalRect = new Rectangle(
                (int)(body.Position.X - m_SpriteHalf.X),
                (int)(m_NextPosition.Y - m_SpriteHalf.Y),
                m_WallCollider.Bound.Width,
                m_WallCollider.Bound.Height
            );

            Vector2 adjustedPosition = body.Position;

            // Check for horizontal and vertical collisions separately
            bool collidedHorizontally = wallsCollection.Any(wall => horizontalRect.Intersects(wall));
            bool collidedVertically = wallsCollection.Any(wall => verticalRect.Intersects(wall));

            if (!collidedHorizontally) {
                // Allow horizontal movement
                adjustedPosition.X = m_NextPosition.X;
            }

            if (!collidedVertically) {
                // Allow vertical movement
                adjustedPosition.Y = m_NextPosition.Y;
            }

            // If there was a collision, return the adjusted position
            if (collidedHorizontally || collidedVertically) {
                return adjustedPosition;
            }

            return null;
        }    

        public override void OnCollisionEvent(ICollidable other) {
            
            

            if(!m_CollidedWithDoor && other.Entity is Door) {
                InteractWith(other.Entity);             
                m_CollidedWithDoor = true;
            } else if (other.Entity is not Door){
                InteractWith(other.Entity);
            }
        }

        public override void OnExitCollisionEvent() {
            m_CollidedWithDoor = false;
        }
        
        private void SubscribeToGameEvents(){
            EventManager.E_TransitionDuring += UpdatePositionOnTransition;
        }

        private void UpdatePositionOnTransition(){
            Position = PlayerTransitionPosition;
        }

    }
}
