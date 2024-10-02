using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Items.Weapons;
using TheDepths.Items.Armor;
using TheDepths.Items;
using TheDepths.Items.Accessories;
using TheDepths.Items.Placeable;
using TheDepths.Items.Placeable.Furniture;
using Terraria.Localization;

namespace TheDepths
{
	public class TheDepthsRecipes : ModSystem
	{
		public override void AddRecipes()
		{
			//Vanilla Recipes
			#region Vanilla Recipes
			//Terraspark boots
			Recipe recipe = Recipe.Create(ItemID.TerrasparkBoots);
			recipe.AddIngredient(ItemID.FrostsparkBoots)
			.AddIngredient(ModContent.ItemType<SilverSlippers>())
			.AddTile(TileID.TinkerersWorkbench)
			.SortAfterFirstRecipesOf(ItemID.TerrasparkBoots)
			.Register();

			//Liquid Sensor
			Recipe recipe2 = Recipe.Create(ItemID.LogicSensor_Liquid);
			recipe2.AddIngredient(ItemID.Cog, 5)
			.AddIngredient(ItemID.MagicWaterDropper)
			.AddIngredient(ModContent.ItemType<MagicQuicksilverDropper>())
			.AddIngredient(ItemID.MagicHoneyDropper)
			.AddIngredient(ItemID.Wire)
			.AddTile(TileID.MythrilAnvil)
			.SortAfterFirstRecipesOf(ItemID.LogicSensor_Liquid)
			.Register();

			//Dry Bomb
			Recipe recipe3 = Recipe.Create(ItemID.DryBomb);
			recipe3.AddIngredient(ModContent.ItemType<Items.Weapons.QuicksilverBomb>())
			.SortAfterFirstRecipesOf(ItemID.DryBomb)
			.Register();

			//Dry Rocket
			Recipe recipe4 = Recipe.Create(ItemID.DryRocket);
			recipe4.AddIngredient(ModContent.ItemType<Items.Weapons.QuicksilverRocket>())
			.SortAfterFirstRecipesOf(ItemID.DryRocket)
			.Register();

			//Seafood dinner
			Recipe recipe5 = Recipe.Create(ItemID.SeafoodDinner);
			recipe5.AddIngredient(ModContent.ItemType<Items.ShadowFightingFish>(), 2)
			.AddTile(TileID.CookingPots)
			.SortAfterFirstRecipesOf(ItemID.SeafoodDinner)
			.Register();
			Recipe recipe6 = Recipe.Create(ItemID.SeafoodDinner);
			recipe6.AddIngredient(ModContent.ItemType<Items.QuartzFeeder>(), 2)
			.AddTile(TileID.CookingPots)
			.SortAfterFirstRecipesOf(ItemID.SeafoodDinner)
			.Register();

			//Nights edge
			Recipe recipe8 = Recipe.Create(ItemID.NightsEdge);
			recipe8.AddIngredient(ItemID.LightsBane)
			.AddIngredient(ItemID.Muramasa)
			.AddIngredient(ItemID.BladeofGrass)
			.AddIngredient(ModContent.ItemType<Items.Weapons.Terminex>())
			.AddTile(TileID.DemonAltar)
			.SortAfterFirstRecipesOf(ItemID.NightsEdge)
			.Register();
			Recipe recipe9 = Recipe.Create(ItemID.NightsEdge);
			recipe9.AddIngredient(ItemID.BloodButcherer)
			.AddIngredient(ItemID.Muramasa)
			.AddIngredient(ItemID.BladeofGrass)
			.AddIngredient(ModContent.ItemType<Items.Weapons.Terminex>())
			.AddTile(TileID.DemonAltar)
			.SortAfterFirstRecipesOf(ItemID.NightsEdge)
			.Register();

			//Shellphone
			Recipe recipe18 = Recipe.Create(ItemID.ShellphoneDummy);
			recipe18.AddIngredient(ItemID.CellPhone)
			.AddIngredient(ItemID.MagicConch)
			.AddIngredient(ModContent.ItemType<Items.ShalestoneConch>())
			.AddTile(TileID.TinkerersWorkbench)
			.SortAfterFirstRecipesOf(ItemID.ShellphoneDummy)
			.Register();

			//Finishing Potion
			Recipe recipe7 = Recipe.Create(ItemID.FishingPotion);
			recipe7.AddIngredient(ItemID.BottledWater)
			.AddIngredient(ModContent.ItemType<Items.Placeable.GlitterBlock>())
			.AddIngredient(ItemID.Waterleaf)
			.AddTile(TileID.Bottles)
			.SortAfterFirstRecipesOf(ItemID.FishingPotion)
			.Register();
			#endregion

			//The Depths Recipes
			//Aqua Glove
			Recipe AquaGlove = Recipe.Create(ModContent.ItemType<AquaGlove>());
			AquaGlove.AddIngredient(ItemID.MechanicalGlove, 1);
			AquaGlove.AddIngredient(ModContent.ItemType<AquaStone>(), 1);
			AquaGlove.AddTile(TileID.TinkerersWorkbench);
			AquaGlove.SortAfterFirstRecipesOf(ItemID.FireGauntlet);
			AquaGlove.Register();

			//Aqua Quiver
			Recipe AquaQuiver = Recipe.Create(ModContent.ItemType<AquaQuiver>());
			AquaQuiver.AddIngredient(ItemID.MagicQuiver, 1);
			AquaQuiver.AddIngredient(ModContent.ItemType<AquaStone>(), 1);
			AquaQuiver.AddTile(TileID.TinkerersWorkbench);
			AquaQuiver.SortAfterFirstRecipesOf(ItemID.MoltenQuiver);
			AquaQuiver.Register();

			#region Crystal Skull Crafting Group
			//Crystal Skull
			Recipe CrystalSkull = Recipe.Create(ModContent.ItemType<CrystalSkull>());
			CrystalSkull.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			CrystalSkull.AddTile(TileID.Furnaces);
			CrystalSkull.SortBeforeFirstRecipesOf(ItemID.ManaFlower);
			CrystalSkull.Register();

			//Crying Skull
			Recipe CryingSkull = Recipe.Create(ModContent.ItemType<CryingSkull>());
			CryingSkull.AddIngredient(ModContent.ItemType<CrystalSkull>());
			CryingSkull.AddIngredient(ModContent.ItemType<AmalgamAmulet>());
			CryingSkull.AddTile(TileID.TinkerersWorkbench);
			CryingSkull.SortAfter(CrystalSkull);
			CryingSkull.Register();

			//Crystal Rose
			Recipe CrystalRose = Recipe.Create(ModContent.ItemType<CrystalRose>());
			CrystalRose.AddIngredient(ModContent.ItemType<StoneRose>());
			CrystalRose.AddIngredient(ModContent.ItemType<CrystalSkull>());
			CrystalRose.AddTile(TileID.TinkerersWorkbench);
			CrystalRose.SortAfter(CryingSkull);
			CrystalRose.Register();

			//Azurite Rose
			Recipe AzuriteRose = Recipe.Create(ModContent.ItemType<AzuriteRose>());
			AzuriteRose.AddIngredient(ModContent.ItemType<AmalgamAmulet>());
			AzuriteRose.AddIngredient(ModContent.ItemType<CrystalRose>());
			AzuriteRose.AddTile(TileID.TinkerersWorkbench);
			AzuriteRose.SortAfter(CrystalRose);
			AzuriteRose.Register();

			Recipe AzuriteRose2 = Recipe.Create(ModContent.ItemType<AzuriteRose>());
			AzuriteRose2.AddIngredient(ModContent.ItemType<CryingSkull>());
			AzuriteRose2.AddIngredient(ModContent.ItemType<StoneRose>());
			AzuriteRose2.AddTile(TileID.TinkerersWorkbench);
			AzuriteRose2.SortAfter(AzuriteRose);
			AzuriteRose2.Register();

			Recipe AzuriteRose3 = Recipe.Create(ModContent.ItemType<AzuriteRose>());
			AzuriteRose3.AddIngredient(ModContent.ItemType<CryingSkull>());
			AzuriteRose3.AddIngredient(ModContent.ItemType<CrystalRose>());
			AzuriteRose3.AddTile(TileID.TinkerersWorkbench);
			AzuriteRose3.SortAfter(AzuriteRose2);
			AzuriteRose3.Register();

			//Silver Charm
			Recipe SilverCharm = Recipe.Create(ModContent.ItemType<SilverCharm>());
			SilverCharm.AddIngredient(ModContent.ItemType<AmalgamAmulet>());
			SilverCharm.AddIngredient(ModContent.ItemType<CrystalSkull>());
			SilverCharm.AddTile(TileID.TinkerersWorkbench);
			SilverCharm.SortAfter(AzuriteRose3);
			SilverCharm.Register();

			//Crystal Walking Boots
			Recipe CrystalWaterWalkingBoots = Recipe.Create(ModContent.ItemType<CrystalWaterWalkingBoots>());
			CrystalWaterWalkingBoots.AddIngredient(ItemID.WaterWalkingBoots, 1);
			CrystalWaterWalkingBoots.AddIngredient(ModContent.ItemType<Items.Accessories.CrystalSkull>(), 1);
			CrystalWaterWalkingBoots.AddTile(TileID.TinkerersWorkbench);
			CrystalWaterWalkingBoots.SortAfter(SilverCharm);
			CrystalWaterWalkingBoots.Register();

			//Silver Slippers
			Recipe SilverSlippers = Recipe.Create(ModContent.ItemType<SilverSlippers>());
			SilverSlippers.AddIngredient(ModContent.ItemType<StoneRose>());
			SilverSlippers.AddIngredient(ModContent.ItemType<AmalgamAmulet>());
			SilverSlippers.AddIngredient(ModContent.ItemType<CrystalWaterWalkingBoots>());
			SilverSlippers.AddTile(TileID.TinkerersWorkbench);
			SilverSlippers.SortAfter(CrystalWaterWalkingBoots);
			SilverSlippers.Register();

			Recipe SilverSlippers2 = Recipe.Create(ModContent.ItemType<SilverSlippers>());
			SilverSlippers2.AddIngredient(ModContent.ItemType<StoneRose>());
			SilverSlippers2.AddIngredient(ModContent.ItemType<SilverCharm>());
			SilverSlippers2.AddIngredient(ModContent.ItemType<CrystalWaterWalkingBoots>());
			SilverSlippers2.AddTile(TileID.TinkerersWorkbench);
			SilverSlippers2.SortAfter(SilverSlippers);
			SilverSlippers2.Register();

			Recipe SilverSlippers3 = Recipe.Create(ModContent.ItemType<SilverSlippers>());
			SilverSlippers3.AddIngredient(ModContent.ItemType<StoneRose>());
			SilverSlippers3.AddIngredient(ModContent.ItemType<SilverCharm>());
			SilverSlippers3.AddIngredient(ItemID.WaterWalkingBoots);
			SilverSlippers3.AddTile(TileID.TinkerersWorkbench);
			SilverSlippers3.SortAfter(SilverSlippers2);
			SilverSlippers3.Register();

			Recipe SilverSlippers4 = Recipe.Create(ModContent.ItemType<SilverSlippers>());
			SilverSlippers4.AddIngredient(ModContent.ItemType<AzuriteRose>());
			SilverSlippers4.AddIngredient(ItemID.WaterWalkingBoots);
			SilverSlippers4.AddTile(TileID.TinkerersWorkbench);
			SilverSlippers4.SortAfter(SilverSlippers3);
			SilverSlippers4.Register();

			Recipe SilverSlippers5 = Recipe.Create(ModContent.ItemType<SilverSlippers>());
			SilverSlippers5.AddIngredient(ModContent.ItemType<AzuriteRose>());
			SilverSlippers5.AddIngredient(ModContent.ItemType<CrystalWaterWalkingBoots>());
			SilverSlippers5.AddTile(TileID.TinkerersWorkbench);
			SilverSlippers5.SortAfter(SilverSlippers4);
			SilverSlippers5.Register();

			//Nightmare flare treads
			Recipe NightmareFlareTreads = Recipe.Create(ModContent.ItemType<NightmareFlareTreads>());
			NightmareFlareTreads.AddIngredient(ItemID.SpectreBoots, 1);
			NightmareFlareTreads.AddIngredient(ModContent.ItemType<Items.Accessories.ShadowflameEmberedTreads>(), 1);
			NightmareFlareTreads.AddTile(TileID.TinkerersWorkbench);
			NightmareFlareTreads.SortAfter(SilverSlippers5);
			NightmareFlareTreads.Register();
			#endregion

			//Crystal Horseshoe
			Recipe CrystalHorseshoe = Recipe.Create(ModContent.ItemType<CrystalHorseshoe>());
			CrystalHorseshoe.AddIngredient(ItemID.LuckyHorseshoe);
			CrystalHorseshoe.AddIngredient(ModContent.ItemType<CrystalSkull>());
			CrystalHorseshoe.AddTile(TileID.TinkerersWorkbench);
			CrystalHorseshoe.SortAfterFirstRecipesOf(ItemID.ObsidianHorseshoe);
			CrystalHorseshoe.Register();

			#region Crystal Shield Crafting Group
			//Crystal Shield
			Recipe CrystalShield = Recipe.Create(ModContent.ItemType<CrystalShield>());
			CrystalShield.AddIngredient(ModContent.ItemType<Items.Accessories.CrystalSkull>(), 1);
			CrystalShield.AddIngredient(ModContent.ItemType<Items.Accessories.PalladiumShield>(), 1);
			CrystalShield.AddTile(TileID.TinkerersWorkbench);
			CrystalShield.SortAfterFirstRecipesOf(ItemID.AnkhShield);
			CrystalShield.Register();

			//Sanctus Shield
			Recipe SanctusShield = Recipe.Create(ModContent.ItemType<SanctusShield>());
			SanctusShield.AddIngredient(ModContent.ItemType<CrystalShield>(), 1);
			SanctusShield.AddIngredient(ItemID.AnkhCharm, 1);
			SanctusShield.AddTile(TileID.TinkerersWorkbench);
			SanctusShield.SortAfter(CrystalShield);
			SanctusShield.Register();
			#endregion

			//Mercury Bobber
			Recipe MercuryMossFishingBobber = Recipe.Create(ModContent.ItemType<MercuryMossFishingBobber>());
			MercuryMossFishingBobber.AddIngredient(ModContent.ItemType<Items.Placeable.MercuryMoss>(), 5);
			MercuryMossFishingBobber.AddIngredient(ItemID.FishingBobberGlowingStar, 1);
			MercuryMossFishingBobber.AddTile(TileID.TinkerersWorkbench);
			MercuryMossFishingBobber.SortAfterFirstRecipesOf(ItemID.FishingBobberGlowingLava);
			MercuryMossFishingBobber.Register();

			//Quicksilver proof tackle bag
			Recipe QuicksilverproofTackleBag = Recipe.Create(ModContent.ItemType<QuicksilverproofTackleBag>());
			QuicksilverproofTackleBag.AddIngredient(ItemID.AnglerTackleBag, 1);
			QuicksilverproofTackleBag.AddIngredient(ModContent.ItemType<Items.Accessories.QuicksilverproofFishingHook>(), 1);
			QuicksilverproofTackleBag.AddTile(TileID.TinkerersWorkbench);
			QuicksilverproofTackleBag.SortAfterFirstRecipesOf(ItemID.LavaproofTackleBag);
			QuicksilverproofTackleBag.Register();

			//Quicksilver surf board
			Recipe QuickSilverSurfboard = Recipe.Create(ModContent.ItemType<QuickSilverSurfboard>());
			QuickSilverSurfboard.AddIngredient(ModContent.ItemType<Items.Placeable.DiamondDust>(), 20);
			QuickSilverSurfboard.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20);
			QuickSilverSurfboard.AddIngredient(ItemID.SoulofFlight, 20);
			QuickSilverSurfboard.AddTile(TileID.MythrilAnvil);
			QuickSilverSurfboard.SortAfterFirstRecipesOf(ItemID.DemonWings);
			QuickSilverSurfboard.Register();

