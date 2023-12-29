using ProjectMystic.Source.ZeldaLikeImGui;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMystic.Source.Managers.Resources {
    public class TextureLoader : ResourceLoader {

        public static readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private static string m_DefaultTexture = "Textures/TEXTURE_default";
        private static string m_TexturePath = "Textures/";

        public static void Init() {
            Texture2D texture = m_ContentManager.Load<Texture2D>(m_DefaultTexture);
            Textures.Add(m_DefaultTexture, texture);
        }

        public static void AddTexture(string name, string path) {
            if (Textures.ContainsKey(name)) {
                return;
            }
            path = string.Concat(m_TexturePath, path);
            Texture2D texture = m_ContentManager.Load<Texture2D>(path);
            Textures.Add(name, texture);
            Logger.Log("Added Texture: {0} {1}", name, path);
        }

        public static Texture2D Texture(string name) {
            if (!Textures.ContainsKey(name)) {
                Logger.Log("The Texture {0} cannot be found", name);
                return Textures[m_DefaultTexture];
            }

            return Textures[name];
        }
    }
}
