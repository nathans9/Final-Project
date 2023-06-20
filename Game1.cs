using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Final_Project
{
    //TODO
    //have a cooldown after you're hit and respawning
    //have your character die when hit
    //consider changing the hitbox.  instead of rectangles, use circles:
    //   - check the distance between the center of each projectile and your player
    //   - if the distance is less than the radius of the projectile + the radius of the penguin hitboxes, they intersect
    //add something to a class to serve as a speed multiplier for each projectile
                                                                                         

    public enum SMove
    {
        Stationary, //done
        Circle, //done
        UpLeft,
        DownLeft,
        UpRight,
        DownRight,
        Left,
        Right,
        Up,
        Down,
        ToCenter,
        Teleport
    }
    public enum SPhase
    {
        One,
        Two,
        Three
    }

    public enum PMove
    {
        Static, //done
        Pinwheel, //done
        Hashtag, //done
        Ripple, 
        Fishspin,
        Meteor, //done
        Fireworks, //done
    }

    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Texture2D senseit, normalt, leftt, rightt, testp;
        public Sensei sensei;
        public Projectile projectile;
        public Player player;
        public static bool init = true, slow = true;
        public static List<Projectile> projectiles = new List<Projectile>();
        public double circleStep;
        public static int frames = 0, timesFired = 0, pastFrames = 0, tick = 0;
        public static KeyboardState keyboard;
        public static MouseState mouse;
        SMove move;
        SPhase phase;
        PMove proj;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 900;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.AllowUserResizing = false;
            sensei = new Sensei();
            projectile = new Projectile();
            player = new Player();
            this.Window.Title = "Club p";
            sensei.domo.X = 425;
            sensei.domo.Y = 300;
            sensei.speed.X = 0;
            sensei.speed.Y = 0;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            senseit = Content.Load<Texture2D>("sensei");
            normalt = Content.Load<Texture2D>("normal");
            leftt = Content.Load<Texture2D>("left");
            rightt = Content.Load<Texture2D>("right");
            testp = Content.Load<Texture2D>("testp");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboard = Keyboard.GetState();

            //move = SMove.Circle;
            proj = PMove.Meteor;
            PlayerMove(player);
            if (move == SMove.Circle)
            {
                circleStep += 0.035;
                sensei.domo.X = (float)(Math.Cos(circleStep) * 100) + 450;
                sensei.domo.Y = (float)(-1 * Math.Sin(circleStep) * 100) + 300;
            }
            if (proj == PMove.Pinwheel)
            {
                Pinwheel(projectiles, sensei);
            }
            else if (proj == PMove.Fireworks)
            {
                Fireworks(projectiles, sensei);
            }
            else if (proj == PMove.Hashtag)
            {
                Hashtag(projectiles);
            }
            else if (proj == PMove.Ripple)
            {
                Ripples(projectiles, sensei);
            }
            else if (proj == PMove.Meteor)
            {
                Meteor(projectiles);
            }
            else if (proj == PMove.Fishspin)
            {

            }
            int counter = 0;
            foreach (Projectile projectile in projectiles)
            {
                if (proj == PMove.Fireworks)
                {
                    if (counter < projectiles.Count - 16)
                    {
                        if (slow)
                        {
                            projectile.domo.X += projectile.speed.X;
                            projectile.domo.Y += projectile.speed.Y;
                        }
                        else
                        {
                            projectile.domo.X += projectile.speed.X * 2;
                            projectile.domo.Y += projectile.speed.Y * 2;
                        }
                    }
                    counter++;
                }
                else if (proj == PMove.Meteor)
                {
                    counter++;
                    projectile.speed.Y = projectile.speed.Y + (float)(projectile.gravity);
                    projectile.domo.X += projectile.speed.X;
                    projectile.domo.Y += projectile.speed.Y;
                }
                else
                {
                    projectile.domo.X += projectile.speed.X;
                    projectile.domo.Y += projectile.speed.Y;
                }
                projectile.hbox.X = projectile.domo.X + testp.Width / 2 - 5;
                projectile.hbox.Y = projectile.domo.Y + testp.Height / 2 - 5;
            }
            frames++;
            BoundaryCheck();
            CollisionCheck(player, projectiles);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.Draw(normalt, player.domo, Color.White);
            _spriteBatch.Draw(senseit, sensei.domo, Color.White);
            foreach (Projectile projectile in projectiles)
            {
                _spriteBatch.Draw(testp, projectile.domo, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void BoundaryCheck()
        {
            List<Projectile> validP = new List<Projectile>();
            foreach(Projectile projectile in projectiles)
            {
                if (projectile.domo.Y > _graphics.PreferredBackBufferHeight || projectile.domo.Y + testp.Height < 0)
                    continue;
                if (projectile.domo.X > _graphics.PreferredBackBufferWidth || projectile.domo.X + testp.Width < 0)
                    continue;
                validP.Add(projectile);
            }
            projectiles = validP;
        }

        public static void CollisionCheck(Player player, List<Projectile> projectiles)
        {
            foreach(Projectile projectile in projectiles)
            {
                if (player.hbox.Rect.Intersects(projectile.hbox.Rect))
                {
                    projectiles.Clear();
                    break;
                }
            }
        }

        public static void PlayerMove(Player player)
        {
            if (keyboard.IsKeyDown(Keys.Up) && keyboard.IsKeyDown(Keys.Left))
            {
                player.domo.X -= (float)Math.Sqrt(2);
                player.domo.Y -= (float)Math.Sqrt(2);
            }
            else if (keyboard.IsKeyDown(Keys.Left) && keyboard.IsKeyDown(Keys.Down))
            {
                player.domo.X -= (float)Math.Sqrt(2);
                player.domo.Y += (float)Math.Sqrt(2);
            }
            else if (keyboard.IsKeyDown(Keys.Down) && keyboard.IsKeyDown(Keys.Right))
            {
                player.domo.X += (float)Math.Sqrt(2);
                player.domo.Y += (float)Math.Sqrt(2);
            }
            else if (keyboard.IsKeyDown(Keys.Up) && keyboard.IsKeyDown(Keys.Right))
            {
                player.domo.X += (float)Math.Sqrt(2);
                player.domo.Y -= (float)Math.Sqrt(2);
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                player.domo.Y -= 2;
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                player.domo.Y += 2;
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                player.domo.X -= 2;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                player.domo.X += 2;
            }

            if (player.domo.X + normalt.Width > _graphics.PreferredBackBufferWidth)
                player.domo.X = _graphics.PreferredBackBufferWidth - normalt.Width;
            else if (player.domo.X < 0)
                player.domo.X = 0;

            if (player.domo.Y + normalt.Height > _graphics.PreferredBackBufferHeight)
                player.domo.Y = _graphics.PreferredBackBufferHeight - normalt.Height;
            else if (player.domo.Y < 0)
                player.domo.Y = 0;

            player.hbox.X = player.domo.X + normalt.Width/ 2 - 1;
            player.hbox.Y = player.domo.Y + normalt.Height/ 2 - 1;
        }

        public static void Hashtag(List<Projectile> projectiles)
        {
            if (timesFired < 30)
            {
                if (frames % (300 - (timesFired) * 10) == 0)
                {
                    timesFired++;
                    //Vertical
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2(_graphics.PreferredBackBufferWidth - testp.Width, (((_graphics.PreferredBackBufferHeight + 300) / 30) * i) - 150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(-2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(-2, (float)0.25);
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2(0, (((_graphics.PreferredBackBufferHeight + 300) / 30) * i) - 150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(2, (float)0.25);
                        projectiles.Add(projectile);
                    }

                    //Horizontal
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, 0);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, 2);
                        else
                            projectile.speed = new Vector2((float)0.25, 2);
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, _graphics.PreferredBackBufferHeight - testp.Height);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, -2);
                        else
                            projectile.speed = new Vector2((float)0.25, -2);
                        projectiles.Add(projectile);
                    }

                    foreach (Projectile projectile in projectiles)
                    {
                        projectile.domo += projectile.speed;
                    }
                }
            }
            else 
            {
                if(frames % 45 == 0)
                {
                    timesFired++;
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2(_graphics.PreferredBackBufferWidth - testp.Width, (((_graphics.PreferredBackBufferHeight + 300) / 30) * i) - 150);
                        else
                            projectile.domo = new Vector2(_graphics.PreferredBackBufferWidth - testp.Width, -150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(-2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(-2, (float)0.25);
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2(0, (((_graphics.PreferredBackBufferHeight + 300) / 30) * i) - 150);
                        else
                            projectile.domo = new Vector2(0, -150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(2, (float)0.25);
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, 0);
                        else
                            projectile.domo = new Vector2(-225 - timesFired * 3, 0);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, 2);
                        else
                            projectile.speed = new Vector2((float)0.25, 2);
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, _graphics.PreferredBackBufferHeight - testp.Height);
                        else
                            projectile.domo = new Vector2(-225 - timesFired * 3, _graphics.PreferredBackBufferHeight - testp.Height);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, -2);
                        else
                            projectile.speed = new Vector2((float)0.25, -2);
                        projectiles.Add(projectile);
                    }
                    foreach (Projectile projectile in projectiles)
                    {
                        projectile.domo += projectile.speed;
                    }
                }
            }
        }

        public static void Pinwheel(List<Projectile> projectiles, Sensei sensei)
        {
            if (frames % 4 == 0)
            {
                timesFired++;
                Vector2 startingSpeed = Vector2.Transform(Vector2.One, Matrix.CreateRotationZ((float)Math.PI / 30 * timesFired));

                for (int i = 0; i < 16; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.speed = Vector2.Transform(startingSpeed, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                    projectile.domo = new Vector2(sensei.domo.X - testp.Width / 2 + senseit.Width / 2, sensei.domo.Y - testp.Height / 2 + senseit.Height / 2);
                    projectile.domo += projectile.speed * 50;

                    projectiles.Add(projectile);
                }
            }
        }

        public static void Ripples(List<Projectile> projectiles, Sensei sensei)
        {
            if (frames % 30 == 0)
            {
                    timesFired++;
                    Vector2 startingSpeed = Vector2.Transform(Vector2.One, Matrix.CreateRotationZ((float)Math.PI / 30 * timesFired));
                    Teleport(sensei);
                    for (int i = 0; i < 16; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.speed = Vector2.Transform(startingSpeed, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                        projectile.domo = new Vector2(sensei.domo.X + senseit.Width / 2, sensei.domo.Y + senseit.Height / 2);
                        projectile.domo += projectile.speed * 50;

                        projectiles.Add(projectile);
                    }
            }
        }

        public static void Fireworks(List<Projectile> projectiles, Sensei sensei)
        {
            if (frames % 30 == 0)
            {
                if (slow)
                {
                    Teleport(sensei);
                    timesFired++;
                    Vector2 startingSpeed = Vector2.Transform(Vector2.One, Matrix.CreateRotationZ((float)Math.PI / 30 * timesFired));

                    for (int i = 0; i < 16; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.speed = Vector2.Transform(startingSpeed, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                        projectile.domo = new Vector2(sensei.domo.X + senseit.Width / 2, sensei.domo.Y + senseit.Height / 2);
                        projectile.domo += projectile.speed * 50;

                        projectiles.Add(projectile);
                    }
                    slow = false;
                }
                else
                {
                    timesFired++;
                    Vector2 startingSpeed = Vector2.Transform(Vector2.One, Matrix.CreateRotationZ((float)Math.PI / 30 * timesFired));

                    for (int i = 0; i < 16; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.speed = Vector2.Transform(startingSpeed, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                        projectile.domo = new Vector2(sensei.domo.X + senseit.Width / 2, sensei.domo.Y + senseit.Height / 2);
                        projectile.domo += projectile.speed * 50;

                        projectiles.Add(projectile);
                    }
                    slow = true;
                }

            }
        }

        public static void Meteor(List<Projectile> projectiles)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile projectile = new Projectile();
                Random d = new Random();
                Random g = new Random();
                Random v = new Random();
                projectile.domo.X = d.Next(0, _graphics.PreferredBackBufferWidth - testp.Width);
                projectile.domo.Y = 0;
                projectile.gravity = g.Next(1, 4) / 200.0;
                projectile.speed.X = (float)0.5 + (float)((v.Next(1, 11)) / 10.0);
                if (tick % 2 == 0)
                    projectile.speed.X = -projectile.speed.X;
                projectile.speed.Y = (float)0.1;
                projectiles.Add(projectile);
                tick++;
            }
            pastFrames = frames;
        }

        public static void Teleport(Sensei sensei)
        {
            Random X = new Random();
            Random Y = new Random();
            sensei.domo.X = X.Next(0, 827);
            sensei.domo.Y = Y.Next(0, 531);
        }
    }

    public class Sensei
    {
        public Vector2 speed, domo;
        public RectangleF hbox;

        public Sensei()
        {
            domo = new Vector2(450, 300);
        }
    }

    public class Player
    {
        public Vector2 speed, domo;
        public RectangleF hbox;

        public Player()
        {
            domo = new Vector2(450, 500);
            hbox = new RectangleF(0, 0, 2, 2);
        }
    }

    public class Projectile
    {
        public Vector2 speed, domo;
        public RectangleF hbox;
        public Texture2D tex;
        public double gravity;

        public Projectile()
        {
            domo = new Vector2(0, 0);
            speed = new Vector2(0, 50);
            hbox = new RectangleF(0, 0, 10, 10);
        }
    }
}