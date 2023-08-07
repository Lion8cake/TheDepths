using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.NPCs
{
    public class TheDepthsGlobalNPC : GlobalNPC
	{
	    public override bool InstancePerEntity => true;

	    public bool merPoison;
		public bool slowWater;
		public bool merBoiling;
		public int MercuryNPCTimer;
		
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
				Lighting.AddLight(npc.position, 0.1f, 0.2f, 0.7f);
			}
		}
		
		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (slowWater && !npc.boss)
			{
				npc.velocity.X = 0f;
				npc.velocity.Y = 0f;
				if (npc.velocity.Y > 0f)
				{
					npc.velocity.Y = 0f;
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

		/*public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.Clothier && Main.moonPhase == 0 && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.PurplePlumbersShirt>());
				shop.item[nextSlot].shopCustomPrice = 250000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.PurplePlumbersPants>());
				shop.item[nextSlot].shopCustomPrice = 250000;
				nextSlot++;
			}
			if (type == NPCID.Dryad && Main.hardMode)
            {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.ShadowShrubPlanterBox>());
				shop.item[nextSlot].shopCustomPrice = 100;
				nextSlot++;
			}
		}*/
	}
}