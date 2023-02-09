using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using System;
using Terraria.GameContent.Events;
using Terraria.Audio;
using Terraria.DataStructures;

namespace TheDepths.Projectiles.Chasme
{
	public class GemforgeScreenflash : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.timeLeft = 81; //2.5 seconds (60 x 2.5)
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			if (Projectile.timeLeft >= 1f)
			{
				MoonlordDeathDrama.RequestLight(((Projectile.timeLeft + 920) - 480f) / 120f, Projectile.Center);
			}
		}
	}
}