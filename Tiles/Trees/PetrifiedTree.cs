using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheDepths.Tiles.Trees
{
    public class PetrifiedTree : ModTree
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
            GrowsOnTileId = new int[1] { ModContent.TileType<ShaleBlock>() };
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheDepths/Tiles/Trees/PetrifiedTree");
        }

        public override int SaplingGrowthType(ref int style) {
			style = 0;
			return ModContent.TileType<Trees.PetrifiedSapling>();
		}

        public override Asset<Texture2D> GetBranchTextures()
        {
            return ModContent.Request<Texture2D>("TheDepths/Tiles/Trees/PetrifiedTree_Branches");
        }

        public override Asset<Texture2D> GetTopTextures()
        {
            return ModContent.Request<Texture2D>("TheDepths/Tiles/Trees/PetrifiedTree_Tops");
        }

        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeable.PetrifiedWood>();
        }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
        }

        public override int TreeLeaf()
        {
            return ModContent.GoreType<PetrifiedTreeLeaf>();
        }
    }
}