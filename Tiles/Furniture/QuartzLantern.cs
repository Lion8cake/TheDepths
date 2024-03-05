using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles.Furniture
{
    public class QuartzLantern : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(Type);
			Main.tileLighted[Type] = true;
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AddMapEntry(new Color(255, 255, 255), CreateMapEntryName());
            DustType = ModContent.DustType<QuartzCrystals>();
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.QuartzLantern>());
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18 % 2;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
			Main.tile[i, topY].TileFrameX += frameAdjustment;
			Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
			Wiring.SkipWire(i, topY);
			Wiring.SkipWire(i, topY + 1);
			NetMessage.SendTileSquare(-1, i, topY + 1, 2, TileChangeType.None);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX % 18 == 0 && Main.tile[i, j].TileFrameY % 36 == 0 && flag)
			{
				Main.instance.TilesRenderer.AddSpecialPoint(i, j, 5);
			}

			return false;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0)
            {
                r = 0.70f;
                g = 0.43f;
                b = 0.21f;
            }
        }
    }
}