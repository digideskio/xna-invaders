using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Invaders
{
    class KeyboardDelayableInput
    {
        public int DelayTime = 500;
        public KeyboardState CurrentState;
        SortedDictionary<Keys, int> keysStates;

        public KeyboardDelayableInput(int delayTime)
        {
            DelayTime = delayTime;
            keysStates = new SortedDictionary<Keys, int>();
        }

        public void Update(KeyboardState currentState)
        {
            CurrentState = currentState;
        }

        public bool KeyCheck(Keys key, GameTime gameTime)
        {
            int TotalMilliseconds = (int)gameTime.TotalGameTime.TotalMilliseconds;
            
            if (CurrentState.IsKeyDown(key) &&
                (!keysStates.ContainsKey(key) ||
                TotalMilliseconds - keysStates[key] > DelayTime))
            {
                keysStates[key] = TotalMilliseconds;
                return true;
            }
            return false;
        }
    }
}
