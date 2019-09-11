using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scroll.GameSystem
{
    static class InputContllorer
    {
        private static KeyboardState keyboardState = Keyboard.GetState();
        private static GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);
        private static GamePadState lastGamePadState = GamePad.GetState(PlayerIndex.One);

        private class KeyState
        {
            public bool push;
            public bool press;

            public KeyState()
            {
                push = false;
                press = false;
            }

            public void InputUpdate(Keys keys)
            {
                if (keyboardState.IsKeyDown(keys))
                {
                    if (press)
                    {
                        push = false;
                    }
                    else
                    {
                        push = true;
                        press = true;
                    }
                }
                else
                {
                    push = false;
                    press = false;
                }
            }
        }

        private static Buttons a;

        private static Dictionary<Keys, KeyState> keyStates = new Dictionary<Keys, KeyState>();

        public static void Init()
        {
            keyStates.Add(Keys.Left, new KeyState());
            keyStates.Add(Keys.Right, new KeyState());
            keyStates.Add(Keys.Up, new KeyState());
            keyStates.Add(Keys.Down, new KeyState());
            keyStates.Add(Keys.Z, new KeyState());
            keyStates.Add(Keys.X, new KeyState());
            keyStates.Add(Keys.C, new KeyState());
            keyStates.Add(Keys.Q, new KeyState());
            keyStates.Add(Keys.E, new KeyState());
            keyStates.Add(Keys.A, new KeyState());
            keyStates.Add(Keys.D, new KeyState());
            keyStates.Add(Keys.LeftShift, new KeyState());
        }

        public static void InputUpdate()
        {
            keyboardState = Keyboard.GetState();

            foreach (var k in keyStates)
            {
                k.Value.InputUpdate(k.Key);
            }

            lastGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public static bool IsPush(Keys keys)
        {
            return keyStates[keys].push;
        }

        public static bool IsPress(Keys keys)
        {
            return keyStates[keys].press;
        }

        public static bool IsPress(Buttons buttons)
        {
            switch (buttons)
            {
                case Buttons.A:
                    return currentGamePadState.Buttons.A == ButtonState.Pressed;
                case Buttons.B:
                    return currentGamePadState.Buttons.B == ButtonState.Pressed;

                default:
                    return false;

            }
        }

        public static bool IsPush(Buttons buttons)
        {
            switch (buttons)
            {
                case Buttons.A:
                    return (currentGamePadState.Buttons.A == ButtonState.Pressed &&
                        lastGamePadState.Buttons.A == ButtonState.Released);
                case Buttons.B:
                    return (currentGamePadState.Buttons.B == ButtonState.Pressed &&
                        lastGamePadState.Buttons.B == ButtonState.Released);
                default:
                    return false;

            }
        }

        public static float StickLeftX()
        {
            return currentGamePadState.ThumbSticks.Left.X;
        }
        public static float StickLeftY()
        {
            return currentGamePadState.ThumbSticks.Left.Y;
        }
        public static float StickRightX()
        {
            return currentGamePadState.ThumbSticks.Right.X;
        }
        public static float StickRightY()
        {
            return currentGamePadState.ThumbSticks.Right.Y;
        }

    }
}
