using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDtk;
using LDtk.Renderer;

using IssacLike.Source.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IssacLike.Source.RogueLikeImGui;
using Microsoft.Xna.Framework;

namespace IssacLike.Source.Managers.Resources {
    internal class LevelLoader : ResourceLoader {
        private static LDtkWorld m_World;
        public static LDtkRenderer m_Renderer;

        private static string m_LevelDirectory = "D:\\MonoGame\\IssacLikeBulletHell\\IssacLike\\Assets\\Levels\\";
        private static List<LDtkLevel> Levels = new List<LDtkLevel>();
        public static LDtkLevel CurrentLevel;

        internal static List<Rectangle> LevelCollisionTiles = new List<Rectangle>(); 

        private static Texture2D m_BoundBox;

        private static bool m_DrawLevelCollision = false;
        public static bool s_DrawLevelCollision { get { return m_DrawLevelCollision; } set { m_DrawLevelCollision = value; } }

        internal static void Init(SpriteBatch batch) {
            LDtkFile file = LDtkFile.FromFile(string.Concat(m_LevelDirectory, "testlevel.ldtk"));
            m_World = file.LoadWorld(file.Worlds[0].Iid);

            m_Renderer = new LDtkRenderer(batch);

            m_BoundBox = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_BoundBox.SetData(new Color[] { new Color(173,216,230,32) });

            foreach (LDtkLevel level in m_World.Levels) {
                m_Renderer.PrerenderLevel(level);
                Levels.Add(level);
                LevelCollisions(level);   
                Logger.Log("");
            }

            CurrentLevel = Levels[0];
        }

        internal static void Draw(SpriteBatch batch) {
            foreach (LDtkLevel level in m_World.Levels) {
               
                m_Renderer.RenderPrerenderedLevel(level);
                if(Debug.DrawDebug)
                    DrawLevelCollision(level, batch);

            }
        }

        internal static void Update(GameTime gameTime) { 
            if(Input.IsKeyPressed(Keys.F1))
                m_DrawLevelCollision = !m_DrawLevelCollision;    
        }

        internal static void LoadWorld(LDtkWorld world) { }

        internal static void DrawLevelCollision(LDtkLevel level, SpriteBatch batch) {
            LDtkIntGrid collisions = level.GetIntGrid("Collision");

            Point topLeftGrid = collisions.FromWorldToGridSpace(new Vector2(0, 0));
            Point bottomRightGrid = collisions.FromWorldToGridSpace(new Vector2(640, 392));

            for (int x = topLeftGrid.X; x < bottomRightGrid.X; x++) {
                for (int y = topLeftGrid.Y; y < bottomRightGrid.Y; y++) {
                    long intGridValue = collisions.GetValueAt(x, y);
                    if (intGridValue is 1) {
                        Vector2 tilePosition = level.Position.ToVector2() + new Vector2(x * collisions.TileSize, y * collisions.TileSize);
                        Vector2 tileSize = new Vector2(collisions.TileSize);
                        batch.Draw(m_BoundBox, new Rectangle((int)tilePosition.X, (int)tilePosition.Y, (int)tileSize.X, (int)tileSize.Y), Color.White);
                    }
                }
            }
        }

        internal static void LevelCollisions(LDtkLevel level) {
            LDtkIntGrid collisions = level.GetIntGrid("Collision");

            Point topLeftGrid = collisions.FromWorldToGridSpace(new Vector2(0, 0));
            Point bottomRightGrid = collisions.FromWorldToGridSpace(new Vector2(640, 392));

            for (int x = topLeftGrid.X; x < bottomRightGrid.X; x++) {
                for (int y = topLeftGrid.Y; y < bottomRightGrid.Y; y++) {
                    long intGridValue = collisions.GetValueAt(x, y);
                    if (intGridValue is 1) {
                        Vector2 tilePosition = level.Position.ToVector2() + new Vector2(x * collisions.TileSize, y * collisions.TileSize);
                        Vector2 tileSize = new Vector2(collisions.TileSize);
                        LevelCollisionTiles.Add(new Rectangle((int)tilePosition.X, (int)tilePosition.Y, (int)tileSize.X, (int)tileSize.Y));
                    }
                }
            }
        }
    }
}
