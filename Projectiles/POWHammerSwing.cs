using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TheDepths.Projectiles
{
	public class POWHammerSwing : ModProjectile
	{
		private enum AttackStage
		{
			Execute,
			Bonk
		}

		private AttackStage CurrentStage {
			get => (AttackStage)Projectile.localAI[0];
			set {
				Projectile.localAI[0] = (float)value;
				Timer = 0;
				AntiTimer = 2f;
				SecondTimer = 0;
				YetAnotherTimer = 0;
			}
		}

		private float AntiTimer = 2f;

		private float SecondTimer;

		private float YetAnotherTimer;

		private ref float InitialAngle => ref Projectile.ai[1];
		private ref float Timer => ref Projectile.ai[2]; 
		private ref float Progress => ref Projectile.localAI[1];
		private ref float Size => ref Projectile.localAI[2];

		private float execTime => (8f * 3) / Owner.GetTotalAttackSpeed(Projectile.DamageType);

		private float bonkTime => 8f * 5 / Owner.GetTotalAttackSpeed(Projectile.DamageType);

		private Player Owner => Main.player[Projectile.owner];

		public override void SetStaticDefaults() {
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.timeLeft = 10000;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false; 
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void OnSpawn(IEntitySource source) {
			Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
			float targetAngle = Owner.MountedCenter.ToRotation();

			if (Projectile.spriteDirection == 1) {
				targetAngle = MathHelper.Clamp(targetAngle, (float)-Math.PI * 5 / 6, (float)Math.PI * 4 / 3);
				InitialAngle = targetAngle - 0.38f * (1.67f * (float)Math.PI) * Projectile.spriteDirection;
			}
			else {
				if (targetAngle < 0) {
					targetAngle += 2 * (float)Math.PI; 
				}

				targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 5 / 6, (float)Math.PI * 4 / 3);
				InitialAngle = targetAngle - 0.45f * (1.67f * (float)Math.PI) * Projectile.spriteDirection;
			}
		}

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write((sbyte)Projectile.spriteDirection);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			Projectile.spriteDirection = reader.ReadSByte(); 
		}

		public override void AI() {
			Owner.itemAnimation = 2;
			Owner.itemTime = 2;

			if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed) {
				Projectile.Kill();
				return;
			}

			switch (CurrentStage) {
				case AttackStage.Execute:
					ExecuteStrike();
					break;
				case AttackStage.Bonk:
					Bonk();
					break;
			}

			SetSwordPosition();
			Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 origin;
            float rotationOffset;
            SpriteEffects effects;

            if (Projectile.spriteDirection > 0)
            {
                origin = new Vector2(0, Projectile.height);
                rotationOffset = MathHelper.ToRadians(45f);
                effects = SpriteEffects.None;
            }
            else
            {
                origin = new Vector2(Projectile.width, Projectile.height);
                rotationOffset = MathHelper.ToRadians(135f);
                effects = SpriteEffects.FlipHorizontally;
            }

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Owner.MountedCenter;
			Vector2 end = start + Projectile.rotation.ToRotationVector2() * ((Projectile.Size.Length()) * Projectile.scale);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
		}

		public override void CutTiles() {
			Vector2 start = Owner.MountedCenter;
			Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
			Utils.PlotTileLine(start, end, 15 * Projectile.scale, DelegateMethods.CutTiles);
		}

		public override bool? CanDamage()
		{
			if (CurrentStage == AttackStage.Bonk)
				return false;
			return base.CanDamage();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.HitDirectionOverride = target.position.X > Owner.MountedCenter.X ? 1 : -1;
			SoundEngine.PlaySound(new SoundStyle("TheDepths/Sounds/Item/UndertalePOWHammerStomp"), Projectile.position);
			if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<POWEffect>()] == 0)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<POWEffect>(), Projectile.damage / 3, 0, Main.myPlayer);
				for (int i = 0; i < 2; i++)
				{
					Gore.NewGore(new EntitySource_Misc(""), target.position, default(Vector2), GoreID.Smoke1);
					Gore.NewGore(new EntitySource_Misc(""), target.position, default(Vector2), GoreID.Smoke2);
					Gore.NewGore(new EntitySource_Misc(""), target.position, default(Vector2), GoreID.Smoke3);
				}
			}
		}

		public void SetSwordPosition() {
			Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress;

			Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f));
			Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); 

			armPosition.Y += Owner.gfxOffY;
			Projectile.Center = armPosition;
			Projectile.scale = Size * 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem); 

			Owner.heldProj = Projectile.whoAmI;
		}

		private void ExecuteStrike() {
			Progress = MathHelper.SmoothStep(0, 1.67f * (float)Math.PI, ((1f - 0.4f) * Timer / execTime) / 1.5f);
			Size = 1;

			if (Timer >= execTime) {
				Bonk();
			}
		}

		private void Bonk()
        {
			YetAnotherTimer++;
			if (YetAnotherTimer <= 5)
            {
				AntiTimer--;
				SecondTimer = 0;
				Progress = (AntiTimer / 10) + 2f;
			}
			else if (YetAnotherTimer <= 8)
            {
				SecondTimer++;
				Progress = (SecondTimer / 10) + 1.65f;
			}
			if (Progress > 1.95f)
            {
				Progress = 1.95f;
			}

			if (Timer >= bonkTime)
            {
				Projectile.Kill();
            }
        }
	}
}