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
using System.Reflection;
using System.Text.Json;

namespace ProjectMystic.Source.Managers.Resources {
    public class LevelLoader : ResourceLoader {

        public static List<Rectangle> LevelCollisionTiles = new List<Rectangle>();
        public static LDtkRenderer m_Renderer { get; private set; }
        public static LDtkLevel CurrentLevel { get; set; }
        public static LDtkLevel NextLevel { get; private set; }
        public static LDtkWorld World { get => m_World; }
        public static Dictionary<Guid, LDtkLevel> WorldLevels = new Dictionary<Guid, LDtkLevel>();
        public static bool s_DrawLevelCollision { get { return m_DrawLevelCollision; } set { m_DrawLevelCollision = value; } }
        public static Dictionary<Guid, Guid> ReferencedEntityLevel = new Dictionary<Guid, Guid>();

        private static List<LDtkLevel> Levels = new List<LDtkLevel>();
        private static List<Entity> LDtkLevelEntities = new List<Entity>();

        private static Texture2D m_BoundBox;
        private static LDtkWorld m_World;
        private static bool m_DrawLevelCollision = false;
        

        public static void Init(SpriteBatch batch, ContentManager content) {
            m_Renderer = new LDtkRenderer(batch, content);
            LDtkFile file = LDtkFile.FromFile("Worlds/TestHome", content);
            m_World = file.LoadWorld(Worlds.World.Iid);
           
            m_BoundBox = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_BoundBox.SetData(new Color[] { new Color(173,216,230,32) });

            foreach (LDtkLevel level in m_World.Levels) {
                m_Renderer.PrerenderLevel(level);
                Levels.Add(level);
                WorldLevels.Add(level.Iid, level);
            }
           
            CurrentLevel = Levels[0];
            LevelCollisions(CurrentLevel);
            NextLevel = Levels[1];
        }

        //Create a renderer wrapper to render the room the player is in
        public static void Draw(SpriteBatch batch) {
            m_Renderer.RenderPrerenderedLevel(CurrentLevel);

            if (Debug.DrawDebug)
                DrawLevelCollision(CurrentLevel, batch);
        }

        public static void Update(GameTime gameTime) { 
            if(Input.IsKeyPressed(Keys.F1))
                m_DrawLevelCollision = !m_DrawLevelCollision;    
        }

        public static void LoadWorld(LDtkWorld world) { 
            
        }

        public static void ChangeLevel(LDtkLevel level) {       
            CurrentLevel = level;            
            UnLoadLevel();
            LevelCollisions(CurrentLevel);
            SpawnEntitiesInLevel<DoorEnt>();
        }

        private static void UnLoadLevel() {
            List<Rectangle> tilesToRemove = new List<Rectangle>(LevelCollisionTiles);
            foreach (var tile in tilesToRemove) {
                LevelCollisionTiles.Remove(tile);
            }
            DespawnEntitiesFromLevel();
        }

        public static void DrawLevelCollision(LDtkLevel level, SpriteBatch batch) {
            LDtkIntGrid collisions = level.GetIntGrid("IntGrid");

            Vector2 levelTopLeft = Vector2.Zero;
            Vector2 levelBottomRight = new Vector2(level.Size.X, level.Size.Y);

            Point topLeftGrid = collisions.FromWorldToGridSpace(levelTopLeft);
            Point bottomRightGrid = collisions.FromWorldToGridSpace(levelBottomRight);

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

            Logger.Log("ADDING: {0} LEVEL COLLISIONS!!", level.Identifier);

            Vector2 levelTopLeft = Vector2.Zero;
            Vector2 levelBottomRight = new Vector2(level.Size.X, level.Size.Y);

            Point topLeftGrid = collisions.FromWorldToGridSpace(levelTopLeft);
            Point bottomRightGrid = collisions.FromWorldToGridSpace(levelBottomRight);

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

        public static List<Rectangle> GetLevelIntGridTile(LDtkLevel level, long type) {
            LDtkIntGrid intGrid = level.GetIntGrid("IntGrid");

            List<Rectangle> tiles = new List<Rectangle>();

            Vector2 levelTopLeft = Vector2.Zero;
            Vector2 levelBottomRight = new Vector2(level.Size.X, level.Size.Y);

            Point topLeftGrid = intGrid.FromWorldToGridSpace(levelTopLeft);
            Point bottomRightGrid = intGrid.FromWorldToGridSpace(levelBottomRight);

            for (int x = topLeftGrid.X; x < bottomRightGrid.X; x++) {
                for (int y = topLeftGrid.Y; y < bottomRightGrid.Y; y++) {
                    long intGridValue = intGrid.GetValueAt(x, y);
                    if (intGridValue == type) {
                        Vector2 tilePosition = level.Position.ToVector2() + new Vector2(x * intGrid.TileSize, y * intGrid.TileSize);
                        Vector2 tileSize = new Vector2(intGrid.TileSize);
                        tiles.Add(new Rectangle((int)tilePosition.X, (int)tilePosition.Y, (int)tileSize.X, (int)tileSize.Y));
                    }
                }
            }

            return tiles;
        }

        public static void SpawnEntitiesInLevel<T>() where T : ILDtkEntity, new() {
            foreach (T entity in CurrentLevel.GetEntities<T>()) {
                Logger.Log("CCC");
                Entity child = CreateGameEntityFactory(entity);
                LDtkLevelEntities.Add(child);
                EntityManager.Add(child);
            }
        }

        private static void DespawnEntitiesFromLevel() {
            List<Entity> entitiesToRemove = new List<Entity>(LDtkLevelEntities);

            foreach (Entity ent in entitiesToRemove) {
                if(ent is Player)
                    continue;

                LDtkLevelEntities.Remove(ent);
                EntityManager.Remove(ent);
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

        public static LDtkLevel EntityLevel(string indentifier, string field, Guid entity) {
            foreach (LayerInstance layer in CurrentLevel.LayerInstances) {
                foreach (EntityInstance ent in layer.EntityInstances) {
                    if (ent._Identifier == indentifier) {
                        foreach (FieldInstance _field in ent.FieldInstances) {
                            if (_field._Identifier == field) {
                                EntityRef _ent = JsonSerializer.Deserialize<EntityRef>(_field._Value.ToString());
                                if(ent.Iid == entity)
                                    return WorldLevels[_ent.LevelIid];
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
