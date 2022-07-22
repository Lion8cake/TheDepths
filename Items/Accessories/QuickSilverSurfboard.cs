using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Accessories
{
    [AutoloadEquip(new EquipType[] { EquipType.Wings })] 
	public class QuickSilverSurfboard : ModItem
	{
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 20;
			Item.value = 50000;
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.wingTimeMax = 50;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration) {
			speed = 20f;
			acceleration *= 5f;
		}
		
		public override bool WingUpdate(Player player, bool inUse)
    	{
	    	int WingTicks = ((!inUse) ? 8 : 6);
	    	if (player.velocity.Y != 0f)
    		{
    			player.wingFrameCounter++;
    			if (player.wingFrameCounter > WingTicks)
    			{
	    			player.wingFrame++;
	    			player.wingFrameCounter = 0;
	     			if (player.wingFrame >= 3)
	    			{
	    				player.wingFrame = 0;
	     			}
	    		}
	    	}
	    	else
	     	{
	    		player.wingFrame = 4;
	    	}
	    	return true;
	    }
	}
}