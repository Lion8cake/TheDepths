using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using On.Terraria.Graphics.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Chasme
{
    public class Chlaser : ModProjectile
    {
        Color beamColor = new(195, 136, 251);
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chlaser");
            base.SetStaticDefaults();
        }
        private float BeamLength
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(BeamLength);
        public override void ReceiveExtraAI(BinaryReader reader) => BeamLength = reader.ReadSingle();

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.aiStyle = 84;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.Opacity = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // If the target is touching the beam's hitbox (which is a small rectangle vaguely overlapping the host Prism), that's good enough.
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            // Otherwise, perform an AABB line collision check to check the whole beam.
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + Projectile.velocity * BeamLength;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, 22 * Projectile.scale, ref _);
        }
        public override void AI()
        {
            NPC MainNPC = Main.npc[(int)Projectile.ai[0]];
            float hitscanBeamLength = PerformBeamHitscan(MainNPC.Center, true, MainNPC);
            BeamLength = MathHelper.Lerp(BeamLength, hitscanBeamLength, 0.75f);




            ProduceBeamDust(beamColor);

            Vector2 beamDims = new Vector2(Projectile.velocity.Length() * BeamLength, Projectile.width * Projectile.scale);

            DelegateMethods.v3_1 = beamColor.ToVector3() * 0.75f;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * BeamLength, beamDims.Y, new Utils.TileActionAttempt(DelegateMethods.CastLight));
            base.AI();
        }
        private float PerformBeamHitscan(Vector2 basePos, bool fullCharge, NPC mainNPC)
        {
            // By default, the hitscan interpolation starts at the Projectile's center.
            // If the host Prism is fully charged, the interpolation starts at the Prism's center instead.
            Vector2 samplingPoint = Projectile.Center;
            samplingPoint = basePos;

            // Overriding that, if the player shoves the Prism into or through a wall, the interpolation starts at the player's center.
            // This last part prevents the player from projecting beams through walls under any circumstances.
            Player player = Main.player[Projectile.owner];
            if (!Collision.CanHitLine(mainNPC.Center, 0, 0, basePos, 0, 0))
            {
                samplingPoint = mainNPC.Center;
            }

            // Perform a laser scan to calculate the correct length of the beam.
            // Alternatively, if you want the beam to ignore tiles, just set it to be the max beam length with the following line.
            // return MaxBeamLength;
            float[] laserScanResults = new float[3];
            Collision.LaserScan(samplingPoint, Projectile.velocity, 1 * Projectile.scale, 2400, laserScanResults);
            float averageLengthSample = 0f;
            for (int i = 0; i < laserScanResults.Length; ++i)
            {
                averageLengthSample += laserScanResults[i];
                
            }
            averageLengthSample /= 3;

            return averageLengthSample;
        }
        private void ProduceBeamDust(Color beamColor)
        {
            // Create one dust per frame a small distance from where the beam ends.
            const int type = 15;
            Vector2 endPosition = Projectile.Center + Projectile.velocity * (BeamLength - 14.5f * Projectile.scale);

            // Main.rand.NextBool is used to give a 50/50 chance for the angle to point to the left or right.
            // This gives the dust a 50/50 chance to fly off on either side of the beam.
            float angle = Projectile.rotation + (Main.rand.NextBool() ? 1f : -1f) * MathHelper.PiOver2;
            float startDistance = Main.rand.NextFloat(1f, 1.8f);
            float scale = Main.rand.NextFloat(0.7f, 1.1f);
            Vector2 velocity = angle.ToRotationVector2() * startDistance;
            Dust dust = Dust.NewDustDirect(endPosition, 0, 0, type, velocity.X, velocity.Y, 0, beamColor, scale);
            dust.color = beamColor;
            dust.noGravity = true;

            // If the beam is currently large, make the dust faster and larger to match.
            if (Projectile.scale > 1f)
            {
                dust.velocity *= Projectile.scale;
                dust.scale *= Projectile.scale;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // If the beam doesn't have a defined direction, don't draw anything.
            if (Projectile.velocity == Vector2.Zero)
            {
                //return false;
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centerFloored = Projectile.Center.Floor() + Projectile.velocity * Projectile.scale * 10.5f;
            Vector2 drawScale = new Vector2(Projectile.scale);

            // Reduce the beam length proportional to its square area to reduce block penetration.
            float visualBeamLength = BeamLength - 14.5f * Projectile.scale * Projectile.scale;

            DelegateMethods.f_1 = 1f; // f_1 is an unnamed decompiled variable whose function is unknown. Leave it at 1.
            Vector2 startPosition = centerFloored - Main.screenPosition;
            Vector2 endPosition = startPosition + Projectile.velocity * visualBeamLength;

            // Draw the outer beam.
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, beamColor * 0.75f * Projectile.Opacity);

            // Draw the inner beam, which is half size.
            drawScale *= 0.5f;
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, Color.White * 0.1f * Projectile.Opacity);

            // Returning false prevents Terraria from trying to draw the Projectile itself.
            return false;
        }
        private static void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
        {
            Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);

            // c_1 is an unnamed decompiled variable which is the render color of the beam drawn by DelegateMethods.RainbowLaserDraw.
            DelegateMethods.c_1 = beamColor;
            Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
        }

        // Automatically iterates through every tile the laser is overlapping to cut grass at all those locations.
        public override void CutTiles()
        {
            // tilecut_0 is an unnamed decompiled variable which tells CutTiles how the tiles are being cut (in this case, via a Projectile).
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.TileActionAttempt cut = new Utils.TileActionAttempt(DelegateMethods.CutTiles);
            Vector2 beamStartPos = Projectile.Center;
            Vector2 beamEndPos = beamStartPos + Projectile.velocity * BeamLength;

            // PlotTileLine is a function which performs the specified action to all tiles along a drawn line, with a specified width.
            // In this case, it is cutting all tiles which can be destroyed by Projectiles, for example grass or pots.
            Utils.PlotTileLine(beamStartPos, beamEndPos, Projectile.width * Projectile.scale, cut);
        }
    }
    //TODO find out why this goddam laser code wont work
}