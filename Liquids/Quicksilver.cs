using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;
using TheDepths.Tiles;

namespace TheDepths.Liquids
{
	public class Quicksilver : ModLiquid
	{
		public override void SetStaticDefaults()
		{
			LiquidFallLength = 3;
			DefaultOpacity = 0.95f;
			SlopeOpacity = 1f;
			VisualViscosity = 200;
			FallDelay = 5;
			VanillaFallbackOnModDeletion = (ushort)LiquidID.Lava;
			ChecksForDrowning = false;
			PlayersEmitBreathBubbles = false;
			AddMapEntry(new Color(85, 96, 102));
		}

		public override void OnPlayerSplash(Player player, bool isEnter)
		{
			if (isEnter)
			{
				for (int num96 = 0; num96 < 20; num96++)
				{
					int num97 = Dust.NewDust(new Vector2(player.position.X - 6f, player.position.Y + (float)(player.height / 2) - 8f), player.width + 12, 24, ModContent.DustType<QuicksilverBubble>());
					Main.dust[num97].velocity.Y -= 1.5f;
					Main.dust[num97].velocity.X *= 2.5f;
					Main.dust[num97].scale = 1.3f;
					Main.dust[num97].alpha = 100;
					Main.dust[num97].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, player.position);
			}
			else
			{
				for (int num104 = 0; num104 < 20; num104++)
				{
					int num105 = Dust.NewDust(new Vector2(player.position.X - 6f, player.position.Y + (float)(player.height / 2) - 8f), player.width + 12, 24, ModContent.DustType<QuicksilverBubble>());
					Main.dust[num105].velocity.Y -= 1.5f;
					Main.dust[num105].velocity.X *= 2.5f;
					Main.dust[num105].scale = 1.3f;
					Main.dust[num105].alpha = 100;
					Main.dust[num105].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, player.position);
			}
		}

		public override void OnNPCSplash(NPC npc, bool isEnter)
		{
			if (isEnter)
			{
				for (int m = 0; m < 10; m++)
				{
					int num4 = Dust.NewDust(new Vector2(npc.position.X - 6f, npc.position.Y + (float)(npc.height / 2) - 8f), npc.width + 12, 24, ModContent.DustType<QuicksilverBubble>());
					Main.dust[num4].velocity.Y -= 1.5f;
					Main.dust[num4].velocity.X *= 2.5f;
					Main.dust[num4].scale = 1.3f;
					Main.dust[num4].alpha = 100;
					Main.dust[num4].noGravity = true;
				}
				if (npc.aiStyle != 1 && npc.type != 1 && npc.type != 16 && npc.type != 147 && npc.type != 59 && npc.type != 300 && npc.aiStyle != 39 && !npc.noGravity)
				{
					SoundEngine.PlaySound(SoundID.SplashWeak, npc.position);
				}
			}
			else
			{
				for (int num10 = 0; num10 < 10; num10++)
				{
					int num11 = Dust.NewDust(new Vector2(npc.position.X - 6f, npc.position.Y + (float)(npc.height / 2) - 8f), npc.width + 12, 24, ModContent.DustType<QuicksilverBubble>());
					Main.dust[num11].velocity.Y -= 1.5f;
					Main.dust[num11].velocity.X *= 2.5f;
					Main.dust[num11].scale = 1.3f;
					Main.dust[num11].alpha = 100;
					Main.dust[num11].noGravity = true;
				}
				if (npc.aiStyle != 1 && npc.type != 1 && npc.type != 16 && npc.type != 59 && npc.type != 300 && npc.aiStyle != 39 && !npc.noGravity)
				{
					SoundEngine.PlaySound(SoundID.SplashWeak,  npc.position);
				}
			}
		}

		public override void OnProjectileSplash(Projectile proj, bool isEnter)
		{
			if (isEnter)
			{
				for (int num7 = 0; num7 < 10; num7++)
				{
					int num8 = Dust.NewDust(new Vector2(proj.position.X - 6f, proj.position.Y + (float)(proj.height / 2) - 8f), proj.width + 12, 24, ModContent.DustType<QuicksilverBubble>());
					Main.dust[num8].velocity.Y -= 1.5f;
					Main.dust[num8].velocity.X *= 2.5f;
					Main.dust[num8].scale = 1.3f;
					Main.dust[num8].alpha = 100;
					Main.dust[num8].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, proj.position);
			}
			else
			{
				for (int num15 = 0; num15 < 10; num15++)
				{
					int num16 = Dust.NewDust(new Vector2(proj.position.X - 6f, proj.position.Y + (float)(proj.height / 2) - 8f), proj.width + 12, 24, ModContent.DustType<QuicksilverBubble>());
					Main.dust[num16].velocity.Y -= 1.5f;
					Main.dust[num16].velocity.X *= 2.5f;
					Main.dust[num16].scale = 1.3f;
					Main.dust[num16].alpha = 100;
					Main.dust[num16].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, proj.position);
			}
		}

