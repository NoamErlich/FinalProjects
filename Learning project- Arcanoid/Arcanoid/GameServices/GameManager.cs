using Arcanoid.GameObjects;
using Arcanoid.Pages;
using DatabaseProject.Models;
using GameEngine.GameServices;
using System;
using static Arcanoid.GameObjects.Brick;

namespace Arcanoid.GameServices
{
    public class GameManager : Manager
    {
        public static GameUser User { get; set; } = new GameUser();
        public GameManager(Scene scene) : base(scene)
        {
            User.Level = LevelsPage.level;
            User.Name = "Noam";
            scene.Ground = scene.ActualHeight - 40;
            InIt();
        }

        public void ResetEvents()
        {
            GameEvent.RemoveAllActions();
        }

        private void InIt()
        {
            Scene.RemoveAllObject();

            var bar = new Bar(Scene, "Bar/bar.png", speed: User.Level.Speed, width: User.Level.BarWidth, Scene.ActualWidth / 2 - 80, Scene.Ground - 25);
            Scene.AddObject(bar);

            var ball = new Ball(Scene, "Ball/ball.png", speed: User.Level.Speed, width: 50, Scene.ActualWidth / 2 - 18, Scene.Ground - 80);
            Scene.AddObject(ball);

            CreateBrickField(User.Level.CountGreenBrick, User.Level.CountPinkBrick, User.Level.CountYellowBrick);
        }


        private void CreateBrickField(int countGreenBricks, int countPinkBricks, int countYellowBricks, int rows = 3)
        {
            Brick.BrickType brickType = 0;
            Random rnd = new Random();
            double y = 10;
            var offset = 52;
            double x = offset;
            int width = 130;
            int height = 130;
            int count = 0;

            while (countGreenBricks + countPinkBricks + countYellowBricks > 0)
            {
                brickType = (Brick.BrickType)rnd.Next(0, 3);
                if (brickType == Brick.BrickType.Green && countGreenBricks == 0)
                    continue;
                else if (brickType == Brick.BrickType.Yellow && countYellowBricks == 0)
                    continue;
                else if (brickType == Brick.BrickType.Pink && countPinkBricks == 0)
                    continue;
                var brick = new Brick(Scene, brickType, width, height, x, y);
                Scene.AddObject(brick);
                x += 160;
                count++;
                if (x + 72 > Scene.ActualWidth || count > 8)
                {
                    y += 75;
                    x = offset;
                    count = 0;
                }
                switch (brickType)
                {
                    case BrickType.Green:
                        countGreenBricks--;
                        break;
                    case BrickType.Pink:
                        countPinkBricks--;
                        break;
                    case BrickType.Yellow:
                        countYellowBricks--;
                        break;
                }

            }


        }
    }
}
