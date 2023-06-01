using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Buffs
{
    public class PhantomFirecartBuff : MinecartBuffBase
    {
		public PhantomFirecartBuff(bool left) : base(left)
		{

		}

        public override int MountType => ModContent.MountType<Mounts.PhantomFirecart>();
    }
}
