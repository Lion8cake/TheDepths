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
	public class ChasmeBody : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("TEST NPC BODY");
			NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}
		
		public override void SetDefaults() {
			NPC.width = 30;
			NPC.height = 30;
			NPC.damage = 5;
			NPC.defense = 0;
			NPC.lifeMax = 100;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 250000f;
			NPC.aiStyle = -1;
			NPC.lavaImmune = true;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.boss = true;
		}

        public override void OnKill()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int CentreX = (int)(NPC.position.X + (NPC.width / 2)) / 16;
				int CentreY = (int)(NPC.position.Y + (NPC.height / 2)) / 16;
				int HalfLength = 3 + 1;
				for (int k = CentreX - HalfLength; k <= CentreX + HalfLength; k++)
				{
					for (int l = CentreY - HalfLength; l <= CentreY + HalfLength; l++)
					{
						if ((k == CentreX - HalfLength || k == CentreX + HalfLength || l == CentreY - HalfLength || l == CentreY + HalfLength) && !Main.tile[k, l].HasTile)
						{
							Tile tile = Main.tile[k, l];
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.ShadowBrick>();
							tile.HasTile = true;
						}
						Main.tile[k, l].LiquidAmount = 0;
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else
						{
							WorldGen.SquareTileFrame(k, l, true);
						}
					}
				}
			}
			if (Main.rand.NextBool(10))
			{
                Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<ShaleBlock>()); //Chasme Trophy
			}
			/*if (Main.expertMode)
			{
				NPC.DropBossBags();
			}*/
			else
			{
				if (Main.rand.NextBool(7))
				{
					Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<PurplePlumbersHat>()); //Chasme Mask 
				}
				Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<POWHammer>());
			}

			if (!Main.hardMode)
			{
				WorldGen.StartHardmode();
			}
		}
	}
}