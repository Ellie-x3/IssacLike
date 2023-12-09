﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Util {
    public class Camera {
        private readonly GraphicsDevice graphicsDevice;

        public Vector2 Position { get; set; }
        public float Zoom { get; set; }
        public Matrix Transform { get; private set; }

        public Camera(GraphicsDevice graphicsDevice) {
            Transform = new();
            this.graphicsDevice = graphicsDevice;
        }

        public void Update() => Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f, 0);
    }
}