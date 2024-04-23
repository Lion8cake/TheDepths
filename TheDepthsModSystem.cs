using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Graphics.Effects;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Tiles;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Liquid;
using System.Reflection;
using System.IO;
using System.Text.Json;
using Terraria.ModLoader.Config;
using Terraria.Localization;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.IO;
using Newtonsoft.Json;
using Terraria.ModLoader.IO;
using TheDepths.Worldgen;
using System.Linq;
using TheDepths.Items;
using System.Threading;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent;
using static Terraria.GameContent.Drawing.TileDrawing;

namespace TheDepths
{
    public class TheDepthsModSystem : ModSystem
    {
		//public static bool NotLavaDestroyable;

		/*public override void PreUpdateWorld()
        {
            //if (Worldgen.TheDepthsWorldGen.InDepths)
            //{
                for (int i = 0; i < TileLoader.TileCount; i++)
                {
                    Main.tileLavaDeath[i] = false;
                }
            //}
            //else if (NotLavaDestroyable == false)
            //{
            //    for (int i = 0; i < TileLoader.TileCount; i++)
            //    {
            //        Main.tileLavaDeath[i] = true;
            //    }
            //}
        }*/

		public override void PostSetupContent()
		{
			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				if (TheDepthsIDs.Sets.AxesAbleToBreakStone[i] == true && ContentSamples.ItemsByType[i].pick > 0)
				{
					TheDepthsIDs.Sets.AxesAbleToBreakStone[i] = false;
				}
			}
		}

		public static int? MossConversion(int thisType, int otherType)
		{
            if (Main.tileMoss[thisType] || TileID.Sets.tileMossBrick[thisType])
            {
                if (otherType == 38)
                {
                    return ModContent.TileType<MercuryMossStoneBricks>();
                }
                if (otherType == 1)
                {
                    return ModContent.TileType<MercuryMoss>();
                }
            }
            return null;
		}

		public override void OnWorldUnload()
        {
            Gemforge.RubyRelicIsOnForge = 1;
        }

