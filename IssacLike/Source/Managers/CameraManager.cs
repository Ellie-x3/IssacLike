using Microsoft.Xna.Framework;
using ProjectMystic.Source.Util;
using ProjectMystic.Source.ZeldaLikeImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaLike.Source.Managers {

    internal static class CameraManager {

        private static Dictionary<string, Camera> m_Cameras = new Dictionary<string, Camera>();
        private static Camera m_CurrentCamera;

        public static Camera CurrentCamera { get => m_CurrentCamera; set => m_CurrentCamera = value; }
        public static Matrix CurrentCameraMatrices { get => m_CurrentCamera.Transform; }

        public static void CreateCamera(string name, Matrix? ScaleMatrix = null, Matrix? TransformMatrix = null, float zoom = 2f) {

            var cam = new Camera(Globals.s_GraphicsDevice);

            if (m_Cameras.ContainsKey(name)) {
                throw new Exception($"Camera {name} already exist, dispose camera or create a new one");
            }

            cam.Zoom = zoom;

            CurrentCamera = cam;

            m_Cameras.Add(name, cam);
        }

        public static void Update() {
            m_CurrentCamera.Update();
        }

        public static void RemoveCamera(string name) {
            if (m_Cameras.ContainsKey(name)) {
                var cam = m_Cameras[name];                
                m_Cameras.Remove(name);
                cam.Dispose();
                Logger.Log("Cam removed: {0}", name);
            }
        }
    }
}
