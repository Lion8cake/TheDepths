using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using TheDepths.Worldgen;

namespace TheDepths.NPCs
{
    public class TheDepthsGlobalNPC : GlobalNPC
	{
	    public override bool InstancePerEntity => true;

	    public bool merPoison;
		public bool slowWater;
		public bool merBoiling;
		public int MercuryNPCTimer;

		public int QuicksilverTimer;

		public override void SetDefaults(NPC npc)
		{
			if (npc.lavaImmune == true)
			{
				NPCID.Sets.SpecificDebuffImmunity[npc.type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
			}
		}

		public override void PostAI(NPC npc)
		{
			bool NPCInDepths = ((TheDepthsWorldGen.isWorldDepths && !TheDepthsWorldGen.DrunkDepthsLeft && !TheDepthsWorldGen.DrunkDepthsRight) || ((TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(npc.position.ToTileCoordinates().X) < Main.maxTilesX / 2) || (TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(npc.position.ToTileCoordinates().X) > Main.maxTilesX / 2)));
			if (Main.player[npc.target] != null)
			{
				if (NPCInDepths && Collision.LavaCollision(npc.position, npc.width, npc.height))
				{
					npc.lavaImmune = true;
					npc.buffImmune[BuffID.OnFire] = true;
					npc.buffImmune[BuffID.OnFire3] = true;
					QuicksilverTimer++;
					if (QuicksilverTimer >= 120)
					{
						QuicksilverTimer = 120;
						npc.AddBuff(ModContent.BuffType<Buffs.MercuryBoiling>(), 60 * 7, false);
					}
				}
				if (NPCInDepths && !Collision.LavaCollision(npc.position, npc.width, npc.height))
				{
					QuicksilverTimer--;
					if (QuicksilverTimer <= 0)
					{
						QuicksilverTimer = 0;
					}
				}
				if (!NPCInDepths && Collision.LavaCollision(npc.position, npc.width, npc.height) && npc.buffImmune[ModContent.BuffType<Buffs.MercuryBoiling>()] == false)
				{
					npc.lavaImmune = false;
				}
			}
			for (int o = 0; o < Main.maxNPCs; o++)
			{
				NPC target = Main.npc[o];
				if (!target.friendly)
				{
					if (target.position.WithinRange(npc.position, 40) && npc.HasBuff(ModContent.BuffType<Buffs.MercuryContagion>()))
					{
						if (!target.HasBuff(ModContent.BuffType<Buffs.MercuryContagion>()) && target != npc)
						{
							target.AddBuff(ModContent.BuffType<Buffs.MercuryContagion>(), npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<Buffs.MercuryContagion>())]); //works but worried about MP
						}
					}
				}
			}
		}

		public override void ResetEffects(NPC npc) {
			merPoison = false;
			slowWater = false;
			merBoiling = false;
		}
		
	    public override void DrawEffects(NPC npc, ref Color drawColor) {
			if (merPoison) {
				if (Main.rand.Next(4) < 3) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<MercuryFire>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4)) {
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
			}
			if (slowWater) {
				if (Main.rand.Next(4) < 3) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<SlowingWaterFire>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4)) {
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
			}
			if (merBoiling) {
				if (Main.rand.Next(4) < 3) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<MercuryFire>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4)) {
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
			}
		}
		
		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (slowWater && !npc.boss && npc.type != NPCID.DungeonGuardian)
			{
				npc.velocity.X = 0f;
				if (npc.noGravity)
				{
					npc.velocity.Y = 0f;
				}
				else
				{
					npc.velocity.Y += npc.gravity;
				}
			}
			if (merBoiling)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				MercuryNPCTimer++;
				npc.lifeRegen -= Utils.Clamp(MercuryNPCTimer / 60, 0, 10);
			}
			if (!merBoiling && MercuryNPCTimer >= 1)
			{
				MercuryNPCTimer--;
			}
		}

        public override void ModifyShop(NPCShop shop)
		{
			var depthsWorld = new Condition("Mods.TheDepths.InDepths", () => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer));
			var underWorld = new Condition("Mods.TheDepths.OutDepths", () => !Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer));
			if (shop.NpcType == NPCID.Clothier)
			{
				shop.InsertAfter(ItemID.PlumbersShirt, ModContent.ItemType<Items.Armor.PurplePlumbersShirt>(), Condition.MoonPhaseFull, depthsWorld);
				shop.InsertAfter(ItemID.PlumbersPants, ModContent.ItemType<Items.Armor.PurplePlumbersPants>(), Condition.MoonPhaseFull, depthsWorld);
				if (shop.TryGetEntry(ItemID.PlumbersShirt, out NPCShop.Entry entry))
				{
					entry.AddCondition(underWorld);
				}
				if (shop.TryGetEntry(ItemID.PlumbersPants, out NPCShop.Entry entry2))
				{
					entry2.AddCondition(underWorld);
				}
			}
			if (shop.NpcType == NPCID.Dryad)
            {
				shop.InsertAfter(ItemID.FireBlossomPlanterBox, ModContent.ItemType<Items.Placeable.ShadowShrubPlanterBox>(), Condition.Hardmode, depthsWorld);

				shop.InsertAfter(ItemID.AshGrassSeeds, ModContent.ItemType<Items.Placeable.NightmareSeeds>(), depthsWorld, Condition.InUnderworldHeight);
				if (shop.TryGetEntry(ItemID.AshGrassSeeds, out NPCShop.Entry entry3))
				{
					entry3.AddCondition(underWorld);
				}
			}
			if (shop.NpcType == NPCID.BestiaryGirl)
            {
				shop.InsertAfter(ItemID.WorldGlobe, new Item(ModContent.ItemType<Items.CoreGlobe>())
				{
					shopCustomPrice = Item.buyPrice(0, 3, 0, 0)
				}, Condition.Hardmode);
			}
		}
	}
}