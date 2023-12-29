using ProjectMystic.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMystic.Source.Scene {
    public interface IScene {
        string Name { get; }

        void AddEntityToScene(Entity entity);

        void SceneContent(SpriteBatch batch);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch, GameTime gameTime);
    }
}
