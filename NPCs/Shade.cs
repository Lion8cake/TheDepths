using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Items.Banners;

namespace TheDepths.NPCs
{
    public class Shade : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shade");
        }

        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 70;
            NPC.damage = 46;
            NPC.defense = 10;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 300f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            AIType = NPCID.FloatyGross;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<ShadeBanner>();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
        }
    }
}
