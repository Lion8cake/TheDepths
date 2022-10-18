using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
    public class TheDepthsGlobalProjectile : GlobalProjectile
    {
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<TheDepthsPlayer>().noHit = true;
        }
    }
}
