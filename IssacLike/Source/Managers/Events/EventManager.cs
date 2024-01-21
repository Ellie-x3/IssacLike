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

        //Transitioning Events
        public delegate void TransitionNormalEventHandler();
        public delegate void TransitionBeforeEventHandler();
        public delegate void TransitionDuringEventHandler();
        public delegate void TransitionAfterEventHandler();
        public delegate void TransitionFinishedEventHandler();

        public static event TransitionNormalEventHandler E_TransitionNormal;
        public static event TransitionBeforeEventHandler E_TransitionBefore; 
        public static event TransitionDuringEventHandler E_TransitionDuring; 
        public static event TransitionAfterEventHandler E_TransitionAfter;
        public static event TransitionFinishedEventHandler E_TransitionFinished;

        public static void OnTransitionNormal(){ E_TransitionNormal?.Invoke(); } 
        public static void OnTransitionBefore(){ E_TransitionBefore?.Invoke(); } 
        public static void OnTransitionDuring(){ E_TransitionDuring?.Invoke(); } 
        public static void OnTransitionAfter(){ E_TransitionAfter?.Invoke(); } 
        public static void OnTransitionFinished(){ E_TransitionFinished?.Invoke(); } 

        //Room Changed Event
        public delegate void RoomChangedEventHandler();
        public static event RoomChangedEventHandler E_RoomChanged;
        
        public static void OnRoomChanged() {
            Logger.Log("Invoking event");
            E_RoomChanged?.Invoke();
        }
    }
}
