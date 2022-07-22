using AltLibrary;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Common.Hooks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Tiles;

namespace TheConfectionRebirth
{
    internal class AltDepthsBiome : AltBiome
    {
        public override Color NameColor => new(210, 196, 145);

        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Hell;
            //BiomeStone = ModContent.TileType<ShaleBlock>();
            //AltarTile = ModContent.TileType<Gemforge>();
            DisplayName.SetDefault("Depths");
            Description.SetDefault("A hot gem filled cave deep below the regular caverns that has liquid mercury and quartz");
        }

        public override string WorldIcon => "";

        public override string LowerTexture => "TheDepths/Assets/Loading/Depths_Outer_Lower";

        public override string IconSmall => "TheDepths/Biomes/BestiaryIcon1";
    }
}
