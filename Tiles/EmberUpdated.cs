using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class EmberUpdated : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;

			ItemDrop = ModContent.ItemType<Items.Placeable.Ember>();
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 18 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			DustType = ModContent.DustType<EmberDust>();

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ember");
			AddMapEntry(new Color(68, 17, 17), name);
		}

		public override bool Drop(int i, int j)
		{
			Tile t = Main.tile[i, j];
			int style = t.TileFrameX / 18;
			if (style == 0)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Placeable.Ember>());
			}
			return base.Drop(i, j);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			if (Main.tile[i + 1, j].TileType == Type && Main.tile[i - 1, j].TileType != Type)
			{
				Main.tile[i, j].TileFrameX = (18 * 0);
			}
			else if (Main.tile[i - 1, j].TileType == Type && Main.tile[i + 1, j].TileType != Type)
			{
				Main.tile[i, j].TileFrameX = (18 * 2);
			}
			else if (Main.tile[i - 1, j].TileType == Type && Main.tile[i + 1, j].TileType == Type)
			{
				Main.tile[i, j].TileFrameX = (18 * 1);
			}
			else if (Main.tile[i - 1, j].TileType != Type && Main.tile[i + 1, j].TileType != Type)
            {
				Main.tile[i, j].TileFrameX = (18 * 3);
			}
		}
	}
}