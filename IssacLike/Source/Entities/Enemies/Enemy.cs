using ProjectMystic.Source.Components;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.ZeldaLikeImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMystic.Source.Entities.Enemies {
    public class Enemy : Entity {

        [Component(typeof(CharacterBody))]
        public CharacterBody body;      

        public Enemy() { 
            body = new CharacterBody(); 
        }

        public override void Start() {
            base.Start();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime) {
            base.Draw(batch, gameTime);
        }
    }
}
