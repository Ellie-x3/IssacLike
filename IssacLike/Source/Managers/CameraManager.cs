using Microsoft.Xna.Framework;
using ProjectMystic.Source.Entities;
using ProjectMystic.Source.Managers.Resources;
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

        public static void CreateCamera(string name, Entity target, Matrix? ScaleMatrix = null, Matrix? TransformMatrix = null, float zoom = 2f) {

            var cam = new Camera(Globals.s_GraphicsDevice, target);

            if (m_Cameras.ContainsKey(name)) {
                throw new Exception($"Camera {name} already exist, dispose camera or create a new one");
            }

            cam.Zoom = zoom;

            CurrentCamera = cam;

            m_Cameras.Add(name, cam);
        }

        public static Vector2 GetCameraLevelBounds() {
            List<Rectangle> tiles = LevelLoader.GetLevelIntGridTile(LevelLoader.CurrentLevel, 1);

            Rectangle highestXValue = tiles.Aggregate((t1, t2) => t1.X > t2.X ? t1 : t2);      
            Rectangle lowestXValue = tiles.Aggregate((t1, t2) => t1.X < t2.X ? t1 : t2);
            
            Rectangle highestYValue = tiles.Aggregate((t1, t2) => t1.Y > t2.Y ? t1 : t2);      
            Rectangle lowestYValue = tiles.Aggregate((t1, t2) => t1.Y < t2.Y ? t1 : t2);      

            return new Vector2(highestXValue.X + Globals.TileSize.X - lowestXValue.X, highestYValue.Y + Globals.TileSize.Y - lowestYValue.Y);
        }

        public static void Update() {
            m_CurrentCamera.Update();
            ChangeCameraPosition();
        }

        public static void ChangeCameraToLevel() {
            m_CurrentCamera.Position = m_CurrentCamera.TargetPosition;
        }

        public static void ChangeCameraPosition() {

            m_CurrentCamera.Position = m_CurrentCamera.TargetPosition;

            var newPosition = new Vector2(0, CurrentCamera.Position.Y);
            float cameraXbounds = GetCameraLevelBounds().X;
            float cameraYbounds = GetCameraLevelBounds().Y;            

            if(cameraXbounds <= Globals.s_GraphicsDevice.Viewport.Width / 4) {
                var newLevelWidth = LevelLoader.CurrentLevel.Size.X;
                var newLevelPosition = LevelLoader.CurrentLevel.Position.X;
                var screenWidth = Globals.s_GraphicsDevice.Viewport.Width / 4;

                var formula = (screenWidth - newLevelWidth) / 2;
                var adjustedXPos = screenWidth + newLevelPosition - formula;
                newPosition = new Vector2(adjustedXPos, newPosition.Y);
            } else {
                var newLevelPosition = LevelLoader.CurrentLevel.Position.X;
                var camMinWidth = Globals.s_GraphicsDevice.Viewport.Width / 4 + newLevelPosition;
                var width = newLevelPosition + Globals.s_GraphicsDevice.Viewport.Width / 4 * 0.5f;
                var camMaxWidth = cameraXbounds + newLevelPosition - Globals.s_GraphicsDevice.Viewport.Width / 4 * 0.5f;


                if (CurrentCamera.Position.X <= camMinWidth && CurrentCamera.TargetPosition.X <= width) {
                    newPosition = new Vector2(camMinWidth, newPosition.Y);
                } else if (CurrentCamera.Position.X >= camMaxWidth) {
                    newPosition = new Vector2(camMaxWidth + Globals.s_GraphicsDevice.Viewport.Width / 4 * 0.5f, newPosition.Y);
                }
                else {
                    newPosition = new Vector2(CurrentCamera.TargetPosition.X + (float)width - newLevelPosition, newPosition.Y);
                }           
            }

            if(cameraYbounds <= Globals.s_GraphicsDevice.Viewport.Height / 4) {
                var newLevelHeight = LevelLoader.CurrentLevel.Size.Y;
                var newLevelPosition = LevelLoader.CurrentLevel.Position.Y;
                var screenWidth = Globals.s_GraphicsDevice.Viewport.Height / 4;

                var formula = (screenWidth - newLevelHeight) / 2;               
                var adjustedYPos = screenWidth + newLevelPosition - formula;
                newPosition = new Vector2(newPosition.X, adjustedYPos);
            } else { 
                var camMaxHeight = 0 + Globals.s_GraphicsDevice.Viewport.Height / 4;
                var camMinHeight = cameraYbounds;

                var height = Globals.s_GraphicsDevice.Viewport.Height / 4 * 0.5;

                if (CurrentCamera.Position.Y <= camMaxHeight && CurrentCamera.TargetPosition.Y <= (float)height)
                    newPosition = new Vector2(newPosition.X, camMaxHeight);
                else if (CurrentCamera.Position.Y + (float)height >= camMinHeight)
                    newPosition = new Vector2(newPosition.X, camMinHeight);
                else {        
                    newPosition = new Vector2(newPosition.X, CurrentCamera.TargetPosition.Y + (float)height);
                }                    
            }

            m_CurrentCamera.Position = newPosition;
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
