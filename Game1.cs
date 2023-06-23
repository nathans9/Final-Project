using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Final_Project
{
    //TODO
    //have a cooldown after you're hit and respawning
    //have your character die when hit
    //have working transitions between the moves and phases
    //Collision for sensei

    public enum SPhase
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }


    public enum Screen
    {
        Menu, //done
        Controls,
        Game,
        Pause,
        End
    }

    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Texture2D menubg,
            senseief,
            tintt,
            effectt,
            hudback,
            senseit,
            normalt,
            leftt,
            rightt,
            testp,
            particles,
            knife,
            kurenai,
            background,
            hudsheet,
            bstart,
            pstart,
            hpt,
            bigsensei;
        public Sensei sensei;
        public SpriteFont penguinFont, boldFont, italicFont;
        public Projectile projectile;
        public Player player;
        public static bool init = true, slow = true, special = false, death = false, iSong = true, start, controls, quit, bossRunning = false, senseiDeath = false;
        public static List<Projectile> projectiles = new List<Projectile>();
        public static List<PlayerProjectile> playerProjectiles = new List<PlayerProjectile>();
        public static List<SpecialProjectile> specialProjectiles = new List<SpecialProjectile>();
        public static double circleStep;
        public static int meffect = 0,
            iFrames = -60,
            rotationFrames,
            frames = 0,
            timesFired = 0,
            pastFrames = 0,
            framesPressed = 0,
            tick = 0,
            fireCol,
            specialFrames = -600,
            backgroundDisplacement = -30,
            screenHeight = 600,
            moveLength,
            totalMoveLength = 30,
            beginningMove,
            deathFrames;
        public static float[] idealTitleLoco, titleShadowLoco, titleLoco, idealMenuLocoX, menuShadowLocoX, menuLocoX, idealMenuLocoY, menuShadowLocoY, menuLocoY;
        public static KeyboardState keyboard;
        public static MouseState mouse;
        public static Texture2D[] colours, maxeffects, tints, senseieffects;
        public static Rectangle backgroundR;
        public static float rotation, transparency = 0.0f, senseiTransparency = 1.0f;
        public Song boss, idle, bossloop, idleloop;
        public static SoundEffect blast1, blast2, blast3, die, exit, ok, pause, powerup, senseidie, senseihurt, shoot, specialfx, specialshoot, select;
        public System.Drawing.Color enemyColour, playerColour, bombColour, powerColour, menuTextC, platinum, platinumShadow, color, selectColour;
        public static SPhase phase;
        public static Screen screen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = screenHeight + 50;
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
            backgroundR = new Rectangle(0, -600, 900, 1200);
            this.Window.Title = "Club p";
            sensei.domo.X = _graphics.PreferredBackBufferWidth / 2.0f - 36.5f;
            sensei.domo.Y = screenHeight / 2.0f - 34.5f;
            player.domo.X = _graphics.PreferredBackBufferWidth / 2.0f - 19.0f;
            player.domo.Y = screenHeight / 2.0f - 18.0f + 175;
            sensei.speed.X = 0f;
            sensei.speed.Y = 0f;
            enemyColour = System.Drawing.ColorTranslator.FromHtml("#93A0A4");
            playerColour = System.Drawing.ColorTranslator.FromHtml("#2E47AA");
            bombColour = System.Drawing.ColorTranslator.FromHtml("#04B418");
            powerColour = System.Drawing.ColorTranslator.FromHtml("#F7C70E");
            menuTextC = System.Drawing.ColorTranslator.FromHtml("#D62300");
            platinum = System.Drawing.ColorTranslator.FromHtml("#989898");
            platinumShadow = System.Drawing.ColorTranslator.FromHtml("#B70000");
            selectColour = System.Drawing.ColorTranslator.FromHtml("#FFDEDE");
            idealTitleLoco = new float[15];
            titleShadowLoco = new float[15];
            titleLoco = new float[15];
            idealTitleLoco[0] = 30;
            idealTitleLoco[1] = 145;
            idealTitleLoco[2] = 180;
            idealTitleLoco[3] = 237;
            idealTitleLoco[4] = 312;
            idealTitleLoco[5] = 344;
            idealTitleLoco[6] = 400;
            idealTitleLoco[7] = 425;
            idealTitleLoco[8] = 300;
            idealTitleLoco[9] = 335;
            idealTitleLoco[10] = 370;
            idealTitleLoco[11] = 405;
            idealTitleLoco[12] = 440;
            idealTitleLoco[13] = 475;
            idealTitleLoco[14] = 495;
            idealMenuLocoX = new float[17];
            menuShadowLocoX = new float[17];
            menuLocoX = new float[17];
            idealMenuLocoY = new float[17];
            menuShadowLocoY = new float[17];
            menuLocoY = new float[17];
            idealMenuLocoX[0] = 650;
            idealMenuLocoX[1] = 673;
            idealMenuLocoX[2] = 690;
            idealMenuLocoX[3] = 715;
            idealMenuLocoX[4] = 735;
            idealMenuLocoX[5] = 650;
            idealMenuLocoX[6] = 671;
            idealMenuLocoX[7] = 690;
            idealMenuLocoX[8] = 710;
            idealMenuLocoX[9] = 729;
            idealMenuLocoX[10] = 749;
            idealMenuLocoX[11] = 769;
            idealMenuLocoX[12] = 780;
            idealMenuLocoX[13] = 650;
            idealMenuLocoX[14] = 672;
            idealMenuLocoX[15] = 694;
            idealMenuLocoX[16] = 705;
            idealMenuLocoY[0] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
            idealMenuLocoY[1] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
            idealMenuLocoY[2] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
            idealMenuLocoY[3] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
            idealMenuLocoY[4] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
            idealMenuLocoY[5] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[6] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[7] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[8] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[9] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[10] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[11] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[12] = _graphics.PreferredBackBufferHeight / 2.0f;
            idealMenuLocoY[13] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
            idealMenuLocoY[14] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
            idealMenuLocoY[15] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
            idealMenuLocoY[16] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;


            for (int i = 0; i < 15; i++)
            {
                if (i < 8)
                {
                    titleShadowLoco[i] = idealTitleLoco[i] - 1000.0f;
                    titleLoco[i] = idealTitleLoco[i] + 1000.0f;
                }
                else
                {
                    titleShadowLoco[i] = idealTitleLoco[i] + 1000.0f;
                    titleLoco[i] = idealTitleLoco[i] - 1000.0f;
                }
            }
            for (int i = 0; i < 17; i++)
            {
                menuLocoY[i] = idealMenuLocoY[i] - 1000f;
                menuShadowLocoY[i] = idealMenuLocoY[i] + 1000f;
            }
            player.tex = normalt;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            MediaPlayer.IsRepeating = true;
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
            background = Content.Load<Texture2D>("background");
            pstart = Content.Load<Texture2D>("pstar");
            bstart = Content.Load<Texture2D>("bstar");
            hpt = Content.Load<Texture2D>("hp");
            kurenai = Content.Load<Texture2D>("kurenai");
            penguinFont = Content.Load<SpriteFont>("File");
            boldFont = Content.Load<SpriteFont>("Bold");
            hudback = Content.Load<Texture2D>("hudback");
            menubg = Content.Load<Texture2D>("menubg");
            idle = Content.Load<Song>("(PC-98 Remix) Inabakumori - Lagtrain");
            boss = Content.Load<Song>("Bad Apple!! (Alternate Version) - Touhou 4_ Lotus Land Story");
            bigsensei = Content.Load<Texture2D>("Sensei (1)");
            italicFont = Content.Load<SpriteFont>("Italic");
            blast1 = Content.Load<SoundEffect>("blast1");
            blast2 = Content.Load<SoundEffect>("blast2");
            blast3 = Content.Load<SoundEffect>("blast3");
            die = Content.Load<SoundEffect>("die");
            exit = Content.Load<SoundEffect>("exit");
            ok = Content.Load<SoundEffect>("ok");
            pause = Content.Load<SoundEffect>("pause");
            powerup = Content.Load<SoundEffect>("powerup");
            select = Content.Load<SoundEffect>("select");
            senseidie = Content.Load<SoundEffect>("senseidie");
            senseihurt = Content.Load<SoundEffect>("senseihurt");
            specialfx = Content.Load<SoundEffect>("special");
            shoot = Content.Load<SoundEffect>("shoot");
            specialshoot = Content.Load<SoundEffect>("specialshoot");

            player.tex = normalt;
            Rectangle pSize;
            colours = new Texture2D[32];
            maxeffects = new Texture2D[4];
            tints = new Texture2D[4];
            senseieffects = new Texture2D[4];
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
            for (int i = 0; i < 4; i++)
            {
                maxeffects[i] = Content.Load<Texture2D>($"maxeffect{i + 1}");
            }
            for (int i = 0; i < 4; i++)
            {
                tints[i] = Content.Load<Texture2D>($"tint{i + 1}");
            }
            for (int i = 0; i < 4; i++)
            {
                senseieffects[i] = Content.Load<Texture2D>($"senseieffect{i + 1}");
            }
            effectt = maxeffects[0];
            tintt = tints[0];
            senseief = senseieffects[0];
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            keyboard = Keyboard.GetState();
            if (screen == Screen.Menu)
            {
                idealMenuLocoX[0] = 650;
                idealMenuLocoX[1] = 673;
                idealMenuLocoX[2] = 690;
                idealMenuLocoX[3] = 715;
                idealMenuLocoX[4] = 735;
                idealMenuLocoX[5] = 650;
                idealMenuLocoX[6] = 671;
                idealMenuLocoX[7] = 690;
                idealMenuLocoX[8] = 710;
                idealMenuLocoX[9] = 729;
                idealMenuLocoX[10] = 749;
                idealMenuLocoX[11] = 769;
                idealMenuLocoX[12] = 780;
                idealMenuLocoX[13] = 650;
                idealMenuLocoX[14] = 672;
                idealMenuLocoX[15] = 694;
                idealMenuLocoX[16] = 705;
                idealMenuLocoY[0] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[1] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[2] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[3] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[4] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[5] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[6] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[7] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[8] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[9] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[10] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[11] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[12] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[13] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                idealMenuLocoY[14] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                idealMenuLocoY[15] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                idealMenuLocoY[16] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                Keyboardd.GetState();
                if (iSong)
                {
                    MediaPlayer.Play(idle);
                    iSong = false;
                }
                if (titleShadowLoco[0] != idealTitleLoco[0])
                {
                    for (int i = 0; i < 15; i++)
                    {
                        if (i < 8)
                        {
                            titleShadowLoco[i] += 20;
                            titleLoco[i] -= 20;
                        }
                        else
                        {
                            titleShadowLoco[i] -= 20;
                            titleLoco[i] += 20;
                        }
                    }
                }
                else
                {
                    if (1.0f - transparency <= 0.01)
                    {
                        transparency = 1.0f;
                    }
                    if (transparency != 1.0f)
                    {
                        transparency += 0.01f;
                    }
                    else
                    {
                        if (idealMenuLocoY[0] != menuLocoY[0])
                        {
                            for (int i = 0; i < 17; i++)
                            {
                                menuLocoX[i] = idealMenuLocoX[i];
                                menuShadowLocoX[i] = idealMenuLocoX[i];
                                menuLocoY[i] += 40;
                                menuShadowLocoY[i] -= 40;
                            }
                        }
                        else
                        {
                            if (init)
                            {
                                start = true;
                                init = false;
                            }
                            if (Keyboardd.HasBeenPressed(Keys.Down))
                            {
                                select.Play();
                                if (start)
                                {
                                    start = false;
                                    controls = true;
                                }
                                else if (controls)
                                {
                                    controls = false;
                                    quit = true;
                                }
                            }
                            if (Keyboardd.HasBeenPressed(Keys.Up))
                            {
                                select.Play();
                                if (controls)
                                {
                                    start = true;
                                    controls = false;
                                }
                                else if (quit)
                                {
                                    controls = true;
                                    quit = false;
                                }
                            }
                            if (Keyboardd.HasBeenPressed(Keys.Z))
                            {
                                if (start)
                                {
                                    ok.Play();
                                    screen = Screen.Game;
                                    bossRunning = true;
                                    iSong = true;
                                    init = true;
                                    MediaPlayer.Stop();
                                }
                                else if (controls)
                                {
                                    ok.Play();
                                    screen = Screen.Controls;
                                }
                                else if (quit)
                                {
                                    exit.Play();
                                    MediaPlayer.Stop();
                                    Environment.Exit(0);
                                }
                            }
                        }
                    }
                }
            }
            else if (screen == Screen.Game)
            {
                Keyboardd.GetState();
                if (iSong)
                {
                    MediaPlayer.Play(boss);
                    iSong = false;
                }
                if (frames - iFrames <= 60)
                    death = true;
                else
                    death = false;
                if (death)
                {
                    Centre(sensei);
                    sensei.domo.X += sensei.speed.X * ((Vector2.Distance(new Vector2(_graphics.PreferredBackBufferWidth / 2.0f - senseit.Width, _graphics.PreferredBackBufferHeight / 2.0f - senseit.Height), sensei.domo)) / 60.0f);
                    sensei.domo.Y += sensei.speed.Y * ((Vector2.Distance(new Vector2(_graphics.PreferredBackBufferWidth / 2.0f - senseit.Width, _graphics.PreferredBackBufferHeight / 2.0f - senseit.Height), sensei.domo)) / 60.0f); ;
                    special = false;
                    if (init)
                    {
                        if (player.domo.Y != screenHeight / 2.0f + 180)
                            player.domo.Y -= player.speed.Y;
                        else
                            init = false;
                    }
                    else
                    {
                        PlayerMove(player);
                    }
                }
                else
                {
                    PlayerMove(player);
                    if (frames - specialFrames >= 600)
                        special = false;
                    if (!special)
                    {
                        SenseiUpdate(sensei, player);
                        int counter = 0;
                        foreach (Projectile projectile in projectiles)
                        {
                            if (phase == SPhase.Eight)
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
                            else if (phase == SPhase.Five)
                            {
                                projectile.speed.Y = projectile.speed.Y + (float)(projectile.gravity);
                                projectile.domo.X += projectile.speed.X;
                                projectile.domo.Y += projectile.speed.Y;
                                counter++;
                            }
                            else if (phase == SPhase.One)
                            {
                                projectile.speed.Y = projectile.speed.Y + projectile.speed.Y * 0.002f;
                                projectile.speed.X = projectile.speed.X + projectile.speed.X * 0.002f;
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
                        sensei.hbox.X = sensei.domo.X + senseit.Width / 2.0f;
                        sensei.hbox.Y = sensei.domo.Y + senseit.Height / 2.0f;
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
                        CollisionCheck(player, sensei);
                        Background();
                    }
                    else
                        Special(player);
                    Effects(player);
                    BoundaryCheck();
                }
                frames++;
            }
            else if (screen == Screen.Pause)
            {
                Keyboardd.GetState();
                idealMenuLocoY[0] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[1] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[2] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[3] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[4] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                idealMenuLocoY[5] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[6] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[7] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[8] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[9] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[10] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[11] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[12] = _graphics.PreferredBackBufferHeight / 2.0f;
                idealMenuLocoY[13] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                idealMenuLocoY[14] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                idealMenuLocoY[15] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                idealMenuLocoY[16] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                if (idealMenuLocoY[0] != menuLocoY[0])
                {
                    for (int i = 0; i < 17; i++)
                    {
                        menuLocoX[i] = idealMenuLocoX[i];
                        menuShadowLocoX[i] = idealMenuLocoX[i];
                        menuLocoY[i] += 40;
                        menuShadowLocoY[i] -= 40;
                    }
                }
                else
                {
                    if (init)
                    {
                        start = true;
                        init = false;
                    }
                    if (Keyboardd.HasBeenPressed(Keys.Down))
                    {
                        select.Play();
                        if (start)
                        {
                            start = false;
                            controls = true;
                        }
                        else if (controls)
                        {
                            controls = false;
                            quit = true;
                        }
                    }
                    if (Keyboardd.HasBeenPressed(Keys.Up))
                    {
                        select.Play();
                        if (controls)
                        {
                            start = true;
                            controls = false;
                        }
                        else if (quit)
                        {
                            controls = true;
                            quit = false;
                        }
                    }
                    if (Keyboardd.HasBeenPressed(Keys.Z))
                    {
                        if (start)
                        {
                            ok.Play();
                            screen = Screen.Game;
                            init = true;
                        }
                        else if (controls)
                        {
                            ok.Play();
                            screen = Screen.Controls;
                        }
                        else if (quit)
                        {
                            exit.Play();
                            MediaPlayer.Stop();
                            iSong = true;
                            start = true;
                            init = true;
                            quit = false;
                            bossRunning = false;
                            screen = Screen.Menu;
                        }
                    }
                    if (Keyboardd.HasBeenPressed(Keys.Escape))
                    {
                        ok.Play();
                        screen = Screen.Game;
                        start = true;
                        controls = false;
                        quit = false;
                        init = true;
                    }
                }
            }
            else if (screen == Screen.Controls)
            {
                Keyboardd.GetState();

                if (bossRunning)
                {
                    if (Keyboardd.HasBeenPressed(Keys.Escape))
                    {
                        exit.Play();
                        screen = Screen.Pause;
                    }
                }
                else
                {
                    if (Keyboardd.HasBeenPressed(Keys.Escape))
                    {
                        exit.Play();
                        screen = Screen.Menu;
                    }
                }
            }
            else if (screen == Screen.End)
            {
                Keyboardd.GetState();
                if (Keyboardd.HasBeenPressed(Keys.Z))
                {
                    exit.Play();
                    screen = Screen.Menu;
                    phase = SPhase.One;
                    sensei.health = 750;
                    projectiles.Clear();
                    playerProjectiles.Clear();
                    specialProjectiles.Clear();
                    player.lives = 9;
                    player.bombs = 4;
                    sensei.domo.X = _graphics.PreferredBackBufferWidth / 2.0f - 36.5f;
                    sensei.domo.Y = screenHeight / 2.0f - 34.5f;
                    player.domo.X = _graphics.PreferredBackBufferWidth / 2.0f - 19.0f;
                    player.domo.Y = screenHeight / 2.0f - 18.0f + 175;
                    sensei.speed.X = 0f;
                    sensei.speed.Y = 0f;
                    titleLoco = new float[15];
                    idealTitleLoco[0] = 30;
                    idealTitleLoco[1] = 145;
                    idealTitleLoco[2] = 180;
                    idealTitleLoco[3] = 237;
                    idealTitleLoco[4] = 312;
                    idealTitleLoco[5] = 344;
                    idealTitleLoco[6] = 400;
                    idealTitleLoco[7] = 425;
                    idealTitleLoco[8] = 300;
                    idealTitleLoco[9] = 335;
                    idealTitleLoco[10] = 370;
                    idealTitleLoco[11] = 405;
                    idealTitleLoco[12] = 440;
                    init = true; slow = true; special = false; death = false; iSong = true; bossRunning = false; senseiDeath = false;
                    idealTitleLoco[13] = 475;
                    idealTitleLoco[14] = 495;
                    idealMenuLocoX = new float[17];
                    menuShadowLocoX = new float[17];
                    menuLocoX = new float[17];
                    idealMenuLocoY = new float[17];
                    menuShadowLocoY = new float[17];
                    menuLocoY = new float[17];
                    idealMenuLocoX[0] = 650;
                    idealMenuLocoX[1] = 673;
                    idealMenuLocoX[2] = 690;
                    idealMenuLocoX[3] = 715;
                    idealMenuLocoX[4] = 735;
                    idealMenuLocoX[5] = 650;
                    idealMenuLocoX[6] = 671;
                    idealMenuLocoX[7] = 690;
                    idealMenuLocoX[8] = 710;
                    idealMenuLocoX[9] = 729;
                    idealMenuLocoX[10] = 749;
                    idealMenuLocoX[11] = 769;
                    idealMenuLocoX[12] = 780;
                    idealMenuLocoX[13] = 650;
                    idealMenuLocoX[14] = 672;
                    idealMenuLocoX[15] = 694;
                    idealMenuLocoX[16] = 705;
                    idealMenuLocoY[0] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                    idealMenuLocoY[1] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                    idealMenuLocoY[2] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                    idealMenuLocoY[3] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                    idealMenuLocoY[4] = _graphics.PreferredBackBufferHeight / 2.0f - 75.0f;
                    idealMenuLocoY[5] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[6] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[7] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[8] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[9] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[10] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[11] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[12] = _graphics.PreferredBackBufferHeight / 2.0f;
                    idealMenuLocoY[13] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                    idealMenuLocoY[14] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                    idealMenuLocoY[15] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;
                    idealMenuLocoY[16] = _graphics.PreferredBackBufferHeight / 2.0f + 75.0f;


                    for (int i = 0; i < 15; i++)
                    {
                        if (i < 8)
                        {
                            titleShadowLoco[i] = idealTitleLoco[i] - 1000.0f;
                            titleLoco[i] = idealTitleLoco[i] + 1000.0f;
                        }
                        else
                        {
                            titleShadowLoco[i] = idealTitleLoco[i] + 1000.0f;
                            titleLoco[i] = idealTitleLoco[i] - 1000.0f;
                        }
                    }
                    for (int i = 0; i < 17; i++)
                    {
                        menuLocoY[i] = idealMenuLocoY[i] - 1000f;
                        menuShadowLocoY[i] = idealMenuLocoY[i] + 1000f;
                    }
                    player.tex = normalt;
                    transparency = 0.0f; senseiTransparency = 1.0f;
                    meffect = 0;
                    iFrames = -60;
                    frames = 0;
                    timesFired = 0;
                    pastFrames = 0;
                    framesPressed = 0;
                    tick = 0;
                    specialFrames = -600;
                    backgroundDisplacement = -30;
                    screenHeight = 600;
                    totalMoveLength = 30;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            if (screen == Screen.Menu)
            {
                //misc
                _spriteBatch.Draw(menubg, new Rectangle(0, 0, 900, 650), Color.White);
                _spriteBatch.Draw(bigsensei, new Vector2(100, 100), Color.White * transparency);
                //Title Shadwo
                _spriteBatch.DrawString(boldFont, "Emb", new Vector2(titleShadowLoco[0] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "o", new Vector2(titleShadowLoco[1] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "di", new Vector2(titleShadowLoco[2] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "me", new Vector2(titleShadowLoco[3] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "n", new Vector2(titleShadowLoco[4] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "t", new Vector2(titleShadowLoco[5] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "o", new Vector2(titleShadowLoco[6] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "f", new Vector2(titleShadowLoco[7] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "P", new Vector2(titleShadowLoco[8] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "e", new Vector2(titleShadowLoco[9] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "n", new Vector2(titleShadowLoco[10] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "g", new Vector2(titleShadowLoco[11] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "u", new Vector2(titleShadowLoco[12] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "i", new Vector2(titleShadowLoco[13] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "n", new Vector2(titleShadowLoco[14] + 3.0f, _graphics.PreferredBackBufferHeight / 2.0f + 3.0f), Color.Black);
                //Title
                _spriteBatch.DrawString(boldFont, "Emb", new Vector2(titleLoco[0], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "o", new Vector2(titleLoco[1], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "di", new Vector2(titleLoco[2], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "me", new Vector2(titleLoco[3], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "n", new Vector2(titleLoco[4], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "t", new Vector2(titleLoco[5], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "o", new Vector2(titleLoco[6], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "f", new Vector2(titleLoco[7], _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "P", new Vector2(titleLoco[8], _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "e", new Vector2(titleLoco[9], _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "n", new Vector2(titleLoco[10], _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "g", new Vector2(titleLoco[11], _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "u", new Vector2(titleLoco[12], _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "i", new Vector2(titleLoco[13], _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(boldFont, "n", new Vector2(titleLoco[14], _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                //Options shadwo
                _spriteBatch.DrawString(italicFont, "S", new Vector2(menuShadowLocoX[0] + 2.0f, menuShadowLocoY[0] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[1] + 2.0f, menuShadowLocoY[1] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "a", new Vector2(menuShadowLocoX[2] + 2.0f, menuShadowLocoY[2] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "r", new Vector2(menuShadowLocoX[3] + 2.0f, menuShadowLocoY[3] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[4] + 2.0f, menuShadowLocoY[4] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "C", new Vector2(menuShadowLocoX[5] + 2.0f, menuShadowLocoY[5] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "o", new Vector2(menuShadowLocoX[6] + 2.0f, menuShadowLocoY[6] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "n", new Vector2(menuShadowLocoX[7] + 2.0f, menuShadowLocoY[7] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[8] + 2.0f, menuShadowLocoY[8] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "r", new Vector2(menuShadowLocoX[9] + 2.0f, menuShadowLocoY[9] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "o", new Vector2(menuShadowLocoX[10] + 2.0f, menuShadowLocoY[10] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "l", new Vector2(menuShadowLocoX[11] + 2.0f, menuShadowLocoY[11] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "s", new Vector2(menuShadowLocoX[12] + 2.0f, menuShadowLocoY[12] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "Q", new Vector2(menuShadowLocoX[13] + 2.0f, menuShadowLocoY[13] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "u", new Vector2(menuShadowLocoX[14] + 2.0f, menuShadowLocoY[14] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "i", new Vector2(menuShadowLocoX[15] + 2.0f, menuShadowLocoY[15] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[16] + 2.0f, menuShadowLocoY[16] + 2.0f), Color.Black);
                //Options
                if (start)
                {
                    _spriteBatch.DrawString(italicFont, "S", new Vector2(menuLocoX[0], menuLocoY[0]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[1], menuLocoY[1]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "a", new Vector2(menuLocoX[2], menuLocoY[2]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[3], menuLocoY[3]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[4], menuLocoY[4]), XNAColor(selectColour));
                }
                else
                {
                    _spriteBatch.DrawString(italicFont, "S", new Vector2(menuLocoX[0], menuLocoY[0]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[1], menuLocoY[1]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "a", new Vector2(menuLocoX[2], menuLocoY[2]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[3], menuLocoY[3]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[4], menuLocoY[4]), XNAColor(menuTextC));
                }
                if (controls)
                {
                    _spriteBatch.DrawString(italicFont, "C", new Vector2(menuLocoX[5], menuLocoY[5]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[6], menuLocoY[6]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "n", new Vector2(menuLocoX[7], menuLocoY[7]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[8], menuLocoY[8]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[9], menuLocoY[9]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[10], menuLocoY[10]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "l", new Vector2(menuLocoX[11], menuLocoY[11]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "s", new Vector2(menuLocoX[12], menuLocoY[12]), XNAColor(selectColour));
                }
                else
                {
                    _spriteBatch.DrawString(italicFont, "C", new Vector2(menuLocoX[5], menuLocoY[5]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[6], menuLocoY[6]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "n", new Vector2(menuLocoX[7], menuLocoY[7]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[8], menuLocoY[8]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[9], menuLocoY[9]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[10], menuLocoY[10]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "l", new Vector2(menuLocoX[11], menuLocoY[11]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "s", new Vector2(menuLocoX[12], menuLocoY[12]), XNAColor(menuTextC));
                }
                if (quit)
                {
                    _spriteBatch.DrawString(italicFont, "Q", new Vector2(menuLocoX[13], menuLocoY[13]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "u", new Vector2(menuLocoX[14], menuLocoY[14]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "i", new Vector2(menuLocoX[15], menuLocoY[15]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[16], menuLocoY[16]), XNAColor(selectColour));
                }
                else
                {
                    _spriteBatch.DrawString(italicFont, "Q", new Vector2(menuLocoX[13], menuLocoY[13]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "u", new Vector2(menuLocoX[14], menuLocoY[14]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "i", new Vector2(menuLocoX[15], menuLocoY[15]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[16], menuLocoY[16]), XNAColor(menuTextC));
                }
            }
            else if (screen == Screen.Game)
            {
                _spriteBatch.Draw(background, backgroundR, Color.White);
                _spriteBatch.Draw(senseief, new Vector2(sensei.domo.X + senseit.Width / 2.0f, sensei.domo.Y + senseit.Height / 2.0f), null, Color.White, rotation, new Vector2(senseief.Width / 2.0f, senseief.Height / 2.0f), 1, SpriteEffects.None, 0);
                if (player.power > 2)
                {
                    _spriteBatch.Draw(effectt, new Vector2(player.domo.X - 6.0f, player.domo.Y - 12.0f), Color.White);
                    _spriteBatch.Draw(tintt, player.domo, Color.White);
                }
                else
                    _spriteBatch.Draw(player.tex, player.domo, Color.White);
                _spriteBatch.Draw(senseit, sensei.domo, Color.White * senseiTransparency);
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
                if (player.domo.Y > 75)
                {
                    _spriteBatch.DrawString(penguinFont, "Enemy", new Vector2(10, 10), XNAColor(enemyColour));
                    for (int i = 0; i < sensei.health; i++)
                    {
                        _spriteBatch.Draw(hpt, new Rectangle(125 + i, 24, 1, 16), Color.White);
                    }
                }
                else
                {
                    _spriteBatch.DrawString(penguinFont, "Enemy", new Vector2(10, 10), XNAColor(enemyColour) * 0.2f);
                    for (int i = 0; i < sensei.health; i++)
                    {
                        _spriteBatch.Draw(hpt, new Rectangle(125 + i, 24, 1, 16), Color.White * 0.2f);
                    }
                }
                _spriteBatch.Draw(hudback, new Rectangle(0, screenHeight, _graphics.PreferredBackBufferWidth, 50), Color.White);
                _spriteBatch.DrawString(penguinFont, "Pl", new Vector2(10, screenHeight + 5), XNAColor(playerColour));
                _spriteBatch.DrawString(penguinFont, "ayer", new Vector2(40, screenHeight + 5), XNAColor(playerColour));
                for (int i = 0; i < player.lives; i++)
                {
                    _spriteBatch.Draw(pstart, new Vector2(125 + 35 * i, screenHeight + 12), Color.White);
                }
                _spriteBatch.DrawString(penguinFont, "Speci", new Vector2(440, screenHeight + 5), XNAColor(bombColour));
                _spriteBatch.DrawString(penguinFont, "al", new Vector2(527, screenHeight + 5), XNAColor(bombColour));
                for (int i = 0; i < player.bombs; i++)
                {
                    _spriteBatch.Draw(bstart, new Vector2(565 + 35 * i, screenHeight + 12), Color.White);
                }
                _spriteBatch.DrawString(penguinFont, "Power", new Vector2(716, screenHeight + 5), XNAColor(powerColour));
                if (player.power < 3)
                    _spriteBatch.DrawString(penguinFont, Convert.ToString(player.power), new Vector2(822, screenHeight + 5), Color.White);
                else
                    _spriteBatch.DrawString(penguinFont, "MAX", new Vector2(822, screenHeight + 5), Color.White);
            }
            else if (screen == Screen.Pause)
            {
                _spriteBatch.Draw(background, backgroundR, Color.White);
                _spriteBatch.Draw(senseief, new Vector2(sensei.domo.X + senseit.Width / 2.0f, sensei.domo.Y + senseit.Height / 2.0f), null, Color.White, rotation, new Vector2(senseief.Width / 2.0f, senseief.Height / 2.0f), 1, SpriteEffects.None, 0);
                if (player.power > 2)
                {
                    _spriteBatch.Draw(effectt, new Vector2(player.domo.X - 6.0f, player.domo.Y - 12.0f), Color.White);
                    _spriteBatch.Draw(tintt, player.domo, Color.White);
                }
                else
                    _spriteBatch.Draw(player.tex, player.domo, Color.White);
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
                _spriteBatch.DrawString(penguinFont, "Enemy", new Vector2(10, 10), XNAColor(enemyColour));
                for (int i = 0; i < sensei.health; i++)
                {
                    _spriteBatch.Draw(hpt, new Rectangle(125 + i, 24, 1, 16), Color.White);
                }
                _spriteBatch.Draw(hudback, new Rectangle(0, screenHeight, _graphics.PreferredBackBufferWidth, 50), Color.White);
                _spriteBatch.DrawString(penguinFont, "Pl", new Vector2(10, screenHeight + 5), XNAColor(playerColour));
                _spriteBatch.DrawString(penguinFont, "ayer", new Vector2(40, screenHeight + 5), XNAColor(playerColour));
                for (int i = 0; i < player.lives; i++)
                {
                    _spriteBatch.Draw(pstart, new Vector2(125 + 35 * i, screenHeight + 12), Color.White);
                }
                _spriteBatch.DrawString(penguinFont, "Speci", new Vector2(275, screenHeight + 5), XNAColor(bombColour));
                _spriteBatch.DrawString(penguinFont, "al", new Vector2(362, screenHeight + 5), XNAColor(bombColour));
                for (int i = 0; i < player.bombs; i++)
                {
                    _spriteBatch.Draw(bstart, new Vector2(400 + 35 * i, screenHeight + 12), Color.White);
                }
                _spriteBatch.DrawString(penguinFont, "Power", new Vector2(551, screenHeight + 5), XNAColor(powerColour));
                if (player.power < 3)
                    _spriteBatch.DrawString(penguinFont, Convert.ToString(player.power), new Vector2(657, screenHeight + 5), Color.White);
                else
                    _spriteBatch.DrawString(penguinFont, "MAX", new Vector2(657, screenHeight + 5), Color.White);
                _spriteBatch.Draw(hudback, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White * 0.8f);
                //Options shadwo
                _spriteBatch.DrawString(italicFont, "S", new Vector2(menuShadowLocoX[0] + 2.0f, menuShadowLocoY[0] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[1] + 2.0f, menuShadowLocoY[1] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "a", new Vector2(menuShadowLocoX[2] + 2.0f, menuShadowLocoY[2] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "r", new Vector2(menuShadowLocoX[3] + 2.0f, menuShadowLocoY[3] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[4] + 2.0f, menuShadowLocoY[4] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "C", new Vector2(menuShadowLocoX[5] + 2.0f, menuShadowLocoY[5] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "o", new Vector2(menuShadowLocoX[6] + 2.0f, menuShadowLocoY[6] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "n", new Vector2(menuShadowLocoX[7] + 2.0f, menuShadowLocoY[7] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[8] + 2.0f, menuShadowLocoY[8] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "r", new Vector2(menuShadowLocoX[9] + 2.0f, menuShadowLocoY[9] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "o", new Vector2(menuShadowLocoX[10] + 2.0f, menuShadowLocoY[10] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "l", new Vector2(menuShadowLocoX[11] + 2.0f, menuShadowLocoY[11] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "s", new Vector2(menuShadowLocoX[12] + 2.0f, menuShadowLocoY[12] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "Q", new Vector2(menuShadowLocoX[13] + 2.0f, menuShadowLocoY[13] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "u", new Vector2(menuShadowLocoX[14] + 2.0f, menuShadowLocoY[14] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "i", new Vector2(menuShadowLocoX[15] + 2.0f, menuShadowLocoY[15] + 2.0f), Color.Black);
                _spriteBatch.DrawString(italicFont, "t", new Vector2(menuShadowLocoX[16] + 2.0f, menuShadowLocoY[16] + 2.0f), Color.Black);
                //Options
                if (start)
                {
                    _spriteBatch.DrawString(italicFont, "S", new Vector2(menuLocoX[0], menuLocoY[0]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[1], menuLocoY[1]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "a", new Vector2(menuLocoX[2], menuLocoY[2]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[3], menuLocoY[3]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[4], menuLocoY[4]), XNAColor(selectColour));
                }
                else
                {
                    _spriteBatch.DrawString(italicFont, "S", new Vector2(menuLocoX[0], menuLocoY[0]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[1], menuLocoY[1]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "a", new Vector2(menuLocoX[2], menuLocoY[2]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[3], menuLocoY[3]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[4], menuLocoY[4]), XNAColor(menuTextC));
                }
                if (controls)
                {
                    _spriteBatch.DrawString(italicFont, "C", new Vector2(menuLocoX[5], menuLocoY[5]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[6], menuLocoY[6]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "n", new Vector2(menuLocoX[7], menuLocoY[7]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[8], menuLocoY[8]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[9], menuLocoY[9]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[10], menuLocoY[10]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "l", new Vector2(menuLocoX[11], menuLocoY[11]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "s", new Vector2(menuLocoX[12], menuLocoY[12]), XNAColor(selectColour));
                }
                else
                {
                    _spriteBatch.DrawString(italicFont, "C", new Vector2(menuLocoX[5], menuLocoY[5]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[6], menuLocoY[6]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "n", new Vector2(menuLocoX[7], menuLocoY[7]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[8], menuLocoY[8]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "r", new Vector2(menuLocoX[9], menuLocoY[9]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "o", new Vector2(menuLocoX[10], menuLocoY[10]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "l", new Vector2(menuLocoX[11], menuLocoY[11]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "s", new Vector2(menuLocoX[12], menuLocoY[12]), XNAColor(menuTextC));
                }
                if (quit)
                {
                    _spriteBatch.DrawString(italicFont, "Q", new Vector2(menuLocoX[13], menuLocoY[13]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "u", new Vector2(menuLocoX[14], menuLocoY[14]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "i", new Vector2(menuLocoX[15], menuLocoY[15]), XNAColor(selectColour));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[16], menuLocoY[16]), XNAColor(selectColour));
                }
                else
                {
                    _spriteBatch.DrawString(italicFont, "Q", new Vector2(menuLocoX[13], menuLocoY[13]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "u", new Vector2(menuLocoX[14], menuLocoY[14]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "i", new Vector2(menuLocoX[15], menuLocoY[15]), XNAColor(menuTextC));
                    _spriteBatch.DrawString(italicFont, "t", new Vector2(menuLocoX[16], menuLocoY[16]), XNAColor(menuTextC));
                }
            }
            else if (screen == Screen.Controls)
            {
                if (!bossRunning)
                {
                    _spriteBatch.Draw(menubg, new Rectangle(0, 0, 900, 650), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(background, backgroundR, Color.White);
                    _spriteBatch.Draw(senseief, new Vector2(sensei.domo.X + senseit.Width / 2.0f, sensei.domo.Y + senseit.Height / 2.0f), null, Color.White, rotation, new Vector2(senseief.Width / 2.0f, senseief.Height / 2.0f), 1, SpriteEffects.None, 0);
                    if (player.power > 2)
                    {
                        _spriteBatch.Draw(effectt, new Vector2(player.domo.X - 6.0f, player.domo.Y - 12.0f), Color.White);
                        _spriteBatch.Draw(tintt, player.domo, Color.White);
                    }
                    else
                        _spriteBatch.Draw(player.tex, player.domo, Color.White);
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
                    _spriteBatch.DrawString(penguinFont, "Enemy", new Vector2(10, 10), XNAColor(enemyColour));
                    for (int i = 0; i < sensei.health; i++)
                    {
                        _spriteBatch.Draw(hpt, new Rectangle(125 + i, 24, 1, 16), Color.White);
                    }
                    _spriteBatch.Draw(hudback, new Rectangle(0, screenHeight, _graphics.PreferredBackBufferWidth, 50), Color.White);
                    _spriteBatch.DrawString(penguinFont, "Pl", new Vector2(10, screenHeight + 5), XNAColor(playerColour));
                    _spriteBatch.DrawString(penguinFont, "ayer", new Vector2(40, screenHeight + 5), XNAColor(playerColour));
                    for (int i = 0; i < player.lives; i++)
                    {
                        _spriteBatch.Draw(pstart, new Vector2(125 + 35 * i, screenHeight + 12), Color.White);
                    }
                    _spriteBatch.DrawString(penguinFont, "Speci", new Vector2(275, screenHeight + 5), XNAColor(bombColour));
                    _spriteBatch.DrawString(penguinFont, "al", new Vector2(362, screenHeight + 5), XNAColor(bombColour));
                    for (int i = 0; i < player.bombs; i++)
                    {
                        _spriteBatch.Draw(bstart, new Vector2(400 + 35 * i, screenHeight + 12), Color.White);
                    }
                    _spriteBatch.DrawString(penguinFont, "Power", new Vector2(551, screenHeight + 5), XNAColor(powerColour));
                    if (player.power < 3)
                        _spriteBatch.DrawString(penguinFont, Convert.ToString(player.power), new Vector2(657, screenHeight + 5), Color.White);
                    else
                        _spriteBatch.DrawString(penguinFont, "MAX", new Vector2(657, screenHeight + 5), Color.White);
                    _spriteBatch.Draw(hudback, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White * 0.8f);

                }
                _spriteBatch.DrawString(penguinFont, "ARROW KEYS - MOVE", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f - 140.0f + 2.0f), Color.Black);
                _spriteBatch.DrawString(penguinFont, "SHIFT - MOVE SLOWER", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f + 2.0f), Color.Black);
                _spriteBatch.DrawString(penguinFont, "Z - SHOOT", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f + 2.0f), Color.Black);
                _spriteBatch.DrawString(penguinFont, "X - SPECIAL", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f + 70.0f + 2.0f), Color.Black);
                _spriteBatch.DrawString(penguinFont, "ESCAPE - EXIT/PAUSE", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f + 140.0f + 2.0f), Color.Black);

                _spriteBatch.DrawString(penguinFont, "ARROW KEYS - MOVE", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f - 140.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(penguinFont, "SHIFT - MOVE SLOWER", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f - 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(penguinFont, "Z - SHOOT", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(penguinFont, "X - SPECIAL", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f + 70.0f), XNAColor(menuTextC));
                _spriteBatch.DrawString(penguinFont, "ESCAPE - EXIT/PAUSE", new Vector2(300, _graphics.PreferredBackBufferHeight / 2.0f + 140.0f), XNAColor(menuTextC));
            }
            else if (screen == Screen.End)
            {
                _spriteBatch.Draw(menubg, new Rectangle(0, 0, 900, 650), Color.White);
                if (senseiDeath)
                {
                    _spriteBatch.Draw(bigsensei, new Vector2(300, 100), Color.White);
                    _spriteBatch.DrawString(boldFont, "YOU WON!", new Vector2(304 + 3.0f, 250 + 3.0f), Color.Black);
                    _spriteBatch.DrawString(boldFont, "YOU WON!", new Vector2(304, 250), XNAColor(menuTextC));

                }
                else
                {
                    _spriteBatch.DrawString(boldFont, "YOU LOST", new Vector2(300 + 3.0f, 250 + 3.0f), Color.Black);
                    _spriteBatch.DrawString(boldFont, "YOU LOST", new Vector2(300, 250), XNAColor(menuTextC));
                }

                _spriteBatch.DrawString(boldFont, "PRESS Z", new Vector2(320 + 3.0f, 400 + 3.0f), Color.Black);
                _spriteBatch.DrawString(boldFont, "PRESS Z", new Vector2(320, 400), XNAColor(menuTextC));
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        //Misc

        public Microsoft.Xna.Framework.Color XNAColor(System.Drawing.Color color)
        {
            return new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
        }

        public class Keyboardd
        {
            static KeyboardState currentKeyState;
            static KeyboardState previousKeyState;

            public static KeyboardState GetState()
            {
                previousKeyState = currentKeyState;
                currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                return currentKeyState;
            }

            public static bool IsPressed(Keys key)
            {
                return currentKeyState.IsKeyDown(key);
            }

            public static bool HasBeenPressed(Keys key)
            {
                return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
            }
        }

        public static void Effects(Player player)
        {
            if (player.power > 2)
            {
                if (frames % 3 == 0)
                {
                    effectt = maxeffects[meffect];
                    tintt = tints[meffect];
                    if (meffect >= 3)
                        meffect = 0;
                    else
                        meffect += 1;
                }
            }
            if (!special)
            {
                rotationFrames++;
                rotation = (float)(Math.PI * rotationFrames) / 50.0f;
            }

        }
        public static void Background()
        {
            if (frames % 20 == 0)
            {
                backgroundR = new Rectangle(0, -600, 900, 1200);
            }
            backgroundR = new Rectangle(0, backgroundR.Y - backgroundDisplacement, 900, 1200);
        }
        public static void BoundaryCheck()
        {
            List<Projectile> validP = new List<Projectile>();
            foreach (Projectile projectile in projectiles)
            {
                if (projectile.domo.Y > screenHeight || projectile.domo.Y + testp.Height < 0)
                    continue;
                if (projectile.domo.X > _graphics.PreferredBackBufferWidth || projectile.domo.X + testp.Width < 0)
                    continue;
                validP.Add(projectile);
            }
            projectiles = validP;
            List<PlayerProjectile> validPP = new List<PlayerProjectile>();
            foreach (PlayerProjectile pprojectile in playerProjectiles)
            {
                if (pprojectile.domo.Y > screenHeight || pprojectile.domo.Y + pprojectile.tex.Height < 0)
                    continue;
                if (pprojectile.domo.X > _graphics.PreferredBackBufferWidth || pprojectile.domo.X + pprojectile.tex.Width < 0)
                    continue;
                validPP.Add(pprojectile);
            }
            playerProjectiles = validPP;
            List<SpecialProjectile> validSP = new List<SpecialProjectile>();
            foreach (SpecialProjectile sprojectile in specialProjectiles)
            {
                if (sprojectile.domo.Y > screenHeight || sprojectile.domo.Y + sprojectile.tex.Height < 0)
                    continue;
                if (sprojectile.domo.X > _graphics.PreferredBackBufferWidth || sprojectile.domo.X + sprojectile.tex.Width < 0)
                    continue;
                validSP.Add(sprojectile);
            }
            specialProjectiles = validSP;
        }

        public static void PlayerDeath(Player player, Sensei sensei)
        {
            Centre(sensei);
            init = true;
            player.lives -= 1;
            player.power = 1;
            player.domo.X = _graphics.PreferredBackBufferWidth / 2.0f - normalt.Width / 2.0f;
            player.domo.Y = screenHeight;
            player.speed = new Vector2(0, 2);
            die.Play();
            projectiles.Clear();
            playerProjectiles.Clear();
            specialProjectiles.Clear();
            iFrames = frames;

            if (player.lives < 0)
            {
                exit.Play();
                MediaPlayer.Stop();
                iSong = true;
                start = true;
                init = true;
                bossRunning = false;
                screen = Screen.End;
            }
        }

        public static void CollisionCheck(Player player, Sensei sensei)
        {
            foreach (Projectile projectile in projectiles)
            {
                double distance = Vector2.Distance(projectile.hbox, player.hbox);
                if (distance <= player.radius + projectile.radius)
                {
                    PlayerDeath(player, sensei);
                    break;
                }
            }
            List<PlayerProjectile> realP = new List<PlayerProjectile>();
            foreach (PlayerProjectile pprojectile in playerProjectiles)
            {
                double distance = Vector2.Distance(pprojectile.hbox, sensei.hbox);
                if (distance <= pprojectile.radius + sensei.radius)
                {
                    senseihurt.Play();
                    sensei.health -= 2;
                    continue;
                }
                realP.Add(pprojectile);
            }
            playerProjectiles = realP;
            foreach (SpecialProjectile sprojectile in specialProjectiles)
            {
                List<Projectile> validP = new List<Projectile>();
                foreach (Projectile projectile in projectiles)
                {
                    double projectileDistance = Vector2.Distance(projectile.hbox, sprojectile.hbox);
                    if (projectileDistance <= sprojectile.radius + projectile.radius)
                    {
                        continue;
                    }
                    validP.Add(projectile);
                }
                projectiles = validP;
                double senseiDistance = Vector2.Distance(sprojectile.hbox, sensei.hbox);
                if (senseiDistance <= sprojectile.radius + sensei.radius)
                {
                    if (sprojectile.damage > 0)
                        senseihurt.Play();
                    sensei.health -= sprojectile.damage;
                    sprojectile.damage = 0;
                }
            }
            double psdistance = Vector2.Distance(player.domo, sensei.domo);
            if (psdistance <= sensei.radius + player.radius)
            {
                PlayerDeath(player, sensei);
            }
        }

        public static void SenseiUpdate(Sensei sensei, Player player)
        {
            if (phase == SPhase.One)
            {
                if (Vector2.Distance(sensei.domo, sensei.idomo) < 1)
                {
                    Stationary(sensei);
                    sensei.domo = sensei.idomo;
                }
                Lasers(sensei, player);
                if (frames % 240 == 0)
                {
                    if (sensei.domo.Y < 0)
                        Down(sensei);
                    else if (sensei.domo.Y + senseit.Height > _graphics.PreferredBackBufferHeight)
                        Up(sensei);
                    else if (sensei.domo.X < 0)
                        Right(sensei);
                    else if (sensei.domo.Y + senseit.Width > _graphics.PreferredBackBufferWidth)
                        Left(sensei);
                    else
                    {
                        if (init)
                        {
                            Random m = new Random();
                            int movement = m.Next(1, 4);
                            if (movement == 1)
                                Up(sensei);
                            else if (movement == 2)
                                Left(sensei);
                            else if (movement == 3)
                                Right(sensei);
                            init = false;
                        }
                        else
                        {
                            Random m = new Random();
                            int movement = m.Next(1, 6);
                            if (movement == 1)
                            {
                                if (sensei.domo.Y > 100)
                                    Up(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 2)
                            {
                                if (sensei.domo.X > 100)
                                    Left(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 3)
                            {
                                if (sensei.domo.Y < _graphics.PreferredBackBufferWidth - 100)
                                    Right(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 4)
                            {
                                if (sensei.domo.Y < _graphics.PreferredBackBufferHeight / 2.0f - 100.0f)
                                    Down(sensei);
                                else if (sensei.domo.Y > _graphics.PreferredBackBufferHeight / 2.0f - 100.0f)
                                    Up(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 5)
                            {
                                Stationary(sensei);
                            }
                        }
                    }
                }
                if (sensei.health <= 500)
                {
                    {
                        phase = SPhase.Two;
                        player.power += 1;
                        init = true;
                        projectiles.Clear();
                        powerup.Play();
                    }
                }
            }
            else if (phase == SPhase.Two)
            {
                Centre(sensei);
                Hashtag(projectiles);
                if (sensei.health <= 250)
                {
                    {
                        phase = SPhase.Three;
                        player.power += 1;
                        init = true;
                        powerup.Play();
                    }
                }
            }
            else if (phase == SPhase.Three)
            {
                Follow(sensei, player);
                Tangent(projectiles, sensei, player);
                if (sensei.health <= 0)
                {
                    sensei.health = 750;
                    phase = SPhase.Four;
                    if (player.bombs < 4)
                        player.bombs += 1;
                    player.power += 1;
                    init = true;
                    projectiles.Clear();
                    powerup.Play();
                }
            }
            else if (phase == SPhase.Four)
            {
                Follow(sensei, player);
                sensei.speed = sensei.speed * 2;
                HVLaser(sensei, player);
                if (sensei.health <= 500)
                {
                    phase = SPhase.Five;
                    player.power += 1;
                    init = true;
                    powerup.Play();
                }

            }
            else if (phase == SPhase.Five)
            {
                Meteor();
                if (Vector2.Distance(sensei.domo, sensei.idomo) < 1)
                {
                    Stationary(sensei);
                    sensei.domo = sensei.idomo;
                }
                if (frames % 240 == 0)
                {
                    if (sensei.domo.Y < 0)
                        Down(sensei);
                    else if (sensei.domo.Y + senseit.Height > _graphics.PreferredBackBufferHeight / 2)
                        Up(sensei);
                    else if (sensei.domo.X < 0)
                        Right(sensei);
                    else if (sensei.domo.Y + senseit.Width > _graphics.PreferredBackBufferWidth)
                        Left(sensei);
                    else
                    {
                        if (init)
                        {
                            Random m = new Random();
                            int movement = m.Next(1, 4);
                            if (movement == 1)
                                Up(sensei);
                            else if (movement == 2)
                                Left(sensei);
                            else if (movement == 3)
                                Right(sensei);
                            init = false;
                        }
                        else
                        {
                            Random m = new Random();
                            int movement = m.Next(1, 6);
                            if (movement == 1)
                            {
                                if (sensei.domo.Y > 100)
                                    Up(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 2)
                            {
                                if (sensei.domo.X > 100)
                                    Left(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 3)
                            {
                                if (sensei.domo.Y < _graphics.PreferredBackBufferWidth - 100)
                                    Right(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 4)
                            {
                                if (sensei.domo.Y < _graphics.PreferredBackBufferHeight / 2.0f - 100.0f)
                                    Down(sensei);
                                else if (sensei.domo.Y > _graphics.PreferredBackBufferHeight / 2.0f - 100.0f)
                                    Up(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 5)
                            {
                                Stationary(sensei);
                            }
                        }
                    }
                }
                if (sensei.health <= 250)
                {
                    phase = SPhase.Six;
                    player.power += 1;
                    init = true;
                    projectiles.Clear();
                    powerup.Play();
                }
            }
            else if (phase == SPhase.Six)
            {
                Centre(sensei);
                Pinwheel(projectiles, sensei);
                if (sensei.health <= 0)
                {
                    sensei.health = 750;
                    phase = SPhase.Seven;
                    if (player.bombs < 4)
                        player.bombs += 1;
                    player.power += 1;
                    init = true;
                    powerup.Play();
                }
            }
            else if (phase == SPhase.Seven)
            {
                Lasers(sensei, player);
                Circlaser(sensei, player);
                if (Vector2.Distance(sensei.domo, sensei.idomo) < 1)
                {
                    Stationary(sensei);
                    sensei.domo = sensei.idomo;
                }
                if (frames % 240 == 0)
                {
                    if (sensei.domo.Y < 0)
                        Down(sensei);
                    else if (sensei.domo.Y + senseit.Height > _graphics.PreferredBackBufferHeight)
                        Up(sensei);
                    else if (sensei.domo.X < 0)
                        Right(sensei);
                    else if (sensei.domo.Y + senseit.Width > _graphics.PreferredBackBufferWidth)
                        Left(sensei);
                    else
                    {
                        if (init)
                        {
                            Random m = new Random();
                            int movement = m.Next(1, 4);
                            if (movement == 1)
                                Up(sensei);
                            else if (movement == 2)
                                Left(sensei);
                            else if (movement == 3)
                                Right(sensei);
                            init = false;
                        }
                        else
                        {
                            Random m = new Random();
                            int movement = m.Next(1, 6);
                            if (movement == 1)
                            {
                                if (sensei.domo.Y > 100)
                                    Up(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 2)
                            {
                                if (sensei.domo.X > 100)
                                    Left(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 3)
                            {
                                if (sensei.domo.Y < _graphics.PreferredBackBufferWidth - 100)
                                    Right(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 4)
                            {
                                if (sensei.domo.Y < _graphics.PreferredBackBufferHeight / 2.0f - 100.0f)
                                    Down(sensei);
                                else if (sensei.domo.Y > _graphics.PreferredBackBufferHeight / 2.0f - 100.0f)
                                    Up(sensei);
                                else
                                    Stationary(sensei);
                            }
                            else if (movement == 5)
                            {
                                Stationary(sensei);
                            }
                        }
                    }
                }
                if (sensei.health <= 500)
                {
                    phase = SPhase.Eight;
                    player.power += 1;
                    init = true;
                    powerup.Play();
                }
            }
            else if (phase == SPhase.Eight)
            {
                Fireworks(projectiles, sensei);
                if (sensei.health <= 250)
                {
                    phase = SPhase.Nine;
                    player.power += 1;
                    init = true;
                    powerup.Play();
                }
            }
            else if (phase == SPhase.Nine)
            {
                if (!senseiDeath)
                {
                    Follow(sensei, player);
                    sensei.speed = sensei.speed / 2.0f;
                    Tangent(projectiles, sensei, player);
                    if (frames % 60 == 0)
                        Pinwheel(projectiles, sensei);
                    if (sensei.health <= 0)
                    {
                        senseiDeath = true;
                        player.power += 1;
                        init = true;
                        deathFrames = frames;
                    }
                }
                else
                {
                    senseidie.Play();
                    Circle(sensei);
                    if (frames % 4 == 0)
                        sensei.speed = sensei.speed * 2;
                    if (frames - deathFrames > 120)
                    {
                        exit.Play();
                        MediaPlayer.Stop();
                        iSong = true;
                        start = true;
                        init = true;
                        bossRunning = false;
                        screen = Screen.End;
                    }
                    senseiTransparency -= 0.01f;
                }
            }
        }

        public static void PlayerMove(Player player)
        {
            float shift = 0;
            if (keyboard.IsKeyDown(Keys.LeftShift))
                shift = 1.5f;
            if (keyboard.IsKeyDown(Keys.Up) && keyboard.IsKeyDown(Keys.Left))
            {
                player.tex = leftt;
                player.domo.X -= (float)Math.Sqrt(3 - shift);
                player.domo.Y -= (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Left) && keyboard.IsKeyDown(Keys.Down))
            {
                player.tex = leftt;
                player.domo.X -= (float)Math.Sqrt(3 - shift);
                player.domo.Y += (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Down) && keyboard.IsKeyDown(Keys.Right))
            {
                player.tex = rightt;
                player.domo.X += (float)Math.Sqrt(3 - shift);
                player.domo.Y += (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Up) && keyboard.IsKeyDown(Keys.Right))
            {
                player.tex = rightt;
                player.domo.X += (float)Math.Sqrt(3 - shift);
                player.domo.Y -= (float)Math.Sqrt(3 - shift);
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                player.tex = normalt;
                player.domo.Y -= 3 - shift;
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                player.tex = normalt;
                player.domo.Y += 3 - shift;
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                player.tex = leftt;
                player.domo.X -= 3 - shift;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                player.tex = rightt;
                player.domo.X += 3 - shift;
            }
            else if (!keyboard.IsKeyDown(Keys.Right) && !keyboard.IsKeyDown(Keys.Left))
                player.tex = normalt;

            if (player.domo.X + normalt.Width > _graphics.PreferredBackBufferWidth)
                player.domo.X = _graphics.PreferredBackBufferWidth - normalt.Width;
            else if (player.domo.X < 0)
                player.domo.X = 0;

            if (player.domo.Y + normalt.Height > screenHeight)
                player.domo.Y = screenHeight - normalt.Height;
            else if (player.domo.Y < 0)
                player.domo.Y = 0;

            player.hbox.X = player.domo.X + normalt.Width / 2 - 1;
            player.hbox.Y = player.domo.Y + normalt.Height / 2 - 1;
            if (!death)
            {
                if (keyboard.IsKeyDown(Keys.Z))
                {
                    if (frames - specialFrames > 600)
                        Shoot(player);
                }
                if (keyboard.IsKeyDown(Keys.X))
                {
                    if (frames - specialFrames > 600)
                    {
                        if (player.bombs > 0)
                        {
                            player.bombs -= 1;
                            special = true;
                            specialFrames = frames;
                            specialfx.Play();
                            Special(player);
                        }
                    }
                }
            }
            if (Keyboardd.HasBeenPressed(Keys.Escape))
            {
                pause.Play();
                init = true;
                screen = Screen.Pause;
            }
        }

        public static void Shoot(Player player)
        {
            if (frames % 10 == 0)
            {
                shoot.Play();
                if (player.power == 1)
                {
                    PlayerProjectile pprojectile = new PlayerProjectile();
                    pprojectile.tex = kurenai;
                    pprojectile.domo = new Vector2((player.domo.X + normalt.Width / 2) - kurenai.Width / 2, (player.domo.Y + normalt.Height / 2) - kurenai.Height / 2);
                    pprojectile.speed = new Vector2(0, -10);
                    playerProjectiles.Add(pprojectile);
                }
                else if (player.power == 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        PlayerProjectile pprojectile = new PlayerProjectile();
                        pprojectile.tex = kurenai;
                        if (i < 1)
                            pprojectile.domo = new Vector2((player.domo.X + normalt.Width / 3) - kurenai.Width / 2, (player.domo.Y + normalt.Height / 2) - kurenai.Height / 2);
                        else
                            pprojectile.domo = new Vector2((player.domo.X + (2 * normalt.Width) / 3) - kurenai.Width / 2, (player.domo.Y + normalt.Height / 2) - kurenai.Height / 2);
                        pprojectile.speed = new Vector2(0, -10);
                        playerProjectiles.Add(pprojectile);
                    }
                }
                else if (player.power > 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        PlayerProjectile pprojectile = new PlayerProjectile();
                        pprojectile.tex = kurenai;
                        if (i == 0)
                            pprojectile.domo = new Vector2((player.domo.X + normalt.Width / 4) - kurenai.Width / 2, (player.domo.Y + normalt.Height / 2) - kurenai.Height / 2);
                        else if (i == 1)
                            pprojectile.domo = new Vector2((player.domo.X + (2 * normalt.Width) / 4) - kurenai.Width / 2, (player.domo.Y + normalt.Height / 2) - kurenai.Height / 2);
                        else
                            pprojectile.domo = new Vector2((player.domo.X + (3 * normalt.Width) / 4) - kurenai.Width / 2, (player.domo.Y + normalt.Height / 2) - kurenai.Height / 2);
                        pprojectile.speed = new Vector2(0, -10);
                        playerProjectiles.Add(pprojectile);
                    }
                }
            }
        }

        public static void Special(Player player)
        {
            if (frames % 8 == 0)
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
            if (timesFired < 18)
            {
                if (frames % (250 - (timesFired) * 10) == 0)
                {
                    timesFired++;
                    //Vertical
                    blast2.Play();
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2(_graphics.PreferredBackBufferWidth - testp.Width, (((screenHeight + 300) / 30) * i) - 150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(-2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(-2, (float)0.25);
                        projectile.tex = colours[6];
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2(0, (((screenHeight + 300) / 30) * i) - 150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(2, (float)0.25);
                        projectile.tex = colours[6];
                        projectiles.Add(projectile);
                    }

                    //Horizontal
                    blast2.Play();
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, 0);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, 2);
                        else
                            projectile.speed = new Vector2((float)0.25, 2);
                        projectile.tex = colours[8];
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, screenHeight - testp.Height);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, -2);
                        else
                            projectile.speed = new Vector2((float)0.25, -2);
                        projectile.tex = colours[8];
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
                if (frames % 75 == 0)
                {
                    timesFired++;
                    blast2.Play();
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2(_graphics.PreferredBackBufferWidth - testp.Width, (((screenHeight + 300) / 30) * i) - 150);
                        else
                            projectile.domo = new Vector2(_graphics.PreferredBackBufferWidth - testp.Width, -150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(-2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(-2, (float)0.25);
                        projectile.tex = colours[14];
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2(0, (((screenHeight + 300) / 30) * i) - 150);
                        else
                            projectile.domo = new Vector2(0, -150);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2(2, (float)-0.25);
                        else
                            projectile.speed = new Vector2(2, (float)0.25);
                        projectile.tex = colours[14];
                        projectiles.Add(projectile);
                    }
                    blast2.Play();
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, 0);
                        else
                            projectile.domo = new Vector2(-225 - timesFired * 3, 0);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, 1.114f);
                        else
                            projectile.speed = new Vector2((float)0.25, 1.114f);
                        projectile.tex = colours[9];
                        projectiles.Add(projectile);
                    }
                    for (int i = 0; i <= 30; i++)
                    {
                        Projectile projectile = new Projectile();
                        if (i != 0)
                            projectile.domo = new Vector2((((_graphics.PreferredBackBufferWidth + 450) / 30) * i) - 225 - timesFired * 3, screenHeight - testp.Height);
                        else
                            projectile.domo = new Vector2(-225 - timesFired * 3, screenHeight - testp.Height);
                        if (i % 2 == 0)
                            projectile.speed = new Vector2((float)-0.25, -1.114f);
                        else
                            projectile.speed = new Vector2((float)0.25, -1.114f);
                        projectile.tex = colours[9];
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
            if (frames % 15 == 0)
                blast1.Play();
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
                if (frames % 60 == 0)
                    blast1.Play();
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
            if (frames % 15 == 0)
                blast2.Play();
            if (frames % 2 == 0)
            {
                Projectile projectile = new Projectile();
                Random d = new Random();
                Random g = new Random();
                Random v = new Random();
                Random c = new Random();
                projectile.domo.X = d.Next(0, _graphics.PreferredBackBufferWidth - testp.Width);
                projectile.domo.Y = 0;
                projectile.gravity = g.Next(1, 4) / 200.0f;
                projectile.speed.X = (float)0.5 + (float)((v.Next(1, 11)) / 10.0f);
                if (tick % 2 == 0)
                    projectile.speed.X = -projectile.speed.X;
                projectile.speed.Y = (float)0.1;
                int col = c.Next(0, 16);
                projectile.tex = colours[col];
                projectiles.Add(projectile);

                tick++;
            }
        }

        public static void Tangent(List<Projectile> projectiles, Sensei sensei, Player player)
        {
            for (int i = 0; i < 16; i++)
            {
                Projectile projectile = new Projectile();
                projectile.speed = Vector2.Transform(Vector2.One, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                projectile.domo = new Vector2(sensei.domo.X - testp.Width / 2 + senseit.Width / 2, sensei.domo.Y - testp.Height / 2 + senseit.Height / 2);
                projectile.domo += projectile.speed * 50;
                Vector2 instantaneous = new Vector2((sensei.domo.X + senseit.Width / 2) - (projectile.domo.X + testp.Width / 2), (sensei.domo.Y + senseit.Height / 2) - (projectile.domo.Y + testp.Height / 2));
                projectile.speed = instantaneous;
                projectile.tex = colours[18];
                projectiles.Add(projectile);
            }
        }

        public static void Lasers(Sensei sensei, Player player)
        {
            if (frames % 120 == 0)
            {
                Random c = new Random();
                fireCol = c.Next(0, 16);
                for (int i = 1; i < 11; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = new Vector2(sensei.hbox.X, sensei.hbox.Y - 50);
                    projectile.speed = (new Vector2(player.hbox.X - sensei.hbox.X, player.hbox.Y - sensei.hbox.Y) / Vector2.Distance(player.hbox, sensei.hbox)) * (i / 3.0f);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
                for (int i = 1; i < 11; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = new Vector2(sensei.hbox.X, sensei.hbox.Y - 50);
                    projectile.speed = (new Vector2(player.hbox.X - sensei.hbox.X - 30, player.hbox.Y - sensei.hbox.Y) / Vector2.Distance(player.hbox, sensei.hbox)) * (i / 2.0f);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
                for (int i = 1; i < 11; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = new Vector2(sensei.hbox.X, sensei.hbox.Y - 50);
                    projectile.speed = (new Vector2(player.hbox.X - sensei.hbox.X + 30, player.hbox.Y - sensei.hbox.Y) / Vector2.Distance(player.hbox, sensei.hbox)) * (i / 3.0f);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
            }
        }

        public static void HVLaser(Sensei sensei, Player player)
        {
            if (frames % 120 == 0)
            {
                Random c = new Random();
                fireCol = c.Next(0, 16);
                for (int i = 0; i < 6; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = sensei.hbox;
                    projectile.speed = new Vector2(0.4f * i, 0);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
                for (int i = 0; i < 6; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = sensei.hbox;
                    projectile.speed = new Vector2(-0.4f * i, 0);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
                for (int i = 0; i < 6; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = sensei.hbox;
                    projectile.speed = new Vector2(0, -0.4f * i);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
                for (int i = 0; i < 6; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = sensei.hbox;
                    projectile.speed = new Vector2(0, 0.4f * i);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
                for (int i = 1; i < 6; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.domo = new Vector2(sensei.hbox.X, sensei.hbox.Y - 50);
                    projectile.speed = (new Vector2(player.hbox.X - sensei.hbox.X, player.hbox.Y - sensei.hbox.Y) / Vector2.Distance(player.hbox, sensei.hbox)) * (i / 3.0f);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
            }

        }

        public static void Circlaser(Sensei sensei, Player player)
        {
            if (frames % 150 == 0)
            {
                Vector2 startingSpeed = Vector2.Transform(Vector2.One, Matrix.CreateRotationZ((float)Math.PI / 30 * timesFired));
                Random c = new Random();
                fireCol = c.Next(0, 16);
                for (int i = 0; i < 16; i++)
                {
                    Projectile projectile = new Projectile();
                    projectile.speed = Vector2.Transform(startingSpeed, Matrix.CreateRotationZ((float)Math.PI / 8 * i));
                    projectile.domo = new Vector2(sensei.domo.X - testp.Width / 2 + senseit.Width / 2, sensei.domo.Y - testp.Height / 2 + senseit.Height / 2);
                    projectile.domo += projectile.speed * 50;
                    projectile.speed = (new Vector2(player.hbox.X - sensei.hbox.X, player.hbox.Y - sensei.hbox.Y) / Vector2.Distance(player.hbox, sensei.hbox)) * (i / 3.0f);
                    projectile.tex = colours[fireCol];
                    projectiles.Add(projectile);
                }
            }
        }

        //Move
        public static void Stationary(Sensei sensei)
        {
            sensei.speed = new Vector2(0, 0);
        }
        public static void Circle(Sensei sensei)
        {
            circleStep += 0.035;
            sensei.domo.X = (float)(Math.Cos(circleStep) * 100) + 450f;
            sensei.domo.Y = (float)(-1 * Math.Sin(circleStep) * 100) + 300f;
        }
        public static void Centre(Sensei sensei)
        {
            Vector2 centre = new Vector2((_graphics.PreferredBackBufferWidth / 2.0f) - (senseit.Width / 2.0f), (screenHeight / 2.0f) - (senseit.Height / 2.0f));
            if (Vector2.Distance(sensei.domo, centre) < 1)
                sensei.domo = centre;
            if (sensei.domo != centre)
            {
                Vector2 distance = new Vector2(centre.X - sensei.domo.X, centre.Y - sensei.domo.Y);
                distance.X = distance.X / Vector2.Distance(centre, sensei.domo);
                distance.Y = distance.Y / Vector2.Distance(centre, sensei.domo);
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
        public static void Up(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X, sensei.domo.Y - 100.0f);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(0, -4.0f);
        }
        public static void Down(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X, sensei.domo.Y + 100.0f);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(0, 4.0f);
        }
        public static void Right(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X + 100.0f, sensei.domo.Y);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(4.0f, 0);
        }
        public static void Left(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X - 100.0f, sensei.domo.Y);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(-4.0f, 0);
        }
        public static void UpLeft(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X - 100.0f, sensei.domo.Y - 100.0f);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(-4.0f, -4.0f);
        }
        public static void UpRight(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X + 100.0f, sensei.domo.Y - 100.0f);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(4.0f, -4.0f);
        }
        public static void DownRight(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X + 100.0f, sensei.domo.Y + 100.0f);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(4.0f, 4.0f);
        }
        public static void DownLeft(Sensei sensei)
        {
            if (init)
            {
                sensei.idomo = new Vector2(sensei.domo.X - 100.0f, sensei.domo.Y + 100.0f);
                init = false;
            }
            if (Vector2.Distance(sensei.idomo, sensei.domo) < 1)
            {
                sensei.domo = sensei.idomo;
                sensei.speed = new Vector2(0, 0);
            }
            else
                sensei.speed = new Vector2(-4.0f, 4.0f);
        }
        public static void Follow(Sensei sensei, Player player)
        {
            Vector2 distance = new Vector2(sensei.hbox.X - player.hbox.X, sensei.hbox.Y - player.hbox.Y);
            sensei.speed = -(distance / Vector2.Distance(sensei.hbox, player.hbox)) / 2.0f;
        }
    }

    public class Sensei
    {
        public Vector2 speed, domo, hbox, idomo;
        public float radius;
        public int health;

        public Sensei()
        {
            domo = new Vector2(450, 300);
            radius = 24;
            health = 675;
        }
    }

    public class Player
    {
        public Vector2 speed, domo, hbox;
        public float radius;
        public Texture2D tex;
        public int lives, bombs, power;

        public Player()
        {
            domo = new Vector2(0, 0);
            radius = 1;
            lives = 9;
            bombs = 4;
            power = 1;
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
        public int damage;
        public double gravity;

        public SpecialProjectile()
        {
            domo = new Vector2(0, 0);
            speed = new Vector2(0, 0);
            radius = 10;
            damage = 4;
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