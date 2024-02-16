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
using Terraria.Utilities;
using Terraria.WorldBuilding;
using TheDepths.Projectiles.SilverSpherePotion;

namespace TheDepths.Buffs
{
	public class SilverSphereBuff : ModBuff
	{
	    public int timer;
	
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
	    	Main.pvpBuff[Type] = true;
	    	Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) 
		{
			timer++;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SilverSpheres>()] < 1 && timer > 20)
			{
				for (int i = 0; i < 4; i++)
				{
					Projectile.NewProjectile(new EntitySource_Misc(""), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<SilverSpheres>(), 50, 8f, player.whoAmI, i);
				}
				timer = 0;
			}
			for (int j = 0; j < Main.maxProjectiles; j++)
			{
				Projectile projectile = Main.projectile[j];
				if (projectile.active && projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<SilverSpheres>())
				{
					projectile.timeLeft = 2;
				}
			}
		}
	}
}
