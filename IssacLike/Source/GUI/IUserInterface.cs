using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaLike.Source.GUI {
    public interface IUserInterface {
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch);
    }
}
