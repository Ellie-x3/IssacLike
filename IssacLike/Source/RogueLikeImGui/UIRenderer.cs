using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.ImGuiNet;

namespace IssacLike.Source.RogueLikeImGui {
    public class UIRenderer {
        public static ImGuiRenderer s_GuiRenderer;

        public static void Init(RogueLike game) {
            s_GuiRenderer = new ImGuiRenderer(game);

            Logger.Start();
        }

        public static void Update(GameTime gameTime) {
            Logger.Update(gameTime);
        }
    }
}
