using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace TheDepths.Projectiles.Chasme
{
	public class ChasmeShockwave : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 1f;
            Projectile.timeLeft = 60;
        }
        float magnitude = 1;
        public override void AI()
        {
            int direction = (int)Projectile.ai[0];
            Point tileCoords = Projectile.Center.ToTileCoordinates();
            Vector2 nextVelocity = new Vector2(0, 0);

            if (Main.tile[tileCoords + new Point(0, -1)].HasTile)
            {
            }
            else if (Main.tile[tileCoords + new Point(0, 1)].HasTile)
            {
                nextVelocity = new Vector2(3, 0);
            }
            else if (Main.tile[tileCoords + new Point(1, 0)].HasTile || Main.tile[tileCoords + new Point(-1, 0)].HasTile)
            {
                nextVelocity = new Vector2(0, -3);
            }
            else
            {
                nextVelocity = Projectile.oldVelocity;
               
            }

            Projectile.velocity = nextVelocity * direction * magnitude;
            Projectile.rotation = Projectile.velocity.ToRotation();
            magnitude *= 1.05f;

            if (Main.tileSolid[((int)Main.tile[tileCoords].BlockType)])
            {
                //Projectile.Kill();
            }
        }
        Color[] colors = { Color.Red, Color.Green };
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Texture2D texture = TextureAssets.Extra[189].Value;
            Rectangle sourceRect = new(0, 0, texture.Width, texture.Height);
            Vector2 projOrigin = sourceRect.Size() / 2f;

            int numIs0 = 0;
            int iterationAmount = -1;
            float maxScale = 0.7f;
            float scaleDiv = 20f;
            float rotationMulti = 0f;

            int trailLength = 19;
            float shineScale = 1f;
            Color color = colors[(int)Projectile.ai[1]];

            

            for (int i = trailLength; (iterationAmount > 0 && i < numIs0) || (iterationAmount < 0 && i > numIs0); i += iterationAmount)
            {
                if (i >= Projectile.oldPos.Length)
                {
                    continue;
                }

                Color trailColor = Projectile.GetFairyQueenWeaponsColor(0.5f);
                trailColor *= Utils.GetLerpValue(0f, 20f, Projectile.timeLeft, clamped: true);

                float colorMulti = numIs0 - i;
                if (iterationAmount < 0)
                {
                    colorMulti = trailLength - i;
                }

                trailColor *= colorMulti / ((float)ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f);
                Vector2 trailOldPos = Projectile.oldPos[i];

                float trailRotation = Projectile.oldRot[i];
                SpriteEffects trailSpriteEffects = ((Projectile.oldSpriteDirection[i] == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

                if (trailOldPos == Vector2.Zero)
                {
                    continue;
                }

                Vector2 trailPos = trailOldPos + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Main.EntitySpriteDraw(texture, trailPos, sourceRect, color, MathHelper.PiOver2, projOrigin, MathHelper.Lerp(Projectile.scale, maxScale, (float)i / scaleDiv), trailSpriteEffects, 0);

            }

            Color fairyQueenWeaponColor = Projectile.GetFairyQueenWeaponsColor(0f);

            Vector2 shinyPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, shinyPos, sourceRect, fairyQueenWeaponColor, Projectile.rotation, projOrigin, Projectile.scale * 0.9f, spriteEffects, 0);
            Texture2D textureExtra98 = TextureAssets.Extra[98].Value;
            Vector2 shinyOrigin = textureExtra98.Size() / 2f;
            Color colorTopBot = fairyQueenWeaponColor * 0.5f;
            Color ColorLeftRight = fairyQueenWeaponColor;
            float scaleWarping = Utils.GetLerpValue(15f, 30f, Projectile.timeLeft, clamped: true) * Utils.GetLerpValue(60, 60 - 40f, Projectile.timeLeft, clamped: true) * (1f + 0.2f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 30f / 0.5f * ((float)Math.PI * 2f) * 3f)) * 0.8f;
            Vector2 scale1 = new Vector2(0.5f, 5f) * scaleWarping * shineScale;
            Vector2 scale2 = new Vector2(0.5f, 2f) * scaleWarping * shineScale;
            colorTopBot *= scaleWarping;
            ColorLeftRight *= scaleWarping;

            Color projColor = Projectile.GetAlpha(lightColor);
            float projScale = Projectile.scale;
            float projRotation = Projectile.rotation;

            //Terraria.Graphics.FlameLashDrawer;
            projColor.A /= 2;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRect, projColor, projRotation, projOrigin, projScale, spriteEffects, 0);

            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return base.OnTileCollide(oldVelocity);
        }
    }
}

