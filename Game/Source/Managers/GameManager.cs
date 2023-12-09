using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Managers {
    internal static class GameManager {

        public static void Start(){

        }

        public static void LoadContent(SpriteBatch batch){
            SceneManager.ActiveScene.SceneContent(batch);
        }

        public static void Update(GameTime gameTime) {
            

            if (Input.IsKeyPressed(Keys.F2)) {

                Logger.Log("Key Pressed");

                switch(SceneManager.ActiveScene.name){
                    case "GameScene":
                        Logger.Log("Changing to MainMenu Scene");
                        SceneManager.ActiveScene = MainMenuScene.Instance;
                        break;
                    case "MainMenuScene":
                        Logger.Log("Changing to Game Scene");
                        SceneManager.ActiveScene = GameScene.Instance;
                        break;
                    default:
                        break;
                }
            }

            SceneManager.ActiveScene.Update(gameTime);
        }

        public static void Draw(SpriteBatch batch, GameTime gameTime){
            SceneManager.ActiveScene.Draw(batch, gameTime);
        }
    }
}
