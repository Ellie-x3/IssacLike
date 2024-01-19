using ProjectMystic.Source.Managers.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProjectMystic.Source.Components;
using ProjectMystic.Source.ZeldaLikeImGui;
using ZeldaLike.Source.Entities;
using ZeldaLike.Source.Managers;
using ProjectMystic.Source.Managers;

namespace ProjectMystic.Source.Entities {
    public abstract class Entity : IDisposable {

        public Vector2 EntityPosition { get => Position; }

        protected const string DEFAULT_NAME = "Entity";

        protected Texture2D Texture { get;set; }
        protected Vector2 Position { get; set;}
        protected Vector2 Direction { get; }
        protected Vector2 Scale = new Vector2(1f);
        protected Vector2 Origin = new Vector2(0f);
        protected SpriteEffects SpriteEffects { get; set; }
        protected Color Color = Color.White;
        public List<IComponent> Components = new List<IComponent>();

        public string Name { get; set; } = "DEFAULT_ENTITY_SHOULD_CHANGE";

        protected bool Destroyed { get;set; }
        protected float Layer { get;set; }

        protected IMediator mediator;

        public virtual void Start() {
            if(Texture == null)
                Texture = TextureLoader.Texture("Textures/TEXTURE_default");


            AddComponents();
        }

        public virtual void Update(GameTime gameTime) {
            var components = Components.OfType<IComponent>();

            foreach (var component in components) { 
                component.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch batch, GameTime gameTime) {
            var drawable = Components.OfType<IDraw>();

            foreach (var component in drawable) {              
                component.Draw(batch, Position, Color, 0f, Origin, Scale, SpriteEffects, Layer);
            }
        }

        public void Draw(SpriteBatch batch, GameTime gameTime, Effect effect) {
            batch.End(); //End current batch to prepare new batch for shader effect on entity

            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: CameraManager.CurrentCameraMatrices, effect: effect);
            var drawable = Components.OfType<IDraw>();

            foreach (var component in drawable) {
                component.Draw(batch, Position, Color, 0f, Origin, Scale, SpriteEffects, Layer);
            }
        }

        public void AddComponent(IComponent component) { 
            Components.Add(component);
            component.Owner = this;
        }

        public void RemoveComponent(IComponent component) {
            if (Components.Contains(component)) {
                Components.Remove(component);
                component.Owner = null;
            }
        }

        public T GetComponent<T>() where T : IComponent {
            foreach (var component in Components) {
                if (component is T c) {
                    return c;
                }
            }

            return default;
        }

        private void AddComponents() {
            var fields = GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            foreach (var field in fields) {
                
                var attributes = field.GetCustomAttributes(typeof(ComponentAttribute), false);

                if(attributes.Length > 0) {
                    var component = (IComponent)field.GetValue(this);
                    AddComponent(component);
                }
            }
        }

        protected void SetMediator(IMediator mediator) {
            this.mediator = mediator;
        }

        protected void InteractWith(Entity other) {
            if(mediator != null) {
                mediator.Notify($"INTERACTION_{GetType().Name}_{other.GetType().Name}", this, other);
            }
                
        }

        //protected abstract Rectangle CalculateBound();

        public void Dispose(Entity entity) { 
            Destroyed = true;
            EntityManager.Remove(entity);
            Dispose();
        }

        public virtual void Dispose() {

        }

        public virtual void OnCollisionEvent(ICollidable other) { }
        public virtual void OnExitCollisionEvent() { }

    }
}
