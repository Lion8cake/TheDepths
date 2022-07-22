using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using TheDepths.Dusts;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Tiles
{
    public class ArqueriteBricks : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Placeable.ArqueriteBricks>();
            AddMapEntry(new Color(71, 84, 105));
			DustType = ModContent.DustType<ArqueriteDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
		
		public override void NearbyEffects(int i, int j, bool closer)
		{
			Player player = Main.LocalPlayer;
			if ((int)Vector2.Distance(player.Center / 16f, new Vector2((float)i, (float)j)) <= 1)
			{
				player.AddBuff(ModContent.BuffType<MercuryPoisoning>(), Main.rand.Next(10, 20));
			}
		}
    }
}