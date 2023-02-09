using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Biomes
{
    public class FreezingWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle()
        {
            return ModContent.Find<ModWaterfallStyle>("TheDepths/FreezingWaterfallStyle").Slot;
        }

        public override int GetSplashDust()
        {
            return ModContent.DustType<FreezingWaterSplash>();
        }

        public override int GetDropletGore()
        {
            return ModContent.Find<ModGore>("TheDepths/FreezingDroplet").Type;
        }

        public override Color BiomeHairColor()
        {
            return new(5, 19, 57);
        }

        public override byte GetRainVariant()
        {
            return (byte)Main.rand.Next(3);
        }

        public override Asset<Texture2D> GetRainTexture()
        {
            return ModContent.Request<Texture2D>("TheDepths/Biomes/FreezingRain");
        }
    }
}