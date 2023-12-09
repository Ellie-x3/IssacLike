using IssacLike.Source.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssacLike.Source.RogueLikeImGui;

namespace IssacLike.Source.Components {
    internal class Sprite : IDraw {
        internal Texture2D Texture { get; set; }

        public Entity Owner { get; set; }
        public string name { get; set; }
        public bool IsDrawable { get => m_Drawable; set => m_Drawable = value; }
        
        private bool m_Drawable = true;

        public void Draw(SpriteBatch batch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layer) {
            if(m_Drawable)
                batch.Draw(Texture, position, null, color, 0f, origin, scale, effects, layer);

        }

        public void Initialize() {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime) {
            
        }

        public Sprite (Texture2D texture) {
            Texture = texture;
        }
    }
}
