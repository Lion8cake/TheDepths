using TheDepths.Buffs;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using System.IO;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using TheDepths.Items;

namespace TheDepths
{
    public class TheDepthsPlayer : ModPlayer
    {
        public BitsByte largeGems;
        public BitsByte ownedLargeGems;
        public BitsByte hasLargeGems;
        public bool merPoison;
        public bool slowWater;
        public bool merBoiling;
        public bool merImbue;
        public bool aStone;
        public bool lodeStone;
        public bool noHit;

        public bool geodeCrystal;
        public bool livingShadow;
        public bool ShadePet;
		
        public override void ResetEffects()
        {
            largeGems = 0;
            merPoison = false;
            slowWater = false;
            merBoiling = false;
            merImbue = false;
            aStone = false;
            lodeStone = false;

            geodeCrystal = false;
            livingShadow = false;
            ShadePet = false;
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (merImbue && Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<MercuryFire>(), Player.velocity.X * 0.2f + (float)Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default(Color), 0.75f);
            }
            if (aStone && Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<SlowingWaterFire>(), Player.velocity.X * 0.2f + (float)Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default(Color), 0.75f);
            }
        }

        public override void PostUpdate()
        {
            if (lodeStone) Player.defaultItemGrabRange = 107;
        }
    }
}