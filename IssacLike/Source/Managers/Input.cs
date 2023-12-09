using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IssacLike.Source.Managers {
    internal static class Input {
        private static KeyboardState m_KeyboardCurrent;
        private static KeyboardState m_KeyboardPrevious;

        private static int[] m_KeyCodes;
        private static bool[] m_KeyPress;
        private static bool[] m_KeyDown;

        public static void Initialize() {
            m_KeyCodes = Enum.GetValues(typeof(Keys)).Cast<int>().ToArray();
            m_KeyPress = new bool[m_KeyCodes.Length];
            m_KeyDown = new bool[m_KeyCodes.Length];
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
        }

        public static bool IsKeyPressed(Keys key) {
            int index = Array.FindIndex(m_KeyCodes, x => x == (int)key);
            return m_KeyPress[index];
        }

        public static bool IsKeyDown(Keys key) {
            int index = Array.FindIndex(m_KeyCodes, x => x == (int)key);
            return m_KeyDown[index];
        }
    }
}
