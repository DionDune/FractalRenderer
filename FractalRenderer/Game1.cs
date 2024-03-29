﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FractalRenderer
{
    public class Game1 : Game
    {
        #region Variable Defenition

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Random random;

        Texture2D Color_White;
        List<Color> Colors = new List<Color>();
        bool randomColors;

        Vector2 InitialPosition;
        float InitialDistance;
        float RotationChange;
        float DistanceChange;
        int BranchSteps;

        List<Keys> Keys_BeingPressed = new List<Keys>();

        #endregion

        #region Initialize

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            random = new Random();

            InitialPosition = new Vector2(900, 900);
            RotationChange = 25F;
            DistanceChange = 0.9F;
            BranchSteps = 10;
            InitialDistance = 100;

            Colors = new List<Color>() { Color.Red, Color.Green, Color.Yellow, Color.HotPink, Color.Blue, Color.Purple };
            randomColors = true;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Procedurally Creating and Assigning a 1x1 white texture to Color_White
            Color_White = new Texture2D(GraphicsDevice, 1, 1);
            Color_White.SetData(new Color[1] { Color.White });
        }

        #endregion

        /////////////////////////////////////////
        
        void KeyboardHandler()
        {
            List<Keys> Keys_NewlyPressed = Keyboard.GetState().GetPressedKeys().ToList();


            bool Shifting = Keys_NewlyPressed.Contains(Keys.LeftShift);


            //Rotate Fractal
            if (Keys_NewlyPressed.Contains(Keys.Right))
            {
                RotationChange += 0.002F;
                if (Shifting)
                {
                    RotationChange += 0.002F;
                }
            }
            else if (Keys_NewlyPressed.Contains(Keys.Left))
            {
                RotationChange -= 0.002F;
                if (Shifting)
                {
                    RotationChange -= 0.002F;
                }
            }
            if (RotationChange < 0)
            {
                RotationChange = 360 + RotationChange;
            }
            else if (RotationChange > 360)
            {
                RotationChange -= 360;
            }

            //Change Distance Multiplier
            if (Keys_NewlyPressed.Contains(Keys.Up))
            {
                DistanceChange += 0.001F;
                if (Shifting)
                {
                    DistanceChange += 0.001F;
                }
            }
            else if (Keys_NewlyPressed.Contains(Keys.Down))
            {
                DistanceChange -= 0.001F;
                if (Shifting)
                {
                    DistanceChange -= 0.001F;
                }
            }

            //Change Length of Intitial Branches, Acts as ZOOM feature
            if (Keys_NewlyPressed.Contains(Keys.OemMinus))
            {
                InitialDistance--;
                if (Shifting)
                {
                    InitialDistance--;
                }
                InitialDistance = Math.Abs(InitialDistance);
            }
            else if (Keys_NewlyPressed.Contains(Keys.OemPlus))
            {
                InitialDistance++;
                if (Shifting)
                {
                    InitialDistance++;
                }
            }

            //Change Number of Brances
            if (Keys_NewlyPressed.Contains(Keys.PageUp) && !Keys_BeingPressed.Contains(Keys.PageUp))
            {
                BranchSteps++;
            }
            else if (Keys_NewlyPressed.Contains(Keys.PageDown) && !Keys_BeingPressed.Contains(Keys.PageDown))
            {
                if (BranchSteps > 1)
                {
                    BranchSteps--;
                }
            }


            //Move Fractal
            if (Keys_NewlyPressed.Contains(Keys.D))
            {
                InitialPosition.X--;
                if (Shifting)
                {
                    InitialPosition.X--;
                }
            }
            else if (Keys_NewlyPressed.Contains(Keys.A))
            {
                InitialPosition.X++;
                if (Shifting)
                {
                    InitialPosition.X++;
                }
            }
            if (Keys_NewlyPressed.Contains(Keys.W))
            {
                InitialPosition.Y++;
                if (Shifting)
                {
                    InitialPosition.Y++;
                }
            }
            else if (Keys_NewlyPressed.Contains(Keys.S))
            {
                InitialPosition.Y--;
                if (Shifting)
                {
                    InitialPosition.Y--;
                }
            }

            
            //Invert Distance Multiplyer
            if (Keys_NewlyPressed.Contains(Keys.I) && !Keys_BeingPressed.Contains(Keys.I))
            {
                DistanceChange = -DistanceChange;
                RotationChange -= 180;
            }
            //Toggle Random Colour
            if (Keys_NewlyPressed.Contains(Keys.C) && !Keys_BeingPressed.Contains(Keys.C))
            {
                randomColors = !randomColors;
            }


            //Toggle Fullscreen
            if (Keys_NewlyPressed.Contains(Keys.F) && !Keys_BeingPressed.Contains(Keys.F))
            {
                _graphics.ToggleFullScreen();
            }



            Keys_BeingPressed = Keys_NewlyPressed;
        }

        void Main(SpriteBatch _spriteBatch, Vector4 Point, int BranchStep)
        {
            if (BranchStep < BranchSteps)
            {
                //Left
                float EndX = Point.X + (Point.Z * (float)Math.Cos(Point.W));
                float EndY = Point.Y + (Point.Z * (float)Math.Sin(Point.W));
                float NewAngle = Point.W + RotationChange;
                Main(_spriteBatch, new Vector4(EndX, EndY, Point.Z * DistanceChange, NewAngle), BranchStep + 1);

                //Right
                EndX = Point.X + (Point.Z * (float)Math.Cos(Point.W));
                EndY = Point.Y + (Point.Z * (float)Math.Sin(Point.W));
                NewAngle = Point.W - RotationChange;

                Main(_spriteBatch, new Vector4(EndX, EndY, Point.Z * DistanceChange, NewAngle), BranchStep + 1);
            }

            //Color Selection
            Color Color = Color.White;
            if (randomColors)
            {
                Color = Colors[random.Next(0, Colors.Count)];
            }

            DrawLine(new Vector2(Point.X, Point.Y), Point.Z, Point.W, Color);
        }

        /////////////////////////////////////////

        #region Fundamentals

        void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness = 1F)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(Length, Thickness);

            _spriteBatch.Draw(Color_White, point, null, Color, Angle, origin, scale, SpriteEffects.None, 0);
        }


        protected override void Update(GameTime gameTime)
        {
            KeyboardHandler();



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();



            Vector4 Start = new Vector4(InitialPosition.X, InitialPosition.Y, InitialDistance, 4.7123889804F);
            Main(_spriteBatch, Start, 0);
            


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}