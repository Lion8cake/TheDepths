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
	public class ChasmeHead : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("TEST NPC HEAD");
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

		public override void SetDefaults() {
			NPC.width = 30;
			NPC.height = 30;
			NPC.lifeMax = 100;
			NPC.aiStyle = -1;
			NPC.lavaImmune = true;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}
	}
}