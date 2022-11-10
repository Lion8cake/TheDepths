using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Items.Banners;
using AltLibrary.Common.Systems;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;
using TheDepths.Items.Placeable;
using TheDepths.Items.Armor;
using TheDepths.Items.Weapons;

namespace TheDepths.NPCs.Chasme
{
	public class ChasmeHands1 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TEST NPC HAND");
			NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public static int BodyType()
		{
			return ModContent.NPCType<ChasmeBody>();
		}

		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 30;
			NPC.lifeMax = 100;
			NPC.aiStyle = -1;
			NPC.lavaImmune = true;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}

		public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}

			Player player = Main.player[NPC.target];

			if (player.dead)
			{

				NPC.velocity.Y -= 0.04f;

				NPC.EncourageDespawn(10);
				return;
			}

            NPC.localAI[1]++;
			if (NPC.localAI[1] > 60)
			{
				NPC.localAI[1] = 0;
			}

			if (NPC.localAI[1] == 0)
			{
				Vector2 fromPlayer = NPC.Center - player.Center;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Important multiplayer concideration: drastic change in behavior (that is also decided by randomness) like this requires
					// to be executed on the server (or singleplayer) to keep the boss in sync

					float angle = fromPlayer.ToRotation();
					float twelfth = MathHelper.Pi / 6;

					angle += MathHelper.Pi + Main.rand.NextFloat(-twelfth, twelfth);
					if (angle > MathHelper.TwoPi)
					{
						angle -= MathHelper.TwoPi;
					}
					else if (angle < 0)
					{
						angle += MathHelper.TwoPi;
					}

					Vector2 relativeDestination = angle.ToRotationVector2() + player.Center;

					NPC.netUpdate = true;
				}
			}
		}
	}
}