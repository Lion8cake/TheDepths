using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Nohitweapon
{
    public class MiracfulWhip : ModProjectile
    {
        private int soundTime;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.alpha = byte.MaxValue;
            Projectile.penetrate = -1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            ++soundTime;
            if (soundTime == 15)
                SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
            Player player = Main.player[Projectile.owner];
            float num1 = 1.570796f;
            player.RotatedRelativePoint(player.MountedCenter, true);
            if (Projectile.localAI[1] > 0.0)
                --Projectile.localAI[1];
            Projectile.alpha -= 42;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
            if (Projectile.localAI[0] == 0.0)
                Projectile.localAI[0] = Utils.ToRotation(Projectile.velocity);
            float num2 = Utils.ToRotationVector2(Projectile.localAI[0]).X >= 0.0 ? 1f : -1f;
            if (Projectile.ai[1] <= 0.0)
                num2 *= -1f;
            Vector2 rotationVector2 = Utils.ToRotationVector2(num2 * (float)(Projectile.ai[0] / 30.0 * 6.28318548202515 - 1.57079637050629));
            float local1 = rotationVector2.Y;
            local1 = local1* (float) Math.Sin(Projectile.ai[1]);
            if (Projectile.ai[1] <= 0.0)
            {
                float local2 = rotationVector2.Y;
                local2 = local2* -1f;
            }
            Vector2 vector2_1 = Utils.RotatedBy(rotationVector2, Projectile.localAI[0], Vector2.Zero);
            ++Projectile.ai[0];
            if (Projectile.ai[0] < 30.0)
            {
                Projectile.velocity = Vector2.Add(Projectile.velocity, Vector2.Multiply(new Vector2(48f), vector2_1));
            }
            else
                Projectile.Kill();
            Projectile.position = Vector2.Subtract(player.RotatedRelativePoint(player.MountedCenter, true), Vector2.Divide(Projectile.Size, 2f));
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + num1;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);
            Vector2 vector2_2 = Vector2.Multiply(Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56], 2f);
            if (player.direction != 1)
            {
                vector2_2.X = player.bodyFrame.Width - vector2_2.X;
            }
            if ((double)player.gravDir != 1.0)
            {
                vector2_2.Y = player.bodyFrame.Height - vector2_2.Y;
            }
            Vector2 vector2_3 = Vector2.Subtract(vector2_2, Vector2.Divide(new Vector2((player.bodyFrame).Width - player.width, player.bodyFrame.Height - 42), 2f));
            Projectile.Center = Vector2.Subtract(player.RotatedRelativePoint(Vector2.Add(player.position, vector2_3), true), Projectile.velocity);
            if (Projectile.alpha != 0)
                return;
            for (int index = 0; index < 2; ++index)
            {
                Dust dust = Main.dust[Dust.NewDust(Vector2.Add(Projectile.position, Vector2.Multiply(Projectile.velocity, 2f)), Projectile.width, Projectile.height, DustID.PurpleCrystalShard, 0.0f, 0.0f, 100, default, 2f)];
                dust.noGravity = true;
                dust.velocity = Vector2.Multiply(dust.velocity, 2f);
                dust.velocity = Vector2.Add(dust.velocity, Utils.ToRotationVector2(Projectile.localAI[0]));
                dust.fadeIn = 1.5f;
            }
            float num3 = 18f;
            for (int index = 0; index < num3; ++index)
            {
                if (Main.rand.Next(4) == 0)
                {
                    Vector2 vector2_4 = Vector2.Add(Vector2.Add(Projectile.position, Projectile.velocity), Vector2.Multiply(Projectile.velocity, index / num3));
                    Dust dust = Main.dust[Dust.NewDust(vector2_4, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, 0.0f, 0.0f, 100, default, 1f)];
                    dust.noGravity = true;
                    dust.fadeIn = 0.5f;
                    dust.velocity = Vector2.Add(dust.velocity, Utils.ToRotationVector2(Projectile.localAI[0]));
                    dust.noLight = true;
                }
            }
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 velocity = Projectile.velocity;
            //_ = Utils.PlotTileLine(projectile.Center, Vector2.Add(projectile.Center, Vector2.Multiply(velocity, projectile.localAI[1])), projectile.width * projectile.scale, new Utils.PerLinePoint((object)null, __methodptr(CutTiles)));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Color color = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
            if (Projectile.hide && !ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type])
                color = Lighting.GetColor((int)mountedCenter.X / 16, (int)(mountedCenter.Y / 16.0));
            Vector2 position = Projectile.position;
            Vector2.Subtract(Vector2.Add(Vector2.Divide(new Vector2(Projectile.width, Projectile.height), 2f), Vector2.Multiply(Vector2.UnitY, Projectile.gfxOffY)), Main.screenPosition);
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            Color alpha = Projectile.GetAlpha(color);
            if (Equals(Projectile.velocity, Vector2.Zero))
                return false;
            float num1 = Projectile.velocity.Length() + 16f;
            bool flag = num1 < 100.0;
            Vector2 vector2_1 = Vector2.Normalize(Projectile.velocity);
            Rectangle rectangle = new Rectangle(0, 0, texture2D.Width, 42);
            Vector2 vector2_2 = new Vector2(0.0f, Main.player[Projectile.owner].gfxOffY);
            float num2 = Projectile.rotation + 3.141593f;
            Main.spriteBatch.Draw(texture2D, Vector2.Add(Vector2.Subtract(Utils.Floor(Projectile.Center), Main.screenPosition), vector2_2), new Rectangle?(rectangle), alpha, num2, Vector2.Subtract(Vector2.Divide(Utils.Size(rectangle), 2f), Vector2.Multiply(Vector2.UnitY, 4f)), Projectile.scale, 0, 0.0f);
            float num3 = num1 - 40f * Projectile.scale;
            Vector2 vector2_3 = Vector2.Add(Utils.Floor(Projectile.Center), Vector2.Multiply(Vector2.Multiply(vector2_1, Projectile.scale), 24f));
            rectangle = new Rectangle(0, 68, texture2D.Width, 18);
            if (num3 > 0.0)
            {
                float num4 = 0.0f;
                while (num4 + 1.0 < num3)
                {
                    if ((double)num3 - num4 < (float)rectangle.Height)
                        rectangle.Height = (int)((double)num3 - num4);
                    Main.spriteBatch.Draw(texture2D, Vector2.Add(Vector2.Subtract(vector2_3, Main.screenPosition), vector2_2), new Rectangle?(rectangle), alpha, num2, new Vector2(rectangle.Width / 2, 0.0f), Projectile.scale, 0, 0.0f);
                    num4 += rectangle.Height * Projectile.scale;
                    vector2_3 = Vector2.Add(vector2_3, Vector2.Multiply(Vector2.Multiply(vector2_1, rectangle.Height), Projectile.scale));
                }
            }
            Vector2 vector2_4 = vector2_3;
            Vector2 vector2_5 = Vector2.Add(Utils.Floor(Projectile.Center), Vector2.Multiply(Vector2.Multiply(vector2_1, Projectile.scale), 24f));
            rectangle = new Rectangle(0, 46, texture2D.Width, 18);
            int num5 = 18;
            if (flag)
                num5 = 9;
            float num6 = num3;
            if (num3 > 0.0)
            {
                float num4 = 0.0f;
                float num7 = num6 / num5;
                float num8 = num4 + num7 * 0.25f;
                Vector2 vector2_6 = Vector2.Add(vector2_5, Vector2.Multiply(Vector2.Multiply(vector2_1, num7), 0.25f));
                for (int index = 0; index < num5; ++index)
                {
                    float num9 = num7;
                    if (index == 0)
                        num9 *= 0.75f;
                    Main.spriteBatch.Draw(texture2D, Vector2.Add(Vector2.Subtract(vector2_6, Main.screenPosition), vector2_2), new Rectangle?(rectangle), alpha, num2, new Vector2(rectangle.Width / 2, 0.0f), Projectile.scale, 0, 0.0f);
                    num8 += num9;
                    vector2_6 = Vector2.Add(vector2_6, Vector2.Multiply(vector2_1, num9));
                }
            }
            rectangle = new Rectangle(0, 90, texture2D.Width, 48);
            Main.spriteBatch.Draw(texture2D, Vector2.Add(Vector2.Subtract(vector2_4, Main.screenPosition), vector2_2), new Rectangle?(rectangle), alpha, num2, Utils.Top(Utils.Frame(texture2D, 1, 1, 0, 0)), Projectile.scale, 0, 0.0f);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
                return new bool?(true);
            float num = 0.0f;
            return Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Vector2.Add(Projectile.Center, (Vector2)Projectile.velocity), 16f * Projectile.scale, ref num) ? new bool?(true) : new bool?(false);
        }
    }
}
