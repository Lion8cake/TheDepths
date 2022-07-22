using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.Utilities;
using TheDepths.Buffs;
using TheDepths.Dusts;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Buffs
{
	public class FlaskofMercury : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Weapon Imbue: Mercury");
			Description.SetDefault("Melee attacks boils enemies with mercury");
			Main.debuff[Type] = false;
	    	Main.pvpBuff[Type] = true;
	    	Main.buffNoSave[Type] = false;
		}
		
	public class InflictMercury : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (player.HasBuff(BuffType<FlaskofMercury>()))
            {
                npc.AddBuff(BuffType<MercuryBoiling>(), Main.rand.Next(100, 200));
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (Main.player[projectile.owner].HasBuff(BuffType<FlaskofMercury>()) && (projectile.CountsAsClass(DamageClass.Melee) || ProjectileID.Sets.IsAWhip[projectile.type]))
            {
                npc.AddBuff(BuffType<MercuryBoiling>(), Main.rand.Next(100, 200));
            }
        }
    }

    public class MercuryImbunedProjectile : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].HasBuff(BuffType<FlaskofMercury>()) && projectile.CountsAsClass(DamageClass.Melee))
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<MercuryFire>());
            }
        }
    }

    public class MercuryImbunedItem : GlobalItem
    {
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (player.HasBuff(BuffType<FlaskofMercury>()))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<MercuryFire>());
            }
        }
    }
	}
}
