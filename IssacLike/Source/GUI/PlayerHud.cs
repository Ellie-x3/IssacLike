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
        private Rectangle WeaponHud;
        private static Vector2 WeaponHudPosition;
        private static Vector2 WeaponHudSize = new Vector2(22);
        private static int WeaponHudPadding = 1;

        //Weapon in circle
        private static Texture2D WeaponHudItemTexture;
        private static Rectangle WeaponHudItem;

        public void LoadContent() {
            TextureLoader.AddTexture("WeaponSelected", "Gui/HUDItem");
            TextureLoader.AddTexture("Sword", "Items/StaticSword");
            WeaponHudTexture = TextureLoader.Texture("WeaponSelected");
            WeaponHudPosition = new Vector2((int)CameraManager.CurrentCamera.Position.X - (int)Globals.ScreenSize.X / 4, (int)CameraManager.CurrentCamera.Position.Y - (int)Globals.ScreenSize.Y / 4);

            WeaponHud = new Rectangle((int)WeaponHudPosition.X + WeaponHudPadding, (int)WeaponHudPosition.Y + WeaponHudPadding, (int)WeaponHudSize.X, (int)WeaponHudSize.Y);
        }

        public void Update(GameTime gameTime) {
            WeaponHudPosition = new Vector2((int)CameraManager.CurrentCamera.Position.X - (int)Globals.ScreenSize.X / 4, (int)CameraManager.CurrentCamera.Position.Y - (int)Globals.ScreenSize.Y / 4);
            WeaponHud = new Rectangle((int)WeaponHudPosition.X + WeaponHudPadding, (int)WeaponHudPosition.Y + WeaponHudPadding, (int)WeaponHudSize.X, (int)WeaponHudSize.Y);

            if (WeaponHudItemTexture != null)
                WeaponHudItem = new Rectangle((int)WeaponHudPosition.X + WeaponHudPadding, (int)WeaponHudPosition.Y + WeaponHudPadding, (int)WeaponHudSize.X, (int)WeaponHudSize.Y);
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(WeaponHudTexture, WeaponHud, Color.White); //Draw weapon slot

            if(WeaponHudItemTexture != null) {
                batch.Draw(WeaponHudItemTexture, WeaponHudItem, Color.White);
            }
        }

        public static void ChangeWeaponSelected(Texture2D item) {
            WeaponHudItemTexture = item;

            WeaponHudItem = new Rectangle((int)WeaponHudPosition.X + (int)WeaponHudPadding, (int)WeaponHudPosition.Y + WeaponHudPadding + 1, (int)WeaponHudSize.X, (int)WeaponHudSize.Y);
        }

    }
}
