using Microsoft.Xna.Framework;
using ProjectMystic.Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaLike.Source.Managers {

    internal static class CameraManager {

        private static List<Camera> camera = new List<Camera>();
        private static Camera m_CurrentCamera;

        public static Camera CurrentCamera { get => m_CurrentCamera; set => m_CurrentCamera = value; }

        public static void Initialize() {

        }

        public static void CreateCamera(Matrix? ScaleMatrix = null, Matrix? TransformMatrix = null) {

            var cam = new Camera(Globals.s_GraphicsDevice);
            
        }

        public static void Update() {

        }
    }
}
