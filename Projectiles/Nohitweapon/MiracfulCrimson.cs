using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Nohitweapon
{
    public abstract class MiracfulCrimsonNote : ModProjectile
    {
        private int type;

        public override string Texture => $"Terraria/Images/Projectile_" + type;

        public MiracfulCrimsonNote(int type)
        {
            this.type = type;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(type);
        }

        public override bool PreAI()
        {
            Lighting.AddLight(Projectile.Center, Color.Crimson.ToVector3());
            return base.PreAI();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Crimson;
        }
    }

    public class MiracfulCrimson1 : MiracfulCrimsonNote
    {
        public MiracfulCrimson1() : base(ProjectileID.QuarterNote)
        {
        }
    }
    public class MiracfulCrimson2 : MiracfulCrimsonNote
    {
        public MiracfulCrimson2() : base(ProjectileID.EighthNote)
        {
        }
    }
    public class MiracfulCrimson3 : MiracfulCrimsonNote
    {
        public MiracfulCrimson3() : base(ProjectileID.TiedEighthNote)
        {
        }
    }
}
