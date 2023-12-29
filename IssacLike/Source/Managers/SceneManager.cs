using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ProjectMystic.Source.ZeldaLikeImGui;
using ProjectMystic.Source.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMystic.Source.Managers {
    public class SceneManager {

        private static IScene m_ActiveScene;

        public static IScene ActiveScene { get => m_ActiveScene; set => m_ActiveScene = value; }

        private static List<IScene> Scenes = new List<IScene>();

        public SceneManager(IScene scene) {
            ActiveScene = scene;
        }


        public static void AddSceneOnTop() {
            //Allow to draw another scene ontop and transparent over another scene (ex: death scene over game scene)
            throw new NotImplementedException();
        }

        public void RemoveScene(IScene scene) {
            if (!Scenes.Contains(scene)){
                Logger.Log("Scene {0} is already not an available scene", scene.Name);
            }

            Scenes.Remove(scene);
        }
    }
}
