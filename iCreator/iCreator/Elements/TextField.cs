using iCreator.Graphics.Advanced;
using iCreator.Graphics.Shapes;
using iCreator.Providers;
using iCreator.Utils;
using iCreator.Window;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace iCreator.Elements
{
    internal enum TextFieldType
    {
        Rectangle,
        RoundedRectangle
    }

    internal enum PointerDirection
    {
        Left,
        Right
    }

    internal enum TextType
    {
        Text,
        Password
    }

    internal sealed class Pointer
    {
        public RectFill Rect;
        public double TimeCheckElapsed = 0;
        public bool IsVisible = false;
        public int Position = 0;
        public Vector2 StartTopLeftCorner;
        public Vector2 Size;
    }

    internal sealed class TextBoxText
    {
        public string CurrentText = "";
        public int CurrentSubstringIndex = 0;
        public int FontSize;
        public float MaxWidth;
        public TextType TextType;
    }

    public sealed class TextField : Element
    {
        public override bool IsVisible { get; set; } = true;
        public string Text { get; private set; }

        private readonly ILogger logger;
        private readonly IKeyHelper keyHelper;
        private readonly ICursorProvider cursorProvider;
        private readonly ApplicationWindow applicationWindow;
        private readonly Text text;
        private readonly Text tooltipText;

        private Pointer pointer;
        private TextBoxText textBoxText;

        private Vector2 fieldPos;
        private Vector2 fieldSize;
        private bool isActive = false;
        private bool entered = false;

        private IShape rect;

        private delegate void KeyCallback();
        private Dictionary<Key, KeyCallback> keyCallbacks = new Dictionary<Key, KeyCallback>();

        private static char newChar;

        internal TextField(ILogger logger, IKeyHelper keyHelper, ICursorProvider cursorProvider, 
            ApplicationWindow applicationWindow, Text text, Text tooltipText)
        {
            this.logger = logger;
            this.keyHelper = keyHelper;
            this.cursorProvider = cursorProvider;
            this.applicationWindow = applicationWindow;
            this.text = text;
            this.tooltipText = tooltipText;

            keyCallbacks.Add(Key.Left, leftKeyPressed);
            keyCallbacks.Add(Key.Right, rightKeyPressed);
            keyCallbacks.Add(Key.BackSpace, backspaceKeyPressed);
            keyCallbacks.Add(Key.Delete, deleteKeyPressed);
        }

        internal void Setup(Vector2 pos, Vector2 size, Color4 color, Color4 textColor, string tooltip,
            Color4 tooltipColor, int fontSize = 16, TextFieldType textFieldType = TextFieldType.RoundedRectangle, 
            TextType textType = TextType.Text)
        {
            fieldPos = pos;
            fieldSize = size;
            setupBox(pos, size, color, textFieldType);
            setupText(pos, size, textColor, tooltip, tooltipColor, fontSize, textType);
            setupPointer(pos, size, textColor, fontSize);
        }

        internal override void OnUpdateFrame(FrameEventArgs args)
        {
            rect.OnUpdateFrame(args);
            text.OnUpdateFrame(args);
            pointer.Rect.OnUpdateFrame(args);
            tooltipText.OnUpdateFrame(args);

            pointer.TimeCheckElapsed += args.Time;

            if (pointer.TimeCheckElapsed >= 0.6)
            {
                pointer.TimeCheckElapsed = 0;
                pointer.IsVisible = !pointer.IsVisible;
            }
        }

        internal override void OnRenderFrame(FrameEventArgs args)
        {
            rect.OnRenderFrame(args);
            text.OnRenderFrame(args);

            if (textBoxText.CurrentText.Length == 0)
                tooltipText.OnRenderFrame(args);

            if (pointer.IsVisible && isActive)
                pointer.Rect.OnRenderFrame(args);
        }

        internal override void OnResize(EventArgs args)
        {
            rect.OnResize(args);
            text.OnResize(args);
            pointer.Rect.OnResize(args);
            tooltipText.OnResize(args);
        }

        internal override void OnKeyUp(KeyboardKeyEventArgs e)
        {

        }

        internal override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (!isActive)
                return;

            if (e.Keyboard.IsKeyDown(Key.LControl) || e.Keyboard.IsKeyDown(Key.RControl)
                || e.Keyboard.IsKeyDown(Key.LAlt) || e.Keyboard.IsKeyDown(Key.RAlt))
                return;

            if (keyCallbacks.ContainsKey(e.Key))
            {
                keyCallbacks[e.Key]();

                return;
            }

            newChar = keyHelper.GetKeyCharEquivalent(e.Key,
                e.Keyboard.IsKeyDown(Key.LShift) || e.Keyboard.IsKeyDown(Key.RShift));

            if (newChar != char.MaxValue)
            {
                textKeyPressed(newChar);
            }

            if(textBoxText.TextType == TextType.Text)
                Text = textBoxText.CurrentText;
        }

        internal override void OnMouseUp(MouseButtonEventArgs e)
        {
            
        }

        internal override void OnMouseDown(MouseButtonEventArgs args)
        {
            if (args.X >= fieldPos.X && args.X <= fieldPos.X + fieldSize.X
                && args.Y >= fieldPos.Y && args.Y <= fieldPos.Y + fieldSize.Y)
            {
                isActive = true;

                pointer.IsVisible = true;
                pointer.TimeCheckElapsed = 0;
            }
            else
                isActive = false;
        }

        internal override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (e.X >= fieldPos.X && e.X <= fieldPos.X + fieldSize.X
                && e.Y >= fieldPos.Y && e.Y <= fieldPos.Y + fieldSize.Y)
            {
                applicationWindow.Cursor = cursorProvider.GetIBeamCursor();

                entered = true;
            }
            else if (entered)
            {
                applicationWindow.Cursor = MouseCursor.Default;

                entered = false;
            }
        }

        private void setupBox(Vector2 pos, Vector2 size, Color4 color, TextFieldType type)
        {
            switch (type)
            {
                case TextFieldType.Rectangle: rect = new RectNoFill(pos, size, color); break;
                case TextFieldType.RoundedRectangle: rect = new RoundRectNoFill(pos, size, color); break;
            }
        }

        private void setupText(Vector2 pos, Vector2 size, Color4 textColor, 
            string tooltip, Color4 tooltipColor, int fontSize, TextType textType)
        {
            System.Drawing.PointF topLeftCorner = new System.Drawing.PointF();
            topLeftCorner.X = pos.X + 10f;
            topLeftCorner.Y = pos.Y + ((size.Y - text.MeasureText("I", fontSize).Y) / 2f) + 2f;

            text.Setup("", topLeftCorner, textColor, fontSize);
            tooltipText.Setup(tooltip, topLeftCorner, tooltipColor, fontSize);

            textBoxText = new TextBoxText();
            textBoxText.FontSize = fontSize;
            textBoxText.MaxWidth = size.X - 24f;
            textBoxText.TextType = textType;
            textBoxText.CurrentText = "";
            Text = "";
        }

        private void setupPointer(Vector2 pos, Vector2 size, Color4 color, int fontSize)
        {
            Vector2 topleftCorner = new Vector2();
            topleftCorner.X = pos.X + 12f;
            topleftCorner.Y = pos.Y + ((size.Y - text.MeasureText("I", fontSize).Y) / 2f);

            Vector2 pointerSize = new Vector2();
            pointerSize.X = 1;
            pointerSize.Y = text.MeasureText("I", fontSize).Y - 2f;

            pointer = new Pointer();
            pointer.StartTopLeftCorner = topleftCorner;
            pointer.Size = pointerSize;

            pointer.Rect = new RectFill(topleftCorner, pointerSize, color);
        }

        private void leftKeyPressed()
        {

        }

        private void rightKeyPressed()
        {

        }

        private void backspaceKeyPressed()
        {
            if (textBoxText.CurrentText.Length > 0)
            {
                textBoxText.CurrentText = textBoxText.CurrentText.Remove(pointer.Position - 1, 1);

                if (textBoxText.TextType == TextType.Password)
                    Text = Text.Remove(pointer.Position - 1, 1);
                else
                    Text = textBoxText.CurrentText;

                fillTextInTextBox();
                updatePointerPosition(PointerDirection.Left);
            }
        }

        private void deleteKeyPressed()
        {
        }

        private void textKeyPressed(char newChar)
        {
            if(textBoxText.TextType == TextType.Password)
            {
                textBoxText.CurrentText = textBoxText.CurrentText.Insert(pointer.Position, "*");
                Text = Text.Insert(pointer.Position, char.ToString(newChar));
            }
            else
            {
                textBoxText.CurrentText = textBoxText.CurrentText.Insert(pointer.Position, char.ToString(newChar));
            }
            
            
            fillTextInTextBox();

            updatePointerPosition(PointerDirection.Right);
        }

        private void fillTextInTextBox()
        {
                while (text.MeasureText(textBoxText.CurrentText.Substring(textBoxText.CurrentSubstringIndex)
                .Replace(" ", "["), textBoxText.FontSize).X > textBoxText.MaxWidth)
                    textBoxText.CurrentSubstringIndex++;

                while (text.MeasureText(textBoxText.CurrentText.Substring(textBoxText.CurrentSubstringIndex)
                    .Replace(" ", "["), textBoxText.FontSize).X <= textBoxText.MaxWidth && textBoxText.CurrentSubstringIndex > 0)
                    textBoxText.CurrentSubstringIndex--;


                text.SetText(textBoxText.CurrentText.Substring(textBoxText.CurrentSubstringIndex));
        }

        private void updatePointerPosition(PointerDirection pointerDirection)
        {
            pointer.IsVisible = true;
            pointer.TimeCheckElapsed = 0;

            switch (pointerDirection)
            {
                case PointerDirection.Right:
                    {
                        string shownText = textBoxText.CurrentText.Substring(textBoxText.CurrentSubstringIndex);

                        if (pointer.Position < textBoxText.CurrentText.Length)
                            pointer.Position++;

                        Vector2 newPosition = pointer.StartTopLeftCorner;

                        newPosition.X += text.MeasureText(shownText.Substring(0, pointer.Position -
                            textBoxText.CurrentSubstringIndex).Replace(" ", "["), textBoxText.FontSize).X - 6f;

                        pointer.Rect.MoveLeftTopCorner(newPosition, pointer.Size);

                        break;
                    }
                case PointerDirection.Left:
                    {
                        if (pointer.Position > 0)
                            pointer.Position--;

                        string shownText = textBoxText.CurrentText.Substring(textBoxText.CurrentSubstringIndex);

                        Vector2 newPosition = pointer.StartTopLeftCorner;

                        if (pointer.Position > textBoxText.CurrentSubstringIndex)
                            newPosition.X += text.MeasureText(shownText.Substring(0, pointer.Position -
                                textBoxText.CurrentSubstringIndex).Replace(" ", "["), textBoxText.FontSize).X - 6f;

                        pointer.Rect.MoveLeftTopCorner(newPosition, pointer.Size);

                        break;
                    }
            }
        }
    }
}
