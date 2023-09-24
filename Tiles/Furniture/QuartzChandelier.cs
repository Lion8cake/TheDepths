using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Tiles.Furniture
{
    public class QuartzChandelier : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AdjTiles = new int[] { TileID.Torches };
            AddMapEntry(new Color(255, 255, 255), CreateMapEntryName());
            TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = ModContent.DustType<Dusts.QuartzCrystals>();
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            bool intoRenderTargets = true;
            bool flag = intoRenderTargets || Main.LightingEveryFrame;

            if (Main.tile[i, j].TileFrameX % 54 == 0 && Main.tile[i, j].TileFrameY % 54 == 0 && flag)
            {
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, 5);
            }

            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX < 88)
            {
                r = 0.13f;
                g = 0.59f;
                b = 0.43f;
            }
        }
    }
}