using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssacLike.Source.Entities;
using IssacLike.Source.Managers;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// ReSharper disable All

namespace IssacLike.Source.Rooms {
    public class Floor { //New floor
        //Create room
        //Manage room door
        //Manage room transition
        //Manage Camera transition
        //Keep track of floor state

        public List<Point> RoomsPositions { get => m_RoomPoints; }
        public List<Room> Rooms { get => m_AllRooms; }
        public List<Door> Doors = new List<Door>();

        private List<Room> m_AllRooms = new List<Room>();
        private List<Point> m_RoomPoints = new List<Point>();
        private List<Rectangle> m_DoorLocations = new List<Rectangle>();

        private Dictionary<Room, Point> m_Rooms = new Dictionary<Room, Point>();
        private Dictionary<Room, Flags> m_RoomNeighbours = new Dictionary<Room, Flags>();    

        private Flags m_RoomFlags;

        private Vector2 m_MaxSize = new Vector2(Globals.RoomSize.X * 3, Globals.RoomSize.Y * 3);
        private Texture2D m_DoorTexture;
        private Point DoorSize = new Point(48,48);
     
        public Floor() {

            m_DoorTexture = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_DoorTexture.SetData(new Color[] { Color.Green });

            m_RoomPoints = GetRoomPoints();

            foreach(Point point in m_RoomPoints) {
                Room room = new Room(point, new Vector2(Globals.RoomSize.X, Globals.RoomSize.Y));
                m_Rooms.Add(room, point);
                m_AllRooms.Add(room);
            }
            CheckNeighbours(m_Rooms);
            GetDoorPoints();
            SpawnDoor();

            FloorManager.CurrentRoom = m_AllRooms[0];
        }

        public void Update(GameTime gameTime) {
           FloorManager.CurrentRoom.Update(gameTime);
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {
            foreach(var room in m_Rooms) {
                room.Key.Draw(batch, gameTime);           
            }
        }

        private List<Point> GetRoomPoints() {

            var maxHorizontalRooms = (int)Math.Floor(m_MaxSize.X / Globals.RoomSize.X);
            var maxVerticalRooms = (int)Math.Floor(m_MaxSize.Y / Globals.RoomSize.Y);

            var availablePoints = new List<Point>();

            for (int y = 0; y < maxVerticalRooms; y++) {
                for(int x = 0; x < maxHorizontalRooms; x++) {
                    var point = new Point((int)Globals.RoomSize.X * x, (int)Globals.RoomSize.Y * y);
                    availablePoints.Add(point);
                }
            }

            return availablePoints;         
        }
        
        private void CheckNeighbours(Dictionary<Room, Point> rooms) { 
            foreach(KeyValuePair<Room, Point> kvp in rooms) {
                var room = kvp.Key;
                var point = kvp.Value;
                m_RoomFlags = Flags.None;

                if (m_RoomPoints.Contains(point + PointDirections.East)) { // East Point
                    m_RoomFlags |= Flags.East;
                }

                if (m_RoomPoints.Contains(point + PointDirections.North)) { // North Point
                    m_RoomFlags |= Flags.North;
                }

                if (m_RoomPoints.Contains(point + PointDirections.West)) { // West Point
                    m_RoomFlags |= Flags.West;
                }

                if (m_RoomPoints.Contains(point + PointDirections.South)) { // South Point
                    m_RoomFlags |= Flags.South;
                }

                m_RoomNeighbours.Add(room, m_RoomFlags);
               
            }
        }

        private void DrawDoors(SpriteBatch batch) {
            for(var i = 0; i < m_DoorLocations.Count; i++) {
                batch.Draw(m_DoorTexture, m_DoorLocations[i], Color.White);
            }
        }

        private void SpawnDoor() {
            for (var i = 0; i < m_DoorLocations.Count; i++) {
                var door = new Door(m_DoorLocations[i]);
                Doors.Add(door);
                EntityManager.Add(door);
            }
        }

        private void GetDoorPoints() {
            foreach (KeyValuePair<Room, Flags> kvp in m_RoomNeighbours) {
                //East door example: 1
                //East with some other doors: 7 //east + north + west

                //Mask out other values to get east
                //& operator masks values
                //flags & Flags.East will mask out all other flags then East

                Flags flags = kvp.Value;

                if ((flags & Flags.East) == Flags.East) {
                    m_DoorLocations.Add(new Rectangle(m_Rooms[kvp.Key].X + (int)Globals.RoomSize.X - 49, m_Rooms[kvp.Key].Y + (int)(Globals.RoomSize.Y / 2), DoorSize.X, DoorSize.Y));
                }

                if ((flags & Flags.North) == Flags.North) {
                    m_DoorLocations.Add(new Rectangle(m_Rooms[kvp.Key].X + (int)(Globals.RoomSize.X / 2) - 32, m_Rooms[kvp.Key].Y + 1, DoorSize.X, DoorSize.Y));  
                }

                if ((flags & Flags.West) == Flags.West) {
                    m_DoorLocations.Add(new Rectangle(m_Rooms[kvp.Key].X + 1, m_Rooms[kvp.Key].Y + (int)(Globals.RoomSize.Y / 2), DoorSize.X, DoorSize.Y));
                }

                if ((flags & Flags.South) == Flags.South) {
                    m_DoorLocations.Add(new Rectangle(m_Rooms[kvp.Key].X + (int)(Globals.RoomSize.X / 2) - 32, m_Rooms[kvp.Key].Y + (int)Globals.RoomSize.Y - 49, DoorSize.X, DoorSize.Y));
                }

            }
        }

        [Flags]
        private enum Flags {
            None = 0,
            East = 1 << 0,  // 1
            North = 1 << 1, // 2
            West = 1 << 2,  // 4
            South = 1 << 3  // 8
        }
    }

    public static class PointDirections {
        public static Point North = new Point(0, -360);
        public static Point East  = new Point(640, 0);
        public static Point South = new Point(0, 360);
        public static Point West  = new Point(-640, 0);
    }
}
