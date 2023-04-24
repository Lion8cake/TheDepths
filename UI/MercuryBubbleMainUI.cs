using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;
using ReLogic.Content;

namespace TheDepths.UI
{
	internal class MercuryBubbleMainUI : UIState
	{
		private UIElement area;
        readonly Asset<Texture2D> cloudtexture = ModContent.Request<Texture2D>("TheDepths/Lava/PoisonCloud");

		public override void OnInitialize() {
			area = new UIElement();
			area.Left.Set(Main.screenWidth / 2 - 10, 0f);
			area.Top.Set(Main.screenHeight / 2 - 62, 0f);
			area.Width.Set(0, 0f);
			area.Height.Set(0, 0f);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			if (Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().aAmulet == false || Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().AmuletTimer == 60 * 4)
				return;
			base.Draw(spriteBatch);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			int MercuryTimer = Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().AmuletTimer;
			Vector2 vector = Main.LocalPlayer.Top + new Vector2(0f, Main.LocalPlayer.gfxOffY);
			if (Main.playerInventory && Main.screenHeight < 1000)
			{
				vector.Y += Main.LocalPlayer.height + 40;
			}
			vector = Vector2.Transform(vector - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix);
			if (!Main.playerInventory || Main.screenHeight >= 1000)
			{
				vector.Y -= 50f;
			}
			vector /= Main.UIScale;
			if (Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
			{
				vector = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 236);
				if (Main.InGameUI.IsVisible)
				{
					vector.Y = Main.screenHeight - 32;
				}
			}
			if (MercuryTimer < 60 * 4 && !Main.LocalPlayer.ghost)
			{
				_ = MercuryTimer / 28;
				int num = 28;
				for (int i = 1; i < MercuryTimer / num + 1; i++)
				{
                    float num3 = 1f;
                    int num2 = 255;
                    if (MercuryTimer >= i * num)
                    {
                        float num4 = (float)(MercuryTimer - i * num) / (float)num;
                        num2 = (int)(30f + 225f * num4);
                        if (num2 < 30)
                        {
                            num2 = 30;
                        }
                        num3 = num4 / 4f + 0.75f;
                        if ((double)num3 < 0.75)
                        {
                            num3 = 0.75f;
                        }
						if (num3 >= 1f)
                        {
							num3 = 1f;
                        }
                    }
                    int num5 = 0;
					int num6 = 0;
					if (i > 10)
					{
						num5 -= 260;
						num6 += 26;
					}
					spriteBatch.Draw(cloudtexture.Value, vector + new Vector2((float)(26 * (i) + num5) - 125f, 32f + ((float)cloudtexture.Height() - (float)cloudtexture.Height() * num3) / 2f + (float)num6), new Microsoft.Xna.Framework.Rectangle(0, 0, cloudtexture.Width(), cloudtexture.Height()), new Microsoft.Xna.Framework.Color(num2, num2, num2, num2), 0f, default(Vector2), num3, SpriteEffects.None, 0f);
				}
			}
		}

        public override void Update(GameTime gameTime) {
			var modPlayer = Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>();
			if (Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().aAmulet == false || modPlayer.AmuletTimer == 60 * 4)
				return;
		}
	}

	class MercuryBubbleUISystem : ModSystem
	{
		private UserInterface MercuryUserInterface;

		internal MercuryBubbleMainUI MercuryBubble;

		public override void Load() {
			if (!Main.dedServ) {
				MercuryBubble = new();
				MercuryUserInterface = new();
				MercuryUserInterface.SetState(MercuryBubble);
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			MercuryUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1) {
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"TheDepths: Mercury Cloud UI",
					delegate {
						MercuryUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
