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
using ProjectMystic.Source.Entities;

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
                    if (intGridValue == 2) {
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
                    if (intGridValue is 2) {
                        Vector2 tilePosition = level.Position.ToVector2() + new Vector2(x * collisions.TileSize, y * collisions.TileSize);
                        Vector2 tileSize = new Vector2(collisions.TileSize);
                        LevelCollisionTiles.Add(new Rectangle((int)tilePosition.X, (int)tilePosition.Y, (int)tileSize.X, (int)tileSize.Y));
                    }
                }
            }
        }

        /*public static T GetLevelEntities<T>() where T : ILDtkEntity, new() {
            T[] entities = CurrentLevel.GetEntities<T>();
        }*/

        public static void SpawnEntitiesInLevel<T>() where T : ILDtkEntity, new() { 
            foreach (T entity in CurrentLevel.GetEntities<T>()) {
                Entity child = CreateGameEntityFactory(entity);
                EntityManager.Add(child);
            }
        }

        private static Entity CreateGameEntityFactory(ILDtkEntity entity) {
            switch (entity) {
                case PlayerEnt playerEntity:
                    return new Player(playerEntity);
                case DoorEnt doorEntity:
                    var size = doorEntity.Size;
                    var Position = doorEntity.Position;
                    Door door = new Door(new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y), doorEntity);
                    return door;

                default:
                    throw new ArgumentException("Unsupported entity type: " + entity.GetType());
            }
        }
    }
}
