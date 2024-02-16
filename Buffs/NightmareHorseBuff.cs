using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Buffs
{
    public class NightmareHorseBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.NightmareHorse>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}
