using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDtk;
using LDtk.Renderer;
using LDtkTypes.TestHome;
using ProjectMystic.Source.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectMystic.Source.ZeldaLikeImGui;
using Microsoft.Xna.Framework;
using ProjectMystic.Source.Entities.Player;
using Microsoft.Xna.Framework.Content;

namespace ProjectMystic.Source.Managers.Resources {
    public class LevelLoader : ResourceLoader {
        private static LDtkWorld m_World;
        public static LDtkRenderer m_Renderer;

        private static string m_LevelDirectory = "D:\\MonoGame\\ZeldaLike\\ZeldaLike\\Assets\\Levels\\ZinkLevels\\";
        private static List<LDtkLevel> Levels = new List<LDtkLevel>();
        public static LDtkLevel CurrentLevel;

        public static List<Rectangle> LevelCollisionTiles = new List<Rectangle>(); 

        private static Texture2D m_BoundBox;

        public static LDtkWorld World { get => m_World; }

        private static bool m_DrawLevelCollision = false;
        public static bool s_DrawLevelCollision { get { return m_DrawLevelCollision; } set { m_DrawLevelCollision = value; } }

        public static void Init(SpriteBatch batch, ContentManager content) {
            m_Renderer = new LDtkRenderer(batch, content);
            LDtkFile file = LDtkFile.FromFile("Worlds/TestHome", content);
            m_World = file.LoadWorld(Worlds.World.Iid);
           
            m_BoundBox = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_BoundBox.SetData(new Color[] { new Color(173,216,230,32) });

            foreach (LDtkLevel level in m_World.Levels) {
                m_Renderer.PrerenderLevel(level);
                Levels.Add(level);
            
                LevelCollisions(level);   
            }

            CurrentLevel = Levels[0];
        }

        //Create a renderer wrapper to render the room the player is in
        public static void Draw(SpriteBatch batch) {

            m_Renderer.RenderPrerenderedLevel(m_World.Levels.First());

            if (Debug.DrawDebug)
                DrawLevelCollision(m_World.Levels.First(), batch);
        }

        public static void Update(GameTime gameTime) { 
            if(Input.IsKeyPressed(Keys.F1))
                m_DrawLevelCollision = !m_DrawLevelCollision;    
        }

        public static void LoadWorld(LDtkWorld world) { }

        public static void DrawLevelCollision(LDtkLevel level, SpriteBatch batch) {
            LDtkIntGrid collisions = level.GetIntGrid("IntGrid");

            Point topLeftGrid = collisions.FromWorldToGridSpace(new Vector2(0, 0));
            Point bottomRightGrid = collisions.FromWorldToGridSpace(new Vector2(1280, 720));

            for (int x = topLeftGrid.X; x < bottomRightGrid.X; x++) {
                for (int y = topLeftGrid.Y; y < bottomRightGrid.Y; y++) {
                    long intGridValue = collisions.GetValueAt(x, y);
                    if (intGridValue == 1) {
                        Vector2 tilePosition = level.Position.ToVector2() + new Vector2(x * collisions.TileSize, y * collisions.TileSize);
                        Vector2 tileSize = new Vector2(collisions.TileSize);
                        batch.Draw(m_BoundBox, new Rectangle((int)tilePosition.X, (int)tilePosition.Y, (int)tileSize.X, (int)tileSize.Y), Color.White);
                    }
                }
            }
        }

        public static void LevelCollisions(LDtkLevel level) {
            LDtkIntGrid collisions = level.GetIntGrid("IntGrid");

            Point topLeftGrid = collisions.FromWorldToGridSpace(new Vector2(0, 0));
            Point bottomRightGrid = collisions.FromWorldToGridSpace(new Vector2(1280, 720));

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

        public static List<Rectangle> GetTileData(LDtkLevel level, long tile, string enumName, Vector2 levelSize) {
            List<Rectangle> tiles = new List<Rectangle>();
            
            LDtkIntGrid data = level.GetIntGrid(enumName);

            Point topLeftGrid = data.FromWorldToGridSpace(new Vector2(0,0));
            Point bottomRightGrid = data.FromWorldToGridSpace(levelSize);

            for (int x = topLeftGrid.X; x < bottomRightGrid.X; x++) {
                for (int y = topLeftGrid.Y; y < bottomRightGrid.Y; y++) {
                    long intGridValue = data.GetValueAt(x, y);
                    if (intGridValue == tile) {
                        Vector2 tilePosition = level.Position.ToVector2() + new Vector2(x * data.TileSize, y * data.TileSize);
                        Vector2 tileSize = new Vector2(data.TileSize);
                        tiles.Add(new Rectangle((int)tilePosition.X, (int)tilePosition.Y, (int)tileSize.X, (int)tileSize.Y));
                    }
                }
            }

            return tiles;
        }
        
        public static PlayerEnt GetEntityData(){
            PlayerEnt data = m_World.GetEntity<PlayerEnt>();
            //Logger.Log(data.MyNumber);
            return data;
        }
    }
}
