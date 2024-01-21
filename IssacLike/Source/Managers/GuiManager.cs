using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMystic.Source.Managers.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.GUI;

namespace ZeldaLike.Source.Managers {
    public static class GuiManager {
        public static Dictionary<string, IUserInterface> Guis = new Dictionary<string, IUserInterface>();
        public static IUserInterface? SetHud {
            get => ActiveHud; 
            set {
                ActiveHud = value;
            }
        }

        private static IUserInterface? ActiveGui;
        private static IUserInterface? ActiveHud;   

        public static void LoadContent() {
            foreach(IUserInterface gui in Guis.Values) {
                gui.LoadContent();
            }
        }

        public static void Add(string name, IUserInterface gui) {
            if (!Guis.ContainsKey(name)) {
                Guis.Add(name, gui);
            }
        }

        public static void Update(GameTime gameTime) {
            ActiveGui?.Update(gameTime);
            ActiveHud?.Update(gameTime);
        }

        public static void Draw(SpriteBatch batch) {
            ActiveGui?.Draw(batch);
            ActiveHud?.Draw(batch);
        }

        public static void SetActiveGui(IUserInterface gui) {
            ActiveGui = gui;
        }

        public static void DisableGui() {
            ActiveGui = null;
        }
    }
}
