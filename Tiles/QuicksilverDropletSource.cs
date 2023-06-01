using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class QuicksilverDropletSource : ModTile
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBall";

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(85, 96, 102), name);
			DustType = 0;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 0;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			EmitLiquidDrops(i, j);

			return base.PreDraw(i, j, spriteBatch);
		}

		public void EmitLiquidDrops(int i, int j) //This code was made by Diverman Sam and possibly others on the thorium dev team
		{
			Tile tile = Main.tile[i, j];
			int x = i - tile.TileFrameX / 18;
			int y = j - tile.TileFrameY / 18;
			int spawnX = x * 16;
			int spawnY = y * 16;

			//Below is vanilla code for spawning droplets
			int chanceDenominator = 60;
			if (tile.LiquidAmount != 0 || !Main.rand.NextBool(chanceDenominator))
			{
				return;
			}

			Rectangle thisGoreAtParticularFrame = new Rectangle(x * 16, y * 16, 16, 16);
			thisGoreAtParticularFrame.X -= 34;
			thisGoreAtParticularFrame.Width += 68;
			thisGoreAtParticularFrame.Y -= 100;
			thisGoreAtParticularFrame.Height = 400;
			int goreType = Mod.Find<ModGore>("QuicksilverDroplet").Type;
			for (int k = 0; k < Main.maxGore; k++)
			{
				Gore otherGore = Main.gore[k];
				if (otherGore.active && otherGore.type == goreType)
				{
					Rectangle otherGoreRect = new Rectangle((int)otherGore.position.X, (int)otherGore.position.Y, 16, 16);
					if (thisGoreAtParticularFrame.Intersects(otherGoreRect))
					{
						//Check if no other droplets exist in the same tile
						return;
					}
				}
			}

			var source = WorldGen.GetItemSource_FromTileBreak(x, y);
			Gore.NewGoreDirect(source, new Vector2(spawnX, spawnY), Vector2.Zero, goreType, 1f).velocity *= 0f;
		}

		public override bool CanDrop(int i, int j)
        {
			return false;
        }
    }
}