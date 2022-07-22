using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.Utilities;

namespace TheDepths.Buffs
{
	public class SilverSphereBuff : ModBuff
	{
	
	    public int timer;
	
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Silver Sphere");
			Description.SetDefault("Four Spheres are around you");
			Main.debuff[Type] = false;
	    	Main.pvpBuff[Type] = true;
	    	Main.buffNoSave[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) {
		timer++;
		if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("SilverSpheres").Type] < 1 && timer > 20)
		{
			for (int i = 0; i < 4; i++)
			{
                //Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("SilverSpheres").Type, 50, 8f, player.whoAmI, i);
			}
			timer = 0;
		}
		}
	}
}
