using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Buffs;

namespace TheDepths.Projectiles.Nohitweapon
{
    public class MiracfulSapphire : SapphireShovelProj
    {
        public override void PostAI()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                int dustMax = 60 / 500 * 20;
                if (dustMax < 10)
                    dustMax = 10;
                if (dustMax > 40)
                    dustMax = 40;
                for (int j = 0; j < dustMax; j++)
                {
                    Vector2 spawnPos = Projectile.Center + Main.rand.NextVector2CircularEdge(60, 60);
                    if (Main.npc[i].Distance(spawnPos) > 1500)
                        continue;
                    Dust dust = Main.dust[Dust.NewDust(spawnPos, 0, 0, 88, 0, 0, 100, Color.White, 1f)];
                    dust.velocity = Projectile.velocity;
                    if (Main.rand.Next(3) == 0)
                    {
                        dust.velocity += Vector2.Normalize(Projectile.Center - dust.position) * Main.rand.NextFloat(5f) * 1f;
                        dust.position += dust.velocity * 5f;
                    }
                    dust.noGravity = true;
                }
                if (Main.npc[i].Distance(Projectile.Center) < 60f)
                {
                    Main.npc[i].AddBuff(ModContent.BuffType<FreezingWater>(), 150, false);
                }
            }
        }
    }
}
