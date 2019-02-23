using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace iCreator.Utils
{
    internal interface IKeyHelper
    {
        char GetKeyCharEquivalent(Key key, bool isLeftShiftPressed = false);
    }

    internal sealed class KeyHelper : IKeyHelper
    {
        private readonly Dictionary<Key, char> lowerDictionary;
        private readonly Dictionary<char, char> upperDictionary;
        private readonly ILogger logger;

        public KeyHelper(ILogger logger)
        {
            this.logger = logger;

            lowerDictionary = new Dictionary<Key, char>();
            upperDictionary = new Dictionary<char, char>();

            setupDictionaries();
        }

        public char GetKeyCharEquivalent(Key key, bool isLeftShiftPressed = false)
        {
            if (!lowerDictionary.ContainsKey(key))
                return char.MaxValue;

            char lowerCaseChar = lowerDictionary[key];

            if (isLeftShiftPressed && !upperDictionary.ContainsKey(lowerCaseChar))
                return char.MaxValue;

            if (key.ToString().Contains("Keypad") && (!Console.NumberLock || isLeftShiftPressed))
                return char.MaxValue;

            return isLeftShiftPressed ? upperDictionary[lowerCaseChar] : Console.CapsLock ? char.ToUpper(lowerCaseChar) : lowerCaseChar;
        }

        private void setupDictionaries()
        {
            setupLetters();
            setupNumbers();
            setupOthers();
        }

        private void setupLetters()
        {
            for (int i = 65; i <= 90; i++)
            {
                upperDictionary.Add((char)(i + 32), (char)i);
                Key key;
                Enum.TryParse(char.ToString((char)i), out key);
                lowerDictionary.Add(key, (char)(i + 32));
            }
        }

        private void setupNumbers()
        {
            char[] tempNumbersTab = new char[] { ')', '!', '@', '#', '$', '%', '^', '&', '*', '(' };

            for (int i = 48; i <= 57; i++)
            {
                Key numberKey;
                Key keypadKey;
                Enum.TryParse($"Number{ i - 48 }", out numberKey);
                Enum.TryParse($"Keypad{ i - 48 }", out keypadKey);

                upperDictionary.Add((char)i, tempNumbersTab[i - 48]);
                lowerDictionary.Add(numberKey, (char)(i));
                lowerDictionary.Add(keypadKey, (char)(i));
            }
        }

        private void setupOthers()
        {
            lowerDictionary.Add(Key.Space, ' ');
            lowerDictionary.Add(Key.Tilde, '`');
            lowerDictionary.Add(Key.Minus, '-');
            lowerDictionary.Add(Key.Plus, '=');
            lowerDictionary.Add(Key.BackSlash, '\\');
            lowerDictionary.Add(Key.BracketLeft, '[');
            lowerDictionary.Add(Key.BracketRight, ']');
            lowerDictionary.Add(Key.Semicolon, ';');
            lowerDictionary.Add(Key.Quote, '\'');
            lowerDictionary.Add(Key.Comma, ',');
            lowerDictionary.Add(Key.Period, '.');
            lowerDictionary.Add(Key.Slash, '/');

            // KEYPAD
            lowerDictionary.Add(Key.KeypadDivide, '/');
            lowerDictionary.Add(Key.KeypadMultiply, '*');
            lowerDictionary.Add(Key.KeypadMinus, '-');
            lowerDictionary.Add(Key.KeypadPlus, '+');
            lowerDictionary.Add(Key.KeypadPeriod, '.');

            upperDictionary.Add(' ', ' ');
            upperDictionary.Add('`', '~');
            upperDictionary.Add('-', '_');
            upperDictionary.Add('=', '+');
            upperDictionary.Add('\\', '|');
            upperDictionary.Add('[', '{');
            upperDictionary.Add(']', '}');
            upperDictionary.Add(';', ':');
            upperDictionary.Add('\'', '"');
            upperDictionary.Add(',', '<');
            upperDictionary.Add('.', '>');
            upperDictionary.Add('/', '?');
        }
    }
}
