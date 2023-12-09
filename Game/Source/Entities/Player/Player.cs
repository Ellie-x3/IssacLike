using IssacLike.Source.Components;
using IssacLike.Source.Managers;
using IssacLike.Source.Managers.Resources;
using IssacLike.Source.RogueLikeImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssacLike.Source.Util;
using LDtk;

namespace IssacLike.Source.Entities.Player
{
    internal class Player : Entity {
        //Componenets

        [Component(typeof(CharacterBody))]
        private CharacterBody body;
     
        [Component(typeof(Animation))]
        private Animation AnimationController;

        [Component(typeof(Collider))]
        private Collider Collider;

        private Animation.Direction m_Facing = 0;

        public Player()
        {
            name = "Player";
            Scale = new Vector2(1f);
            Origin = new Vector2(1f);
            TextureLoader.AddTexture("Walk", "Player/Player_Walk");
            TextureLoader.AddTexture("Idle", "Player/Player_Idle");
            TextureLoader.AddTexture("Static", "Player/mcfront");
        }

        public override void Start()
        {
           
            AnimationController = new Animation();
         
            AnimationController.Create("Walk", TextureLoader.Texture("Walk"), new Vector2(64,64), 4, 16, 0.17f);
            AnimationController.Create("Idle", TextureLoader.Texture("Idle"), new Vector2(64,64), 1, 4, 0f);

            body = new CharacterBody();
            body.Position = new Vector2(150, 70);
            body.Speed = 175.0f;
            body.Velocity = new Vector2();

            Position = body.Position;

            Collider = new Collider(new Rectangle((int)Position.X, (int)Position.Y, 64, 64));

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerMove(gameTime);
            Collider.Location = new Point((int)Position.X, (int)Position.Y);
            //Logger.Log(m_Bound.Location);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {

            //batch.Draw(m_BoundBox, Bound, Color.White);
            
            base.Draw(batch, gameTime);  
            
        }

        private void OnCollision(List<Rectangle> tiles) {
            foreach(Rectangle tile in tiles) {
                if (Collider.Bound.Intersects(tile)) {
                    Logger.Log("Player is hitting level collision!!!");
                }
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

           
            body.Magnitude = Directions.Normalize(body.Magnitude);
            body.Velocity = body.Magnitude * body.Speed * Globals.Delta;
            OnCollision(LevelLoader.LevelCollisionTiles);

            Position = body.Move();
        }
    }
}
