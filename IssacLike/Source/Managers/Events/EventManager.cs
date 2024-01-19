using ProjectMystic.Source.ZeldaLikeImGui;
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

        public delegate void TransitionHalfFinishedEventHandler();
        public static event TransitionHalfFinishedEventHandler E_TransitionHalfFinished;

        public static void OnRoomChanged() {
            Logger.Log("Invoking event");
            E_RoomChanged?.Invoke();
        }

        public static void OnTransitionHalfFinished() {
            E_TransitionHalfFinished?.Invoke();
        }
    }
}
