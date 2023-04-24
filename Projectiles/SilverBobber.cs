using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
	public class SilverBobber : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 61;
			Projectile.bobber = true;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;

			DrawOriginOffsetY = -8;
		}

		public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
		{
			lineOriginOffset = new Vector2(47, -31);
		}
	}
}