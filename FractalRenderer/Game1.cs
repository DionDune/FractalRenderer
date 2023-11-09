using Microsoft.Xna.Framework;
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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D Color_White;

        float InitialDistance;
        float RotationChange;
        float DistanceChange;
        int BranchSteps;

        List<Keys> Keys_BeingPressed = new List<Keys>();

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
            RotationChange = 25F;
            DistanceChange = 0.9F;
            BranchSteps = 10;
            InitialDistance = 100;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Color_White = Content.Load<Texture2D>("Color_White");
        }

        #endregion

        /////////////////////////////////////////
        void KeyboardHandler()
        {
            List<Keys> Keys_NewlyPressed = Keyboard.GetState().GetPressedKeys().ToList();



            if (Keys_NewlyPressed.Contains(Keys.Right))
            {
                RotationChange += 0.002F;
            }
            else if (Keys_NewlyPressed.Contains(Keys.Left))
            {
                RotationChange -= 0.002F;
            }
            if (RotationChange < 0)
            {
                RotationChange = 360 + RotationChange;
            }
            else if (RotationChange > 360)
            {
                RotationChange -= 360;
            }

            if (Keys_NewlyPressed.Contains(Keys.Up))
            {
                DistanceChange += 0.001F;
            }
            else if (Keys_NewlyPressed.Contains(Keys.Down))
            {
                DistanceChange -= 0.001F;
            }

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



            Keys_BeingPressed = Keys_NewlyPressed;
        }

        /////////////////////////////////////////

        #region Fundamentals

        void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness = 0.95F)
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
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            Vector4 Start = new Vector4(900,900,InitialDistance, 4.7123889804F);
            (int, Vector4) StartBranch = (0, Start);


            List<(int, Vector4)> ToCheck = new List<(int, Vector4)>() { StartBranch };

            while (ToCheck.Count > 0)
            {
                if (ToCheck[0].Item1 < BranchSteps)
                {
                    //Left
                    float EndX = ToCheck[0].Item2.X + (ToCheck[0].Item2.Z * (float)Math.Cos(ToCheck[0].Item2.W));
                    float EndY = ToCheck[0].Item2.Y + (ToCheck[0].Item2.Z * (float)Math.Sin(ToCheck[0].Item2.W));
                    float NewAngle = ToCheck[0].Item2.W + RotationChange;
                    ToCheck.Add((ToCheck[0].Item1 + 1, new Vector4(EndX, EndY, ToCheck[0].Item2.Z * DistanceChange, NewAngle)));

                    //Right
                    EndX = ToCheck[0].Item2.X + (ToCheck[0].Item2.Z * (float)Math.Cos(ToCheck[0].Item2.W));
                    EndY = ToCheck[0].Item2.Y + (ToCheck[0].Item2.Z * (float)Math.Sin(ToCheck[0].Item2.W));
                    NewAngle = ToCheck[0].Item2.W - RotationChange;

                    ToCheck.Add((ToCheck[0].Item1 + 1, new Vector4(EndX, EndY, ToCheck[0].Item2.Z * DistanceChange, NewAngle)));
                }


                DrawLine(new Vector2(ToCheck[0].Item2.X, ToCheck[0].Item2.Y), ToCheck[0].Item2.Z, ToCheck[0].Item2.W, Color.Black);
                //_spriteBatch.Draw(Color_White, new Rectangle((int)ToCheck[0].Item2.X - 2, (int)ToCheck[0].Item2.Y - 2, 4, 4), Color.Black);

                ToCheck.RemoveAt(0);
            }
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}