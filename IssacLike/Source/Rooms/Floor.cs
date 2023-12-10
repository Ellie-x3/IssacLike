﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IssacLike.Source.Rooms {
    internal class Floor { //New floor
        //Create room
        //Manage room door
        //Manage room transition
        //Manage Camera transition
        //Keep track of floor state

        public Room CurrentRoom { get => m_CurrentRoom; }

        private Dictionary<Room, Point> m_Rooms = new Dictionary<Room, Point>();
        private List<Point> m_RoomPoints = new List<Point>();
        private Dictionary<Room, Flags> m_RoomNeighbours = new Dictionary<Room, Flags>();
        private Flags m_RoomFlags;
        private Room m_CurrentRoom;

        private Vector2 m_MaxSize = new Vector2(Globals.RoomSize.X * 3, Globals.RoomSize.Y * 3);

        private Texture2D m_DoorTexture;
     
        internal Floor() {

            m_DoorTexture = new Texture2D(Globals.s_GraphicsDevice, 1, 1);
            m_DoorTexture.SetData(new Color[] { Color.Green });

            m_RoomPoints = GetRoomPoints();

            foreach(Point point in m_RoomPoints) {
                m_Rooms.Add(new Room(point, new Vector2(Globals.RoomSize.X, Globals.RoomSize.Y)), point);
            }
            CheckNeighbours(m_Rooms);

            m_CurrentRoom = m_Rooms.Keys.First();

        }

        internal void Update(GameTime gameTime) {
            m_CurrentRoom.Update(gameTime);
        }

        internal void Draw(SpriteBatch batch, GameTime gameTime) {
            foreach(KeyValuePair<Room, Point> room in m_Rooms) {
                room.Key.Draw(batch, gameTime);
                DrawDoors(batch);
            }
        }

        private List<Point> GetRoomPoints() {

            var maxHorizontalRooms = (int)Math.Floor(m_MaxSize.X / Globals.RoomSize.X);
            var maxVerticalRooms = (int)Math.Floor(m_MaxSize.Y / Globals.RoomSize.Y);

            List<Point> availablePoints = new List<Point>();

            for (int y = 0; y < maxVerticalRooms; y++) {
                for(int x = 0; x < maxHorizontalRooms; x++) {
                    Point point = new Point((int)Globals.RoomSize.X * x, (int)Globals.RoomSize.Y * y);
                    availablePoints.Add(point);
                }
            }

            return availablePoints;         
        }
        
        private void CheckNeighbours(Dictionary<Room, Point> rooms) { 

            Point minSize = new Point(0,0);
            Point maxSize = new Point((int)m_MaxSize.X, (int)m_MaxSize.Y);

            Point roomSize = new Point((int)Globals.RoomSize.X, (int)Globals.RoomSize.Y);

            bool[] hasNeighbor = new bool[4]; 

            foreach(KeyValuePair<Room, Point> kvp in rooms) {
                Room room = kvp.Key;
                Point point = kvp.Value;
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
            foreach(KeyValuePair<Room, Flags> kvp in m_RoomNeighbours) {
                //bit math yay
                //East door example: 1
                //East with some other doors: 7 //east + north + west
                
                //Mask out other values to get east, idk how :(
                //& operator masks values
                //flags & Flags.East will mask out all other flags then East

                Flags flags = kvp.Value;

                if((flags & Flags.East) == Flags.East) {
                    Logger.Log("East door connected");
                    batch.Draw(m_DoorTexture, new Rectangle(m_Rooms[kvp.Key].X + (int)Globals.RoomSize.X - 32, m_Rooms[kvp.Key].Y + (int)(Globals.RoomSize.Y / 2), 32 ,32), Color.White);
                }

                if ((flags & Flags.North) == Flags.North) {
                    Logger.Log("North door connected");
                    batch.Draw(m_DoorTexture, new Rectangle(m_Rooms[kvp.Key].X + (int)(Globals.RoomSize.X / 2) - 32, m_Rooms[kvp.Key].Y, 32, 32), Color.White);
                }

                if ((flags & Flags.West) == Flags.West) {
                    Logger.Log("West door connected");
                    batch.Draw(m_DoorTexture, new Rectangle(m_Rooms[kvp.Key].X, m_Rooms[kvp.Key].Y + (int)(Globals.RoomSize.Y / 2), 32, 32), Color.White);
                }

                if ((flags & Flags.South) == Flags.South) {
                    Logger.Log("South door connected");
                    batch.Draw(m_DoorTexture, new Rectangle(m_Rooms[kvp.Key].X + (int)(Globals.RoomSize.X / 2) - 32, m_Rooms[kvp.Key].Y + (int)Globals.RoomSize.Y - 32, 32, 32), Color.White);
                }
            }
        }

        private enum Flags {
            None = 0,
            East = 1 << 0,  // 1
            North = 1 << 1, // 2
            West = 1 << 2,  // 4
            South = 1 << 3  // 8
        }
    }

    internal static class PointDirections {
        internal static Point North = new Point(0, -360);
        internal static Point East  = new Point(640, 0);
        internal static Point South = new Point(0, 360);
        internal static Point West  = new Point(-640, 0);
    }
}
