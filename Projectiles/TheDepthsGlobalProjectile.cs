using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
    public class TheDepthsGlobalProjectile : GlobalProjectile
    {
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            target.GetModPlayer<TheDepthsPlayer>().noHit = true;
        }
    }
}
