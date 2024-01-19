using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMystic.Source.Managers.Resources;
using ProjectMystic.Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Managers;

namespace ZeldaLike.Source.GUI {
    public class PlayerHud : IUserInterface {
        //Circle around weapon
        private Texture2D WeaponHudTexture;
        private Vector2 WeaponHud;
        private static Vector2 WeaponHudPosition;
        private static Vector2 WeaponHudSize = new Vector2(22);
        private static int WeaponHudPadding = 1;

        //Weapon in circle
        private static Texture2D WeaponHudItemTexture;
        private static Vector2 WeaponHudItem;

        public void LoadContent() {
            TextureLoader.AddTexture("WeaponSelected", "Gui/HUDItem");
            WeaponHudTexture = TextureLoader.Texture("WeaponSelected");
            WeaponHudPosition = new Vector2(CameraManager.CurrentCamera.Position.X - Globals.ScreenSize.X / 4, CameraManager.CurrentCamera.Position.Y - Globals.ScreenSize.Y / 4);

            WeaponHud = new Vector2(WeaponHudPosition.X + WeaponHudPadding, WeaponHudPosition.Y + WeaponHudPadding);
        }

        public void Update(GameTime gameTime) {
            WeaponHudPosition = new Vector2(CameraManager.CurrentCamera.Position.X - Globals.ScreenSize.X / 4, CameraManager.CurrentCamera.Position.Y - Globals.ScreenSize.Y / 4);
            WeaponHud = new Vector2(WeaponHudPosition.X + WeaponHudPadding, WeaponHudPosition.Y + WeaponHudPadding);

            if (WeaponHudItemTexture != null)
                WeaponHudItem = new Vector2(WeaponHudPosition.X + 8.5f, WeaponHudPosition.Y + 5);
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(WeaponHudTexture, WeaponHud, Color.White); //Draw weapon slot

            if(WeaponHudItemTexture != null) {
                batch.Draw(WeaponHudItemTexture, WeaponHudItem, Color.White);
            }
        }

        public static void ChangeWeaponSelected(Texture2D item) {
            WeaponHudItemTexture = item;

            WeaponHudItem = new Vector2(WeaponHudPosition.X + 8.5f, WeaponHudPosition.Y + 5);
        }

    }
}
