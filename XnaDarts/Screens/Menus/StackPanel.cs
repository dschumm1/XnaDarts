using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public class StackPanel : GuiItem
    {
        public List<GuiItem> Items = new List<GuiItem>();
        public StackPanelOrientation Orientation = StackPanelOrientation.Vertical;

        public override int Width
        {
            get { return Items.Max(item => item.Width); }
            set { throw new NotImplementedException(); }
        }

        public override int Height
        {
            get { return Items.Max(item => item.Height); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Helper method for adding several items at once
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(params GuiItem[] items)
        {
            foreach (var menuItem in items)
            {
                Items.Add(menuItem);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, float transitionAlpha)
        {
            position += new Vector2(Margin.Left, Margin.Top);

            for (var i = 0; i < Items.Count; i++)
            {
                position += new Vector2(Items[i].Margin.Left, Items[i].Margin.Top);

                Items[i].Draw(spriteBatch, position, transitionAlpha);

                switch (Orientation)
                {
                    case StackPanelOrientation.Horizontal:
                        position.X += Items[i].Width + Items[i].Margin.Right;
                        break;
                    case StackPanelOrientation.Vertical:
                        position.Y += Items[i].Height + Items[i].Margin.Bottom;
                        break;
                }
            }
        }

        public override void HandleInput(InputState input)
        {
        }
    }
}