		public static bool AnyProjectiles(int Type)
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.type == Type)
				{
					return true;
				}
			}
			return false;
		}

		private static void TileGlowmaskList(TileDrawInfo drawData, int type, out Texture2D altGlowTexture, out Texture2D glowTexture, out Rectangle glowSourceRect, out Color glowColor)
		{
			glowTexture = null;
			altGlowTexture = null;
			glowSourceRect = Rectangle.Empty;
			glowColor = Color.Transparent;
			if (type == ModContent.TileType<Tiles.Trees.NightwoodTree>())
			{
				glowTexture = (Texture2D)ModContent.Request<Texture2D>("TheDepths/Tiles/Trees/NightwoodTree_Glow");
				glowSourceRect = new Rectangle((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
				glowColor = Color.White;
			}
			else if (type == ModContent.TileType<Tiles.NightmareGrass>())
			{
				altGlowTexture = (Texture2D)ModContent.Request<Texture2D>("TheDepths/Tiles/NightmareGrass_Glow");
			}
			else if (type == ModContent.TileType<Tiles.MercuryMoss>())
			{
				altGlowTexture = (Texture2D)ModContent.Request<Texture2D>("TheDepths/Tiles/MercuryMoss_Glow");
			}
			else if (type == ModContent.TileType<Tiles.MercuryMossBricks>())
			{
				altGlowTexture = (Texture2D)ModContent.Request<Texture2D>("TheDepths/Tiles/MercuryMossBricks_Glow");
			}
			else if (type == ModContent.TileType<Tiles.MercuryMossStoneBricks>())
			{
				altGlowTexture = (Texture2D)ModContent.Request<Texture2D>("TheDepths/Tiles/MercuryMossStoneBricks_Glow");
			}
		}

		public static void DrawGlowmask(int tileX, int tileY)
		{
			ThreadLocal<TileDrawInfo> _currentTileDrawInfo = (ThreadLocal<TileDrawInfo>)typeof(TileDrawing).GetField("_currentTileDrawInfo", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			TileDrawInfo drawData = _currentTileDrawInfo.Value;
			Vector2 screenPosition = Main.Camera.UnscaledPosition;
			Vector2 screenOffset = new((float)Main.offScreenRange, (float)Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}
			drawData.tileCache = Main.tile[tileX, tileY];
			drawData.typeCache = drawData.tileCache.TileType;
			drawData.tileFrameX = drawData.tileCache.TileFrameX;
			drawData.tileFrameY = drawData.tileCache.TileFrameY;
			drawData.tileLight = Lighting.GetColor(tileX, tileY);
			TileGlowmaskList(drawData, drawData.tileCache.TileType, out drawData.drawTexture, out drawData.glowTexture, out drawData.glowSourceRect, out drawData.glowColor);
			bool flag = true;
			flag &= IsVisible(drawData.tileCache);

			Rectangle rectangle = new(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight - drawData.halfBrickHeight);
			Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + drawData.halfBrickHeight)) + screenOffset;
			if (!flag)
			{
				return;
			}
			drawData.colorTint = Color.White;
			if (drawData.drawTexture != null)
			{
				Color color2 = Color.White;
				if (drawData.tileCache.Slope == 0 && !drawData.tileCache.IsHalfBlock)
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), color2, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				else if (drawData.tileCache.IsHalfBlock)
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)rectangle, color2, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				else if (TileID.Sets.Platforms[drawData.tileCache.TileType])
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)rectangle, color2, 0f, Vector2.Zero, 1f, drawData.tileSpriteEffect, 0f);
					if (drawData.tileCache.Slope == (SlopeType)1 && Main.tile[tileX + 1, tileY + 1].HasTile && Main.tileSolid[Main.tile[tileX + 1, tileY + 1].TileType] && Main.tile[tileX + 1, tileY + 1].Slope != (SlopeType)2 && !Main.tile[tileX + 1, tileY + 1].IsHalfBlock && (!Main.tile[tileX, tileY + 1].HasTile || (Main.tile[tileX, tileY + 1].BlockType != 0 && Main.tile[tileX, tileY + 1].BlockType != (BlockType)5) || (!TileID.Sets.BlocksStairs[Main.tile[tileX, tileY + 1].TileType] && !TileID.Sets.BlocksStairsAbove[Main.tile[tileX, tileY + 1].TileType])))
					{
						Rectangle value = new(198, (int)drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[Main.tile[tileX + 1, tileY + 1].TileType] && Main.tile[tileX + 1, tileY + 1].Slope == 0)
						{
							value.X = 324;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2(0f, 16f), (Rectangle?)value, color2, 0f, Vector2.Zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					else if (drawData.tileCache.Slope == (SlopeType)2 && Main.tile[tileX - 1, tileY + 1].HasTile && Main.tileSolid[Main.tile[tileX - 1, tileY + 1].TileType] && Main.tile[tileX - 1, tileY + 1].Slope != (SlopeType)1 && !Main.tile[tileX - 1, tileY + 1].IsHalfBlock && (!Main.tile[tileX, tileY + 1].HasTile || (Main.tile[tileX, tileY + 1].BlockType != 0 && Main.tile[tileX, tileY + 1].BlockType != (BlockType)4) || (!TileID.Sets.BlocksStairs[Main.tile[tileX, tileY + 1].TileType] && !TileID.Sets.BlocksStairsAbove[Main.tile[tileX, tileY + 1].TileType])))
					{
						Rectangle value2 = new(162, (int)drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[Main.tile[tileX - 1, tileY + 1].TileType] && Main.tile[tileX - 1, tileY + 1].Slope == 0)
						{
							value2.X = 306;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2(0f, 16f), (Rectangle?)value2, color2, 0f, Vector2.Zero, 1f, drawData.tileSpriteEffect, 0f);
					}
				}
				else if (TileID.Sets.HasSlopeFrames[drawData.tileCache.TileType])
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, 16, 16), color2, 0f, Vector2.Zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				else
				{
					int num4 = (int)drawData.tileCache.Slope;
					int num5 = 2;
					for (int i = 0; i < 8; i++)
					{
						int num6 = i * -2;
						int num7 = 16 - i * 2;
						int num8 = 16 - num7;
						int num9;
						switch (num4)
						{
							case 1:
								num6 = 0;
								num9 = i * 2;
								num7 = 14 - i * 2;
								num8 = 0;
								break;
							case 2:
								num6 = 0;
								num9 = 16 - i * 2 - 2;
								num7 = 14 - i * 2;
								num8 = 0;
								break;
							case 3:
								num9 = i * 2;
								break;
							default:
								num9 = 16 - i * 2 - 2;
								break;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2((float)num9, (float)(i * num5 + num6)), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX + num9, drawData.tileFrameY + drawData.addFrY + num8, num5, num7), color2, 0f, Vector2.Zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					int num10 = ((num4 <= 2) ? 14 : 0);
					Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2(0f, (float)num10), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + num10, 16, 2), color2, 0f, Vector2.Zero, 1f, drawData.tileSpriteEffect, 0f);
				}
			}
			if (drawData.glowTexture != null)
			{
				Vector2 position = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset;
				if (TileID.Sets.Platforms[drawData.typeCache])
				{
					position = vector;
				}
				Main.spriteBatch.Draw(drawData.glowTexture, position, (Rectangle?)drawData.glowSourceRect, drawData.glowColor, 0f, Vector2.Zero, 1f, drawData.tileSpriteEffect, 0f);
			}
		}
	}
}