using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheDepths.Tiles.Trees
{
    public class NightTree : ModTree
    {
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { ModContent.TileType<NightmareGrass>() };
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheDepths/Tiles/Trees/NightTree");
        }

        public override int SaplingGrowthType(ref int style) {
			style = 0;
			return ModContent.TileType<Trees.NightSapling>();
		}

        public override Asset<Texture2D> GetBranchTextures()
        {
            return ModContent.Request<Texture2D>("TheDepths/Tiles/Trees/NightTree_Branches");
        }

        public override Asset<Texture2D> GetTopTextures()
        {
            return ModContent.Request<Texture2D>("TheDepths/Tiles/Trees/NightTree_Tops");
        }

        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeable.NightWood>();
        }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
        }

        public override bool Shake(int x, int y, ref bool createLeaves)
        {
			if (Main.getGoodWorld && WorldGen.genRand.NextBool(17))
			{
				Projectile.NewProjectile(new EntitySource_ShakeTree(x, y), x * 16, y * 16, (float)Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f);
				return false;
			}
			else if (WorldGen.genRand.NextBool(7))
			{
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Acorn, WorldGen.genRand.Next(1, 3));
				return false;
			}
			else if (WorldGen.genRand.NextBool(35)&& Main.halloween)
			{
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, WorldGen.genRand.Next(1, 3));
				return false;
			}
			else if (WorldGen.genRand.NextBool(20))
			{
				int type = ItemID.CopperCoin;
				int num2 = WorldGen.genRand.Next(50, 100);
				if (WorldGen.genRand.NextBool(30))
				{
					type = ItemID.GoldCoin;
					num2 = 1;
					if (WorldGen.genRand.NextBool(5))
					{
						num2++;
					}
					if (WorldGen.genRand.NextBool(10))
					{
						num2++;
					}
				}
				else if (WorldGen.genRand.NextBool(10))
				{
					type = ItemID.SilverCoin;
					num2 = WorldGen.genRand.Next(1, 21);
					if (WorldGen.genRand.NextBool(3))
					{
						num2 += WorldGen.genRand.Next(1, 21);
					}
					if (WorldGen.genRand.NextBool(4))
					{
						num2 += WorldGen.genRand.Next(1, 21);
					}
				}
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, type, num2);
				return false;
			}
			else if (WorldGen.genRand.NextBool(20)&& y > Main.maxTilesY - 250)
			{
				int num3 = WorldGen.genRand.Next(3);
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, num3 switch
				{
					0 => ModContent.NPCType<NPCs.AlbinoRat>(),
					1 => ModContent.NPCType<NPCs.QuartzCrawler>(),
					_ => ModContent.NPCType<NPCs.EnchantedNightmareWorm>(),
				});
				return false;
			}
			else if (Main.remixWorld && WorldGen.genRand.NextBool(20)&& y > Main.maxTilesY - 250)
			{
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Rope, WorldGen.genRand.Next(20, 41));
				return false;
			}
			else if (WorldGen.genRand.NextBool(12))
			{
				Item.NewItem(Type: (!WorldGen.genRand.NextBool(2)) ? ModContent.ItemType<Items.BlackOlive>() : ModContent.ItemType<Items.Ciamito>(), source: new EntitySource_ShakeTree(x, y), X: x * 16, Y: y * 16, Width: 16, Height: 16);
				return false;
			}
			return false;
		}
    }
}