﻿using System.Linq;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SquidTestingMod.Helpers;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace SquidTestingMod.UI
{
    public class ItemsButton(Asset<Texture2D> texture, string hoverText) : BaseButton(texture, hoverText)
    {
        private ItemsPanel itemsPanel;
        private bool isPanelVisible = false;

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            ButtonsSystem sys = ModContent.GetInstance<ButtonsSystem>();
            if (!sys?.myState?.AreButtonsVisible ?? false)
                return;
        }

        public override void HandleClick()
        {
            ToggleItemsPanel();
            Main.LocalPlayer.ToggleInv(); // open and close inventory when clicking Item Button
        }

        public void ToggleItemsPanel()
        {
            // Toggle the panel's visibility flag.
            isPanelVisible = !isPanelVisible;

            // Ensure the button is part of a UIState. (ButtonState in our case right now)
            if (Parent is not UIState state)
            {
                Log.Warn("ItemsButton has no parent UIState!");
                return;
            }

            if (isPanelVisible)
            {
                // Create the panel if it doesn't already exist.
                if (itemsPanel == null)
                {
                    itemsPanel = new ItemsPanel();
                    Log.Info("Created new ItemsPanel.");
                }

                Log.Info("Appending ItemsPanel to parent state.");
                if (!state.Children.Contains(itemsPanel))
                    state.Append(itemsPanel);

                // Force recalculation of the layout.
                itemsPanel.Recalculate();
                state.Recalculate();
            }
            else
            {
                Log.Info("Removing ItemsPanel from parent state.");
                if (state.Children.Contains(itemsPanel))
                    state.RemoveChild(itemsPanel);
                state.Recalculate();
            }
        }
    }
}
