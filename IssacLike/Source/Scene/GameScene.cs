﻿using IssacLike.Source.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IssacLike.Source.Util;
using IssacLike.Source.Managers;
using IssacLike.Source.Entities.Player;
using IssacLike.Source.Managers.Resources;
using LDtk.Renderer;
using Microsoft.Xna.Framework.Graphics;
using IssacLike.Source.Rooms;

namespace IssacLike.Source.Scene
{
    internal class GameScene : IScene {
        public string name { get => "GameScene"; }

        private Floor floor;

        public static IScene Instance { get {
                if(m_Instance == null){
                    m_Instance = new GameScene();
                }

                return m_Instance;
            } 
        }
        
        private static IScene m_Instance;

        public GameScene() {
            Player player = new Player();
            AddEntityToScene(player);

            floor = new Floor();
        }

        public void AddEntityToScene(Entity entity) {
            EntityManager.Add(entity);
        }

        public void Update(GameTime gameTime) {
            //LevelLoader.Update(gameTime);
            EntityManager.Update(gameTime);
            floor.Update(gameTime);   
        }

        public void SceneContent(SpriteBatch batch) {
           // LevelLoader.m_Renderer = new LDtkRenderer(batch);
           // LevelLoader.Init(batch);
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {
           // LevelLoader.Draw(batch);
            floor.Draw(batch, gameTime);
            EntityManager.Draw(batch, gameTime);
        }
    }
}