		public override void OnItemSplash(Item item, bool isEnter)
		{
			if (isEnter)
			{
				for (int n = 0; n < 5; n++)
				{
					int num8 = Dust.NewDust(new Vector2(item.position.X - 6f, item.position.Y + (float)(item.height / 2) - 8f), item.width + 12, 24, 35);
					Main.dust[num8].velocity.Y -= 1.5f;
					Main.dust[num8].velocity.X *= 2.5f;
					Main.dust[num8].scale = 1.3f;
					Main.dust[num8].alpha = 100;
					Main.dust[num8].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, item.position);
			}
			else
			{
				for (int num15 = 0; num15 < 5; num15++)
				{
					int num16 = Dust.NewDust(new Vector2(item.position.X - 6f, item.position.Y + (float)(item.height / 2) - 8f), item.width + 12, 24, 35);
					Main.dust[num16].velocity.Y -= 1.5f;
					Main.dust[num16].velocity.X *= 2.5f;
					Main.dust[num16].scale = 1.3f;
					Main.dust[num16].alpha = 100;
					Main.dust[num16].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, item.position);
			}
		}

		public override int ChooseWaterfallStyle(int i, int j)
		{
			return ModContent.GetInstance<QuicksilverSilverfall>().Slot;
		}

		public override int LiquidMerge(int i, int j, int otherLiquid)
		{
			if (otherLiquid == LiquidID.Water)
			{
				return ModContent.TileType<Quartz>();
			}
			else if (otherLiquid == LiquidID.Honey)
			{
				return ModContent.TileType<GlitterBlock>();
			}
			else if (otherLiquid == LiquidID.Shimmer)
			{
				return TileID.ShimmerBlock;
			}
			return TileID.Stone;
		}

		public override void LiquidMergeSound(int i, int j, int otherLiquid, ref SoundStyle? collisionSound)
		{
			collisionSound = SoundID.LiquidsHoneyWater;
			if (otherLiquid == LiquidID.Lava)
			{
				collisionSound = SoundID.LiquidsHoneyLava;
			}
			else if (otherLiquid == LiquidID.Shimmer)
			{
				collisionSound = SoundID.ShimmerWeak1;
			}
		}

		public override void EmitEffects(int i, int j, LiquidCache liquidCache)
		{
			//can probs be removed tbh
			if (liquidCache.HasVisibleLiquid)
			{
				if (Main.rand.NextBool(700))
				{
					Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, ModContent.DustType<QuicksilverBubble>(), 0f, 0f, 0, Color.White);
				}
				if (Main.rand.NextBool(350))
				{
					int num27 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 8, ModContent.DustType<QuicksilverBubble>(), 0f, 0f, 50, Color.White, 1.5f);
					Dust obj = Main.dust[num27];
					obj.velocity *= 0.8f;
					Main.dust[num27].velocity.X *= 2f;
					Main.dust[num27].velocity.Y -= (float)Main.rand.Next(1, 7) * 0.1f;
					if (Main.rand.NextBool(10))
					{
						Main.dust[num27].velocity.Y *= Main.rand.Next(2, 5);
					}
					Main.dust[num27].noGravity = true;
				}
			}
		}

		public override void RetroDrawEffects(int j, int i, SpriteBatch spriteBatch, ref RetroLiquidDrawInfo drawData, float liquidAmountModified, int liquidGFXQuality)
		{
			drawData.liquidAlphaMultiplier *= 1.8f;
			if (drawData.liquidAlphaMultiplier > 1f)
			{
				drawData.liquidAlphaMultiplier = 1f;
			}
			if (Main.instance.IsActive && !Main.gamePaused)
			{
				if (Main.tile[j, i].LiquidAmount > 200 && Main.rand.NextBool(700))
				{
					Dust.NewDust(new Vector2((float)(j * 16), (float)(i * 16)), 16, 16, ModContent.DustType<QuicksilverBubble>());
				}
				if (drawData.liquidFraming.Y == 0 && Main.rand.NextBool(350))
				{
					int num22 = Dust.NewDust(new Vector2((float)(j * 16), (float)(i * 16) + liquidAmountModified * 2f - 8f), 16, 8, ModContent.DustType<QuicksilverBubble>(), 0f, 0f, 50, default(Color), 1.5f);
					Dust obj2 = Main.dust[num22];
					obj2.velocity *= 0.8f;
					Main.dust[num22].velocity.X *= 2f;
					Main.dust[num22].velocity.Y -= (float)Main.rand.Next(1, 7) * 0.1f;
					if (Main.rand.NextBool(10))
					{
						Main.dust[num22].velocity.Y *= Main.rand.Next(2, 5);
					}
					Main.dust[num22].noGravity = true;
				}
			}
		}
	}
}
