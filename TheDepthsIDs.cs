using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths {
	public class TheDepthsIDs
    {
		public class Sets {
			public class RecipeBlacklist {
				public static bool[] HellstoneBarOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.HellfireArrow, ItemID.Flamarang, ItemID.MoltenFury, ItemID.FieryGreatsword, ItemID.MoltenPickaxe, ItemID.MoltenHamaxe, ItemID.PhoenixBlaster, ItemID.ImpStaff, ItemID.MoltenHelmet, ItemID.MoltenBreastplate, ItemID.MoltenGreaves, ItemID.FireproofBugNet);

				public static bool[] HellstoneOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.Lava1Echo, ItemID.Lava2Echo, ItemID.Lava3Echo, ItemID.Lava4Echo, ItemID.FlaskofFire, ItemID.ObsidianChest, ItemID.ObsidianSink, ItemID.ToiletObsidian, ItemID.HellstoneBrick, ItemID.HellstoneBar);

				public static bool[] ObsidianOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.DemonTorch, ItemID.ObsidianSkinPotion, ItemID.ObsidianBrick, ItemID.ObsidianBackEcho, ItemID.ObsidianChest, ItemID.ObsidianSink, ItemID.ToiletObsidian, ItemID.ObsidianHelm, ItemID.ObsidianShirt, ItemID.ObsidianPants, ItemID.HellstoneBar, ItemID.ObsidianSkull);

				public static bool[] AnkhShieldOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] FireGauntletOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] AshBlockOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] AshWoodOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.AshWoodBathtub, ItemID.AshWoodBed, ItemID.AshWoodBookcase, ItemID.AshWoodBow, ItemID.AshWoodBreastplate, ItemID.AshWoodCandelabra, ItemID.AshWoodCandle, ItemID.AshWoodChair, ItemID.AshWoodChandelier, ItemID.AshWoodChest, ItemID.AshWoodClock, ItemID.AshWoodDoor, ItemID.AshWoodDresser, ItemID.AshWoodFence, ItemID.AshWoodGreaves, ItemID.AshWoodHammer, ItemID.AshWoodHelmet, ItemID.AshWoodLamp, ItemID.AshWoodLantern, ItemID.AshWoodPiano, ItemID.AshWoodPlatform, ItemID.AshWoodSink, ItemID.AshWoodSofa, ItemID.AshWoodSword, ItemID.AshWoodTable, ItemID.AshWoodToilet, ItemID.AshWoodWall, ItemID.AshWoodWorkbench, ItemID.Fake_AshWoodChest);

				public static bool[] FireblossomOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.ObsidianSkinPotion, ItemID.InfernoPotion, ItemID.PotSuspendedFireblossom);

				public static bool[] ObsidifishOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.SeafoodDinner, ItemID.InfernoPotion);

				public static bool[] FlarefinKoiOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.SeafoodDinner, ItemID.InfernoPotion);

				public static bool[] PwnhammerOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] FireblossomSeedsOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] HellforgeOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] LivingFireBlockOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.LivingCursedFireBlock, ItemID.LivingDemonFireBlock, ItemID.LivingFrostFireBlock, ItemID.LivingIchorBlock, ItemID.LivingUltrabrightFireBlock);

				public static bool[] CobaltShieldOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.ObsidianShield);

				public static bool[] CascadeOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] TreasureMagnetOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] LavaBucketOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] BottomlessLavaBucketOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] LavaSpongeOnlyItem = ItemID.Sets.Factory.CreateBoolSet(); //For Modders

				public static bool[] LavaFishingHookOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.LavaproofTackleBag);
			}

			/// <summary>
			/// Projectiles that cannot be reflected by the Palladium shield and its upgrades, for example the moonlord's Phantasmal Deathray is unparry-able due to it reversing its movement and attacking itself
			/// <br />This is usefull for not breaking bossfights or special attacks done by enemies or pvp players. Also used by various sand balls for obious reasons
			/// </summary>
			public static bool[] UnreflectiveProjectiles = ProjectileID.Sets.Factory.CreateBoolSet(ProjectileID.FairyQueenSunDance, ProjectileID.PhantasmalDeathray, ProjectileID.SandBallFalling, ProjectileID.PearlSandBallFalling, ProjectileID.CrimsandBallFalling, ProjectileID.EbonsandBallFalling);

			/// <summary>
			/// For axes that can destroy petrified trees, normally this is only used by the Axe of Regrowth.
			/// <br />Make sure that said axes that can break petrified trees have their pick value set to 0. Axes with a pickaxe value higher than 0 will already be able to destroy Petrified Trees
			/// </summary>
			public static bool[] AxesAbleToBreakStone = ItemID.Sets.Factory.CreateBoolSet(ItemID.AcornAxe);

			/// <summary>
			/// NPCs that should be unaffected by the Freezing Water debuff granted by the sapphire shovel, aqua stone and more
			/// If an npc is a boss, it is automatically immune.
			/// </summary>
			public static bool[] IsntFreezable = NPCID.Sets.Factory.CreateBoolSet(NPCID.DungeonGuardian, NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.PlanterasHook, NPCID.PlanterasTentacle, NPCID.TheHungry, NPCID.TheHungryII, NPCID.SkeletronHand, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice);
		}
    }
}
