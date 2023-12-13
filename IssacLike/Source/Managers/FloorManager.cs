using IssacLike.Source.Entities.Player;
using IssacLike.Source.RogueLikeImGui;
using IssacLike.Source.Rooms;
using IssacLike.Source.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace IssacLike.Source.Managers {
    public static class FloorManager {
        //Keep track of floors
        //Create new floors
        //Keep track of where player is on floor
        //Keep track of which floor player is on
        //Keep track of which room player is in

        private static Floor m_CurrentFloor;
        private static Room m_CurrentRoom;
        private static Room m_SpawnRoom;
        private static Vector2 m_SpawnPosition = new Vector2(Globals.RoomSize.X / 2, Globals.RoomSize.Y / 2);

        private static List<Point> m_RoomPostions { get => m_CurrentFloor.RoomsPositions; }

        private static Vector2 m_PlayerPosition;

        public static Vector2 PlayerPosition { get => m_PlayerPosition; set => m_PlayerPosition = value; }
        
        public static Vector2 SpawnPosition { get => m_SpawnPosition; }

        public static Room CurrentRoom {  
            get => m_CurrentRoom;
            set
            {
                if (m_CurrentRoom == value) return;
                m_CurrentRoom = value;

                OnRoomChanged();
            }
        }

        public delegate void RoomChangedEventHandler();
        public static event RoomChangedEventHandler E_RoomChanged;

        public static void Create() {
            Floor floor = new Floor();
            E_RoomChanged += UpdateCamera;
            m_CurrentFloor = floor;
            m_SpawnRoom = m_CurrentRoom;
            m_SpawnPosition = new Vector2(m_SpawnRoom.RoomPosition.X + (Globals.RoomSize.X / 2), m_SpawnRoom.RoomPosition.Y + (Globals.RoomSize.Y / 2));
            Globals.Camera.Position = new Vector2(640,360);
        }

        public static void Update(GameTime gameTime) {
            m_CurrentFloor.Update(gameTime);
            PlayerIsInsideRoom(m_PlayerPosition);
        }

        public static void Draw(SpriteBatch batch, GameTime gameTime) {
            m_CurrentFloor.Draw(batch, gameTime);
        }

        private static void OnRoomChanged() {
            E_RoomChanged?.Invoke();
        }

        private static void UpdateCamera() {
            Logger.Log("Changed rooms");
            Globals.Camera.Position = new Vector2(CurrentRoom.RoomPosition.X + Globals.RoomSize.X, CurrentRoom.RoomPosition.Y + Globals.RoomSize.Y);
        }

        private static void PlayerIsInsideRoom(Vector2 playerPosition) {
            for(var i = 0; i < m_RoomPostions.Count; i++) {
                if(ContainsPoint(playerPosition, m_RoomPostions[i])) {
                    CurrentRoom = m_CurrentFloor.Rooms[i];
                }
            }
        }

        private static bool ContainsPoint(Vector2 playerPosition, Point roomPositions) {
            return playerPosition.X >= roomPositions.X && playerPosition.X <= roomPositions.X + Globals.RoomSize.X 
                && playerPosition.Y >= roomPositions.Y && playerPosition.Y <= roomPositions.Y + Globals.RoomSize.Y;
        }
    }
}
