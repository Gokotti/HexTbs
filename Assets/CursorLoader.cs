using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Assets
{
    public enum AssetCursor
    {
        arrow, attack, blocked, move, scroll, select, support
    }

    public static class CursorLoader
    {
        public static Microsoft.Xna.Framework.Game game;

        public static void SetCursor(AssetCursor c)
        {
            Cursor myCursor = LoadCursor(c);
            Form winForm = (Form)Form.FromHandle(game.Window.Handle);
            winForm.Cursor = myCursor;
        }

        private static Cursor LoadCursor(AssetCursor c)
        {
            if (c == AssetCursor.arrow)
                return LoadCustomCursor( "" );
            else if (c == AssetCursor.attack)
                return LoadCustomCursor(@"Content\Cursors\attack.cur");
            else if (c == AssetCursor.blocked)
                return LoadCustomCursor(@"Content\Cursors\blocked.cur");
            else if (c == AssetCursor.move)
                return LoadCustomCursor(@"Content\Cursors\move.cur");
            else if (c == AssetCursor.scroll)
                return LoadCustomCursor(@"Content\Cursors\scroll.cur");
            else if (c == AssetCursor.select)
                return LoadCustomCursor(@"Content\Cursors\select.cur");
            else if (c == AssetCursor.support)
                return LoadCustomCursor(@"Content\Cursors\support.cur");
            else
                return LoadCustomCursor(@"Content\Cursors\arrow.cur");
        }

        // Cursor loader
        private static Cursor LoadCustomCursor(string path)
        {
            IntPtr hCurs = LoadCursorFromFile(path);
            if (hCurs == IntPtr.Zero) throw new Win32Exception();
            var curs = new Cursor(hCurs);
            // Note: force the cursor to own the handle so it gets released properly
            var fi = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(curs, true);
            return curs;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);
    }
}
