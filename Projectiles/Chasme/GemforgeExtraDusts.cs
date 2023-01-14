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
	public class GemforgeExtraDusts : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.timeLeft = 300; //5 seconds (60 x 5)
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Vector2 vector311 = Projectile.Center + new Vector2(0f, -20f);
			float num1589 = 0.99f;
			if (Projectile.timeLeft >= 160f)
			{
				num1589 = 0.79f % 2;
			}
			if (Projectile.timeLeft >= 180f)
			{
				num1589 = 0.58f % 2;
			}
			if (Projectile.timeLeft >= 220f)
			{
				num1589 = 0.43f % 2;
			}
			if (Projectile.timeLeft >= 260f)
			{
				num1589 = 0.33f % 2;
			}
			if (Projectile.timeLeft >= 300f)
			{
				num1589 = 1f % 2;
			}
			for (int num1590 = 0; num1590 < 9; num1590++)
			{
				if (!(Main.rand.NextFloat() < num1589))
				{
					float num1591 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
					float num1592 = Main.rand.NextFloat();
					Vector2 vector312 = vector311 + num1591.ToRotationVector2() * (110f + 600f * num1592);
					Vector2 vector313 = (num1591 - (float)Math.PI).ToRotationVector2() * (14f + 0f * Main.rand.NextFloat() + 8f * num1592);
					Dust dust26 = Dust.NewDustPerfect(vector312, 264, vector313);
					dust26.scale = 0.9f;
					dust26.fadeIn = 1.15f + num1592 * 0.3f;
					dust26.color = new Color(1f, 1f, 1f, num1589) * (1f - num1589);
					dust26.noGravity = true;
					dust26.noLight = true;
				}
			}
		}
	}
}