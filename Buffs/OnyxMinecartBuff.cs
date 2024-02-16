using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Buffs
{
    public class OnyxMinecartBuff : MinecartBuffBase
    {
		public OnyxMinecartBuff(bool left) : base(left)
		{

		}

        public override int MountType => ModContent.MountType<Mounts.OnyxMinecart>();
    }
}
