using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace PONG
{
    internal enum GameState
    {
        Start,
        Game,
        End
    }

    internal class GameStateManager
    {
        // Game State variables

        // Start screen
        //private static bool startVisible = true;

        // Function for start screen
        internal static void StartScreen(GameState gamestate, KeyboardState kstate)
        {
            // Start visibility
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //long timeElapsed = 500;

            // Blinking Start Instructions
            //if (stopwatch.ElapsedMilliseconds >= timeElapsed)
            //{
            //    if (startVisible == true)
            //    {
            //        startVisible = false;
            //        stopwatch.Restart();
            //    }
            //    else
            //    {
            //        startVisible = true;
            //        stopwatch.Restart();
            //    }
            //}

            // Start game
            if (kstate.IsKeyDown(Keys.Enter))
            {
                gamestate = GameState.Game;
            }
        }

        // Function for game loop
        internal static void WeGaming(
            ref GameState gamestate,
            GameTime gameTime,
            KeyboardState kstate,
            ref float _boneSpeed1,
            ref Vector2 _bonePosition1,
            GraphicsDeviceManager _graphics,
            Texture2D _boneTexture,
            ref float _boneSpeed2,
            ref Vector2 _bonePosition2,
            ref float _skullSpeed,
            ref Vector2 _skullPosition,
            Texture2D _skullTexture,
            ref int _randomIntX,
            ref int _randomIntY,
            Random _random,
            ref int _p1_points,
            ref int _p2_points,
            ref string _p1_points_string,
            ref string _p2_points_string
            )
        {
            // Bone1 movement

            // Time since last Update call, duration of a single frame
            float updatedBoneSpeed1 = _boneSpeed1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move Bone1 up
            if (kstate.IsKeyDown(Keys.W))
            {
                _bonePosition1.Y -= updatedBoneSpeed1;
            }

            // Move Bone1 down
            if (kstate.IsKeyDown(Keys.S))
            {
                _bonePosition1.Y += updatedBoneSpeed1;
            }

            // Bone 1 Edge Collision
            if (_bonePosition1.Y > GameConstants.VirtualHeight - _boneTexture.Height / 2)
            {
                _bonePosition1.Y = GameConstants.VirtualHeight - _boneTexture.Height / 2;
            }
            else if (_bonePosition1.Y < _boneTexture.Height / 2)
            {
                _bonePosition1.Y = _boneTexture.Height / 2;
            }

            // Bone2 movement

            // Time since last Update call, duration of a single frame
            float updatedBoneSpeed2 = _boneSpeed2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move Bone2 up
            if (kstate.IsKeyDown(Keys.Up))
            {
                _bonePosition2.Y -= updatedBoneSpeed2;
            }

            // Move Bone2 down
            if (kstate.IsKeyDown(Keys.Down))
            {
                _bonePosition2.Y += updatedBoneSpeed2;
            }

            // Bone 2 Edge Collision
            if (_bonePosition2.Y > GameConstants.VirtualHeight - _boneTexture.Height / 2)
            {
                _bonePosition2.Y = GameConstants.VirtualHeight - _boneTexture.Height / 2;
            }
            else if (_bonePosition2.Y < _boneTexture.Height / 2)
            {
                _bonePosition2.Y = _boneTexture.Height / 2;
            }

            // Skull movement

            // Time since last Update call, duration of a single frame
            float updatedSkullSpeed = _skullSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move Skull Left or Right
            if (_randomIntX == 0)
            {
                _skullPosition.X -= updatedSkullSpeed;
            }
            else
            {
                _skullPosition.X += updatedSkullSpeed;
            }

            // Move Skull Up or Down
            if (_randomIntY == 0)
            {
                _skullPosition.Y -= updatedSkullSpeed / 4;
            }
            else
            {
                _skullPosition.Y += updatedSkullSpeed / 4;
            }

            // Skull Edge Bounce
            if (_skullPosition.Y > GameConstants.VirtualHeight - _skullTexture.Height / 2)
            {
                _randomIntY = _randomIntY == 0 ? 1 : 0;
            }
            else if (_skullPosition.Y < _skullTexture.Height / 2)
            {
                _randomIntY = _randomIntY == 0 ? 1 : 0;
            }

            // Skull Bone Bounce
            if (_skullPosition.X - _skullTexture.Width / 2 < _bonePosition1.X + _boneTexture.Width / 2 &&
                _skullPosition.X + _skullTexture.Width / 2 > _bonePosition1.X - _boneTexture.Width / 2 &&
                _skullPosition.Y - _skullTexture.Height / 2 < _bonePosition1.Y + _boneTexture.Height / 2 &&
                _skullPosition.Y + _skullTexture.Height / 2 > _bonePosition1.Y - _boneTexture.Height / 2)
            {
                _randomIntX = _randomIntX == 0 ? 1 : 0;
                _randomIntY = _randomIntY == 0 ? 0 : 1;
                _skullSpeed += 25f;
            }
            else if (_skullPosition.X - _skullTexture.Width / 2 < _bonePosition2.X + _boneTexture.Width / 2 &&
                _skullPosition.X + _skullTexture.Width / 2 > _bonePosition2.X - _boneTexture.Width / 2 &&
                _skullPosition.Y - _skullTexture.Height / 2 < _bonePosition2.Y + _boneTexture.Height / 2 &&
                _skullPosition.Y + _skullTexture.Height / 2 > _bonePosition2.Y - _boneTexture.Height / 2)
            {
                _randomIntX = _randomIntX == 0 ? 1 : 0;
                _randomIntY = _randomIntY == 0 ? 0 : 1;
                _skullSpeed += 25f;
            }

            // Skull Reset
            if (_skullPosition.X > _bonePosition2.X || _skullPosition.X < _bonePosition1.X)
            {
                if (_skullPosition.X < _bonePosition1.X)
                {
                    // Skull reset, launch to P1
                    _skullPosition = new Vector2(
                        GameConstants.VirtualWidth / 2,
                        GameConstants.VirtualHeight / 2
                    );

                    _skullSpeed = 150f;

                    _randomIntX = 0;
                    _randomIntY = _random.Next(0, 2);

                    // P2 gets a point
                    _p2_points++;
                    switch
                        (_p2_points)
                    {
                        case 0:
                            _p2_points_string = "O";
                            break;
                        case 1:
                            _p2_points_string = "I";
                            break;
                        case 2:
                            _p2_points_string = "II";
                            break;
                        case 3:
                            _p2_points_string = "III";
                            break;
                        case 4:
                            _p2_points_string = "IV";
                            break;
                        case 5:
                            _p2_points_string = "V";
                            break;
                        case 6:
                            _p2_points_string = "VI";
                            break;
                        case 7:
                            _p2_points_string = "VII";
                            break;
                        case 8:
                            _p2_points_string = "VIII";
                            break;
                        case 9:
                            _p2_points_string = "IX";
                            break;
                        case 10:
                            _p2_points_string = "X";
                            break;
                    }
                }
                else
                {
                    // Skull reset, launch to P2
                    _skullPosition = new Vector2(
                        GameConstants.VirtualWidth / 2,
                        GameConstants.VirtualHeight / 2
                    );

                    _skullSpeed = 150f;

                    _randomIntX = 1;
                    _randomIntY = _random.Next(0, 2);

                    // P1 gets a point
                    _p1_points++;
                    switch
                        (_p1_points)
                    {
                        case 0:
                            _p1_points_string = "O";
                            break;
                        case 1:
                            _p1_points_string = "I";
                            break;
                        case 2:
                            _p1_points_string = "II";
                            break;
                        case 3:
                            _p1_points_string = "III";
                            break;
                        case 4:
                            _p1_points_string = "IV";
                            break;
                        case 5:
                            _p1_points_string = "V";
                            break;
                        case 6:
                            _p1_points_string = "VI";
                            break;
                        case 7:
                            _p1_points_string = "VII";
                            break;
                        case 8:
                            _p1_points_string = "VIII";
                            break;
                        case 9:
                            _p1_points_string = "IX";
                            break;
                        case 10:
                            _p1_points_string = "X";
                            break;
                    }
                }
            }

            // End game
            if (_p1_points == 11 || _p2_points == 11)
            {
                gamestate = GameState.End;
            }
        }

        // Function for end screen
        internal static string EndScreen(int _p1_points, string player)
        {
            if (_p1_points == 11)
            {
                // P1 wins
                player = "Player I";
            }
            else
            {
                // P2 wins
                player = "Player II";
            }

            return player;
        }
    }
}
