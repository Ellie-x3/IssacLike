using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.ImGuiNet;
using Microsoft.Xna.Framework.Input;
using System.Numerics;
using System.Threading;

//Game using directories
using ProjectMystic.Source.Util;
using System.Collections;
using ProjectMystic.Source.Managers;

namespace ProjectMystic.Source.ZeldaLikeImGui {
    public class Logger : UIRenderer {

        private static bool s_IsConsoleShowPressed = false;
        private static bool s_IsDrawColliderChecked = false;

        private static byte[] inputBuf = new byte[64];
        private static List<string> ConsoleOutput = new List<string>();
        private static List<string> LogOutput = new List<string>();
        private static List<string> LogOutputQueue = new List<string>();
        private static List<string> Commands = new List<string> { 
            "CLEAR",
            "CLEARLOG",
            "GOTO",
            "HELP"
        };

        private enum RenderTarget {
            Console,
            Log,
            Debug,
            Default
        }

        private static RenderTarget target = RenderTarget.Log;

        public static void Start() {
            //Coroutine.StartCoroutine(() => testCoroutine());
        }

        public static new void Update(GameTime gt) {
            if (Input.IsKeyPressed(Keys.OemTilde))
                s_IsConsoleShowPressed = !s_IsConsoleShowPressed;

            if (Input.IsKeyPressed(Keys.F3)) {
                s_IsDrawColliderChecked = !s_IsDrawColliderChecked;
                Debug.DrawDebug = !Debug.DrawDebug;
            }
        }

        private static void RenderConsole() {
            ImGui.SetKeyboardFocusHere();
            ImGui.Text("Console");
           
            ImGui.Separator();
            
            float footerHeightReserve = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();

            if(ImGui.BeginChild("ScrollingRegion", new System.Numerics.Vector2(0, -footerHeightReserve), false, ImGuiWindowFlags.HorizontalScrollbar)){ 
                ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(4, 1));

                foreach (string item in ConsoleOutput) {
                    ImGui.TextUnformatted(item);
                }

                ImGui.PopStyleVar();
            }
            ImGui.EndChild();
            ImGui.Separator();
            

            //ImGui.SetKeyboardFocusHere();
            ImGuiInputTextFlags textFlags = ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.EscapeClearsAll | ImGuiInputTextFlags.CallbackCompletion | ImGuiInputTextFlags.CallbackHistory;
            if(ImGui.InputText("Input", inputBuf, 64, textFlags)){
                
                string s = Encoding.ASCII.GetString(inputBuf);
                s = s.Trim('\0'); //trim extra null characters

                if (IsCommand(s)) {
                    ExecuteCommand(s);
                } else {
                    AddConsoleOutput(s);
                }

                inputBuf = new byte[64];
            }

        }

        private static void RenderLog() {
            ImGui.Text("This is the log!");

            ImGui.Separator();
            
            if (ImGui.BeginChild("ScrollingRegion", new System.Numerics.Vector2(0, 0), false, ImGuiWindowFlags.HorizontalScrollbar)) {
                ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(4, 1));

                for(int i = 0; i < LogOutput.Count; i++) {
                    ImGui.TextUnformatted(LogOutput[i]);
                }

                if(LogOutput.Count >= 200) {
                    LogOutput = new List<string>();
                }

                if(ImGui.GetScrollY() >= ImGui.GetScrollMaxY())
                    ImGui.SetScrollHereY(1.0f);

                ImGui.PopStyleVar();
            }

            ImGui.EndChild();
            ImGui.Separator();
        }

        private static void RenderDebug() {
            if (ImGui.Checkbox("Draw Colliders", ref s_IsDrawColliderChecked)) {
                Debug.DrawDebug = !Debug.DrawDebug;
            }
        }

        public static void Draw(GameTime gt) {

            s_GuiRenderer.BeginLayout(gt);
 
            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoSavedSettings;
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(Globals.ScreenSize.X, 225.0f));
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, Globals.ScreenSize.Y - 225.0f));

            if (ImGui.Begin("Console", flags) && s_IsConsoleShowPressed) {
                if (ImGui.BeginTabBar("Tabs")) {
                    if (ImGui.BeginTabItem("Log")) {
                        target = RenderTarget.Log;
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Console")) {
                        target = RenderTarget.Console;
                        ImGui.EndTabItem();
                    }

                    if(ImGui.BeginTabItem("Debug Options")) {
                        target = RenderTarget.Debug;
                        
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }

                switch (target) {
                    case RenderTarget.Console:
                        RenderConsole();
                        break;
                    case RenderTarget.Log:
                        RenderLog();
                        break;
                    case RenderTarget.Debug:
                        RenderDebug();
                        break;
                    default:
                        break;
                }

                //ImGui.ShowDemoWindow();

                ImGui.End();
                s_GuiRenderer.EndLayout();
            }   
        }

        public static void Log(string log, params object[] args) {
            string formattedLog = string.Format(log, args);
            LogOutput.Add(formattedLog);
        }

        public static void Log(params object[] logs) {
            string formattedLog = "";

            for (int i = 0; i < logs.Count(); i++) {
                var log = logs[i];
                formattedLog += log.ToString() + ", ";
            }
      
            LogOutput.Add(formattedLog);
        }

        private static void AddConsoleOutput(string fmt, params object[] args) {
            string formattedString = string.Format(fmt, args);
            ConsoleOutput.Add(formattedString);
        }

        private static bool IsCommand(string s) {
            return Commands.Contains(s.ToUpper());
        }

        private static void ExecuteCommand(string s) {
            string cmd = Commands.FirstOrDefault(f => f == s.ToUpper());

            switch(cmd) {
                case "HELP":
                    foreach(string command in Commands) {
                        AddConsoleOutput(command);
                    }
                    break;
                case "CLEAR":
                    ConsoleOutput = new List<string>();
                    break;
                case "CLEARLOG":
                    LogOutput = new List<string>();
                    break;
                default:
                    AddConsoleOutput("Unrecognized command: {0}", cmd);
                    break;
            }
        }

        private static IEnumerator testCoroutine() {
            WaitForSeconds wait = new WaitForSeconds(2.0f);
            while (!wait.IsWaitFinished()) {
                yield return null;
            }

            Log("This is a log");
        }
    }
}
