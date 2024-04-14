using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TheDepths.Projectiles
{
	public class CoreGlobe : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item107, Projectile.position);
			for (int num405 = 0; num405 < 15; num405++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 13, 0f, -2f, 0, default(Color), 1.5f);
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (Worldgen.TheDepthsWorldGen.isWorldDepths)
				{
					Worldgen.TheDepthsWorldGen.isWorldDepths = false;
				}
				else
				{
					Worldgen.TheDepthsWorldGen.isWorldDepths = true;
				}
				NetMessage.SendData(MessageID.WorldData);
			}
		}
	}
}
