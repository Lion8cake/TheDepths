using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;
using ReLogic.Content;
using Terraria.Graphics.Effects;

namespace TheDepths
{
    public class TheDepthsMenuTheme : ModMenu
	{
		public override string DisplayName => "Depths";
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Depths");

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TheDepths/Assets/Title");

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor) //Taken from SLR 
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(default, default, SamplerState.PointClamp, default, default, default, Main.UIScaleMatrix);

			for (int k = 4; k >= 0; k--)
			{
				Texture2D tex = ModContent.Request<Texture2D>("TheDepths/Backgrounds/Menu/DepthsMenuUnderworldBG_" + k).Value;

				float heightRatio = Main.screenHeight / (float)Main.screenWidth;
				int width = (int)(tex.Width * heightRatio);
				var pos = new Vector2((int)(Main.screenPosition.X * 0.05f * -(k - 5)) % width, 0);

				Color color = Color.White;

				byte a = color.A;

				color.A = a;

				for (int h = 0; h < Main.screenWidth + width; h += width)//during loading the texture has a width of one
					Main.spriteBatch.Draw(tex, new Rectangle(h - (int)pos.X, (int)pos.Y, width, Main.screenHeight), null, color, 0, default, 0, 0);
			}
			return true;
		}
	}
}