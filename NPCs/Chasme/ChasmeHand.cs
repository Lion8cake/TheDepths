using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using static Terraria.ModLoader.PlayerDrawLayer;
using TheDepths.Projectiles.Chasme;

namespace TheDepths.NPCs.Chasme
{
	public abstract class ChasmeHand : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailCacheLength[Type] = 10;
			NPCID.Sets.TrailingMode[Type] = 1;
		}
		public override void SetDefaults()
		{
			NPC.defense = 14;
			NPC.lifeMax = 350;
			NPC.damage = 35;
			NPC.HitSound = SoundID.Item70;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.noTileCollide = true;
		}


		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			float offsetX;
			//if (Throwing hands (this is the after images)
			//{
			/*if (NPC.spriteDirection == 1)
			{
				offsetX = NPC.width / 2;
			}
			else
			{
				offsetX = NPC.width / 2 * -NPC.spriteDirection;
			}
			Main.instance.LoadNPC(Type);
			Texture2D HandTexture = TextureAssets.Npc[Type].Value;
			Rectangle Source = new Rectangle(0, 0, HandTexture.Width, HandTexture.Height);
			SpriteEffects spriteEffects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(HandTexture.Width * 0.5f, NPC.height * 0.5f);
			for (int i = 0; i < NPC.oldPos.Length; i++)
			{
				Vector2 drawPos = NPC.oldPos[i] + new Vector2(offsetX, NPC.height / 2) - Main.screenPosition;
				Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(HandTexture, drawPos, null, color, NPC.rotation, drawOrigin, NPC.scale - i / (float)NPC.oldPos.Length / 3, spriteEffects, 0f);
			}*/
			//}
			return true;
		}
	}
}