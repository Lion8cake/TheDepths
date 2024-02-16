using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
    public class MercuryMossBricks : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = ModContent.DustType<Dusts.MercuryMoss>();
			AddMapEntry(new Color(119, 135, 162));
        }

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TheDepthsModSystem.DrawGlowmask(i, j);
		}
	}
}