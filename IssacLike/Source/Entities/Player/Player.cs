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
using ZeldaLike.Source.Managers.Events;
using System.Reflection.Metadata;

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
        private Collider m_WallCollider;

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

        private bool m_CollidedWithDoor = false;

        private Rectangle horizontalRect;
        private Rectangle verticalRect;

        private Vector2 m_PrevPosition;
        private Vector2 m_NextPosition;
        private Vector2? m_OvrridePosition;

        //DEBUG / TESTING
        public Vector2 PlayerPosition { get => Position; set {
                Position = value;
            }
        }

        private bool shadertesting = false;
        public Effect overlayEffect;
        Vector4 overlayColor = new Vector4(0.5f, 0.35f, 0.35f, 1f);

        //LDtk
        private PlayerEnt data;

        public Player(PlayerEnt data)
        {
            this.data = data;
            Name = "Player";
            Scale = new Vector2(1f);
            Origin = new Vector2(8f, 8f);
            TextureLoader.AddTexture("Walk", "Player/Zink_Walk");
            TextureLoader.AddTexture("Idle", "Player/Zink_Idle");

            overlayEffect = Globals.Content.Load<Effect>("Effects\\player");
            overlayEffect.Parameters["OverlayColor"].SetValue(overlayColor);
        }

    public override void Start()
        {
            var mediator = MediatorHandler.PlayerMediator;
            SetMediator(mediator);

            AnimationController = new Animation();

            AnimationController.Create("Walk", TextureLoader.Texture("Walk"), new Vector2(16,16), 2, 8, 0.17f);
            AnimationController.Create("Idle", TextureLoader.Texture("Idle"), new Vector2(16,16), 1, 4, 0f);            

            body = new CharacterBody
            {
                Position = new Vector2(data.Position.X, data.Position.Y),
                Speed = 60.0f,
                Velocity = new Vector2()
            };

            Position = body.Position;
            //Origin = Position;
            m_DoorCollider = new Collider(new Rectangle(DoorColliderLocation.X, DoorColliderLocation.Y, 16,16)) {
                CanCollide = true,
                Tag = "Player",
                //Color = new Color(140, 238, 100, 10)
            };

            m_WallCollider = new Collider(new Rectangle(DoorColliderLocation.X, DoorColliderLocation.Y, 16, 16)) {
                CanCollide = false,
                Tag = "Player",
                Color = new Color(255, 0, 100, 150)
            };

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerMove(gameTime);

            if (Input.IsKeyPressed(Keys.F10)) {
                m_CollidedWithDoor = false;
            }

            if (Input.IsKeyDown(Keys.F9)) {
                shadertesting = true;
                overlayColor = new Vector4(1f, 0.35f, 0.35f, 1f);
                overlayEffect.Parameters["OverlayColor"].SetValue(overlayColor);
            } else {
                shadertesting = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (shadertesting) {
                Draw(batch, gameTime, overlayEffect);
                Logger.Log("Trying to draw");
                return;
            }

            base.Draw(batch, gameTime);          
        }

        private Vector2? OnWallCollision(List<Rectangle> walls) {
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
            bool collidedHorizontally = walls.Any(wall => horizontalRect.Intersects(wall));
            bool collidedVertically = walls.Any(wall => verticalRect.Intersects(wall));

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

        private void PlayerMove(GameTime gameTime) {
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

            if (body.Magnitude != Directions.Zero) {
                AnimationController.Play("Walk", m_Facing);
            } else {
                AnimationController.Play("Idle", m_Facing);
            }

            body.Magnitude = Directions.Normalize(body.Magnitude);
            body.Velocity = body.Magnitude * body.Speed * Globals.Delta;
            body.Position = Position;

            m_PrevPosition = body.Position;
            m_NextPosition = body.Position + body.Velocity;

            m_OvrridePosition = OnWallCollision(LevelLoader.LevelCollisionTiles);

            Position = m_OvrridePosition ?? m_NextPosition;
            m_DoorCollider.Location = DoorColliderLocation;
            m_WallCollider.Location = DoorColliderLocation;
        }

        public override void OnCollisionEvent(ICollidable other) {
            if(!m_CollidedWithDoor) {
                InteractWith(other.Entity);
                
                m_CollidedWithDoor = true;
            }
        }

        public override void OnExitCollisionEvent() {
            m_CollidedWithDoor = false;
        }
    }
}
