using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Items.Banners;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using TheDepths.Items.Armor;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using TheDepths.Projectiles;
using TheDepths.Items;
using TheDepths.NPCs.Chasme;

namespace TheDepths.NPCs
{
	public class CrystalBoundTaxCollector : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
		}

		public override void SetDefaults()
		{
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 34;
			NPC.aiStyle = 0;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.Item27;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.knockBackResist = 0.5f;
			NPC.rarity = 2;
			NPC.netAlways = true;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.UIInfoProvider = new TaxCollectorInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type]);
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.CrystalBoundTaxCollector")
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 3; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 63);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 62);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && !Main.remixWorld) || Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld) && !NPC.savedTaxCollector && !NPC.AnyNPCs(Type))
			{
				return 0.2f;
			}
			return 0f;
		}

	}
	internal class TaxCollectorInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private readonly string _persistentIdentifierToCheck;

		public TaxCollectorInfoProvider(string persistentId)
		{
			_persistentIdentifierToCheck = persistentId;
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryEntryUnlockState unlockState = GetUnlockState(Main.BestiaryTracker.Kills.GetKillCount(_persistentIdentifierToCheck));
			BestiaryUICollectionInfo result = default;
			result.UnlockState = unlockState;
			return result;
		}

		public static BestiaryEntryUnlockState GetUnlockState(int kills)
		{
			if (NPC.savedTaxCollector)
			{
				return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			}
			return BestiaryEntryUnlockState.NotKnownAtAll_0;
		}
	}
}