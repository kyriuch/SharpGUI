using iCreator.Models;
using iCreator.Utils;
using OpenTK;
using System.Drawing;

namespace iCreator.Providers
{
    internal interface ICursorProvider
    {
        MouseCursor GetIBeamCursor();
        MouseCursor GetHandCursor();
    }

    internal sealed class CursorProvider : ICursorProvider
    {
        private readonly ILogger logger;

        private MouseCursor iBeamCursor;
        private MouseCursor handCursor;

        public CursorProvider(ILogger logger, View view)
        {
            this.logger = logger;

            string iBeamCursorFilePath, handCursorFilePath;

            if(view.ThemeName.Equals("Dark"))
                iBeamCursorFilePath = "iCreator\\Resources\\i_beam_cursor_light.png";
            else
                iBeamCursorFilePath = "iCreator\\Resources\\i_beam_cursor_dark.png";

            handCursorFilePath = "iCreator\\Resources\\hand_cursor.png";

            Bitmap bitmap = new Bitmap(iBeamCursorFilePath);
            bitmap = new Bitmap(bitmap, new Size(bitmap.Width / 10, bitmap.Height / 10));

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            iBeamCursor = new MouseCursor(bitmap.Width / 2, bitmap.Height / 2, bitmap.Width, bitmap.Height, data.Scan0);

            bitmap.UnlockBits(data);

            bitmap = new Bitmap(handCursorFilePath);
            bitmap = new Bitmap(bitmap, new Size(bitmap.Width / 24, bitmap.Height / 24));

            data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            handCursor = new MouseCursor(bitmap.Width / 8, 0, bitmap.Width, bitmap.Height, data.Scan0);

            bitmap.UnlockBits(data);
        }

        public MouseCursor GetIBeamCursor() => iBeamCursor;

        public MouseCursor GetHandCursor() => handCursor;
    }
}
