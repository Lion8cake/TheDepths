using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
    public class SapphireSerpentKite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IsAKite[Type] = true;
            ItemID.Sets.HasAProjectileThatHasAUsabilityCheck[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultTokite(ModContent.ProjectileType<Projectiles.SapphireSerpentKite>());
        }

		public override bool CanUseItem(Player player)
		{
            return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
