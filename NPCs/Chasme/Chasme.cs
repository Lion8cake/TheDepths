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
	public class Chasme : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("TEST NPC");
			NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public NPC owner;

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

        public override void AI()
        {
			if (owner == null || !owner.active)
			{
				NPC.life = int.MinValue;
				NPC.checkDead();
			}
			else if (owner.ModNPC is ChasmeBody m)
				NPC.position = m.NPC.Center + new Vector2(owner.spriteDirection == -1 ? 10f : -20f, -55f);
		}
		public override bool CheckActive() => false;
	}
}