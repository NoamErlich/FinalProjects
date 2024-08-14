using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using GameEngine.GameObjects;

namespace GameEngine.GameServices
{
    /// <summary>
    /// Abstract class representing a scene in the game.
    /// </summary>
    public abstract class Scene : Canvas
    {
        private List<GameObject> _gameObjects = new List<GameObject>(); // Collection of all game objects
        protected List<GameObject> _gameObjectsSnapshot => _gameObjects.ToList(); // Copy of game objects list
        public double Ground { get; set; } // Ground level

        /// <summary>
        /// Constructor for the Scene class.
        /// </summary>
        public Scene()
        {
            Manager.GameEvent.OnRun += Run; // Activates the Run action continuously
            Manager.GameEvent.OnRun += CheckRoundCollision; // Activates the CheckRoundCollision action continuously
            Manager.GameEvent.OnRun += CheckRectCollision; // Activates the CheckRectCollision action continuously
        }

        /// <summary>
        /// Checks for collisions between rectangular game objects.
        /// </summary>
        private void CheckRectCollision()
        {
            foreach (var gameObject in _gameObjectsSnapshot)
            {
                // Check if the current game object is collisional
                if (gameObject.Collisional)
                {
                    // Find another collisional object that intersects with the current object
                    var otherObject = _gameObjectsSnapshot.FirstOrDefault(g =>
                        !ReferenceEquals(g, gameObject) && // Ensure it's not the same object
                        g.Collisional && // Ensure the other object is collisional
                        !RectHelper.Intersect(g.Rect, gameObject.Rect).IsEmpty); // Check for intersection

                    // If an intersecting object is found, trigger collision
                    if (otherObject != null)
                    {
                        gameObject.Collide(otherObject);
                    }
                }
            }
        }

        /// <summary>
        /// Checks for collisions between round game objects.
        /// </summary>
        public void CheckRoundCollision()
        {
            foreach (var gameObject in _gameObjectsSnapshot)
            {
                // Check if the current game object is a round and collisional object
                if (gameObject is RoundGameObject roundObject && roundObject.Collisional)
                {
                    // Find another round and collisional object that collides with the current object
                    var otherObject = _gameObjectsSnapshot.FirstOrDefault(g =>
                        !ReferenceEquals(g, roundObject) && // Ensure it's not the same object
                        g is RoundGameObject otherRoundObject && // Ensure it's a round object
                        otherRoundObject.Collisional && // Ensure the other object is collisional
                        IsColliding(roundObject, otherRoundObject)); // Check for collision

                    // If a colliding object is found, trigger collision
                    if (otherObject != null)
                    {
                        roundObject.Collide(otherObject);
                    }
                }
            }
        }


        /// <summary>
        /// Checks if two round game objects are colliding.
        /// </summary>
        private bool IsColliding(RoundGameObject obj1, RoundGameObject obj2)
        {
            double distance = Math.Sqrt(Math.Pow(obj2._CenterX - obj1._CenterX, 2) + Math.Pow(obj2._CenterY - obj1._CenterY, 2)); //calculates the distance between the 2 objects
            return distance <= obj1._Radius + obj2._Radius;
        }

        /// <summary>
        /// Action executed continuously during the game loop.
        /// </summary>
        private void Run()
        {
            foreach (var obj in _gameObjects)
            {
                if (obj is GameMovingObject)
                    obj.Render();
            }
        }

        /// <summary>
        /// Initializes all game objects to their initial positions.
        /// </summary>
        public void Init()
        {
            foreach (GameObject obj in _gameObjects)
            {
                obj.InIt();
            }
        }

        /// <summary>
        /// Removes the specified game object from the scene.
        /// </summary>
        /// <param name="gameObject">The game object to remove.</param>
        public void RemoveObject(GameObject gameObject)
        {
            if (_gameObjects.Contains(gameObject))
            {
                _gameObjects.Remove(gameObject);
                Children.Remove(gameObject.Image);
            }
        }

        /// <summary>
        /// Removes all game objects from the scene.
        /// </summary>
        public void RemoveAllObject()
        {
            foreach (GameObject obj in _gameObjectsSnapshot)
            {
                RemoveObject(obj);
            }
        }

        /// <summary>
        /// Adds a game object to the scene.
        /// </summary>
        /// <param name="obj">The game object to add.</param>
        public void AddObject(GameObject obj)
        {
            _gameObjects.Add(obj);
            Children.Add(obj.Image);
        }

    }
}
