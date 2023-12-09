using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.ImGuiNet;

namespace IssacLike.Source.RogueLikeImGui {
    internal class UIRenderer {
        internal static ImGuiRenderer s_GuiRenderer;

        internal static void Init(RogueLike game) {
            s_GuiRenderer = new ImGuiRenderer(game);

            Logger.Start();
        }

        internal static void Update(GameTime gameTime) {
            Logger.Update(gameTime);
        }
    }
}
