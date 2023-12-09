﻿using IssacLike.Source.RogueLikeImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Managers.Resources
{
    internal abstract class ResourceLoader
    {
        protected static ContentManager m_ContentManager;

        public static void Initialize(ContentManager content_manager)
        {
            Logger.Log("Intializing Content Manager");
            m_ContentManager = content_manager;

            TextureLoader.Init();
          
        }
    }
}