using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheDepths.Projectiles.Chasme;
namespace TheDepths.NPCs.Chasme
{
    public class ChasmeHead : ModNPC
    {
        public int HeartID;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 272;
            NPC.height = 170;
            NPC.defense = 18;
            NPC.lifeMax = 2500;
            NPC.damage = 40;
            NPC.knockBackResist = 0f;
			NPC.HitSound = SoundID.Item70;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
		}

		public override bool CheckDead()
		{
			if (Main.npc[HeartID].life > 0)
            {
                NPC.life = 1;
                return false;
            }
            else
                return true;
		}

		public override void AI()
		{
			if (Main.npc[HeartID].type != ModContent.NPCType<ChasmeHeart>())
            {
                NPC.active = false;
            }
			NPC chasmeSoul = Main.npc[HeartID];
            //Positioning
			NPC.spriteDirection = NPC.direction = chasmeSoul.direction;
			NPC.Center = chasmeSoul.Center + new Vector2(36 * NPC.direction, -150);

            //Death checks
			if (chasmeSoul.life <= 0)
            {
                NPC.life = 0;
                NPC.checkDead();
            }
            else if (NPC.life <= 0)
            {
                NPC.life = 1;
            }
            NPC.dontTakeDamage = NPC.life == 1;

            if (NPC.dontTakeDamage)
            {

            }
		}

		public override void FindFrame(int frameHeight)
		{
            int frame = NPC.frame.Y / frameHeight;
			if (NPC.life <= NPC.lifeMax / 2)
            {
                if (frame < 3)
                {
					NPC.frameCounter++;
					if (NPC.frameCounter >= 6)
					{
						frame++;
						NPC.frameCounter = 0;
					}
				}
			}
            else
            {
                if (frame > 0 && frame <= 3)
                {
					NPC.frameCounter++;
					if (NPC.frameCounter >= 6)
					{
						frame++;
						NPC.frameCounter = 0;
					}
				}
            }
            if (frame >= 5)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
		}
	}
}