using AltLibrary;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Common.Hooks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Tiles;
using Terraria.GameContent;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using TheDepths;
using Terraria.GameContent.Generation;

namespace TheDepths
{
    internal class AltDepthsBiome : AltBiome
    {
        public override Color NameColor => new(27, 29, 33);

        public override Asset<Texture2D>[] AltUnderworldBackgrounds => TheDepths.texture;
		public override Color AltUnderworldColor => new(27, 29, 33);

		public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Hell;
            BiomeStone = ModContent.TileType<ShaleBlock>();
            AltarTile = ModContent.TileType<Gemforge>();
            BiomeOre = ModContent.TileType<ArqueriteOre>();

            DisplayName.SetDefault("Depths");
            Description.SetDefault("A deeper below cave with almost freezing in tempratures where liquid mercury fills the reagon");
            GenPassName.SetDefault("Creating depths...");
        }

        public override string WorldIcon => "";

        public override string LowerTexture => "TheDepths/Assets/Loading/Depths_Outer_Lower";

        public override string IconSmall => "TheDepths/Biomes/DepthsBestiaryIcon";

        public override WorldGenLegacyMethod GetHellforgeGenerationPass()
        {
            return new(TheDepthsWorldGen.Gemforge);
        }
    }
}
