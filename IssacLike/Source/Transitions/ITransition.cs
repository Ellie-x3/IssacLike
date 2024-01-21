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
        void FirstDrawPass(SpriteBatch batch);
        void SecondDrawPass(SpriteBatch batch);
        
        void FirstPassUpdate(GameTime gameTime);
        void Update(GameTime gameTime);
        void SecondPassUpdate(GameTime gameTime);

        bool FirstPassIsFinished { get; }
        bool SecondPassIsFinished { get; }
    }
}
