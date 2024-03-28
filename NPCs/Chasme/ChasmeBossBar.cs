using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme
{
	public class ChasmeBossBar : ModBossBar
	{
		private int bossHeadIndex = -1;

		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame) {
			if (bossHeadIndex != -1) {
				return TextureAssets.NpcHeadBoss[bossHeadIndex];
			}
			return null;
		}

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax) {
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active)
				return false;

			bossHeadIndex = npc.GetBossHeadTextureIndex();

			life = npc.life;
			lifeMax = npc.lifeMax;

			if (npc.ModNPC is ChasmeHeart core) {
				NPC headNPC = Main.npc[core.ChasmePartIDs[0]];
				shield = headNPC.life == 1 ? 0 : headNPC.life;
				shieldMax = headNPC.lifeMax;
			}
			return true;
		}
	}
}
