using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IPCA.MonoGame
{
    public enum KeysState { Up, Down, GoingUp, GoingDown }


    public class KeyboardManager : GameComponent
    {
        private static KeyboardManager _instance;
        private Dictionary<Keys, KeysState> _keyboardState;
        private Dictionary<Keys, Dictionary<KeysState, List<Action>>> _actions;


        public KeyboardManager(Game game) : base(game)
        {
            if (_instance != null) throw new Exception("KeyboarManager foi chamado duas vezes");
            _instance = this;
            _keyboardState = new Dictionary<Keys, KeysState>();

            _actions = new Dictionary<Keys, Dictionary<KeysState, List<Action>>>();
            game.Components.Add(this);


        }

        public static void Register(Keys key, KeysState state, Action code)
        {
            if (!_instance._actions.ContainsKey(key))
                _instance._actions[key] = new Dictionary<KeysState, List<Action>>();
            if (!_instance._actions[key].ContainsKey(state))
                _instance._actions[key][state] = new List<Action>();

            _instance._actions[key][state].Add(code);
            _instance._keyboardState[key] = KeysState.Up;
        }

        public static bool IsKeyDown(Keys k) => _instance._keyboardState.ContainsKey(k) && _instance._keyboardState[k] == KeysState.Down;
        public static bool IsKeyUp(Keys k) => _instance._keyboardState.ContainsKey(k) && _instance._keyboardState[k] == KeysState.Up;
        public static bool IsGoingDown(Keys k) => _instance._keyboardState.ContainsKey(k) && _instance._keyboardState?[k] == KeysState.GoingDown;
        public static bool IsGoingUp(Keys k) => _instance._keyboardState.ContainsKey(k) && _instance._keyboardState?[k] == KeysState.GoingUp;


        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            List<Keys> pressedKeys = state.GetPressedKeys().ToList();

            foreach (Keys key in pressedKeys)
            {
                if (!_keyboardState.ContainsKey(key)) _keyboardState[key] = KeysState.Up;

                switch (_keyboardState[key])
                {
                    case KeysState.Down:
                    case KeysState.GoingDown:
                        _keyboardState[key] = KeysState.Down; break;
                    case KeysState.Up:
                    case KeysState.GoingUp:
                        _keyboardState[key] = KeysState.GoingDown; break;
                }
            }

            foreach (Keys key in _keyboardState.Keys.Except(pressedKeys).ToArray())
            {
                switch (_keyboardState[key])
                {
                    case KeysState.Down:
                    case KeysState.GoingDown:
                        _keyboardState[key] = KeysState.GoingUp; break;
                    case KeysState.Up:
                    case KeysState.GoingUp:
                        _keyboardState[key] = KeysState.Up; break;
                }


            }

            foreach (Keys key in _actions.Keys)
            {

                KeysState kstate = _keyboardState[key];
                if (_actions[key].ContainsKey(kstate))
                {
                    List<Action> actionsList = _actions[key][kstate];
                    foreach (Action action in actionsList)
                    {
                        action();
                    }
                }
            }
        }

    }
}

