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
    //sensei tangents not working.  ask dad.
    //make a bomb where you can stop time and move wherever and place knives, then when the time unfreezes, it clears the projectiles it hits/attacks sensei
    //Set up how long the bomb will last, and then its cooldown.                                      

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
        Teleport //done
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
        Tangent,
        Meteor, //done
        Fireworks, //done
    }

    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Texture2D senseit, normalt, leftt, rightt, testp, particles, knife;
        public Sensei sensei;
        public Projectile projectile;
        public Player player;
        public static bool init = true, slow = true, special = false;
        public static List<Projectile> projectiles = new List<Projectile>();
        public static List<PlayerProjectile> playerProjectiles = new List<PlayerProjectile>();
        public static List<SpecialProjectile> specialProjectiles = new List<SpecialProjectile>();
        public static double circleStep;
        public static int frames = 0, timesFired = 0, pastFrames = 0, tick = 0, fireCol, specialFrames = 0;
        public static KeyboardState keyboard;
        public static MouseState mouse;
        public static Texture2D[] colours;
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
            sensei.domo.X = _graphics.PreferredBackBufferWidth / 2.0f - 36.5f;
            sensei.domo.Y = 0f;
            sensei.speed.X = 0f;
            sensei.speed.Y = 0f;
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
            particles = Content.Load<Texture2D>("projectiles");
            knife = Content.Load<Texture2D>("knife");
            Rectangle pSize;
            colours = new Texture2D[32];
            for (int i = 0; i < 16; i++)
            {
                pSize = new Rectangle(i * 16, 0, 16, 16);
                colours[i] = new Texture2D(GraphicsDevice, pSize.Width, pSize.Height);
                Color[] data = new Color[pSize.Width * pSize.Height];
                particles.GetData(0, pSize, data, 0, data.Length);
                colours[i].SetData(data);
            }
            for (int i = 16; i < 32; i++)
            {
                pSize = new Rectangle((i - 16) * 16, 16, 16, 16);
                colours[i] = new Texture2D(GraphicsDevice, pSize.Width, pSize.Height);
                Color[] data = new Color[pSize.Width * pSize.Height];
                particles.GetData(0, pSize, data, 0, data.Length);
                colours[i].SetData(data);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboard = Keyboard.GetState();
            PlayerMove(player);
            if (frames - specialFrames >= 600)
                special = false;
            if (!special)
            {
                //move = SMove.ToCenter;
                proj = PMove.Meteor;
                if (move == SMove.Circle)
                {
                    Circle(sensei);
                }
                else if (move == SMove.ToCenter)
                {
                    Centre(sensei);
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
                else if (proj == PMove.Meteor)
                {
                    Meteor();
                }
                else if (proj == PMove.Tangent)
                {
                    Tangent(projectiles, sensei, player);
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
                    projectile.hbox.X = projectile.domo.X + (testp.Width / 2.0f);
                    projectile.hbox.Y = projectile.domo.Y + (testp.Height / 2.0f);
                }
                sensei.domo.X += sensei.speed.X;
                sensei.domo.Y += sensei.speed.Y;
                Debug.WriteLine("D: " + sensei.domo);
                Debug.WriteLine("S: " + sensei.speed);
                foreach (PlayerProjectile pprojectile in playerProjectiles)
                {
                    pprojectile.domo.X += pprojectile.speed.X;
                    pprojectile.domo.Y += pprojectile.speed.Y;
                    pprojectile.hbox.X = pprojectile.domo.X + (knife.Width / 2.0f);
                    pprojectile.hbox.Y = pprojectile.domo.Y + (knife.Height / 2.0f);
                }
                foreach (SpecialProjectile sprojectile in specialProjectiles)
                {
                    sprojectile.domo.X += sprojectile.speed.X;
                    sprojectile.domo.Y += sprojectile.speed.Y;
                    sprojectile.hbox.X = sprojectile.domo.X + (knife.Width / 2.0f);
                    sprojectile.hbox.Y = sprojectile.domo.Y + (knife.Height / 2.0f);
                }
                CollisionCheck(player);
            }
            else
                Special(player);
            BoundaryCheck();
            frames++;
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
                _spriteBatch.Draw(projectile.tex, projectile.domo, Color.White);
            }
            foreach (PlayerProjectile pprojectile in playerProjectiles)
            {
                _spriteBatch.Draw(pprojectile.tex, pprojectile.domo, Color.White);
            }
            foreach (SpecialProjectile sprojectile in specialProjectiles)
            {
                _spriteBatch.Draw(sprojectile.tex, sprojectile.domo, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        //Misc

        public static void BoundaryCheck()
        {
            List<Projectile> validP = new List<Projectile>();
            foreach (Projectile projectile in projectiles)
            {
                if (projectile.domo.Y > _graphics.PreferredBackBufferHeight || projectile.domo.Y + testp.Height < 0)
                    continue;
                if (projectile.domo.X > _graphics.PreferredBackBufferWidth || projectile.domo.X + testp.Width < 0)
                    continue;
                validP.Add(projectile);
            }
            projectiles = validP;
        }

        public static void CollisionCheck(Player player)
        {
            foreach (Projectile projectile in projectiles)
            {
                double distance = Vector2.Distance(projectile.hbox, player.hbox);
                if (distance <= player.radius + projectile.radius)
                {
                    projectiles.Clear();
                    break;
                }
            }
            foreach (SpecialProjectile sprojectile in specialProjectiles)
            {
                List<Projectile> validP = new List<Projectile>();
                foreach (Projectile projectile in projectiles)
                {
                    double distance = Vector2.Distance(projectile.hbox, sprojectile.hbox);
                    if (distance <= sprojectile.radius + projectile.radius)
                    {
                        continue;
                    }
                    validP.Add(projectile);
                }
                projectiles = validP;
            }
        }

        public static void PlayerMove(Player player)
        {
            int shift = 0;
            if (keyboard.IsKeyDown(Keys.LeftShift))
                shift = 1;
            if (keyboard.IsKeyDown(Keys.Up) && keyboard.IsKeyDown(Keys.Left))
            {
                player.domo.X -= (float)Math.Sqrt(3 - shift);
                player.domo.Y -= (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Left) && keyboard.IsKeyDown(Keys.Down))
            {
                player.domo.X -= (float)Math.Sqrt(3 - shift);
                player.domo.Y += (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Down) && keyboard.IsKeyDown(Keys.Right))
            {
                player.domo.X += (float)Math.Sqrt(3 - shift);
                player.domo.Y += (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Up) && keyboard.IsKeyDown(Keys.Right))
            {
                player.domo.X += (float)Math.Sqrt(3 - shift);
                player.domo.Y -= (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                player.domo.Y -= 3 - shift;
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                player.domo.Y += 3 - shift;
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                player.domo.X -= 3 - shift;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                player.domo.X += 3 - shift;
            }

            if (player.domo.X + normalt.Width > _graphics.PreferredBackBufferWidth)
                player.domo.X = _graphics.PreferredBackBufferWidth - normalt.Width;
            else if (player.domo.X < 0)
                player.domo.X = 0;

            if (player.domo.Y + normalt.Height > _graphics.PreferredBackBufferHeight)
                player.domo.Y = _graphics.PreferredBackBufferHeight - normalt.Height;
            else if (player.domo.Y < 0)
                player.domo.Y = 0;

            player.hbox.X = player.domo.X + normalt.Width / 2 - 1;
            player.hbox.Y = player.domo.Y + normalt.Height / 2 - 1;
            if (keyboard.IsKeyDown(Keys.Z))
                Shoot(player);
            if (keyboard.IsKeyDown(Keys.X))
            {
                if (frames - specialFrames > 600)
                {
                    special = true;
                    specialFrames = frames;
                    Special(player);
                }
            }

        }

        public static void Shoot(Player player)
        {
            if (frames % 10 == 0)
            {
                PlayerProjectile pprojectile = new PlayerProjectile();
                pprojectile.tex = knife;
                pprojectile.domo = new Vector2((player.domo.X + normalt.Width / 2) - knife.Width / 2, (player.domo.Y + normalt.Height / 2) - knife.Height / 2);
                pprojectile.speed = new Vector2(0, -10);
                playerProjectiles.Add(pprojectile);
            }
        }

        public static void Special(Player player)
        {
            if (frames % 5 == 0)
            {
                SpecialProjectile sprojectile = new SpecialProjectile();
                sprojectile.tex = knife;
                sprojectile.domo = new Vector2((player.domo.X + normalt.Width / 2) - knife.Width / 2, (player.domo.Y + normalt.Height / 2) - knife.Height / 2);
                sprojectile.speed = new Vector2(0, -5);
                specialProjectiles.Add(sprojectile);
            }
        }
        //Attacks
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
                if (frames % 45 == 0)
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
                    projectile.tex = colours[18];
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
                    Random c = new Random();
                    fireCol = c.Next(16, 32);
                    for (int i = 0; i < 16; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.speed = Vector2.Transform(startingSpeed, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                        projectile.domo = new Vector2(sensei.domo.X + senseit.Width / 2, sensei.domo.Y + senseit.Height / 2);
                        projectile.domo += projectile.speed * 50;
                        projectile.tex = colours[fireCol];
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
                        projectile.tex = colours[fireCol];
                        projectiles.Add(projectile);
                    }
                    slow = true;
                }

            }
        }

        public static void Meteor()
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile projectile = new Projectile();
                Random d = new Random();
                Random g = new Random();
                Random v = new Random();
                Random c = new Random();
                projectile.domo.X = d.Next(0, _graphics.PreferredBackBufferWidth - testp.Width);
                projectile.domo.Y = 0;
                projectile.gravity = g.Next(1, 4) / 200.0;
                projectile.speed.X = (float)0.5 + (float)((v.Next(1, 11)) / 10.0);
                if (tick % 2 == 0)
                    projectile.speed.X = -projectile.speed.X;
                projectile.speed.Y = (float)0.1;
                int col = c.Next(0, 16);
                projectile.tex = colours[col];
                projectiles.Add(projectile);
                tick++;
            }
            pastFrames = frames;
        }

        public static void Tangent(List<Projectile> projectiles, Sensei sensei, Player player)
        {
            for (int i = 0; i < 16; i++)
            {
                Projectile projectile = new Projectile();
                projectile.speed = Vector2.Transform(Vector2.One, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                projectile.domo = new Vector2(sensei.domo.X - testp.Width / 2 + senseit.Width / 2, sensei.domo.Y - testp.Height / 2 + senseit.Height / 2);
                projectile.domo += projectile.speed * 50;
                Vector2 instantaneous = new Vector2((player.domo.X + senseit.Width / 2) - (projectile.domo.X + testp.Width / 2), (player.domo.Y + senseit.Height / 2) - (projectile.domo.Y + testp.Height / 2));
                projectile.speed = instantaneous;
                projectile.tex = colours[18];
                projectiles.Add(projectile);
            }
        }

        //Move

        public static void Circle(Sensei sensei)
        {
            circleStep += 0.035;
            sensei.domo.X = (float)(Math.Cos(circleStep) * 100) + 450f;
            sensei.domo.Y = (float)(-1 * Math.Sin(circleStep) * 100) + 300f;
        }

        public static void Centre(Sensei sensei)
        {
            Vector2 centre = new Vector2((_graphics.PreferredBackBufferWidth / 2.0f) - (senseit.Width / 2.0f), (_graphics.PreferredBackBufferHeight / 2.0f) - (senseit.Height / 2.0f));

            Debug.WriteLine("Center:" + centre);
            if (Vector2.Distance(sensei.domo, centre) < 1)
                sensei.domo = centre;
            if (sensei.domo != centre)
            {
                Vector2 distance = new Vector2(centre.X - sensei.domo.X, centre.Y - sensei.domo.Y);
                Debug.WriteLine("distance:" + distance);
                Debug.WriteLine("distance (calc):" + Vector2.Distance(centre, sensei.domo));
                Debug.WriteLine("distanceX:" + distance.X);
                distance.X = distance.X / Vector2.Distance(centre, sensei.domo);
                Debug.WriteLine("distanceY:" + distance.Y);
                Debug.WriteLine("distanceY (calc):" + distance.Y / Vector2.Distance(centre, sensei.domo));
                distance.Y = distance.Y / Vector2.Distance(centre, sensei.domo);
                Debug.WriteLine("unit vector:" + distance);
                sensei.speed = distance;
            }
            else
                sensei.speed = new Vector2(0, 0);
        }
        public static void Teleport(Sensei sensei)
        {
            Random X = new Random();
            Random Y = new Random();
            sensei.domo.X = (float)X.Next(0, 827);
            sensei.domo.Y = (float)Y.Next(0, 531);
        }
    }

    public class Sensei
    {
        public Vector2 speed, domo, hbox;
        public float radius;

        public Sensei()
        {
            domo = new Vector2(450, 300);
            radius = 24;
        }
    }

    public class Player
    {
        public Vector2 speed, domo, hbox;
        public float radius;

        public Player()
        {
            domo = new Vector2(450, 500);
            radius = 1;
        }
    }

    public class Projectile
    {
        public Vector2 speed, domo, hbox;
        public Texture2D tex;
        public float radius;
        public double gravity;

        public Projectile()
        {
            domo = new Vector2(0, 0);
            speed = new Vector2(0, 50);
            radius = 5;
        }
    }

    public class SpecialProjectile
    {
        public Vector2 speed, domo, hbox;
        public Texture2D tex;
        public float radius;
        public double gravity;

        public SpecialProjectile()
        {
            domo = new Vector2(0, 0);
            speed = new Vector2(0, 0);
            radius = 10;
        }
    }

    public class PlayerProjectile
    {
        public Vector2 speed, domo, hbox;
        public Texture2D tex;
        public float radius;
        public double gravity;

        public PlayerProjectile()
        {
            domo = new Vector2(0, 0);
            speed = new Vector2(0, 0);
            radius = 5;
        }
    }
}