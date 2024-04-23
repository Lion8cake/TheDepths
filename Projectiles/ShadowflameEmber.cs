using Microsoft.Xna.Framework;
using TheDepths.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using TheDepths.Dusts;
using Terraria.Graphics.Shaders;
using TheDepths.Mounts;

namespace TheDepths.Projectiles
{
	public class ShadowflameEmber1 : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
		}

		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.scale = Projectile.timeLeft / 120f;

            Projectile.velocity.Y += (float)-Projectile.timeLeft / 10000;

			if (Main.rand.NextBool(30))
			{
				Dust dust2 = Dust.NewDustDirect(Projectile.Center, 4, 4, ModContent.DustType<ShadowflameEmber>(), 0f, 0f, 100);
				if (!Main.rand.NextBool(3))
				{
					int projectileDesiredShader = (Main.player[Projectile.owner].mount._type == ModContent.MountType<NightmareHorse>() ? Main.player[Projectile.owner].cMount : Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().cShadowFlame);
					dust2.shader = GameShaders.Armor.GetSecondaryShader(projectileDesiredShader, Main.LocalPlayer);
					dust2.noGravity = true;
					dust2.velocity.Y -= 3f;
					dust2.noLight = true;
				}
			}
			Lighting.AddLight(Projectile.Center, r: 2.22f / 10, g: 1.01f / 10, b: 2.84f / 10);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			int projectileDesiredShader = 0;
			if (Projectile.owner != 255)
			{
				projectileDesiredShader = (Main.player[Projectile.owner].mount._type == ModContent.MountType<NightmareHorse>() ? Main.player[Projectile.owner].cMount : Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().cShadowFlame);
			}
			Matrix value = Main.Transform;
			if (Projectile.isAPreviewDummy)
			{
				value = Main.UIScaleMatrix;
			}
			Main.instance.PrepareDrawnEntityDrawing(Projectile, projectileDesiredShader, value);
			return false;
		}

		public override void PostDraw(Color lightColor) //Adapted code from the funny dd2 ghost pet
        {
			SpriteEffects dir = SpriteEffects.None;
			Vector2 vector82 = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D value99 = ModContent.Request<Texture2D>("TheDepths/Projectiles/ShadowflameEmber1").Value;
			Vector2 origin29 = value99.Size() / 2f;
			ulong seed2 = (ulong)(Projectile.localAI[0] / 4f);
			for (int num372 = 0; num372 < 5; num372++)
			{
				Microsoft.Xna.Framework.Color color103 = new Microsoft.Xna.Framework.Color(100, 100, 100, 0);
				float x14 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
				float y23 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
				Main.EntitySpriteDraw(value99, vector82 + new Vector2(x14, y23), null, color103, Projectile.rotation, origin29, Projectile.timeLeft / 120f, dir);
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}

	public class ShadowflameEmber2 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
		}

		public override void AI()
		{
			Projectile.tileCollide = false;
			Projectile.scale = Projectile.timeLeft / 120f;

			Projectile.velocity.Y += (float)-Projectile.timeLeft / 10000;
			if (Main.rand.NextBool(30))
			{
				Dust dust2 = Dust.NewDustDirect(Projectile.Center, 4, 4, ModContent.DustType<ShadowflameEmber>(), 0f, 0f, 100);
				if (!Main.rand.NextBool(3))
				{
					int projectileDesiredShader = (Main.player[Projectile.owner].mount._type == ModContent.MountType<NightmareHorse>() ? Main.player[Projectile.owner].cMount : Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().cShadowFlame);
					dust2.shader = GameShaders.Armor.GetSecondaryShader(projectileDesiredShader, Main.LocalPlayer);
					dust2.noGravity = true;
					dust2.velocity.Y -= 3f;
					dust2.noLight = true;
				}
			}
			Lighting.AddLight(Projectile.Center, r: 2.22f / 10, g: 1.01f / 10, b: 2.84f / 10);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			int projectileDesiredShader = 0;
			if (Projectile.owner != 255)
			{
				projectileDesiredShader = (Main.player[Projectile.owner].mount._type == ModContent.MountType<NightmareHorse>() ? Main.player[Projectile.owner].cMount : Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().cShadowFlame);
			}
			Matrix value = Main.Transform;
			if (Projectile.isAPreviewDummy)
			{
				value = Main.UIScaleMatrix;
			}
			Main.instance.PrepareDrawnEntityDrawing(Projectile, projectileDesiredShader, value);
			return false;
		}

		public override void PostDraw(Color lightColor) //Adapted code from the funny dd2 ghost pet
		{
			SpriteEffects dir = SpriteEffects.None;
			Vector2 vector82 = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D value99 = ModContent.Request<Texture2D>("TheDepths/Projectiles/ShadowflameEmber2").Value;
			Vector2 origin29 = value99.Size() / 2f;
			ulong seed2 = (ulong)(Projectile.localAI[0] / 4f);
			for (int num372 = 0; num372 < 5; num372++)
			{
				Microsoft.Xna.Framework.Color color103 = new Microsoft.Xna.Framework.Color(100, 100, 100, 0);
				float x14 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
				float y23 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
				Main.EntitySpriteDraw(value99, vector82 + new Vector2(x14, y23), null, color103, Projectile.rotation, origin29, Projectile.timeLeft / 120f, dir);
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}

	public class ShadowflameEmber3 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
		}

		public override void AI()
		{
			Projectile.tileCollide = false;
			Projectile.scale = Projectile.timeLeft / 120f;

			Projectile.velocity.Y += (float)-Projectile.timeLeft / 10000;

			if (Main.rand.NextBool(30))
			{
				Dust dust2 = Dust.NewDustDirect(Projectile.Center, 4, 4, ModContent.DustType<ShadowflameEmber>(), 0f, 0f, 100);
				if (!Main.rand.NextBool(3))
				{
					int projectileDesiredShader = (Main.player[Projectile.owner].mount._type == ModContent.MountType<NightmareHorse>() ? Main.player[Projectile.owner].cMount : Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().cShadowFlame);
					dust2.shader = GameShaders.Armor.GetSecondaryShader(projectileDesiredShader, Main.LocalPlayer);
					dust2.noGravity = true;
					dust2.velocity.Y -= 3f;
					dust2.noLight = true;
				}
			}
			Lighting.AddLight(Projectile.Center, r: 2.22f / 10, g: 1.01f / 10, b: 2.84f / 10);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			int projectileDesiredShader = 0;
			if (Projectile.owner != 255)
			{
				projectileDesiredShader = (Main.player[Projectile.owner].mount._type == ModContent.MountType<NightmareHorse>() ? Main.player[Projectile.owner].cMount : Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().cShadowFlame);
			}
			Matrix value = Main.Transform;
			if (Projectile.isAPreviewDummy)
			{
				value = Main.UIScaleMatrix;
			}
			Main.instance.PrepareDrawnEntityDrawing(Projectile, projectileDesiredShader, value);
			return false;
		}

		public override void PostDraw(Color lightColor) //Adapted code from the funny dd2 ghost pet
		{
			SpriteEffects dir = SpriteEffects.None;
			Vector2 vector82 = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D value99 = ModContent.Request<Texture2D>("TheDepths/Projectiles/ShadowflameEmber3").Value;
			Vector2 origin29 = value99.Size() / 2f;
			ulong seed2 = (ulong)(Projectile.localAI[0] / 4f);
			for (int num372 = 0; num372 < 5; num372++)
			{
				Microsoft.Xna.Framework.Color color103 = new Microsoft.Xna.Framework.Color(100, 100, 100, 0);
				float x14 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
				float y23 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
				Main.EntitySpriteDraw(value99, vector82 + new Vector2(x14, y23), null, color103, Projectile.rotation, origin29, Projectile.timeLeft / 120f, dir);
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}