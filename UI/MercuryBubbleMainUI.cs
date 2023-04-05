using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;

namespace TheDepths.UI
{
	internal class MercuryBubbleMainUI : UIState
	{
		// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
		// Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
		private UIElement area;
		private UIImage cloud1;
		private UIElement area2;
		private UIImage cloud2;
		private UIImage cloud3;
		private UIImage cloud4;
		private UIImage cloud5;
		private UIImage cloud6;
		private UIImage cloud7;
		private UIImage cloud8;
        readonly UIElement cloudTexture = new UIImage(ModContent.Request<Texture2D>("TheDepths/Lava/PoisonCloud"));

		public override void OnInitialize() {
			// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
			// UIElement is invisible and has no padding.
			area = new UIElement();
			area.Left.Set(Main.screenWidth / 2 - 10, 0f);
			area.Top.Set(Main.screenHeight / 2 - 62, 0f);
			area.Width.Set(0, 0f);
			area.Height.Set(0, 0f);

			cloud1 = (UIImage)cloudTexture;
			cloud1.Left.Set(26 * 4, 0f);
			cloud1.Top.Set(0, 0f);
			cloud1.Width.Set(24, 0f);
			cloud1.Height.Set(18, 0f);

			cloud2 = (UIImage)cloudTexture;
			cloud2.Left.Set(26 * 3, 0f);
			cloud2.Top.Set(0, 0f);
			cloud2.Width.Set(24, 0f);
			cloud2.Height.Set(18, 0f);

			area.Append(cloud2);
			area.Append(cloud1);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			if (Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().aAmulet == false)
				return;
			base.Draw(spriteBatch);
		}

		// Here we draw our UI
		/*protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			var modPlayer = Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>();
			// Calculate quotient
			float quotient = modPlayer.AmuletTimer; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox = cloud1.GetInnerDimensions().ToRectangle();
			hitbox.X += 12;
			hitbox.Width -= 24;
			hitbox.Y += 8;
			hitbox.Height -= 16;

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			for (int i = 0; i < steps; i += 1) {
				// float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
		}*/

		public override void Update(GameTime gameTime) {
			if (Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().aAmulet == false)
				return;
			area.Left.Set(Main.screenWidth / 2 - 10, 0f);
			area.Top.Set(Main.screenHeight / 2 - 62, 0f);
		}
	}

	class MercuryBubbleUISystem : ModSystem
	{
		private UserInterface ExampleResourceBarUserInterface;

		internal MercuryBubbleMainUI ExampleResourceBar;

		public override void Load() {
			// All code below runs only if we're not loading on a server
			if (!Main.dedServ) {
				ExampleResourceBar = new();
				ExampleResourceBarUserInterface = new();
				ExampleResourceBarUserInterface.SetState(ExampleResourceBar);
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			ExampleResourceBarUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1) {
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"ExampleMod: Example Resource Bar",
					delegate {
						ExampleResourceBarUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
