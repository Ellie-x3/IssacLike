using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMystic.Source.Managers.Events
{
    public static class EventManager
    {
        public delegate void RoomChangedEventHandler();
        public static event RoomChangedEventHandler E_RoomChanged;

        public static void OnRoomChanged() {
            E_RoomChanged?.Invoke();
        }
    }
}