			#region Geode Armour Crafting Group
			//Shaderang
			Recipe Shaderang = Recipe.Create(ModContent.ItemType<Shaderang>());
			Shaderang.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 3);
			Shaderang.AddIngredient(ItemID.ThornChakram);
			Shaderang.AddTile(TileID.Anvils);
			Shaderang.SortAfterFirstRecipesOf(ItemID.FossilPants);
			Shaderang.Register();

			//Geode Helmet
			Recipe GeodeHelmet = Recipe.Create(ModContent.ItemType<GeodeHelmet>());
			GeodeHelmet.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 5);
			GeodeHelmet.AddTile(TileID.Anvils);
			GeodeHelmet.SortAfter(Shaderang);
			GeodeHelmet.Register();

			//Geode Chestplate
			Recipe GeodeChestplate = Recipe.Create(ModContent.ItemType<GeodeChestplate>());
			GeodeChestplate.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 10);
			GeodeChestplate.AddTile(TileID.Anvils);
			GeodeChestplate.SortAfter(GeodeHelmet);
			GeodeChestplate.Register();

			//Geode Leggings
			Recipe GeodeLeggings = Recipe.Create(ModContent.ItemType<GeodeLeggings>());
			GeodeLeggings.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 8);
			GeodeLeggings.AddTile(TileID.Anvils);
			GeodeLeggings.SortAfter(GeodeChestplate);
			GeodeLeggings.Register();
			#endregion

			AddNightmareWoodFurnitureArmorAndItems(out Recipe NightwoodToilet);

			AddPetrifiedWoodFurnitureArmorAndItems(NightwoodToilet);

			//Onyx Robe
			Recipe OnyxRobe = Recipe.Create(ModContent.ItemType<OnyxRobe>());
			OnyxRobe.AddIngredient(ItemID.Robe, 1);
			OnyxRobe.AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 10);
			OnyxRobe.AddTile(TileID.Loom);
			OnyxRobe.SortAfterFirstRecipesOf(ItemID.AmberRobe);
			OnyxRobe.Register();

			#region Quartz armour Crafting Group
			//Quartz Hood
			Recipe QuartzHood = Recipe.Create(ModContent.ItemType<QuartzHood>());
			QuartzHood.AddIngredient(ItemID.Silk, 10);
			QuartzHood.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			QuartzHood.AddIngredient(ItemID.ShadowScale, 5);
			QuartzHood.AddTile(TileID.Hellforge);
			QuartzHood.SortBeforeFirstRecipesOf(ItemID.AmethystRobe);
			QuartzHood.Register();

			//Quartz Winter Coat
			Recipe QuartzWinterCoat = Recipe.Create(ModContent.ItemType<QuartzWinterCoat>());
			QuartzWinterCoat.AddIngredient(ItemID.Silk, 10);
			QuartzWinterCoat.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			QuartzWinterCoat.AddIngredient(ItemID.ShadowScale, 10);
			QuartzWinterCoat.AddTile(TileID.Hellforge);
			QuartzWinterCoat.SortAfter(QuartzHood);
			QuartzWinterCoat.Register();

			//Quartz Leggings
			Recipe QuartzLeggings = Recipe.Create(ModContent.ItemType<QuartzLeggings>());
			QuartzLeggings.AddIngredient(ItemID.Silk, 10);
			QuartzLeggings.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			QuartzLeggings.AddIngredient(ItemID.ShadowScale, 5);
			QuartzLeggings.AddTile(TileID.Hellforge);
			QuartzLeggings.SortAfter(QuartzWinterCoat);
			QuartzLeggings.Register();

			//Quartz Hood (Tissue)
			Recipe QuartzHood2 = Recipe.Create(ModContent.ItemType<QuartzHood>());
			QuartzHood2.AddIngredient(ItemID.Silk, 10);
			QuartzHood2.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			QuartzHood2.AddIngredient(ItemID.TissueSample, 5);
			QuartzHood2.AddTile(TileID.Hellforge);
			QuartzHood2.SortAfter(QuartzLeggings);
			QuartzHood2.Register();

			//Quartz Winter Coat (Tissue)
			Recipe QuartzWinterCoat2 = Recipe.Create(ModContent.ItemType<QuartzWinterCoat>());
			QuartzWinterCoat2.AddIngredient(ItemID.Silk, 10);
			QuartzWinterCoat2.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			QuartzWinterCoat2.AddIngredient(ItemID.TissueSample, 10);
			QuartzWinterCoat2.AddTile(TileID.Hellforge);
			QuartzWinterCoat2.SortAfter(QuartzHood2);
			QuartzWinterCoat2.Register();

			//Quartz Leggings (Tissue)
			Recipe QuartzLeggings2 = Recipe.Create(ModContent.ItemType<QuartzLeggings>());
			QuartzLeggings2.AddIngredient(ItemID.Silk, 10);
			QuartzLeggings2.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			QuartzLeggings2.AddIngredient(ItemID.TissueSample, 5);
			QuartzLeggings2.AddTile(TileID.Hellforge);
			QuartzLeggings2.SortAfter(QuartzWinterCoat2);
			QuartzLeggings2.Register();
			#endregion

			//Black Phaseblade
			Recipe BlackPhaseblade = Recipe.Create(ModContent.ItemType<BlackPhaseblade>());
			BlackPhaseblade.AddIngredient(ItemID.MeteoriteBar, 15);
			BlackPhaseblade.AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 10);
			BlackPhaseblade.AddTile(TileID.Anvils);
			BlackPhaseblade.SortAfterFirstRecipesOf(ItemID.OrangePhaseblade);
			BlackPhaseblade.Register();

			//Black Phasesabler
			Recipe BlackPhasesaber = Recipe.Create(ModContent.ItemType<BlackPhasesaber>());
			BlackPhasesaber.AddIngredient(ItemID.CrystalShard, 25);
			BlackPhasesaber.AddIngredient(ModContent.ItemType<BlackPhaseblade>(), 1);
			BlackPhasesaber.AddTile(TileID.MythrilAnvil);
			BlackPhasesaber.SortAfterFirstRecipesOf(ItemID.OrangePhasesaber);
			BlackPhasesaber.Register();

			//Diamond Arrow
			Recipe DiamondArrow = Recipe.Create(ModContent.ItemType<DiamondArrow>(), 33);
			DiamondArrow.AddIngredient(ItemID.WoodenArrow, 33);
			DiamondArrow.AddIngredient(ModContent.ItemType<DiamondDust>(), 1);
			DiamondArrow.AddTile(TileID.Anvils);
			DiamondArrow.SortAfterFirstRecipesOf(ItemID.HellfireArrow);
			DiamondArrow.Register();

			//Diamond Bolt
			Recipe DiamondBolt = Recipe.Create(ModContent.ItemType<DiamondBolt>());
			DiamondBolt.AddIngredient(ItemID.DiamondStaff);
			DiamondBolt.AddIngredient(ModContent.ItemType<Items.Placeable.DiamondDust>(), 30);
			DiamondBolt.AddTile(TileID.MythrilAnvil);
			DiamondBolt.SortAfterFirstRecipesOf(ItemID.DiamondStaff);
			DiamondBolt.Register();

			//Ruby Bolt
			Recipe RubyBolt = Recipe.Create(ModContent.ItemType<RubyBolt>());
			RubyBolt.AddIngredient(ItemID.RubyStaff);
			RubyBolt.AddIngredient(ModContent.ItemType<Items.Placeable.Ember>(), 30);
			RubyBolt.AddTile(TileID.MythrilAnvil);
			RubyBolt.SortAfterFirstRecipesOf(ItemID.RubyStaff);
			RubyBolt.Register();

			//Quicksilver Bomb
			Recipe QuicksilverBomb = Recipe.Create(ModContent.ItemType<QuicksilverBomb>());
			QuicksilverBomb.AddIngredient(ItemID.DryBomb);
			QuicksilverBomb.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"), () => Worldgen.TheDepthsWorldGen.isWorldDepths && Main.LocalPlayer.adjLava);
			QuicksilverBomb.SortAfterFirstRecipesOf(ItemID.LavaBomb);
			QuicksilverBomb.Register();

			//Quicksilver Rocket
			Recipe QuicksilverRocket = Recipe.Create(ModContent.ItemType<QuicksilverRocket>());
			QuicksilverRocket.AddIngredient(ItemID.DryRocket);
			QuicksilverRocket.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"), () => Worldgen.TheDepthsWorldGen.isWorldDepths && Main.LocalPlayer.adjLava);
			QuicksilverRocket.SortAfterFirstRecipesOf(ItemID.LavaRocket);
			QuicksilverRocket.Register();

			#region Mercury Equipment Crafting Group
			//Terminex
			Recipe Terminex = Recipe.Create(ModContent.ItemType<Terminex>());
			Terminex.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20);
			Terminex.AddTile(TileID.Anvils);
			Terminex.SortAfterFirstRecipesOf(ItemID.FireproofBugNet);
			Terminex.Register();

			//Mercury Pickaxe
			Recipe MercuryPickaxe = Recipe.Create(ModContent.ItemType<MercuryPickaxe>());
			MercuryPickaxe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20);
			MercuryPickaxe.AddTile(TileID.Anvils);
			MercuryPickaxe.SortAfter(Terminex);
			MercuryPickaxe.Register();

			//Mercury Hamaxe
			Recipe MercuryHamaxe = Recipe.Create(ModContent.ItemType<MercuryHamaxe>());
			MercuryHamaxe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 15);
			MercuryHamaxe.AddTile(TileID.Anvils);
			MercuryHamaxe.SortAfter(MercuryPickaxe);
			MercuryHamaxe.Register();

			//Silverado
			Recipe Silverado = Recipe.Create(ModContent.ItemType<Silverado>());
			Silverado.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 10);
			Silverado.AddIngredient(ItemID.Handgun, 1);
			Silverado.AddTile(TileID.Anvils);
			Silverado.SortAfter(MercuryHamaxe);
			Silverado.Register();

			//Onyx Septre
			Recipe OnyxSepter = Recipe.Create(ModContent.ItemType<OnyxSepter>());
			OnyxSepter.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 10);
			OnyxSepter.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 6);
			OnyxSepter.AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 8);
			OnyxSepter.AddTile(TileID.MythrilAnvil);
			OnyxSepter.SortAfter(Silverado);
			OnyxSepter.Register();

			//Mercury Helmet
			Recipe MercuryHelmet = Recipe.Create(ModContent.ItemType<MercuryHelmet>());
			MercuryHelmet.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 10);
			MercuryHelmet.AddTile(TileID.Anvils);
			MercuryHelmet.SortAfter(OnyxSepter);
			MercuryHelmet.Register();

			//Mercury Chestplate
			Recipe MercuryChestplate = Recipe.Create(ModContent.ItemType<MercuryChestplate>());
			MercuryChestplate.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20);
			MercuryChestplate.AddTile(TileID.Anvils);
			MercuryChestplate.SortAfter(MercuryHelmet);
			MercuryChestplate.Register();

			//Mercury Greaves
			Recipe MercuryGreaves = Recipe.Create(ModContent.ItemType<MercuryGreaves>());
			MercuryGreaves.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 15);
			MercuryGreaves.AddTile(TileID.Anvils);
			MercuryGreaves.SortAfter(MercuryChestplate);
			MercuryGreaves.Register();

			//Quicksilver Proof Bug Net
			Recipe QuicksilverproofBugNet = Recipe.Create(ModContent.ItemType<QuicksilverproofBugNet>());
			QuicksilverproofBugNet.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 15);
			QuicksilverproofBugNet.AddIngredient(ItemID.BugNet, 1);
			QuicksilverproofBugNet.AddTile(TileID.Hellforge);
			QuicksilverproofBugNet.SortAfter(MercuryGreaves);
			QuicksilverproofBugNet.Register();

			//Core Builder
			Recipe CoreBuilder = Recipe.Create(ModContent.ItemType<CoreBuilder>());
			CoreBuilder.AddIngredient(ItemID.AshBlock, 50);
			CoreBuilder.AddIngredient(ItemID.Hellforge, 1);
			CoreBuilder.AddIngredient(ItemID.HellstoneBar, 10);
			CoreBuilder.AddTile(TileID.Anvils);
			CoreBuilder.SortAfter(QuicksilverproofBugNet);
			CoreBuilder.Register();
			AddAndReplaceCoreBuilder<RubyRelic>(ItemID.GuideVoodooDoll, CoreBuilder);
			AddAndReplaceCoreBuilder<ShalestoneConch>(ItemID.DemonConch, CoreBuilder);
			AddAndReplaceCoreBuilder<AquaStone>(ItemID.MagmaStone, CoreBuilder);
			#endregion

			//Crystal Skin potion
			Recipe CrystalSkinPotion = Recipe.Create(ModContent.ItemType<CrystalSkinPotion>());
			CrystalSkinPotion.AddIngredient(ItemID.BottledWater, 1);
			CrystalSkinPotion.AddIngredient(ItemID.Waterleaf, 1);
			CrystalSkinPotion.AddIngredient(ModContent.ItemType<Items.ShadowShrub>(), 1);
			CrystalSkinPotion.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 1);
			CrystalSkinPotion.AddTile(TileID.Bottles);
			CrystalSkinPotion.SortAfterFirstRecipesOf(ItemID.ObsidianSkinPotion);
			CrystalSkinPotion.Register();

			//Flask of Mercury
			Recipe FlaskofMercury = Recipe.Create(ModContent.ItemType<FlaskofMercury>());
			FlaskofMercury.AddIngredient(ItemID.BottledWater, 1);
			FlaskofMercury.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 3);
			FlaskofMercury.AddTile(TileID.ImbuingStation);
			FlaskofMercury.SortAfterFirstRecipesOf(ItemID.FlaskofFire);
			FlaskofMercury.Register();

			//Intense Shadow Dye
			Recipe IntenseShadowDye = Recipe.Create(ModContent.ItemType<IntenseShadowDye>());
			IntenseShadowDye.AddIngredient(ItemID.ShadowDye, 2);
			IntenseShadowDye.AddTile(TileID.DyeVat);
			IntenseShadowDye.SortAfterFirstRecipesOf(ItemID.IntenseRainbowDye);
			IntenseShadowDye.Register();

			//Large Onyx
			Recipe LargeOnyx = Recipe.Create(ModContent.ItemType<LargeOnyx>());
			LargeOnyx.AddIngredient(ModContent.ItemType<Onyx>(), 15);
			LargeOnyx.AddTile(TileID.Anvils);
			LargeOnyx.SortAfterFirstRecipesOf(ItemID.LargeAmber);
			LargeOnyx.Register();

			//Onyx Hook
			Recipe OnyxHook = Recipe.Create(ModContent.ItemType<OnyxHook>());
			OnyxHook.AddIngredient(ModContent.ItemType<Onyx>(), 15);
			OnyxHook.AddTile(TileID.Anvils);
			OnyxHook.SortAfterFirstRecipesOf(ItemID.AmberHook);
			OnyxHook.Register();

			//Onyx Minecart
			Recipe OnyxMinecart = Recipe.Create(ModContent.ItemType<OnyxMinecart>());
			OnyxMinecart.AddIngredient(ItemID.Minecart);
			OnyxMinecart.AddIngredient(ModContent.ItemType<LargeOnyx>());
			OnyxMinecart.AddTile(TileID.Anvils);
			OnyxMinecart.SortAfterFirstRecipesOf(ItemID.AmberMinecart);
			OnyxMinecart.Register();

			//Silver Sphere Potion
			Recipe SilverSpherePotion = Recipe.Create(ModContent.ItemType<SilverSpherePotion>());
			SilverSpherePotion.AddIngredient(ItemID.BottledWater, 1);
			SilverSpherePotion.AddIngredient(ModContent.ItemType<ShadowFightingFish>(), 1);
			SilverSpherePotion.AddIngredient(ModContent.ItemType<QuartzFeeder>(), 2);
			SilverSpherePotion.AddIngredient(ModContent.ItemType<ShadowShrub>(), 1);
			SilverSpherePotion.AddTile(TileID.Bottles);
			SilverSpherePotion.SortAfterFirstRecipesOf(ItemID.InfernoPotion);
			SilverSpherePotion.Register();

			#region Depths Critters Crafting Group
			//Albino Rat Cage
			Recipe AlbinoRatCage = Recipe.Create(ModContent.ItemType<AlbinoRatCage>());
			AlbinoRatCage.AddIngredient(ModContent.ItemType<Items.AlbinoRat>());
			AlbinoRatCage.AddIngredient(ItemID.Terrarium, 1);
			AlbinoRatCage.SortAfterFirstRecipesOf(ItemID.MagmaSnailCage);
			AlbinoRatCage.Register();

			//Enchanted Nightmare Worm Cage
			Recipe EnchantedNightmareWormCage = Recipe.Create(ModContent.ItemType<EnchantedNightmareWormCage>());
			EnchantedNightmareWormCage.AddIngredient(ModContent.ItemType<Items.EnchantedNightmareWorm>());
			EnchantedNightmareWormCage.AddIngredient(ItemID.Terrarium, 1);
			EnchantedNightmareWormCage.SortAfter(AlbinoRatCage);
			EnchantedNightmareWormCage.Register();

			//Quartz Crawler Cage
			Recipe QuartzCrawlerCage = Recipe.Create(ModContent.ItemType<QuartzCrawlerCage>());
			QuartzCrawlerCage.AddIngredient(ModContent.ItemType<Items.QuartzCrawler>());
			QuartzCrawlerCage.AddIngredient(ItemID.Terrarium, 1);
			QuartzCrawlerCage.SortAfter(EnchantedNightmareWormCage);
			QuartzCrawlerCage.Register();
			#endregion

			#region Gem stone blocks
			//Onyx Stone Block
			Recipe OnyxStoneBlock = Recipe.Create(ModContent.ItemType<OnyxStoneBlock>());
			OnyxStoneBlock.AddIngredient(ModContent.ItemType<Onyx>());
			OnyxStoneBlock.AddIngredient(ItemID.StoneBlock);
			OnyxStoneBlock.AddCondition(Condition.InGraveyard);
			OnyxStoneBlock.AddTile(TileID.HeavyWorkBench);
			OnyxStoneBlock.SortAfterFirstRecipesOf(ItemID.AmberStoneBlock);
			OnyxStoneBlock.Register();

			//Amethyst Shalestone Block
			Recipe AmethystShalestoneBlock = Recipe.Create(ModContent.ItemType<AmethystShalestoneBlock>());
			AmethystShalestoneBlock.AddIngredient(ItemID.Amethyst);
			AmethystShalestoneBlock.AddIngredient(ModContent.ItemType<Shalestone>());
			AmethystShalestoneBlock.AddCondition(Condition.InGraveyard);
			AmethystShalestoneBlock.AddTile(TileID.HeavyWorkBench);
			AmethystShalestoneBlock.SortAfter(OnyxStoneBlock);
			AmethystShalestoneBlock.Register();

			//Topaz Shalestone Block
			Recipe TopazShalestoneBlock = Recipe.Create(ModContent.ItemType<TopazShalestoneBlock>());
			TopazShalestoneBlock.AddIngredient(ItemID.Topaz);
			TopazShalestoneBlock.AddIngredient(ModContent.ItemType<Shalestone>());
			TopazShalestoneBlock.AddCondition(Condition.InGraveyard);
			TopazShalestoneBlock.AddTile(TileID.HeavyWorkBench);
			TopazShalestoneBlock.SortAfter(AmethystShalestoneBlock);
			TopazShalestoneBlock.Register();

			//Sapphire Shalestone Block
			Recipe SapphireShalestoneBlock = Recipe.Create(ModContent.ItemType<SapphireShalestoneBlock>());
			SapphireShalestoneBlock.AddIngredient(ItemID.Sapphire);
			SapphireShalestoneBlock.AddIngredient(ModContent.ItemType<Shalestone>());
			SapphireShalestoneBlock.AddCondition(Condition.InGraveyard);
			SapphireShalestoneBlock.AddTile(TileID.HeavyWorkBench);
			SapphireShalestoneBlock.SortAfter(TopazShalestoneBlock);
			SapphireShalestoneBlock.Register();

			//Emerald Shalestone Block
			Recipe EmeraldShalestoneBlock = Recipe.Create(ModContent.ItemType<EmeraldShalestoneBlock>());
			EmeraldShalestoneBlock.AddIngredient(ItemID.Emerald);
			EmeraldShalestoneBlock.AddIngredient(ModContent.ItemType<Shalestone>());
			EmeraldShalestoneBlock.AddCondition(Condition.InGraveyard);
			EmeraldShalestoneBlock.AddTile(TileID.HeavyWorkBench);
			EmeraldShalestoneBlock.SortAfter(SapphireShalestoneBlock);
			EmeraldShalestoneBlock.Register();

			//Ruby Shalestone Block
			Recipe RubyShalestoneBlock = Recipe.Create(ModContent.ItemType<RubyShalestoneBlock>());
			RubyShalestoneBlock.AddIngredient(ItemID.Ruby);
			RubyShalestoneBlock.AddIngredient(ModContent.ItemType<Shalestone>());
			RubyShalestoneBlock.AddCondition(Condition.InGraveyard);
			RubyShalestoneBlock.AddTile(TileID.HeavyWorkBench);
			RubyShalestoneBlock.SortAfter(EmeraldShalestoneBlock);
			RubyShalestoneBlock.Register();

			//Diamond Shalestone Block
			Recipe DiamondShalestoneBlock = Recipe.Create(ModContent.ItemType<DiamondShalestoneBlock>());
			DiamondShalestoneBlock.AddIngredient(ItemID.Diamond);
			DiamondShalestoneBlock.AddIngredient(ModContent.ItemType<Shalestone>());
			DiamondShalestoneBlock.AddCondition(Condition.InGraveyard);
			DiamondShalestoneBlock.AddTile(TileID.HeavyWorkBench);
			DiamondShalestoneBlock.SortAfter(RubyShalestoneBlock);
			DiamondShalestoneBlock.Register();

			//Onyx Shalestone Block
			Recipe OnyxShalestoneBlock = Recipe.Create(ModContent.ItemType<OnyxShalestoneBlock>());
			OnyxShalestoneBlock.AddIngredient(ModContent.ItemType<Onyx>());
			OnyxShalestoneBlock.AddIngredient(ModContent.ItemType<Shalestone>());
			OnyxShalestoneBlock.AddCondition(Condition.InGraveyard);
			OnyxShalestoneBlock.AddTile(TileID.HeavyWorkBench);
			OnyxShalestoneBlock.SortAfter(DiamondShalestoneBlock);
			OnyxShalestoneBlock.Register();
			#endregion

			//Arquerite Bar
			Recipe ArqueriteBar = Recipe.Create(ModContent.ItemType<ArqueriteBar>());
			ArqueriteBar.AddIngredient(ModContent.ItemType<ArqueriteOre>(), 3);
			ArqueriteBar.AddIngredient(ModContent.ItemType<Quartz>(), 1);
			ArqueriteBar.AddTile(TileID.Hellforge);
			ArqueriteBar.SortAfterFirstRecipesOf(ItemID.HellstoneBar);
			ArqueriteBar.Register();

			#region Ore Brick Crafting Groups
			//Arquerite Bricks
			Recipe ArqueriteBricks = Recipe.Create(ModContent.ItemType<ArqueriteBricks>(), 5);
			ArqueriteBricks.AddIngredient(ItemID.StoneBlock, 5);
			ArqueriteBricks.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 1);
			ArqueriteBricks.AddTile(TileID.Furnaces);
			ArqueriteBricks.DisableDecraft();
			ArqueriteBricks.SortAfterFirstRecipesOf(ItemID.ObsidianBackEcho);
			ArqueriteBricks.Register();

			//Arquerite Brick wall
			Recipe ArqueriteBrickWall = Recipe.Create(ModContent.ItemType<ArqueriteBrickWall>(), 4);
			ArqueriteBrickWall.AddIngredient(ModContent.ItemType<ArqueriteBricks>());
			ArqueriteBrickWall.AddTile(TileID.WorkBenches);
			ArqueriteBrickWall.SortAfter(ArqueriteBricks);
			ArqueriteBrickWall.Register();

			Recipe ArqueriteBricks2 = Recipe.Create(ModContent.ItemType<ArqueriteBricks>());
			ArqueriteBricks2.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBrickWall>(), 4);
			ArqueriteBricks2.AddTile(TileID.WorkBenches);
			ArqueriteBricks2.SortAfter(ArqueriteBrickWall);
			ArqueriteBricks2.Register();

			//Leaky Mercury Wall
			Recipe LeakyMercuryWall = Recipe.Create(ModContent.ItemType<LeakyMercuryWall>(), 4);
			LeakyMercuryWall.AddIngredient(ModContent.ItemType<ArqueriteOre>());
			LeakyMercuryWall.AddCondition(Condition.InGraveyard);
			LeakyMercuryWall.AddTile(TileID.WorkBenches);
			LeakyMercuryWall.SortAfter(ArqueriteBrickWall);
			LeakyMercuryWall.Register();

			Recipe ArqueriteOre = Recipe.Create(ModContent.ItemType<ArqueriteOre>());
			ArqueriteOre.AddIngredient(ModContent.ItemType<LeakyMercuryWall>(), 4);
			ArqueriteOre.AddTile(TileID.WorkBenches);
			ArqueriteOre.SortAfter(LeakyMercuryWall);
			ArqueriteOre.Register();

			//Bubble Mercury Wall
			Recipe MercuryBubbleWall = Recipe.Create(ModContent.ItemType<MercuryBubbleWall>(), 4);
			MercuryBubbleWall.AddIngredient(ModContent.ItemType<ArqueriteOre>());
			MercuryBubbleWall.AddCondition(Condition.InGraveyard);
			MercuryBubbleWall.AddTile(TileID.WorkBenches);
			MercuryBubbleWall.SortAfter(ArqueriteOre);
			MercuryBubbleWall.Register();

			Recipe ArqueriteOre2 = Recipe.Create(ModContent.ItemType<ArqueriteOre>());
			ArqueriteOre2.AddIngredient(ModContent.ItemType<MercuryBubbleWall>(), 4);
			ArqueriteOre2.AddTile(TileID.WorkBenches);
			ArqueriteOre2.SortAfter(MercuryBubbleWall);
			ArqueriteOre2.Register();

			//Dotted Quicksilver Wall
			Recipe DottedQuicksilverWall = Recipe.Create(ModContent.ItemType<DottedQuicksilverWall>(), 4);
			DottedQuicksilverWall.AddIngredient(ModContent.ItemType<ArqueriteOre>());
			DottedQuicksilverWall.AddCondition(Condition.InGraveyard);
			DottedQuicksilverWall.AddTile(TileID.WorkBenches);
			DottedQuicksilverWall.SortAfter(ArqueriteOre2);
			DottedQuicksilverWall.Register();

			Recipe ArqueriteOre3 = Recipe.Create(ModContent.ItemType<ArqueriteOre>());
			ArqueriteOre3.AddIngredient(ModContent.ItemType<DottedQuicksilverWall>(), 4);
			ArqueriteOre3.AddTile(TileID.WorkBenches);
			ArqueriteOre3.SortAfter(DottedQuicksilverWall);
			ArqueriteOre3.Register();

			//Quartz Bricks
			Recipe QuartzBricks = Recipe.Create(ModContent.ItemType<QuartzBricks>(), 5);
			QuartzBricks.AddIngredient(ItemID.StoneBlock, 5);
			QuartzBricks.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 1);
			QuartzBricks.AddTile(TileID.Furnaces);
			QuartzBricks.SortAfter(ArqueriteOre3);
			QuartzBricks.Register();

			//Quartz Block
			Recipe QuartzBlock = Recipe.Create(ModContent.ItemType<QuartzBlock>());
			QuartzBlock.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 2);
			QuartzBlock.AddTile(TileID.Furnaces);
			QuartzBlock.SortAfter(QuartzBricks);
			QuartzBlock.Register();

			//Quartz Pillar
			Recipe QuartzPillar = Recipe.Create(ModContent.ItemType<QuartzPillar>());
			QuartzPillar.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 2);
			QuartzPillar.AddTile(TileID.Furnaces);
			QuartzPillar.SortAfter(QuartzBlock);
			QuartzPillar.Register();

			//Quartz Brick Wall
			Recipe QuartzBrickWall = Recipe.Create(ModContent.ItemType<QuartzBrickWall>(), 4);
			QuartzBrickWall.AddIngredient(ModContent.ItemType<QuartzBricks>());
			QuartzBrickWall.AddTile(TileID.WorkBenches);
			QuartzBrickWall.SortAfter(QuartzPillar);
			QuartzBrickWall.Register();

			Recipe QuartzBricks2 = Recipe.Create(ModContent.ItemType<QuartzBricks>());
			QuartzBricks2.AddIngredient(ModContent.ItemType<Items.Placeable.QuartzBrickWall>(), 4);
			QuartzBricks2.AddTile(TileID.WorkBenches);
			QuartzBricks2.SortAfter(QuartzBrickWall);
			QuartzBricks2.Register();

			//Quartz Stack Wall
			Recipe QuartzStackWall = Recipe.Create(ModContent.ItemType<QuartzStackWall>(), 4);
			QuartzStackWall.AddIngredient(ModContent.ItemType<Quartz>());
			QuartzStackWall.AddCondition(Condition.InGraveyard);
			QuartzStackWall.AddTile(TileID.WorkBenches);
			QuartzStackWall.SortAfter(QuartzBricks2);
			QuartzStackWall.Register();

			Recipe Quartz = Recipe.Create(ModContent.ItemType<Quartz>());
			Quartz.AddIngredient(ModContent.ItemType<QuartzStackWall>(), 4);
			Quartz.AddTile(TileID.WorkBenches);
			Quartz.SortAfter(QuartzStackWall);
			Quartz.Register();
			#endregion

			//Black Gemspark
			Recipe BlackGemspark = Recipe.Create(ModContent.ItemType<BlackGemspark>(), 20);
			BlackGemspark.AddIngredient(ItemID.Glass, 20);
			BlackGemspark.AddIngredient(ModContent.ItemType<Onyx>(), 1);
			BlackGemspark.AddTile(TileID.WorkBenches);
			BlackGemspark.SortAfterFirstRecipesOf(ItemID.AmberGemsparkBlock);
			BlackGemspark.Register();

			//Black Gemspark Wall
			Recipe BlackGemsparkWall = Recipe.Create(ModContent.ItemType<BlackGemsparkWall>(), 4);
			BlackGemsparkWall.AddIngredient(ModContent.ItemType<BlackGemspark>());
			BlackGemsparkWall.AddTile(TileID.WorkBenches);
			BlackGemsparkWall.SortAfterFirstRecipesOf(ItemID.AmberGemsparkWallOff);
			BlackGemsparkWall.Register();

			Recipe BlackGemspark2 = Recipe.Create(ModContent.ItemType<BlackGemspark>());
			BlackGemspark2.AddIngredient(ModContent.ItemType<Items.Placeable.BlackGemsparkWall>(), 4);
			BlackGemspark2.AddTile(TileID.WorkBenches);
			BlackGemspark2.SortAfter(BlackGemsparkWall);
			BlackGemspark2.Register();

			//Offline Black Gemspark Wall
			Recipe BlackGemsparkWallOffline = Recipe.Create(ModContent.ItemType<BlackGemsparkWallOffline>(), 4);
			BlackGemsparkWallOffline.AddIngredient(ModContent.ItemType<BlackGemspark>());
			BlackGemsparkWallOffline.AddTile(TileID.WorkBenches);
			BlackGemsparkWallOffline.SortAfter(BlackGemspark2);
			BlackGemsparkWallOffline.Register();

			Recipe BlackGemspark3 = Recipe.Create(ModContent.ItemType<BlackGemspark>());
			BlackGemspark3.AddIngredient(ModContent.ItemType<Items.Placeable.BlackGemsparkWallOffline>(), 4);
			BlackGemspark3.AddTile(TileID.WorkBenches);
			BlackGemspark3.SortAfter(BlackGemsparkWall);
			BlackGemspark3.Register();

			//Black Stained Glass
			Recipe BlackStainedGlass = Recipe.Create(ModContent.ItemType<BlackStainedGlass>(), 20);
			BlackStainedGlass.AddIngredient(ItemID.GlassWall, 20);
			BlackStainedGlass.AddIngredient(ModContent.ItemType<Onyx>());
			BlackStainedGlass.AddTile(TileID.WorkBenches);
			BlackStainedGlass.SortAfterFirstRecipesOf(ItemID.OrangeStainedGlass);
			BlackStainedGlass.Register();

			//Black Torch
			Recipe BlackTorch = Recipe.Create(ModContent.ItemType<BlackTorch>(), 10);
			BlackTorch.AddIngredient(ItemID.Torch, 10);
			BlackTorch.AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 1);
			BlackTorch.SortAfterFirstRecipesOf(ItemID.OrangeTorch);
			BlackTorch.Register();

			//Geo Campfire
			Recipe GeoCampfire = Recipe.Create(ModContent.ItemType<GeoCampfire>());
			GeoCampfire.AddRecipeGroup(RecipeGroupID.Wood, 10);
			GeoCampfire.AddIngredient(ModContent.ItemType<GeoTorch>(), 5);
			GeoCampfire.SortAfterFirstRecipesOf(ItemID.DemonCampfire);
			GeoCampfire.Register();

			//Geo Torch
			Recipe GeoTorch = Recipe.Create(ModContent.ItemType<GeoTorch>(), 3);
			GeoTorch.AddIngredient(ItemID.Torch, 3);
			GeoTorch.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 1);
			GeoTorch.SortAfterFirstRecipesOf(ItemID.DemonTorch);
			GeoTorch.Register();

			//Giant Fluorescent Light Bulb
			Recipe GiantFluorescentLightBulb = Recipe.Create(ModContent.ItemType<GiantFluorescentLightBulb>());
			GiantFluorescentLightBulb.AddIngredient(ModContent.ItemType<FluorescentLightBulb>(), 3);
			GiantFluorescentLightBulb.SortAfterFirstRecipesOf(ItemID.VolcanoLarge);
			GiantFluorescentLightBulb.Register();

			//Hanging Shadow Shrub
			Recipe HangingShadowShrub = Recipe.Create(ModContent.ItemType<HangingShadowShrub>());
			HangingShadowShrub.AddIngredient(ItemID.PotSuspended);
			HangingShadowShrub.AddIngredient(ModContent.ItemType<ShadowShrub>());
			HangingShadowShrub.SortAfterFirstRecipesOf(ItemID.PotSuspendedFireblossom);
			HangingShadowShrub.Register();

			#region Quartz Furniture Crafting Group
			//Quartz Chest
			Recipe QuartzChest = Recipe.Create(ModContent.ItemType<QuartzChest>());
			QuartzChest.AddIngredient(ModContent.ItemType<Quartz>(), 6);
			QuartzChest.AddIngredient(ModContent.ItemType<ArqueriteOre>(), 2);
			QuartzChest.AddRecipeGroup(RecipeGroupID.IronBar, 2);
			QuartzChest.AddTile(TileID.WorkBenches);
			QuartzChest.SortAfterFirstRecipesOf(ItemID.ToiletObsidian);
			QuartzChest.Register();

			//Quartz Sink
			Recipe QuartzSink = Recipe.Create(ModContent.ItemType<QuartzSink>());
			QuartzSink.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 4);
			QuartzSink.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 2);
			QuartzSink.AddIngredient(ItemID.WaterBucket, 1);
			QuartzSink.AddTile(TileID.WorkBenches);
			QuartzSink.SortAfter(QuartzChest);
			QuartzSink.Register();

			//Quartz Toilet
			Recipe QuartzToilet = Recipe.Create(ModContent.ItemType<QuartzToilet>());
			QuartzToilet.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 4);
			QuartzToilet.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 2);
			QuartzToilet.AddTile(TileID.Sawmill);
			QuartzToilet.SortAfter(QuartzSink);
			QuartzToilet.Register();

			//Large Quartz
			Recipe LargeCrystal = Recipe.Create(ModContent.ItemType<LargeCrystal>());
			LargeCrystal.AddIngredient(ModContent.ItemType<Quartz>(), 4);
			LargeCrystal.AddCondition(Condition.InGraveyard);
			LargeCrystal.AddTile(TileID.WorkBenches);
			LargeCrystal.SortAfter(QuartzToilet);
			LargeCrystal.Register();
			#endregion

			//Quicksilver Dropper
			Recipe MagicQuicksilverDropper = Recipe.Create(ModContent.ItemType<MagicQuicksilverDropper>());
			MagicQuicksilverDropper.AddIngredient(ItemID.EmptyDropper);
			MagicQuicksilverDropper.AddTile(TileID.CrystalBall);
			MagicQuicksilverDropper.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"), () => Worldgen.TheDepthsWorldGen.isWorldDepths && Main.LocalPlayer.adjLava);
			MagicQuicksilverDropper.SortAfterFirstRecipesOf(ItemID.MagicLavaDropper);
			MagicQuicksilverDropper.Register();

			//Mercury Moss Brick
			Recipe MercuryMossBricks = Recipe.Create(ModContent.ItemType<MercuryMossBricks>(), 10);
			MercuryMossBricks.AddIngredient(ModContent.ItemType<MercuryMoss>(), 1);
			MercuryMossBricks.AddIngredient(ItemID.ClayBlock, 10);
			MercuryMossBricks.AddTile(TileID.Furnaces);
			MercuryMossBricks.SortAfterFirstRecipesOf(ItemID.VioletMossBlock);
			MercuryMossBricks.Register();

			//Mercury Moss Brick Wall
			Recipe MercuryMossBrickWall = Recipe.Create(ModContent.ItemType<MercuryMossBrickWall>(), 4);
			MercuryMossBrickWall.AddIngredient(ModContent.ItemType<MercuryMossBricks>());
			MercuryMossBrickWall.AddTile(TileID.WorkBenches);
			MercuryMossBrickWall.SortAfterFirstRecipesOf(ItemID.VioletMossBlockWall);
			MercuryMossBrickWall.Register();

			Recipe MercuryMossBricks2 = Recipe.Create(ModContent.ItemType<MercuryMossBricks>());
			MercuryMossBricks2.AddIngredient(ModContent.ItemType<MercuryMossBrickWall>(), 4);
			MercuryMossBricks2.AddTile(TileID.WorkBenches);
			MercuryMossBricks2.SortAfter(MercuryMossBrickWall);
			MercuryMossBricks2.Register();

			//Nightmarewood Fence
			Recipe NightwoodFence = Recipe.Create(ModContent.ItemType<NightwoodFence>(), 4);
			NightwoodFence.AddIngredient(ModContent.ItemType<NightWood>());
			NightwoodFence.AddTile(TileID.WorkBenches);
			NightwoodFence.SortAfterFirstRecipesOf(ItemID.AshWoodFence);
			NightwoodFence.Register();

			//Petrifiedwood Fence
			Recipe PetrifiedWoodFence = Recipe.Create(ModContent.ItemType<PetrifiedWoodFence>(), 4);
			PetrifiedWoodFence.AddIngredient(ModContent.ItemType<PetrifiedWood>());
			PetrifiedWoodFence.AddTile(TileID.WorkBenches);
			PetrifiedWoodFence.SortAfter(NightwoodFence);
			PetrifiedWoodFence.Register();

			//Nightmarewood Wall
			Recipe NightwoodWall = Recipe.Create(ModContent.ItemType<NightwoodWall>(), 4);
			NightwoodWall.AddIngredient(ModContent.ItemType<NightWood>());
			NightwoodWall.AddTile(TileID.WorkBenches);
			NightwoodWall.SortAfterFirstRecipesOf(ItemID.AshWoodWall);
			NightwoodWall.Register();

			Recipe NightWood = Recipe.Create(ModContent.ItemType<NightWood>());
			NightWood.AddIngredient(ModContent.ItemType<NightwoodWall>(), 4);
			NightWood.AddTile(TileID.WorkBenches);
			NightWood.SortAfter(NightwoodWall);
			NightWood.Register();

			//Petrifiedwood Wall
			Recipe PetrifiedWoodWall = Recipe.Create(ModContent.ItemType<PetrifiedWoodWall>(), 4);
			PetrifiedWoodWall.AddIngredient(ModContent.ItemType<PetrifiedWood>());
			PetrifiedWoodWall.AddTile(TileID.WorkBenches);
			PetrifiedWoodWall.SortAfter(NightWood);
			PetrifiedWoodWall.Register();

			Recipe PetrifiedWood = Recipe.Create(ModContent.ItemType<PetrifiedWood>());
			PetrifiedWood.AddIngredient(ModContent.ItemType<PetrifiedWoodWall>(), 4);
			PetrifiedWood.AddTile(TileID.WorkBenches);
			PetrifiedWood.SortAfter(PetrifiedWoodWall);
			PetrifiedWood.Register();

			//Onyx Bunny Cage
			Recipe OnyxBunnyCage = Recipe.Create(ModContent.ItemType<OnyxBunnyCage>());
			OnyxBunnyCage.AddIngredient(ModContent.ItemType<OnyxBunny>());
			OnyxBunnyCage.AddIngredient(ItemID.Terrarium, 1);
			OnyxBunnyCage.SortAfterFirstRecipesOf(ItemID.AmberBunnyCage);
			OnyxBunnyCage.Register();

			//Onyx Squirel Cage
			Recipe OnyxSquirrelCage = Recipe.Create(ModContent.ItemType<OnyxSquirrelCage>());
			OnyxSquirrelCage.AddIngredient(ModContent.ItemType<OnyxSquirrel>());
			OnyxSquirrelCage.AddIngredient(ItemID.Terrarium, 1);
			OnyxSquirrelCage.SortAfterFirstRecipesOf(ItemID.AmberSquirrelCage);
			OnyxSquirrelCage.Register();

			//Onyx Gemcorn
			Recipe OnyxGemcorn = Recipe.Create(ModContent.ItemType<OnyxGemcorn>());
			OnyxGemcorn.AddIngredient(ItemID.Acorn);
			OnyxGemcorn.AddIngredient(ModContent.ItemType<Onyx>());
			OnyxGemcorn.SortAfterFirstRecipesOf(ItemID.GemTreeAmberSeed);
			OnyxGemcorn.Register();

			//Onyx Gem Lock
			Recipe OnyxGemLock = Recipe.Create(ModContent.ItemType<OnyxGemLock>());
			OnyxGemLock.AddIngredient(ModContent.ItemType<Onyx>(), 5);
			OnyxGemLock.AddIngredient(ItemID.StoneBlock, 10);
			OnyxGemLock.AddTile(TileID.HeavyWorkBench);
			OnyxGemLock.SortAfterFirstRecipesOf(ItemID.GemLockAmber);
			OnyxGemLock.Register();

			//Onyx Stone Wall
			Recipe OnyxStoneWall = Recipe.Create(ModContent.ItemType<OnyxStoneWall>(), 4);
			OnyxStoneWall.AddIngredient(ModContent.ItemType<OnyxStoneBlock>());
			OnyxStoneWall.AddCondition(Condition.InGraveyard);
			OnyxStoneWall.AddTile(TileID.WorkBenches);
			OnyxStoneWall.SortAfterFirstRecipesOf(ItemID.AmberStoneWallEcho);
			OnyxStoneWall.Register();

			Recipe OnyxStoneBlock2 = Recipe.Create(ModContent.ItemType<OnyxStoneBlock>());
			OnyxStoneBlock2.AddIngredient(ModContent.ItemType<OnyxStoneWall>(), 4);
			OnyxStoneBlock2.AddTile(TileID.WorkBenches);
			OnyxStoneBlock2.DisableDecraft();
			OnyxStoneBlock2.SortAfter(OnyxStoneWall);
			OnyxStoneBlock2.Register();

			//Quicksilver Sensor
			Recipe QuicksilverSensor = Recipe.Create(ModContent.ItemType<QuicksilverSensor>());
			QuicksilverSensor.AddIngredient(ItemID.Cog, 5);
			QuicksilverSensor.AddIngredient(ModContent.ItemType<MagicQuicksilverDropper>());
			QuicksilverSensor.AddIngredient(ItemID.Wire);
			QuicksilverSensor.AddTile(TileID.MythrilAnvil);
			QuicksilverSensor.SortAfterFirstRecipesOf(ItemID.LogicSensor_Lava);
			QuicksilverSensor.Register();

			//Shale Bricks
			Recipe ShaleBricks = Recipe.Create(ModContent.ItemType<ShaleBricks>());
			ShaleBricks.AddIngredient(ModContent.ItemType<Items.Placeable.ShaleBlock>(), 2);
			ShaleBricks.AddTile(TileID.Furnaces);
			ShaleBricks.SortAfterFirstRecipesOf(ItemID.IridescentBrickWall);
			ShaleBricks.Register();

			//Shale Wall
			Recipe ShaleWall = Recipe.Create(ModContent.ItemType<ShaleWall>(), 4);
			ShaleWall.AddIngredient(ModContent.ItemType<ShaleBlock>());
			ShaleWall.AddCondition(Condition.InGraveyard);
			ShaleWall.AddTile(TileID.WorkBenches);
			ShaleWall.SortAfter(ShaleBricks);
			ShaleWall.Register();

			//Silverfall Block
			Recipe SilverfallBlock = Recipe.Create(ModContent.ItemType<SilverfallBlock>());
			SilverfallBlock.AddIngredient(ItemID.Glass);
			SilverfallBlock.AddTile(TileID.CrystalBall);
			SilverfallBlock.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"), () => Worldgen.TheDepthsWorldGen.isWorldDepths && Main.LocalPlayer.adjLava);
			SilverfallBlock.SortAfterFirstRecipesOf(ItemID.LavafallWall);
			SilverfallBlock.Register();

			//Silverfall Wall
			Recipe SilverfallWall = Recipe.Create(ModContent.ItemType<SilverfallWall>(), 4);
			SilverfallWall.AddIngredient(ModContent.ItemType<SilverfallBlock>());
			SilverfallWall.AddTile(TileID.WorkBenches);
			SilverfallWall.SortAfter(SilverfallBlock);
			SilverfallWall.Register();

			Recipe SilverfallBlock2 = Recipe.Create(ModContent.ItemType<SilverfallBlock>());
			SilverfallBlock2.AddIngredient(ModContent.ItemType<Items.Placeable.SilverfallWall>(), 4);
			SilverfallBlock2.AddTile(TileID.WorkBenches);
			SilverfallBlock2.SortAfter(SilverfallWall);
			SilverfallBlock2.Register();

			//Nightmarewood Platform
			Recipe NightwoodPlatform = Recipe.Create(ModContent.ItemType<NightwoodPlatform>(), 2);
			NightwoodPlatform.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 1);
			NightwoodPlatform.SortAfterFirstRecipesOf(ItemID.EchoPlatform);
			NightwoodPlatform.Register();

			Recipe NightWood2 = Recipe.Create(ModContent.ItemType<NightWood>());
			NightWood2.AddIngredient(ModContent.ItemType<Items.Placeable.Furniture.NightwoodPlatform>(), 2);
			NightWood2.SortAfter(NightwoodPlatform);
			NightWood2.Register();

			//Petrifiedwood Platform
			Recipe PetrifiedWoodPlatform = Recipe.Create(ModContent.ItemType<PetrifiedWoodPlatform>(), 2);
			PetrifiedWoodPlatform.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 1);
			PetrifiedWoodPlatform.SortAfter(NightwoodPlatform);
			PetrifiedWoodPlatform.Register();

			Recipe PetrifiedWood2 = Recipe.Create(ModContent.ItemType<PetrifiedWood>());
			PetrifiedWood2.AddIngredient(ModContent.ItemType<Items.Placeable.Furniture.PetrifiedWoodPlatform>(), 2);
			PetrifiedWood2.SortAfter(PetrifiedWoodPlatform);
			PetrifiedWood2.Register();

			//Quartz Platform
			Recipe QuartzPlatform = Recipe.Create(ModContent.ItemType<QuartzPlatform>(), 2);
			QuartzPlatform.AddIngredient(ModContent.ItemType<Items.Placeable.QuartzBricks>(), 1);
			QuartzPlatform.SortAfterFirstRecipesOf(ItemID.ObsidianPlatform);
			QuartzPlatform.Register();

			Recipe QuartzBricks3 = Recipe.Create(ModContent.ItemType<QuartzBricks>());
			QuartzBricks3.AddIngredient(ModContent.ItemType<Items.Placeable.Furniture.QuartzPlatform>(), 2);
			QuartzBricks3.SortAfter(QuartzPlatform);
			QuartzBricks3.Register();

			//Trapped Nightwood Chest
			Recipe NightwoodTrappedChest = Recipe.Create(ModContent.ItemType<NightwoodTrappedChest>());
			NightwoodTrappedChest.AddIngredient(ModContent.ItemType<NightwoodChest>());
			NightwoodTrappedChest.AddIngredient(ItemID.Wire, 10);
			NightwoodTrappedChest.AddTile(TileID.HeavyWorkBench);
			NightwoodTrappedChest.SortAfterFirstRecipesOf(ItemID.Fake_AshWoodChest);
			NightwoodTrappedChest.Register();

			//Trapped Petrified Chest
			Recipe PetrifiedWoodTrappedChest = Recipe.Create(ModContent.ItemType<PetrifiedWoodTrappedChest>());
			PetrifiedWoodTrappedChest.AddIngredient(ModContent.ItemType<PetrifiedWoodChest>());
			PetrifiedWoodTrappedChest.AddIngredient(ItemID.Wire, 10);
			PetrifiedWoodTrappedChest.AddTile(TileID.HeavyWorkBench);
			PetrifiedWoodTrappedChest.SortAfter(NightwoodTrappedChest);
			PetrifiedWoodTrappedChest.Register();

			//Trapped Quartz Chest
			Recipe QuartzTrappedChest = Recipe.Create(ModContent.ItemType<QuartzTrappedChest>());
			QuartzTrappedChest.AddIngredient(ModContent.ItemType<QuartzChest>());
			QuartzTrappedChest.AddIngredient(ItemID.Wire, 10);
			QuartzTrappedChest.AddTile(TileID.HeavyWorkBench);
			QuartzTrappedChest.SortAfterFirstRecipesOf(ItemID.Fake_ObsidianChest);
			QuartzTrappedChest.Register();
		}

		private static void AddNightmareWoodFurnitureArmorAndItems(out Recipe lastrecipe)
		{
			//Nightmarewood Helmet
			Recipe NightwoodHelmet = Recipe.Create(ModContent.ItemType<NightwoodHelmet>());
			NightwoodHelmet.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 20);
			NightwoodHelmet.AddTile(TileID.WorkBenches);
			NightwoodHelmet.SortAfterFirstRecipesOf(ItemID.AshWoodToilet);
			NightwoodHelmet.Register();

			//Nightmarewood Chestplate
			Recipe NightwoodBreastplate = Recipe.Create(ModContent.ItemType<NightwoodBreastplate>());
			NightwoodBreastplate.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 30);
			NightwoodBreastplate.AddTile(TileID.WorkBenches);
			NightwoodBreastplate.SortAfter(NightwoodHelmet);
			NightwoodBreastplate.Register();

			//Nightmarewood Greaves
			Recipe NightwoodGreaves = Recipe.Create(ModContent.ItemType<NightwoodGreaves>());
			NightwoodGreaves.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 25);
			NightwoodGreaves.AddTile(TileID.WorkBenches);
			NightwoodGreaves.SortAfter(NightwoodBreastplate);
			NightwoodGreaves.Register();

			//Nightmarewood Sword
			Recipe NightwoodSword = Recipe.Create(ModContent.ItemType<NightwoodSword>());
			NightwoodSword.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 7);
			NightwoodSword.AddTile(TileID.WorkBenches);
			NightwoodSword.SortAfter(NightwoodGreaves);
			NightwoodSword.Register();

			//Nightmarewood Hammer
			Recipe NightwoodHammer = Recipe.Create(ModContent.ItemType<NightwoodHammer>());
			NightwoodHammer.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 8);
			NightwoodHammer.AddTile(TileID.WorkBenches);
			NightwoodHammer.SortAfter(NightwoodSword);
			NightwoodHammer.Register();

			//Nightmarewood Bow
			Recipe NightwoodBow = Recipe.Create(ModContent.ItemType<NightwoodBow>());
			NightwoodBow.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), ItemID.Torch);
			NightwoodBow.AddTile(TileID.WorkBenches);
			NightwoodBow.SortAfter(NightwoodHammer);
			NightwoodBow.Register();

			//Nightmarewood Bath
			Recipe NightwoodBath = Recipe.Create(ModContent.ItemType<NightwoodBath>());
			NightwoodBath.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 14);
			NightwoodBath.AddTile(TileID.Sawmill);
			NightwoodBath.SortAfter(NightwoodBow);
			NightwoodBath.Register();

			//Nightmarewood Bed
			Recipe NightwoodBed = Recipe.Create(ModContent.ItemType<NightwoodBed>());
			NightwoodBed.AddIngredient(ModContent.ItemType<NightWood>(), 15);
			NightwoodBed.AddIngredient(ItemID.Silk, 5);
			NightwoodBed.AddTile(TileID.Sawmill);
			NightwoodBed.SortAfter(NightwoodBath);
			NightwoodBed.Register();

			//Nightmarewood Bookcase
			Recipe NightwoodBookcase = Recipe.Create(ModContent.ItemType<NightwoodBookcase>());
			NightwoodBookcase.AddIngredient(ModContent.ItemType<NightWood>(), 20);
			NightwoodBookcase.AddIngredient(ItemID.Book, 10);
			NightwoodBookcase.AddTile(TileID.Sawmill);
			NightwoodBookcase.SortAfter(NightwoodBed);
			NightwoodBookcase.Register();

			//Nightmarewood Dresser
			Recipe NightwoodDresser = Recipe.Create(ModContent.ItemType<NightwoodDresser>());
			NightwoodDresser.AddIngredient(ModContent.ItemType<NightWood>(), 16);
			NightwoodDresser.AddTile(TileID.Sawmill);
			NightwoodDresser.SortAfter(NightwoodBookcase);
			NightwoodDresser.Register();

			//Nightmarewood Candelabra
			Recipe NightwoodCandelabra = Recipe.Create(ModContent.ItemType<NightwoodCandelabra>());
			NightwoodCandelabra.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 5);
			NightwoodCandelabra.AddIngredient(ItemID.Torch, 3);
			NightwoodCandelabra.AddTile(TileID.WorkBenches);
			NightwoodCandelabra.SortAfter(NightwoodDresser);
			NightwoodCandelabra.Register();

			//Nightmarewood Candle
			Recipe NightwoodCandle = Recipe.Create(ModContent.ItemType<NightwoodCandle>());
			NightwoodCandle.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 4);
			NightwoodCandle.AddIngredient(ItemID.Torch, 1);
			NightwoodCandle.AddTile(TileID.WorkBenches);
			NightwoodCandle.SortAfter(NightwoodCandelabra);
			NightwoodCandle.Register();

			//Nightmarewood Chair
			Recipe NightwoodChair = Recipe.Create(ModContent.ItemType<NightwoodChair>());
			NightwoodChair.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 4);
			NightwoodChair.AddTile(TileID.WorkBenches);
			NightwoodChair.SortAfter(NightwoodCandle);
			NightwoodChair.Register();

			//Nightmarewood Chandelier
			Recipe NightwoodChandelier = Recipe.Create(ModContent.ItemType<NightwoodChandelier>());
			NightwoodChandelier.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 4);
			NightwoodChandelier.AddIngredient(ItemID.Torch, 4);
			NightwoodChandelier.AddIngredient(ItemID.Chain, 1);
			NightwoodChandelier.AddTile(TileID.Anvils);
			NightwoodChandelier.SortAfter(NightwoodChair);
			NightwoodChandelier.Register();

			//Nightmarewood Chest
			Recipe NightwoodChest = Recipe.Create(ModContent.ItemType<NightwoodChest>());
			NightwoodChest.AddIngredient(ModContent.ItemType<NightWood>(), 8);
			NightwoodChest.AddRecipeGroup(RecipeGroupID.IronBar, 2);
			NightwoodChest.AddTile(TileID.WorkBenches);
			NightwoodChest.SortAfter(NightwoodChandelier);
			NightwoodChest.Register();

			//Nightmarewood Clock
			Recipe NightwoodClock = Recipe.Create(ModContent.ItemType<NightwoodClock>());
			NightwoodClock.AddIngredient(ModContent.ItemType<NightWood>(), 10);
			NightwoodClock.AddRecipeGroup(RecipeGroupID.IronBar, 3);
			NightwoodClock.AddIngredient(ItemID.Glass, 6);
			NightwoodClock.AddTile(TileID.Sawmill);
			NightwoodClock.SortAfter(NightwoodChest);
			NightwoodClock.Register();

			//Nightmarewood Door
			Recipe NightwoodDoor = Recipe.Create(ModContent.ItemType<NightwoodDoor>());
			NightwoodDoor.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 6);
			NightwoodDoor.AddTile(TileID.WorkBenches);
			NightwoodDoor.SortAfter(NightwoodClock);
			NightwoodDoor.Register();

			//Nightmarewood Lamp
			Recipe NightwoodLamp = Recipe.Create(ModContent.ItemType<NightwoodLamp>());
			NightwoodLamp.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 3);
			NightwoodLamp.AddIngredient(ItemID.Torch, 1);
			NightwoodLamp.AddTile(TileID.WorkBenches);
			NightwoodLamp.SortAfter(NightwoodDoor);
			NightwoodLamp.Register();

			//Nightmarewood Lantern
			Recipe NightwoodLantern = Recipe.Create(ModContent.ItemType<NightwoodLantern>());
			NightwoodLantern.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 6);
			NightwoodLantern.AddIngredient(ItemID.Torch, 1);
			NightwoodLantern.AddTile(TileID.WorkBenches);
			NightwoodLantern.SortAfter(NightwoodLamp);
			NightwoodLantern.Register();

			//Nightmarewood Piano
			Recipe NightwoodPiano = Recipe.Create(ModContent.ItemType<NightwoodPiano>());
			NightwoodPiano.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 15);
			NightwoodPiano.AddIngredient(ItemID.Bone, 4);
			NightwoodPiano.AddIngredient(ItemID.Book, 1);
			NightwoodPiano.AddTile(TileID.Sawmill);
			NightwoodPiano.SortAfter(NightwoodLantern);
			NightwoodPiano.Register();

			//Nightmarewood Sink
			Recipe NightwoodSink = Recipe.Create(ModContent.ItemType<NightwoodSink>());
			NightwoodSink.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 6);
			NightwoodSink.AddIngredient(ItemID.WaterBucket, 1);
			NightwoodSink.AddTile(TileID.WorkBenches);
			NightwoodSink.SortAfter(NightwoodPiano);
			NightwoodSink.Register();

			//Nightmarewood Sofa
			Recipe NightwoodSofa = Recipe.Create(ModContent.ItemType<NightwoodSofa>());
			NightwoodSofa.AddIngredient(ModContent.ItemType<NightWood>(), 5);
			NightwoodSofa.AddIngredient(ItemID.Silk, 2);
			NightwoodSofa.AddTile(TileID.Sawmill);
			NightwoodSofa.SortAfter(NightwoodSink);
			NightwoodSofa.Register();

			//Nightmarewood Table
			Recipe NightwoodTable = Recipe.Create(ModContent.ItemType<NightwoodTable>());
			NightwoodTable.AddIngredient(ModContent.ItemType<NightWood>(), 8);
			NightwoodTable.AddTile(TileID.WorkBenches);
			NightwoodTable.SortAfter(NightwoodSofa);
			NightwoodTable.Register();

			//Nightmarewood Workbench
			Recipe NightwoodWorkbench = Recipe.Create(ModContent.ItemType<NightwoodWorkbench>());
			NightwoodWorkbench.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 10);
			NightwoodWorkbench.SortAfter(NightwoodTable);
			NightwoodWorkbench.Register();

			//Nightmarewood Toilet
			Recipe NightwoodToilet = Recipe.Create(ModContent.ItemType<NightwoodToilet>());
			NightwoodToilet.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 6);
			NightwoodToilet.AddTile(TileID.Sawmill);
			NightwoodToilet.SortAfter(NightwoodWorkbench);
			NightwoodToilet.Register();

			lastrecipe = NightwoodToilet;
		}

		private static void AddPetrifiedWoodFurnitureArmorAndItems(Recipe recipeAfter)
		{
			//Petrifiedwood Helmet
			Recipe PetrifiedWoodHelmet = Recipe.Create(ModContent.ItemType<PetrifiedWoodHelmet>());
			PetrifiedWoodHelmet.AddIngredient(ModContent.ItemType<Items.Placeable.PetrifiedWood>(), 20);
			PetrifiedWoodHelmet.AddTile(TileID.WorkBenches);
			PetrifiedWoodHelmet.SortAfter(recipeAfter);
			PetrifiedWoodHelmet.Register();

			//Petrifiedwood Chestplate
			Recipe PetrifiedWoodBreastplate = Recipe.Create(ModContent.ItemType<PetrifiedWoodBreastplate>());
			PetrifiedWoodBreastplate.AddIngredient(ModContent.ItemType<Items.Placeable.PetrifiedWood>(), 30);
			PetrifiedWoodBreastplate.AddTile(TileID.WorkBenches);
			PetrifiedWoodBreastplate.SortAfter(PetrifiedWoodHelmet);
			PetrifiedWoodBreastplate.Register();

			//Petrifiedwood Greaves
			Recipe PetrifiedWoodGreaves = Recipe.Create(ModContent.ItemType<PetrifiedWoodGreaves>());
			PetrifiedWoodGreaves.AddIngredient(ModContent.ItemType<Items.Placeable.PetrifiedWood>(), 25);
			PetrifiedWoodGreaves.AddTile(TileID.WorkBenches);
			PetrifiedWoodGreaves.SortAfter(PetrifiedWoodBreastplate);
			PetrifiedWoodGreaves.Register();

			//Petrifiedwood Sword
			Recipe PetrifiedWoodSword = Recipe.Create(ModContent.ItemType<PetrifiedWoodSword>());
			PetrifiedWoodSword.AddIngredient(ModContent.ItemType<Items.Placeable.PetrifiedWood>(), 7);
			PetrifiedWoodSword.AddTile(TileID.WorkBenches);
			PetrifiedWoodSword.SortAfter(PetrifiedWoodGreaves);
			PetrifiedWoodSword.Register();

			//Petrifiedwood Hammer
			Recipe PetrifiedWoodHammer = Recipe.Create(ModContent.ItemType<PetrifiedWoodHammer>());
			PetrifiedWoodHammer.AddIngredient(ModContent.ItemType<Items.Placeable.PetrifiedWood>(), 8);
			PetrifiedWoodHammer.AddTile(TileID.WorkBenches);
			PetrifiedWoodHammer.SortAfter(PetrifiedWoodSword);
			PetrifiedWoodHammer.Register();

			//Petrifiedwood Bow
			Recipe PetrifiedWoodBow = Recipe.Create(ModContent.ItemType<PetrifiedWoodBow>());
			PetrifiedWoodBow.AddIngredient(ModContent.ItemType<Items.Placeable.PetrifiedWood>(), 8);
			PetrifiedWoodBow.AddTile(TileID.WorkBenches);
			PetrifiedWoodBow.SortAfter(PetrifiedWoodHammer);
			PetrifiedWoodBow.Register();

			//Petrifiedwood Bath
			Recipe PetrifiedWoodBath = Recipe.Create(ModContent.ItemType<PetrifiedWoodBath>());
			PetrifiedWoodBath.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 14);
			PetrifiedWoodBath.AddTile(TileID.Sawmill);
			PetrifiedWoodBath.SortAfter(PetrifiedWoodBow);
			PetrifiedWoodBath.Register();

			//Petrifiedwood Bed
			Recipe PetrifiedWoodBed = Recipe.Create(ModContent.ItemType<PetrifiedWoodBed>());
			PetrifiedWoodBed.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 15);
			PetrifiedWoodBed.AddIngredient(ItemID.Silk, 5);
			PetrifiedWoodBed.AddTile(TileID.Sawmill);
			PetrifiedWoodBed.SortAfter(PetrifiedWoodBath);
			PetrifiedWoodBed.Register();

			//Petrifiedwood Bookcase
			Recipe PetrifiedWoodBookcase = Recipe.Create(ModContent.ItemType<PetrifiedWoodBookcase>());
			PetrifiedWoodBookcase.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 20);
			PetrifiedWoodBookcase.AddIngredient(ItemID.Book, 10);
			PetrifiedWoodBookcase.AddTile(TileID.Sawmill);
			PetrifiedWoodBookcase.SortAfter(PetrifiedWoodBed);
			PetrifiedWoodBookcase.Register();

			//Petrifiedwood Dresser
			Recipe PetrifiedWoodDresser = Recipe.Create(ModContent.ItemType<PetrifiedWoodDresser>());
			PetrifiedWoodDresser.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 16);
			PetrifiedWoodDresser.AddTile(TileID.Sawmill);
			PetrifiedWoodDresser.SortAfter(PetrifiedWoodBookcase);
			PetrifiedWoodDresser.Register();

			//Petrifiedwood Candelabra
			Recipe PetrifiedWoodCandelabra = Recipe.Create(ModContent.ItemType<PetrifiedWoodCandelabra>());
			PetrifiedWoodCandelabra.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 5);
			PetrifiedWoodCandelabra.AddIngredient(ItemID.Torch, 3);
			PetrifiedWoodCandelabra.AddTile(TileID.WorkBenches);
			PetrifiedWoodCandelabra.SortAfter(PetrifiedWoodDresser);
			PetrifiedWoodCandelabra.Register();

			//Petrifiedwood Candle
			Recipe PetrifiedWoodCandle = Recipe.Create(ModContent.ItemType<PetrifiedWoodCandle>());
			PetrifiedWoodCandle.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 4);
			PetrifiedWoodCandle.AddIngredient(ItemID.Torch, 1);
			PetrifiedWoodCandle.AddTile(TileID.WorkBenches);
			PetrifiedWoodCandle.SortAfter(PetrifiedWoodCandelabra);
			PetrifiedWoodCandle.Register();

			//Petrifiedwood Chair
			Recipe PetrifiedWoodChair = Recipe.Create(ModContent.ItemType<PetrifiedWoodChair>());
			PetrifiedWoodChair.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 4);
			PetrifiedWoodChair.AddTile(TileID.WorkBenches);
			PetrifiedWoodChair.SortAfter(PetrifiedWoodCandle);
			PetrifiedWoodChair.Register();

			//Petrifiedwood Chandelier
			Recipe PetrifiedWoodChandelier = Recipe.Create(ModContent.ItemType<PetrifiedWoodChandelier>());
			PetrifiedWoodChandelier.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 4);
			PetrifiedWoodChandelier.AddIngredient(ItemID.Torch, 4);
			PetrifiedWoodChandelier.AddIngredient(ItemID.Chain, 1);
			PetrifiedWoodChandelier.AddTile(TileID.Anvils);
			PetrifiedWoodChandelier.SortAfter(PetrifiedWoodChair);
			PetrifiedWoodChandelier.Register();

			//Petrifiedwood Chest
			Recipe PetrifiedWoodChest = Recipe.Create(ModContent.ItemType<PetrifiedWoodChest>());
			PetrifiedWoodChest.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 8);
			PetrifiedWoodChest.AddRecipeGroup(RecipeGroupID.IronBar, 2);
			PetrifiedWoodChest.AddTile(TileID.WorkBenches);
			PetrifiedWoodChest.SortAfter(PetrifiedWoodChandelier);
			PetrifiedWoodChest.Register();

			//Petrifiedwood Clock
			Recipe PetrifiedWoodClock = Recipe.Create(ModContent.ItemType<PetrifiedWoodClock>());
			PetrifiedWoodClock.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 10);
			PetrifiedWoodClock.AddRecipeGroup(RecipeGroupID.IronBar, 3);
			PetrifiedWoodClock.AddIngredient(ItemID.Glass, 6);
			PetrifiedWoodClock.AddTile(TileID.Sawmill);
			PetrifiedWoodClock.SortAfter(PetrifiedWoodChest);
			PetrifiedWoodClock.Register();

			//Petrifiedwood Door
			Recipe PetrifiedWoodDoor = Recipe.Create(ModContent.ItemType<PetrifiedWoodDoor>());
			PetrifiedWoodDoor.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 6);
			PetrifiedWoodDoor.AddTile(TileID.WorkBenches);
			PetrifiedWoodDoor.SortAfter(PetrifiedWoodClock);
			PetrifiedWoodDoor.Register();

			//Petrifiedwood Lamp
			Recipe PetrifiedWoodLamp = Recipe.Create(ModContent.ItemType<PetrifiedWoodLamp>());
			PetrifiedWoodLamp.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 3);
			PetrifiedWoodLamp.AddIngredient(ItemID.Torch, 1);
			PetrifiedWoodLamp.AddTile(TileID.WorkBenches);
			PetrifiedWoodLamp.SortAfter(PetrifiedWoodDoor);
			PetrifiedWoodLamp.Register();

			//Petrifiedwood Lantern
			Recipe PetrifiedWoodLantern = Recipe.Create(ModContent.ItemType<PetrifiedWoodLantern>());
			PetrifiedWoodLantern.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 6);
			PetrifiedWoodLantern.AddIngredient(ItemID.Torch, 1);
			PetrifiedWoodLantern.AddTile(TileID.WorkBenches);
			PetrifiedWoodLantern.SortAfter(PetrifiedWoodLamp);
			PetrifiedWoodLantern.Register();

			//Petrifiedwood Piano
			Recipe PetrifiedWoodPiano = Recipe.Create(ModContent.ItemType<PetrifiedWoodPiano>());
			PetrifiedWoodPiano.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 15);
			PetrifiedWoodPiano.AddIngredient(ItemID.Bone, 4);
			PetrifiedWoodPiano.AddIngredient(ItemID.Book, 1);
			PetrifiedWoodPiano.AddTile(TileID.Sawmill);
			PetrifiedWoodPiano.SortAfter(PetrifiedWoodLantern);
			PetrifiedWoodPiano.Register();

			//Petrifiedwood Sink
			Recipe PetrifiedWoodSink = Recipe.Create(ModContent.ItemType<PetrifiedWoodSink>());
			PetrifiedWoodSink.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 6);
			PetrifiedWoodSink.AddIngredient(ItemID.WaterBucket, 1);
			PetrifiedWoodSink.AddTile(TileID.WorkBenches);
			PetrifiedWoodSink.SortAfter(PetrifiedWoodPiano);
			PetrifiedWoodSink.Register();

			//Petrifiedwood Sofa
			Recipe PetrifiedWoodSofa = Recipe.Create(ModContent.ItemType<PetrifiedWoodSofa>());
			PetrifiedWoodSofa.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 5);
			PetrifiedWoodSofa.AddIngredient(ItemID.Silk, 2);
			PetrifiedWoodSofa.AddTile(TileID.Sawmill);
			PetrifiedWoodSofa.SortAfter(PetrifiedWoodSink);
			PetrifiedWoodSofa.Register();

			//Petrifiedwood Table
			Recipe PetrifiedWoodTable = Recipe.Create(ModContent.ItemType<PetrifiedWoodTable>());
			PetrifiedWoodTable.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 8);
			PetrifiedWoodTable.AddTile(TileID.WorkBenches);
			PetrifiedWoodTable.SortAfter(PetrifiedWoodSofa);
			PetrifiedWoodTable.Register();

			//Petrifiedwood Workbench
			Recipe PetrifiedWoodWorkbench = Recipe.Create(ModContent.ItemType<PetrifiedWoodWorkbench>());
			PetrifiedWoodWorkbench.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 10);
			PetrifiedWoodWorkbench.SortAfter(PetrifiedWoodTable);
			PetrifiedWoodWorkbench.Register();

			//Petrifiedwood Toilet
			Recipe PetrifiedWoodToilet = Recipe.Create(ModContent.ItemType<PetrifiedWoodToilet>());
			PetrifiedWoodToilet.AddIngredient(ModContent.ItemType<PetrifiedWood>(), 6);
			PetrifiedWoodToilet.AddTile(TileID.Sawmill);
			PetrifiedWoodToilet.SortAfter(PetrifiedWoodWorkbench);
			PetrifiedWoodToilet.Register();
		}

		private static void AddAndReplaceCoreBuilder<TConf>(int hall, Recipe coreBuilderRecipe) where TConf : ModItem
		{
			Recipe recipe = Recipe.Create(hall);
			recipe.AddIngredient(ContentInstance<TConf>.Instance.Type);
			recipe.AddTile(ModContent.TileType<Tiles.CoreBuilderTile>());
			recipe.DisableDecraft();
			recipe.SortAfter(coreBuilderRecipe);
			recipe.Register();
			recipe = Recipe.Create(ContentInstance<TConf>.Instance.Type);
			recipe.AddIngredient(hall);
			recipe.AddTile(ModContent.TileType<Tiles.CoreBuilderTile>());
			recipe.DisableDecraft();
			recipe.SortAfter(coreBuilderRecipe);
			recipe.Register();
		}
	}
}
