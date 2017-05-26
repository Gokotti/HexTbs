using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Assets
{
    public static class MouseController
    {
        static private Vector2 mouseVector;
        static private MouseState oldMouseState;
        static private int scrollValue;
        static private int oldScrollValue;

        static private bool rightClick = false;
        static private bool leftClick = false;
        static private bool dragged = false;
        static private bool rightDragged = false;
        static private bool doubleClick = false;

        static private Vector2 dragStart = Vector2.Zero;
        static private Vector2 dragEnd = Vector2.Zero;
        static public Vector2 dragTreshold = new Vector2(24, 24); // How long drag doesn't count as a drag
        static public Vector2 rightDragTreshold = new Vector2(5, 5);

        static private Timer doubleClickTimer = new Timer(250);

        static public void Update(GameTime gt)
        {
            mouseVector = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            MouseState newMouseState = Mouse.GetState();

            doubleClickTimer.Update(gt);

            if (doubleClickTimer.IsTimedOut())
            {
                doubleClickTimer.Stop();
                doubleClickTimer.Reset();
            }

            // Drag
            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                if (!dragged)
                {
                    dragStart = mouseVector;
                    dragEnd = mouseVector;
                    dragged = true;
                    leftClick = false;
                }
                else
                {
                    dragEnd = mouseVector;
                }
            }
            // When neither leftclick nor drag
            else if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Released)
            {
                leftClick = false;
                dragged = false;
            }
            // Left click
            else if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                leftClick = true;

                // Doubleclicks
                if (!doubleClickTimer.IsStarted())
                {
                    doubleClickTimer.Start();
                }
                else
                {
                    doubleClick = true;
                    doubleClickTimer.Stop();
                    doubleClickTimer.Reset();
                }
            }

            // Right drag
            if (newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed)
            {
                if (!rightDragged)
                {
                    dragStart = mouseVector;
                    dragEnd = mouseVector;
                    rightDragged = true;
                    rightClick = false;
                }
                else
                {
                    dragEnd = mouseVector;
                }
            }
            // When neither rightclick nor drag
            else if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Released)
            {
                rightClick = false;
                rightDragged = false;
            }
            // Righty
            else if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
            {
                rightClick = true;
            }

            /* Scroll */
            if (newMouseState.ScrollWheelValue < oldScrollValue)
                scrollValue = -1;
            else if (newMouseState.ScrollWheelValue > oldScrollValue)
                scrollValue = 1;
            else
                scrollValue = 0;

            oldScrollValue = newMouseState.ScrollWheelValue;

            // This reassigns the old state so that it is ready for next time
            oldMouseState = newMouseState;
        }

        static public bool LeftClicked()
        {
            if (leftClick && !Dragged())
            {
                dragStart = Vector2.Zero;
                dragEnd = Vector2.Zero;
                leftClick = false;
                return true;
            }

            return false;
        }

        static public bool RightClicked()
        {
            if (rightClick && !RightDragged())
            {
                dragStart = Vector2.Zero;
                dragEnd = Vector2.Zero;
                rightClick = false;
                return true;
            }

            return false;
        }

        static public bool Dragged()
        {
            int width = Math.Abs((int)dragStart.X - (int)dragEnd.X);
            int height = Math.Abs((int)dragStart.Y - (int)dragEnd.Y);
            if (dragged && (width > dragTreshold.X || height > dragTreshold.Y))
            {
                return true;
            }
            return false;
        }

        static public bool RightDragged()
        {
            int width = Math.Abs((int)dragStart.X - (int)dragEnd.X);
            int height = Math.Abs((int)dragStart.Y - (int)dragEnd.Y);
            if (rightDragged && (width > rightDragTreshold.X || height > rightDragTreshold.Y))
            {
                return true;
            }
            return false;
        }

        static public Vector2 GetMouseVector()
        {
            return mouseVector;
        }

        static public Rectangle GetDragtangle()
        {
            int x = Math.Min((int)dragStart.X, (int)dragEnd.X);
            int y = Math.Min((int)dragStart.Y, (int)dragEnd.Y);
            int width = Math.Abs((int)dragStart.X - (int)dragEnd.X);
            int height = Math.Abs((int)dragStart.Y - (int)dragEnd.Y);
            Rectangle dragtangle = new Rectangle(x, y, width, height);
            return dragtangle;
        }

        static public Vector2 GetDragtor()
        {
            return (dragStart - dragEnd);
        }

        static public int GetScrollValue()
        {
            return scrollValue;
        }

        static public bool DoubleClicked()
        {
            if (doubleClick)
            {
                doubleClick = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
