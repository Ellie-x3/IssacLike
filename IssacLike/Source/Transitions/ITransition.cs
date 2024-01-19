using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaLike.Source.Transitions {
    public interface ITransition : IDisposable {
        void Draw(SpriteBatch batch);
        void Update(GameTime gameTime);
        bool IsFinished { get; set; }
    }
}
