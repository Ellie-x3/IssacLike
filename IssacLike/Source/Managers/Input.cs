using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssacLike.Source.RogueLikeImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IssacLike.Source.Managers {
    public static class Input {
        private static KeyboardState m_KeyboardCurrent;
        private static KeyboardState m_KeyboardPrevious;

        private static MouseState m_MouseCurrent;
        private static MouseState m_MousePrevious;

        private static int[] m_KeyCodes;
        private static bool[] m_KeyPress;
        private static bool[] m_KeyDown;

        private static int[] m_MouseCodes;
        private static bool[] m_MouseButtonPress;
        private static bool[] m_MouseButtonDown;

        private enum MouseButtons {
            LeftMouse,
            RightMouse,
        }

        public static void Initialize() {
            m_KeyCodes = Enum.GetValues(typeof(Keys)).Cast<int>().ToArray();
            m_KeyPress = new bool[m_KeyCodes.Length];
            m_KeyDown = new bool[m_KeyCodes.Length];

            m_MouseCodes = Enum.GetValues(typeof(MouseButtons)).Cast<int>().ToArray();
            m_MouseButtonPress = new bool[m_MouseCodes.Length];
            m_MouseButtonDown = new bool[m_MouseCodes.Length];
        }

        public static void Update(GameTime gameTime) {
            m_KeyboardCurrent = Keyboard.GetState();

            for(int i = 0; i < m_KeyCodes.Length; i++) {
                Keys key = (Keys)m_KeyCodes[i];

                if(m_KeyboardCurrent.IsKeyDown(key) && !m_KeyboardPrevious.IsKeyDown(key))
                    m_KeyPress[i] = true;
                else
                    m_KeyPress[i] = false;

                if(m_KeyboardCurrent.IsKeyDown(key) && m_KeyboardPrevious.IsKeyDown(key))
                    m_KeyDown[i] = true;
                else
                    m_KeyDown[i] = false;
            }

            m_KeyboardPrevious = Keyboard.GetState();

            m_MouseCurrent = Mouse.GetState();

            for(int i = 0; i < m_MouseCodes.Length; i++) {
                MouseButtons button = (MouseButtons)m_KeyCodes[i];

                if (button == MouseButtons.LeftMouse && m_MouseCurrent.LeftButton == ButtonState.Pressed && m_MousePrevious.LeftButton == ButtonState.Released) {
                    m_MouseButtonPress[i] = true;                   
                } else {
                    m_MouseButtonPress[i] = false;
                }
            }

            m_MousePrevious = Mouse.GetState();
        }

        public static bool IsKeyPressed(Keys key) {
            int index = Array.FindIndex(m_KeyCodes, x => x == (int)key);
            return m_KeyPress[index];
        }

        public static bool IsKeyDown(Keys key) {
            int index = Array.FindIndex(m_KeyCodes, x => x == (int)key);
            return m_KeyDown[index];
        }

        public static bool IsLeftMouseButtonPressed() {
            return m_MouseButtonPress[(int)MouseButtons.LeftMouse];
        }

        public static Point MousePosition() {
            return m_MouseCurrent.Position;
        }
    }
}
