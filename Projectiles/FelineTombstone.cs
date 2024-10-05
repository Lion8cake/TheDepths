using Microsoft.Xna.Framework;
using TheDepths.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;

namespace TheDepths.Projectiles
{
	public class FelineTombstone : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAGravestone[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.knockBack = 12f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 17;
			Projectile.penetrate = -1;
			if (Main.getGoodWorld)
			{
				Projectile.friendly = true;
				Projectile.hostile = true;
			}
		}

		public override bool PreAI()
		{
			if (Projectile.velocity.Y == 0f)
			{
				Projectile.velocity.X *= 0.98f;
			}
			Projectile.rotation += Projectile.velocity.X * 0.1f;
			Projectile.velocity.Y += 0.2f;
			if (Main.getGoodWorld && Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 1f)
			{
				Projectile.damage = 0;
				Projectile.knockBack = 0f;
			}
			if (Projectile.owner != Main.myPlayer)
			{
				return false;
			}
			int num156 = (int)((Projectile.position.X + (float)(Projectile.width / 2)) / 16f);
			int num157 = (int)((Projectile.position.Y + (float)Projectile.height - 4f) / 16f);
			if (Main.tile[num156, num157] == null)
			{
				return false;
			}
			bool flag67 = false;
			TileObject objectData = default(TileObject);
			if (TileObject.CanPlace(num156, num157, ModContent.TileType<Tiles.FelineTombstone>(), 0, Projectile.direction, out objectData))
			{
				flag67 = TileObject.Place(objectData);
			}
			if (flag67)
			{
				NetMessage.SendObjectPlacement(-1, num156, num157, objectData.type, objectData.style, objectData.alternate, objectData.random, Projectile.direction);
				SoundEngine.PlaySound(SoundID.Dig, new Vector2(num156 * 16, num157 * 16));
				int num158 = Sign.ReadSign(num156, num157);
				if (num158 >= 0)
				{
					Sign.TextSign(num158, Projectile.miscText);
					NetMessage.SendData(MessageID.ReadSign, -1, -1, null, num158, 0f, (int)(byte)new BitsByte(b1: true));
				}
				Projectile.Kill();
			}
			return false;
		}
	}
}