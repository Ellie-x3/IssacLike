﻿using ProjectMystic.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace ProjectMystic.Source.Scene {
    public interface IScene {
        string Name { get; }

        void Initialize(SpriteBatch batch, ContentManager content);
        void SceneContent(SpriteBatch batch, ContentManager content);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch, GameTime gameTime);
    }
}
