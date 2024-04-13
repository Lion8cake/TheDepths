using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.Skies;
using Terraria.GameContent.Skies.CreditsRoll;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Light;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;
using TheDepths.Dusts;
using TheDepths.Hooks;
using TheDepths.Items;
using TheDepths.Items.Accessories;
using TheDepths.Items.Weapons;
using TheDepths.Tiles.Furniture;
using TheDepths.Worldgen;
using static Terraria.Graphics.FinalFractalHelper;
using Terraria.ObjectData;
using TheDepths.Tiles;

namespace TheDepths
{
	public class TheDepths : Mod
	{
		public static Mod mod;
		public static List<int> livingFireBlockList;
		private static float[] UWBGAlpha = new float[2];
		private static int UWBGStyle;
		private Color[] UWBGBottomColor = new Color[2];
		private Asset<Texture2D>[][] UWBGTexture = new Asset<Texture2D>[2][];
		public static ModKeybind GroundSlamKeybind { get; private set; }

		public override void Load()
		{
			GroundSlamKeybind = KeybindLoader.RegisterKeybind(this, "GroundSlam", "LeftControl");
			
			Asset<Effect> shader = ModContent.Request<Effect>("TheDepths/Shaders/DepthsFog", AssetRequestMode.ImmediateLoad);
			Asset<Effect> shader2 = ModContent.Request<Effect>("TheDepths/Shaders/SilhouetteShader", AssetRequestMode.ImmediateLoad);
			Ref<Effect> vertexPixelShaderRef = Main.VertexPixelShaderRef;
			Filters.Scene["TheDepths:FogShader"] = new Filter(new ScreenShaderData(new Ref<Effect>(shader.Value), "DepthsFogShaderPass"), EffectPriority.VeryHigh);
			GameShaders.Misc["TheDepths:SilhouetteShader"] = new MiscShaderData(new Ref<Effect>(shader2.Value), "SilhouettePass");
			GameShaders.Misc["TheDepths:ShadowLash"] = new MiscShaderData(vertexPixelShaderRef, "MagicMissile").UseProjectionMatrix(doUse: true);
			GameShaders.Misc["TheDepths:ShadowLash"].UseImage0(ModContent.Request<Texture2D>("TheDepths/Shaders/ShadowflameFlameHue", AssetRequestMode.ImmediateLoad));
			GameShaders.Misc["TheDepths:ShadowLash"].UseImage1("Images/Extra_" + (short)189);
			GameShaders.Misc["TheDepths:ShadowLash"].UseImage2("Images/Extra_" + (short)190);

			TheDepthsWindUtilities.Load();

			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

			fractalProfiles.Add(ModContent.ItemType<Terminex>(), new FinalFractalProfile(70f, new Color(75, 103, 214))); // new Color(119, 135, 162)));

			mod = this;
			livingFireBlockList = new List<int> { 336, 340, 341, 342, 343, 344, ModContent.TileType<Tiles.LivingFog>() };

			if (!Main.dedServ)
			{
				DefaultRenderTargetOverrider.Patch();
				EquipLoader.AddEquipTexture(this, "TheDepths/Items/Armor/OnyxRobe_Legs", EquipType.Legs, name: "OnyxRobe_Legs");
			}

			IL_Liquid.Update += Evaporation;
			Terraria.IL_Player.UpdateBiomes += HeatRemoval;

			IL_Main.DrawBG += UWBGInsert;
			IL_Main.DrawCapture += UWBGInsertCapture;

			Terraria.On_Main.UpdateAudio_DecideOnTOWMusic += Main_UpdateAudio_DecideOnTOWMusic;

			Terraria.Graphics.Light.On_TileLightScanner.ApplyLiquidLight += On_TileLightScanner_ApplyLiquidLight;
			Terraria.Graphics.Light.On_TileLightScanner.ApplyHellLight += TileLightScanner_ApplyHellLight;
			On_WaterfallManager.AddLight += On_WaterfallManager_AddLight;

			On_UIWorldListItem.DrawSelf += (orig, self, spriteBatch) =>
			{
				orig(self, spriteBatch);
				DrawWorldSelectItemOverlay(self, spriteBatch);
			};

			Terraria.On_WaterfallManager.DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects += On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects;
			Terraria.On_WaterfallManager.StylizeColor += On_WaterfallManager_StylizeColor;

			On_Liquid.GetLiquidMergeTypes += On_Liquid_GetLiquidMergeTypes;
			On_Player.PlaceThing_Tiles_CheckLavaBlocking += On_Player_PlaceThing_Tiles_CheckLavaBlocking;

			IL_UIGenProgressBar.DrawSelf += ProgressBarEdit;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.BuildPage += DepthsSelectionMenu.ILBuildPage;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.MakeInfoMenu += DepthsSelectionMenu.ILMakeInfoMenu;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.ShowOptionDescription +=
				DepthsSelectionMenu.ILShowOptionDescription;


			Terraria.GameContent.UI.States.On_UIWorldCreation.SetDefaultOptions += DepthsSelectionMenu.OnSetDefaultOptions;
			On_Player.ItemCheck_CatchCritters += On_Player_ItemCheck_CatchCritters;
			IL_LiquidRenderer.InternalPrepareDraw += LavaBubbleReplacer;
			IL_Player.Update += SplashPlayerLava;
			IL_NPC.Collision_WaterCollision += SplashNPCLava;
			IL_Projectile.Update += SplashProjectileLava;
			IL_Item.MoveInWorld += SplashItemLava;
			IL_Main.oldDrawWater += LavaBubbleReplacer;
			On_TELogicSensor.GetState += On_TELogicSensor_GetState;

			On_TileDrawing.DrawMultiTileVinesInWind += On_TileDrawing_DrawMultiTileVinesInWind;
			On_TileDrawing.PostDrawTiles += On_TileDrawing_PostDrawTiles;
			On_TileDrawing.GetWindCycle += On_TileDrawing_GetWindCycle;

			On_AmbientSky.HellBatsGoupSkyEntity.ctor += HellBatsGoupSkyEntity_ctor;
			IL_Player.ItemCheck_UseBuckets += BucketCollectionItem;

			IL_WorldGen.NotTheBees += NightmareGrassGFBPatcher;
			On_Player.TryReplantingTree += TreeReplantingDetour;
			On_LegacyPlayerRenderer.DrawPlayerFull += PlayerAfterImages;
			On_Player.KeyDoubleTap += SlamDoubleTap;
			IL_Player.RocketBootVisuals += RocketBootVfx;
			On_Player.PlaceThing_PaintScrapper_LongMoss += On_Player_PlaceThing_PaintScrapper_LongMoss;
			On_TileLightScanner.ApplySurfaceLight += On_TileLightScanner_ApplySurfaceLight;
			IL_NPC.SpawnNPC += NPCSpawningEdit;
			On_Player.ItemCheck_ManageRightClickFeatures += On_Player_ItemCheck_ManageRightClickFeatures;
			On_ItemSlot.TryItemSwap += On_ItemSlot_TryItemSwap;
			IL_Main.DrawInfoAccs += DepthMeterTextChanger;
			On_Player.GetItemGrabRange += On_Player_GetItemGrabRange;
			IL_Player.GetAnglerReward_MainReward += HotRodReplacer;
			On_Player.RemoveAnglerAccOptionsFromRewardPool += On_Player_RemoveAnglerAccOptionsFromRewardPool;

			//credits
			IL_CreditsRollComposer.FillSegments += FillCreditSegmentILEdit;
			IL_CreditsRollEvent.TryStartingCreditsRoll += CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.UpdateTime += CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.SetRemainingTimeDirect += CreditsRollIngameTimeDurationExtention;
		}

		public override void Unload()
		{
			GroundSlamKeybind = null;
			TheDepthsWindUtilities.Unload();

			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

			fractalProfiles.Remove(ModContent.ItemType<Terminex>());

			livingFireBlockList = null;
			IL_Liquid.Update -= Evaporation;
			Terraria.IL_Player.UpdateBiomes -= HeatRemoval;

			IL_Main.DrawBG -= UWBGInsert;
			IL_Main.DrawCapture -= UWBGInsertCapture;

			Terraria.On_Main.UpdateAudio_DecideOnTOWMusic -= Main_UpdateAudio_DecideOnTOWMusic;

			Terraria.Graphics.Light.On_TileLightScanner.ApplyLiquidLight -= On_TileLightScanner_ApplyLiquidLight;
			Terraria.Graphics.Light.On_TileLightScanner.ApplyHellLight -= TileLightScanner_ApplyHellLight;
			On_WaterfallManager.AddLight -= On_WaterfallManager_AddLight;

			Terraria.On_WaterfallManager.DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects -= On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects;
			Terraria.On_WaterfallManager.StylizeColor -= On_WaterfallManager_StylizeColor;

			On_Liquid.GetLiquidMergeTypes -= On_Liquid_GetLiquidMergeTypes;
			On_Player.PlaceThing_Tiles_CheckLavaBlocking -= On_Player_PlaceThing_Tiles_CheckLavaBlocking;

			IL_UIGenProgressBar.DrawSelf -= ProgressBarEdit;

			On_Player.ItemCheck_CatchCritters -= On_Player_ItemCheck_CatchCritters;
			IL_LiquidRenderer.InternalPrepareDraw -= LavaBubbleReplacer;
			IL_Player.Update -= SplashPlayerLava;
			IL_NPC.Collision_WaterCollision -= SplashNPCLava;
			IL_Projectile.Update -= SplashProjectileLava;
			IL_Item.MoveInWorld -= SplashItemLava;
			IL_Main.oldDrawWater -= LavaBubbleReplacer;
			On_TELogicSensor.GetState -= On_TELogicSensor_GetState;

			On_TileDrawing.DrawMultiTileVinesInWind -= On_TileDrawing_DrawMultiTileVinesInWind;
			On_TileDrawing.PostDrawTiles -= On_TileDrawing_PostDrawTiles;
			On_TileDrawing.GetWindCycle -= On_TileDrawing_GetWindCycle;

			On_AmbientSky.HellBatsGoupSkyEntity.ctor -= HellBatsGoupSkyEntity_ctor;
			IL_Player.ItemCheck_UseBuckets -= BucketCollectionItem;

			IL_WorldGen.NotTheBees -= NightmareGrassGFBPatcher;
			On_Player.TryReplantingTree -= TreeReplantingDetour;
			On_LegacyPlayerRenderer.DrawPlayerFull -= PlayerAfterImages;
			On_Player.KeyDoubleTap -= SlamDoubleTap;
			IL_Player.RocketBootVisuals -= RocketBootVfx;
			On_Player.PlaceThing_PaintScrapper_LongMoss -= On_Player_PlaceThing_PaintScrapper_LongMoss;
			On_TileLightScanner.ApplySurfaceLight -= On_TileLightScanner_ApplySurfaceLight;
			IL_NPC.SpawnNPC -= NPCSpawningEdit;
			On_Player.ItemCheck_ManageRightClickFeatures -= On_Player_ItemCheck_ManageRightClickFeatures;
			On_ItemSlot.TryItemSwap -= On_ItemSlot_TryItemSwap;
			IL_Main.DrawInfoAccs -= DepthMeterTextChanger;
			On_Player.GetItemGrabRange -= On_Player_GetItemGrabRange;
			IL_Player.GetAnglerReward_MainReward -= HotRodReplacer;
			On_Player.RemoveAnglerAccOptionsFromRewardPool -= On_Player_RemoveAnglerAccOptionsFromRewardPool;

			//credits
			IL_CreditsRollComposer.FillSegments -= FillCreditSegmentILEdit;
			IL_CreditsRollEvent.TryStartingCreditsRoll -= CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.UpdateTime -= CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.SetRemainingTimeDirect -= CreditsRollIngameTimeDurationExtention;
		}

		public override void PostSetupContent()
		{
			if (!Main.dedServ)
			{
				UWBGTexture[0] = new Asset<Texture2D>[14];
				UWBGTexture[1] = new Asset<Texture2D>[14];
				for (int i = 0; i < 14; i++)
				{
					UWBGTexture[0][i] = TextureAssets.Underworld[i];
					UWBGTexture[1][i] = ModContent.Request<Texture2D>("TheDepths/Backgrounds/DepthsUnderworldBG_" + i);
				}
				UWBGBottomColor[0] = new Color(11, 3, 7);
				UWBGBottomColor[1] = new Color(0, 0, 0);
			}
		}

		public override object Call(params object[] args)
		{
			//For Content creators: Message me (Lion8cake) on discord if you have any mod call suggestions
			return args switch
			{
				["InDepths", Player player] => TheDepthsWorldGen.InDepths(player),
				["IsPlayerInRightDepths", Player player] => TheDepthsWorldGen.IsPlayerInRightDepths(player),
				["IsPlayerInLeftDepths", Player player] => TheDepthsWorldGen.IsPlayerInLeftDepths(player),
				["depthsorHell"] => TheDepthsWorldGen.isWorldDepths,
				["DrunkDepthsLeft"] => TheDepthsWorldGen.DrunkDepthsLeft,
				["DrunkDepthsRight"] => TheDepthsWorldGen.DrunkDepthsRight,
				["SetdepthsorHell", bool boolean] => TheDepthsWorldGen.isWorldDepths = boolean,
				["SetDrunkDepthsLeft", bool boolean] => TheDepthsWorldGen.DrunkDepthsLeft = boolean,
				["SetDrunkDepthsRight", bool boolean] => TheDepthsWorldGen.DrunkDepthsRight = boolean,

				//IDs
				["UnreflectiveProjectiles", int projectileID, bool flag] => TheDepthsIDs.Sets.UnreflectiveProjectiles[projectileID] = flag,
				["AxesAbleToBreakStone", int itemID, bool flag] => TheDepthsIDs.Sets.AxesAbleToBreakStone[itemID] = flag,
				["HellstoneBarOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.HellstoneBarOnlyItem[itemID] = flag,
				["HellstoneOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.HellstoneOnlyItem[itemID] = flag,
				["ObsidianOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.ObsidianOnlyItem[itemID] = flag,
				["AnkhShieldOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.AnkhShieldOnlyItem[itemID] = flag,
				["FireGauntletOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.FireGauntletOnlyItem[itemID] = flag,
				["AshBlockOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.AshBlockOnlyItem[itemID] = flag,
				["AshWoodOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.AshWoodOnlyItem[itemID] = flag,
				["FireblossomOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.FireblossomOnlyItem[itemID] = flag,
				["ObsidifishOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.ObsidifishOnlyItem[itemID] = flag,
				["FlarefinKoiOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.FlarefinKoiOnlyItem[itemID] = flag,
				["PwnhammerOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.PwnhammerOnlyItem[itemID] = flag,
				["FireblossomSeedsOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.FireblossomSeedsOnlyItem[itemID] = flag,
				["HellforgeOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.HellforgeOnlyItem[itemID] = flag,
				["LivingFireBlockOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.LivingFireBlockOnlyItem[itemID] = flag,
				["CobaltShieldOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.CobaltShieldOnlyItem[itemID] = flag,
				["CascadeOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.CascadeOnlyItem[itemID] = flag,
				["TreasureMagnetOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.TreasureMagnetOnlyItem[itemID] = flag,
				["LavaBucketOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.LavaBucketOnlyItem[itemID] = flag,
				["BottomlessLavaBucketOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.BottomlessLavaBucketOnlyItem[itemID] = flag,
				["LavaSpongeOnlyItem", int itemID, bool flag] => TheDepthsIDs.Sets.RecipeBlacklist.LavaSpongeOnlyItem[itemID] = flag,
				_ => throw new Exception("TheDepths: Unknown mod call, make sure you are calling the right method/field with the right parameters!")
			};
		}

		#region NOTTHEBEESCrispyHoneyProtection
		private void NightmareGrassGFBPatcher(ILContext il)
		{
			var c = new ILCursor(il);
			ILLabel IL_0898 = null;
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdindU2(), i => i.MatchLdcI4(633), i => i.MatchBeq(out IL_0898)))
			{
				ModContent.GetInstance<TheDepths>().Logger.Debug("The Depths: Could not locate the AshGrass not the bees protection");
				return;
			}
			if (IL_0898 == null) return;
			c.EmitLdloc(1);
			c.EmitLdloc(2);
			c.EmitDelegate((int i, int j) =>
			{
				return Main.tile[i, j].TileType != ModContent.TileType<NightmareGrass>() && Main.tile[i, j].TileType != ModContent.TileType<ShaleBlock>() && Main.tile[i, j].TileType != ModContent.TileType<Shalestone>();
			});
			c.EmitBrfalse(IL_0898);
		}
		#endregion

		#region AxeofRegrowthDetour
		private void TreeReplantingDetour(On_Player.orig_TryReplantingTree orig, Player self, int x, int y)
		{
			orig.Invoke(self, x, y);
			int style = 0;
			Tile GrownOn = Main.tile[x, y + 1];
			if (GrownOn.TileType == ModContent.TileType<Tiles.NightmareGrass>() || GrownOn.TileType == ModContent.TileType<Tiles.ShaleBlock>())
			{
				int type = (GrownOn.TileType == ModContent.TileType<Tiles.NightmareGrass>() ? ModContent.TileType<Tiles.Trees.NightSapling>() : ModContent.TileType<Tiles.Trees.PetrifiedSapling>());
				if (!TileObject.CanPlace(Player.tileTargetX, Player.tileTargetY, type, style, self.direction, out var objectData))
				{
					return;
				}
				bool num = TileObject.Place(objectData);
				WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
				if (num)
				{
					TileObjectData.CallPostPlacementPlayerHook(Player.tileTargetX, Player.tileTargetY, type, style, self.direction, objectData.alternate, objectData);
					if (Main.netMode == 1)
					{
						NetMessage.SendObjectPlacement(-1, Player.tileTargetX, Player.tileTargetY, objectData.type, objectData.style, objectData.alternate, objectData.random, self.direction);
					}
				}
			}
		}
		#endregion

		#region GroundSlamDetours
		private static void PlayerAfterImages(On_LegacyPlayerRenderer.orig_DrawPlayerFull orig, LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, Player player)
		{
			SpriteBatch spriteBatch = camera.SpriteBatch;
			SamplerState samplerState = camera.Sampler;
			if (player.mount.Active && player.fullRotation != 0f)
			{
				samplerState = LegacyPlayerRenderer.MountedSamplerState;
			}
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, camera.Rasterizer, null, camera.GameViewMatrix.TransformationMatrix);
			if (player.GetModPlayer<TheDepthsPlayer>().isSlamming)
			{
				for (int num12 = 0; num12 < 3; num12++)
				{
					self.DrawPlayer(camera, player, player.shadowPos[num12], player.shadowRotation[num12], player.shadowOrigin[num12], 0.5f + 0.2f * num12);
				}
			}
			spriteBatch.End();
			orig.Invoke(self, camera, player);
		}

		private void SlamDoubleTap(On_Player.orig_KeyDoubleTap orig, Player self, int keyDir)
		{
			orig.Invoke(self, keyDir);
			if (self.GetModPlayer<TheDepthsPlayer>().Gslam && keyDir == 0 && GroundSlamKeybind.GetAssignedKeys().Count == 0)
			{
				self.GetModPlayer<TheDepthsPlayer>().GSlamkeybindPressed = true;
			}
		}
		#endregion

		#region BucketILEdit
		private void BucketCollectionItem(ILContext il)
		{
			var c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(1), i => i.MatchSub(), i => i.MatchStfld<Item>("stack"), i => i.MatchLdarg0(), i => i.MatchLdcI4(207));
			c.EmitLdarg(0);
			c.EmitDelegate((int item, Player player) => Worldgen.TheDepthsWorldGen.InDepths(player) ? ModContent.ItemType<QuicksilverBucket>() : item);
		}
		#endregion

		#region AnglerDetoursILEdits
		private void On_Player_RemoveAnglerAccOptionsFromRewardPool(On_Player.orig_RemoveAnglerAccOptionsFromRewardPool orig, Player self, List<int> itemIdsOfAccsWeWant, Item itemToTestAgainst)
		{
			orig.Invoke(self, itemIdsOfAccsWeWant, itemToTestAgainst);
			if (!itemToTestAgainst.IsAir)
			{
				if (itemToTestAgainst.type == ModContent.ItemType<QuicksilverproofTackleBag>())
				{
					itemIdsOfAccsWeWant.Remove(2373);
					itemIdsOfAccsWeWant.Remove(2375);
					itemIdsOfAccsWeWant.Remove(2374);
				}
				if (itemToTestAgainst.type == ModContent.ItemType<MercuryMossFishingBobber>())
				{
					itemIdsOfAccsWeWant.Remove(5139);
				}
				if (itemToTestAgainst.type == ModContent.ItemType<ShellPhoneDepths>())
				{
					itemIdsOfAccsWeWant.Remove(3120);
					itemIdsOfAccsWeWant.Remove(3037);
					itemIdsOfAccsWeWant.Remove(3096);
				}
			}
		}

		private void HotRodReplacer(ILContext il)
		{
			var c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchLdloc0(), i => i.MatchLdcI4(2422));
			c.EmitDelegate((int item) => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.ItemType<SilverLiner>() : item);
		}
		#endregion

		#region lodestonedetour
		private int On_Player_GetItemGrabRange(On_Player.orig_GetItemGrabRange orig, Player self, Item item)
		{
			int num = Player.defaultItemGrabRange;
			if (self.GetModPlayer<TheDepthsPlayer>().lodeStone)
			{
				num += 160;
				return num;
			}
			else
			{
				return orig.Invoke(self, item);
			}
		}
		#endregion

		#region QuicksilverBubbleILedits 
		private void SplashItemLava(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchStloc(15), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type);
			c.GotoNext(MoveType.After, i => i.MatchStloc(23), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type2 => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type2);
		}

		private void SplashProjectileLava(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchStloc(22), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type);
			c.GotoNext(MoveType.After, i => i.MatchStloc(30), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type2 => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type2);
		}

		private void SplashNPCLava(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchStloc(10), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type);
			c.GotoNext(MoveType.After, i => i.MatchStloc(19), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type2 => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type2);
		}

		private void SplashPlayerLava(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchStloc(172), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type);
			c.GotoNext(MoveType.After, i => i.MatchStloc(180), i => i.MatchBr(out _), i => i.MatchLdarg0(), i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("X"), i => i.MatchLdcR4(6), i => i.MatchSub(), i => i.MatchLdarg0(),
				i => i.MatchLdflda<Entity>("position"), i => i.MatchLdfld<Vector2>("Y"), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("height"), i => i.MatchLdcI4(2), i => i.MatchDiv(), i => i.MatchConvR4(), i => i.MatchAdd(), i => i.MatchLdcR4(8),
				i => i.MatchSub(), i => i.MatchNewobj(out _), i => i.MatchLdarg0(), i => i.MatchLdfld<Entity>("width"), i => i.MatchLdcI4(12), i => i.MatchAdd(), i => i.MatchLdcI4(24), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type2 => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type2);
		}

		private void LavaBubbleReplacer(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(16), i => i.MatchLdcI4(16), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type);
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(16), i => i.MatchLdcI4(8), i => i.MatchLdcI4(35));
			c.EmitDelegate<Func<int, int>>(type2 => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? ModContent.DustType<QuicksilverBubble>() : type2);
		}
		#endregion

		#region DepthMeterILEdit
		private void DepthMeterTextChanger(ILContext il)
		{
			var c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchLdstr("GameUI.LayerUnderworld"));
			c.EmitDelegate((string text) => Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) ? "Mods.TheDepths.GameUI.LayerDepths" : text);
		}
		#endregion

		#region ShellphoneDetour
		private void On_ItemSlot_TryItemSwap(On_ItemSlot.orig_TryItemSwap orig, Item item)
		{
			Player player = Main.LocalPlayer;
			int type = item.type;
			if (item.type == ModContent.ItemType<ShellPhoneDepths>())
			{
				item.ChangeItemType(ItemID.ShellphoneSpawn);
				SoundEngine.PlaySound(SoundID.Unlock);
				Main.stackSplit = 30;
				Main.mouseRightRelease = false;
				Recipe.FindRecipes();
			}
			if (Worldgen.TheDepthsWorldGen.InDepths(player))
			{
				if (item.type == ItemID.ShellphoneOcean)
				{
					item.ChangeItemType(ModContent.ItemType<ShellPhoneDepths>());
					SoundEngine.PlaySound(SoundID.Unlock);
					Main.stackSplit = 30;
					Main.mouseRightRelease = false;
					Recipe.FindRecipes();
					return;
				}
			}
			orig.Invoke(item);
		}

		private void On_Player_ItemCheck_ManageRightClickFeatures(On_Player.orig_ItemCheck_ManageRightClickFeatures orig, Player self)
		{
			List<int> _projectilesToInteractWith = (List<int>)typeof(Player).GetField("_projectilesToInteractWith", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			bool flag = self.selectedItem != 58 && self.controlUseTile && Main.myPlayer == self.whoAmI && !self.tileInteractionHappened && self.releaseUseItem && !self.controlUseItem && !self.mouseInterface && !CaptureManager.Instance.Active && !Main.HoveringOverAnNPC && !Main.SmartInteractShowingGenuine;
			bool flag2 = flag;
			if (!ItemID.Sets.ItemsThatAllowRepeatedRightClick[self.inventory[self.selectedItem].type] && !Main.mouseRightRelease)
			{
				flag2 = false;
			}
			if (flag2 && self.altFunctionUse == 0)
			{
				for (int i = 0; i < _projectilesToInteractWith.Count; i++)
				{
					Projectile projectile = Main.projectile[_projectilesToInteractWith[i]];
					Rectangle hitbox = projectile.Hitbox;
					if (hitbox.Contains(Main.MouseWorld.ToPoint()) || Main.SmartInteractProj == projectile.whoAmI)
					{
						flag = false;
						flag2 = false;
						break;
					}
				}
			}
			if (flag2 && self.altFunctionUse == 0 && self.itemTime == 0 && self.itemAnimation == 0)
			{
				if (self.inventory[self.selectedItem].type == ModContent.ItemType<ShellPhoneDepths>())
				{
					self.releaseUseTile = false;
					Main.mouseRightRelease = false;
					SoundEngine.PlaySound(SoundID.Unlock);
					self.inventory[self.selectedItem].ChangeItemType(ItemID.ShellphoneSpawn);
				}
				if (Worldgen.TheDepthsWorldGen.InDepths(self))
				{
					if (self.inventory[self.selectedItem].type == ItemID.ShellphoneOcean)
					{
						self.releaseUseTile = false;
						Main.mouseRightRelease = false;
						SoundEngine.PlaySound(SoundID.Unlock);
						self.inventory[self.selectedItem].ChangeItemType(ModContent.ItemType<ShellPhoneDepths>());
						return;
					}
				}
			}
			orig.Invoke(self);
		}
		#endregion

		#region UnderworldandTimSpawningILedits
		private void NPCSpawningEdit(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			ILLabel IL_10d3d = null;
			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchBr(out _),
				i => i.MatchLdloc(5),
				i => i.MatchLdsfld<Main>("maxTilesY"),
				i => i.MatchLdcI4(190),
				i => i.MatchSub(),
				i => i.MatchBle(out IL_10d3d)))
			{
				ModContent.GetInstance<TheDepths>().Logger.Debug("The Depths: Could not locate the underworld enemy spawning check");
				return;
			}
			if (IL_10d3d == null) return;
			c.EmitLdloc(14);
			c.EmitDelegate((int k) =>
			{
				return !Worldgen.TheDepthsWorldGen.InDepths(Main.player[k]);
			});
			c.EmitBrfalse(IL_10d3d);

			//below code was implemented on 1/3/2024 through special ID for gemrobes
			/*ILLabel IL_1185a = null;
			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdsfld<Main>("player"),
				i => i.MatchLdloc(14),
				i => i.MatchLdelemRef(),
				i => i.MatchLdfld<Player>("armor"),
				i => i.MatchLdcI4(1),
				i => i.MatchLdelemRef(),
				i => i.MatchLdfld<Item>("type"),
				i => i.MatchLdcI4(4256),
				i => i.MatchBeq(out IL_1185a)))
			{
				ModContent.GetInstance<TheDepths>().Logger.Debug("The Depths: Could not locate the tim spawning check");
				return;
			}
			if (IL_1185a == null) return;
			c.EmitLdloc(14);
			c.EmitDelegate((int k) =>
			{
				return Main.player[k].armor[1].type == ModContent.ItemType<Items.Armor.OnyxRobe>();
			});
			c.EmitBrtrue(IL_1185a);*/
		}
		#endregion

		#region CreditsILeditandDetours
		private SegmentInforReport PlaySegment_ModdedTextRoll(CreditsRollComposer self, int startTime, string sourceCategory, Vector2 anchorOffset = default(Vector2))
		{
			//We have our own text roll segment due to tmodloader using Hjson instead of json meaning that sometimes the order of names becomes backwards
			//if you want to use vanilla text for some reason i would recomend that you reflect CreditsRollComposer.PlaySegment_TextRoll
			List<IAnimationSegment> _segments = (List<IAnimationSegment>)typeof(CreditsRollComposer).GetField("_segments", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			anchorOffset.Y -= 40f;
			int extraDuration = 80;
			LocalizedText[] TextArray = Language.FindAll(Lang.CreateDialogFilter(sourceCategory + ".", null));
			for (int i = 0; i < TextArray.Length; i++)
			{
				_segments.Add(new Segments.LocalizedTextSegment(startTime + i * extraDuration, Language.GetText(sourceCategory + "." + (i + 1)), anchorOffset));
			}
			SegmentInforReport result = default(SegmentInforReport);
			result.totalTime = TextArray.Length * extraDuration + extraDuration * -1;
			return result;
		}

		private SegmentInforReport PlaySegment_LionEightCake_ChasmesCurseIsLifted(CreditsRollComposer self, int startTime, Vector2 sceneAnchorPosition)
		{
			//Our own animation, reffer to the Terraria.GameContent.Skies.Credits.CreditsRollComposer for examples of used and unused animations
			List<IAnimationSegment> _segments = (List<IAnimationSegment>)typeof(CreditsRollComposer).GetField("_segments", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _backgroundOffset = (Vector2)typeof(CreditsRollComposer).GetField("_backgroundOffset", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _originAtBottom = (Vector2)typeof(CreditsRollComposer).GetField("_originAtBottom", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _emoteBubbleOffsetWhenOnRight = (Vector2)typeof(CreditsRollComposer).GetField("_emoteBubbleOffsetWhenOnRight", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _emoteBubbleOffsetWhenOnLeft = (Vector2)typeof(CreditsRollComposer).GetField("_emoteBubbleOffsetWhenOnLeft", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 GetSceneFixVector = (Vector2)typeof(CreditsRollComposer).GetMethod("GetSceneFixVector", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(self, new object[] { });
			//Reflection chunk for all the little trinkets that use it
			sceneAnchorPosition += GetSceneFixVector;
			int duration = startTime; //Set an initial time
			sceneAnchorPosition.X += 0;
			Asset<Texture2D> SceneAsset = ModContent.Request<Texture2D>("TheDepths/Assets/DepthsCreditsScene", AssetRequestMode.ImmediateLoad);  //Make sure that its ImmediateLoad otherwise 90% of the time it wont load
			Rectangle SceneAssetFrame = SceneAsset.Frame();
			DrawData SceneAssetDrawData = new DrawData(SceneAsset.Value, Vector2.Zero, SceneAssetFrame, Color.White, 0f, SceneAssetFrame.Size() * new Vector2(0.5f, 1f) + new Vector2((float)0, -42f), 1f, (SpriteEffects)0);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> SceneAssetSegment = new Segments.SpriteSegment(SceneAsset, startTime, SceneAssetDrawData, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
				.Then(new Actions.Sprites.Wait(120));
			_segments.Add(SceneAssetSegment); //We make and add the background segment
			Segments.AnimationSegmentWithActions<NPC> WizardNPCSegment = new Segments.NPCSegment(startTime, NPCID.Wizard, sceneAnchorPosition + new Vector2(190f, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
				.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)); //We create the wizard moving from right to left at a speed of 0.5 for 127 frames (60 frames a second)
			Segments.AnimationSegmentWithActions<NPC> PrincessNPCSegment = new Segments.NPCSegment(startTime, NPCID.Princess, sceneAnchorPosition + new Vector2(240f, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
				.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)); //We create the princess moving from right to left
			Asset<Texture2D> ChasmeSoulAsset = ModContent.Request<Texture2D>("TheDepths/Assets/DepthsCreditsChasme", AssetRequestMode.ImmediateLoad);
			Rectangle ChasmeSoulFrame = ChasmeSoulAsset.Frame(1, 3, 0, 0);
			DrawData ChasmeSoulDrawData = new DrawData(ChasmeSoulAsset.Value, Vector2.Zero, ChasmeSoulFrame, Color.White, 0f, ChasmeSoulFrame.Size(), 1f, (SpriteEffects)0);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> chasmeSoul = new Segments.SpriteSegment(ChasmeSoulAsset, startTime, ChasmeSoulDrawData, sceneAnchorPosition + new Vector2(10f, 2f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51))
				.Then(new Actions.Sprites.Wait(120)); //We use Sprites to draw chasme, we draw a few more assets higher up to make the animation feel more lively
			duration += (int)WizardNPCSegment.DedicatedTimeNeeded; //Increment using the dedicated movement time from the wizard to the duration
																   //We use these little chunks to make sure all Main components of the animation and the duration.
																   //These chunks will last however long a certain action goes for
																   //In this first action we make the wizard react with the alert emote for 120 frames (2 seconds)
																   //while making the scene, wizard, princess, chasme and duration wait for 120 frames
			Segments.EmoteSegment AlertEmote = new Segments.EmoteSegment(EmoteID.EmotionAlert, duration, 120, sceneAnchorPosition + new Vector2(134f, 0f) + _emoteBubbleOffsetWhenOnRight, 0);
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			chasmeSoul.Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//We make the princess react with the lightning emote for 120 frames
			Segments.EmoteSegment LightingEnchantmentEmote = new Segments.EmoteSegment(EmoteID.WeatherLightning, duration, 120, sceneAnchorPosition + new Vector2(184f, 0f) + _emoteBubbleOffsetWhenOnRight, 0);
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			chasmeSoul.Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//We move the wizard towards chasme for 60 frames
			SceneAssetSegment.Then(new Actions.Sprites.Wait(60));
			WizardNPCSegment.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 60));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(60));
			chasmeSoul.Then(new Actions.Sprites.Wait(60));
			duration += 60;
			//We make chasme react with sadness
			Segments.EmoteSegment SadEmote = new Segments.EmoteSegment(EmoteID.EmoteSadness, duration, 120, sceneAnchorPosition + new Vector2(-8f, 12f) + _emoteBubbleOffsetWhenOnLeft, SpriteEffects.FlipHorizontally);
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			chasmeSoul.Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//We make chasme switch to frame 2 (her looking up) and react with the anger emote
			Segments.EmoteSegment AngerEmote = new Segments.EmoteSegment(EmoteID.EmoteAnger, duration, 120, sceneAnchorPosition + new Vector2(-8f, 12f) + _emoteBubbleOffsetWhenOnLeft, SpriteEffects.FlipHorizontally);
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			chasmeSoul.Then(new Actions.Sprites.SetFrame(0, 1, 0, 0)).Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//Alot goes on in this section
			//Firstly we make the wizard go completely invisible,
			//this is due to NPC segments allowing for only the use of walking, showing items, blinking and standing frames. Not their attack frames
			//We then draw 2 assets
			//The wizard
			//The Magic Aura (this asset is usually orange when the wizard is attacking but we color it purple here)
			SceneAssetSegment.Then(new Actions.Sprites.Wait(60));
			WizardNPCSegment.Then(new Actions.NPCs.Fade(255));
			Point[] wizardEnchantAnim = (Point[])(object)new Point[2]
			{
				new Point(0, 21),
				new Point(0, 22)
			};
			Point[] uncurseGlowAnim = (Point[])(object)new Point[4]
			{
				new Point(0, 0),
				new Point(0, 1),
				new Point(0, 2),
				new Point(0, 3)
			};
			//Points are used to set animation frames to their appropriate frames
			//For the first point we set it to frame 22 and 23 for the wizard's attacking frames
			//for the second point we set it to frame 1-4 for the full magic aura frames
			Main.instance.LoadNPC(NPCID.Wizard);//We load the NPC data including its assets so any texturepack or mod that changes the wizard's assets will still work
			Asset<Texture2D> wizardAsset = TextureAssets.Npc[NPCID.Wizard];
			Rectangle wizardFrame = wizardAsset.Frame(1, Main.npcFrameCount[NPCID.Wizard]);
			DrawData wizarddrawdata = new DrawData(wizardAsset.Value, Vector2.Zero, wizardFrame, Color.White, 0f, wizardFrame.Size(), 1f, (SpriteEffects)0);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> wizardpreforminguncurse = new Segments.SpriteSegment(wizardAsset, duration, wizarddrawdata, sceneAnchorPosition + new Vector2(120f, 6f))
				.Then(new Actions.Sprites.SetFrameSequence(240, wizardEnchantAnim, 6, 0, 0));
			Asset<Texture2D> MagicAuraAsset = TextureAssets.Extra[ExtrasID.MagicAura];
			Rectangle MagicAuraFrame = MagicAuraAsset.Frame(1, 4);
			DrawData MagicAuraDrawData = new DrawData(MagicAuraAsset.Value, Vector2.Zero, MagicAuraFrame, new Color(158, 95, 245), 0f, MagicAuraFrame.Size(), 1f, (SpriteEffects)0);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> wizardlyPowers = new Segments.SpriteSegment(MagicAuraAsset, duration, MagicAuraDrawData, sceneAnchorPosition + new Vector2(118f, 4f))
				.Then(new Actions.Sprites.SetFrameSequence(240, uncurseGlowAnim, 3, 0, 0));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(60));
			chasmeSoul.Then(new Actions.Sprites.Wait(60));
			duration += 60;
			//We make chasme slowly assend upwards and switch to her 3rd and final frame
			//We use Simulate gravity as sprites dont have a move function like NPCs do
			SceneAssetSegment.Then(new Actions.Sprites.Wait(60));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(60));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(60));
			chasmeSoul.Then(new Actions.Sprites.SetFrame(0, 2, 0, 0)).Then(new Actions.Sprites.SimulateGravity(Vector2.Zero, new Vector2(0f, -0.014f), 0, 60));
			duration += 60;
			//We make chasme freeze in the air and spawn a an asset ontop of her making her fade
			Asset<Texture2D> ChasmeFadeAsset = ModContent.Request<Texture2D>("TheDepths/Assets/DepthsCreditsChasmeWhite", AssetRequestMode.ImmediateLoad);
			Rectangle ChasmeFadeFrame = ChasmeFadeAsset.Frame();
			DrawData ChasmeFadeDrawData = new DrawData(ChasmeFadeAsset.Value, Vector2.Zero, null, Color.White, 0f, ChasmeFadeFrame.Size(), 1f, (SpriteEffects)0);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> ChasmeEnchantment = new Segments.SpriteSegment(ChasmeFadeAsset, duration, ChasmeFadeDrawData, sceneAnchorPosition + new Vector2(10f, -49f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
				.Then(new Actions.Sprites.Wait(120));
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			chasmeSoul.Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//We make the wizard return back to the normal opacity and then spawn a new asset which is chasme in her human form
			//Chasme then slowly decends downards
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(60)).Then(new Actions.NPCs.Fade(-255)).Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			Asset<Texture2D> chasmeHumanAsset = ModContent.Request<Texture2D>("TheDepths/Assets/DepthsCreditsChasmeHuman", AssetRequestMode.ImmediateLoad);
			Rectangle chasmeHumanFrame = chasmeHumanAsset.Frame(1, 23, 0, 0);
			DrawData ChasmeHumanDrawData = new DrawData(chasmeHumanAsset.Value, Vector2.Zero, chasmeHumanFrame, Color.White, 0f, chasmeHumanFrame.Size(), 1f, (SpriteEffects)1);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> ChasmeHuman = new Segments.SpriteSegment(chasmeHumanAsset, duration, ChasmeHumanDrawData, sceneAnchorPosition + new Vector2(6f, -46f))
				.Then(new Actions.Sprites.SetFrame(0, 1, 0, 0)).Then(new Actions.Sprites.SimulateGravity(Vector2.Zero, new Vector2(0f, 0.014f), 0, 60))
				.Then(new Actions.Sprites.SetFrame(0, 0, 0, 0)).Then(new Actions.Sprites.Wait(60));
			duration += 120;
			//We make chasme react with the alert emote
			Segments.EmoteSegment ChasmeAlertEmote = new Segments.EmoteSegment(EmoteID.EmotionAlert, duration, 120, sceneAnchorPosition + new Vector2(-14f, 0f) + _emoteBubbleOffsetWhenOnLeft, SpriteEffects.FlipHorizontally);
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			ChasmeHuman.Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//We make chasme react with the love emote
			Segments.EmoteSegment LoveEmote = new Segments.EmoteSegment(EmoteID.EmotionLove, duration, 120, sceneAnchorPosition + new Vector2(-14f, 0f) + _emoteBubbleOffsetWhenOnLeft, SpriteEffects.FlipHorizontally);
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			ChasmeHuman.Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//We then make the wizard and princess both react with Happiness and Ballon emotes
			Segments.EmoteSegment HappyforChasmeEmote = new Segments.EmoteSegment(EmoteID.EmoteHappiness, duration, 120, sceneAnchorPosition + new Vector2(104f, 0f) + _emoteBubbleOffsetWhenOnRight, 0);
			Segments.EmoteSegment CelebrateEmote = new Segments.EmoteSegment(EmoteID.PartyBalloons, duration, 120, sceneAnchorPosition + new Vector2(184f, 0f) + _emoteBubbleOffsetWhenOnRight, 0);
			SceneAssetSegment.Then(new Actions.Sprites.Wait(120));
			WizardNPCSegment.Then(new Actions.NPCs.Wait(120));
			PrincessNPCSegment.Then(new Actions.NPCs.Wait(120));
			ChasmeHuman.Then(new Actions.Sprites.Wait(120));
			duration += 120;
			//We use another point for chasme's walking animation while making all NPCs (and chasme) move left while slowly fading out
			Point[] ChasmeHumanWalkAnim = (Point[])(object)new Point[12]
			 {
				new Point(0, 2),
				new Point(0, 3),
				new Point(0, 4),
				new Point(0, 5),
				new Point(0, 6),
				new Point(0, 7),
				new Point(0, 8),
				new Point(0, 9),
				new Point(0, 10),
				new Point(0, 11),
				new Point(0, 12),
				new Point(0, 13)
			 };
			ChasmeHuman.Then(new Actions.Sprites.SimulateGravity(new Vector2(0.5f, 0f), Vector2.Zero, 0f, 160)).With(new Actions.Sprites.SetFrameSequence(127, ChasmeHumanWalkAnim, 6, 0, 0)).With(new Actions.Sprites.Fade(0f, 127));
			PrincessNPCSegment.Then(new Actions.NPCs.Move(new Vector2(0.4f, 0f), 160)).With(new Actions.NPCs.Fade(2, 127));
			WizardNPCSegment.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 160)).With(new Actions.NPCs.Fade(2, 127));
			SceneAssetSegment.Then(new Actions.Sprites.Fade(0f, 127));
			duration += 187;
			//With all the segments we then add them all to make sure everything loads (once a segment's instructions has ran out it will automatically unload
			_segments.Add(WizardNPCSegment);
			_segments.Add(PrincessNPCSegment);
			_segments.Add(chasmeSoul);
			_segments.Add(AlertEmote);
			_segments.Add(LightingEnchantmentEmote);
			_segments.Add(SadEmote);
			_segments.Add(AngerEmote);
			_segments.Add(wizardpreforminguncurse);
			_segments.Add(wizardlyPowers);
			_segments.Add(ChasmeEnchantment);
			_segments.Add(ChasmeHuman);
			_segments.Add(ChasmeAlertEmote);
			_segments.Add(LoveEmote);
			_segments.Add(HappyforChasmeEmote);
			_segments.Add(CelebrateEmote);
			SegmentInforReport result = default(SegmentInforReport);
			result.totalTime = duration - startTime;
			return result; //we return the end result time for the animation for the use of the credit's segment fill and how long the animation should play for
		}

		private void FillCreditSegmentILEdit(ILContext il)
		{
			ILCursor c = new ILCursor(il); //place a IL Cursor
			c.GotoNext(MoveType.Before, i => i.MatchLdloc0(), i => i.MatchLdarg0(), i => i.MatchLdloc0(), i => i.MatchLdstr("CreditsRollCategory_Creator"), i => i.MatchLdloc3());
			//make sure all instructions match, movetype will place our code before the first instruction once all instructions match
			c.EmitLdarg(0); //Emit ldarg_0 (self)
			c.EmitLdloca(0); //Emit ldloc_0 (num)
			c.EmitLdloca(2); //Emit ldloc_2 (num3)
			c.EmitLdloca(3); //Emit ldloc_3 (vector2 or val2)
			c.EmitDelegate((CreditsRollComposer self, ref int num, ref int num3, ref Vector2 vector2) =>
			{ //Get the needed variables and instance
			  //Edit inside here for more text and animations, shown here is just how to add 1 text and 1 animation
				num += PlaySegment_ModdedTextRoll(self, num, "Mods.TheDepths.CreditsRollCategory_DepthsTeam", vector2).totalTime; //Play our credit text
				num += num3; //wait
				num += PlaySegment_LionEightCake_ChasmesCurseIsLifted(self, num, vector2).totalTime; //Play our custom animation
				num += num3; //wait
			});
		}

		private void CreditsRollIngameTimeDurationExtention(ILContext il)
		{
			ILCursor c = new ILCursor(il); //place a IL cursor
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(28800)); //Look for a LDC I4 instruction with 28800 (all timers use this)
			c.EmitDelegate<Func<int, int>>(maxDuration => maxDuration + 60 * 20); //Adds ontop of the max duration to account for the custom credits
		}
		#endregion

		#region MossScapper
		private void On_Player_PlaceThing_PaintScrapper_LongMoss(On_Player.orig_PlaceThing_PaintScrapper_LongMoss orig, Player self, int x, int y)
		{
			orig.Invoke(self, x, y);
			Tile tile = Main.tile[x, y];
			if (tile.TileType != ModContent.TileType<Tiles.MercuryMoss_Foliage>())
			{
				return;
			}
			self.cursorItemIconEnabled = true;
			if (!self.ItemTimeIsZero || self.itemAnimation <= 0 || !self.controlUseItem)
			{
				return;
			}
			tile = Main.tile[x, y];
			int frameX = tile.TileFrameX;
			WorldGen.KillTile(x, y);
			self.ApplyItemTime(self.inventory[self.selectedItem]);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(17, -1, -1, null, 0, x, y);
			}
			if (Main.rand.Next(9) == 0)
			{
				int number = Item.NewItem(new EntitySource_ItemUse(self, self.HeldItem), x * 16, y * 16, 16, 16, ModContent.ItemType<Items.Placeable.MercuryMoss>());
				NetMessage.SendData(21, -1, -1, null, number, 1f);
			}
		}
		#endregion

		#region RocketbootDustILedit
		static void RocketBootVfx(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(instruction => instruction.MatchCall<Dust>("NewDustDirect"));
			c.GotoNext(instruction => instruction.MatchCall<Dust>("NewDustDirect"));

			c.GotoPrev(MoveType.After, i => i.MatchLdloc(4));
			c.Emit(OpCodes.Ldarg, 0);
			c.EmitDelegate((int type, Player player) =>
			{
				if (Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().nFlare)
				{
					return ModContent.DustType<NightmareEmberDust>();
				}
				else
				{
					return type;
				}
			});
		}
		#endregion

		#region AlbinoBatSkyObject
		private void HellBatsGoupSkyEntity_ctor(On_AmbientSky.HellBatsGoupSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
		{
			orig.Invoke(self, player, random);
			if (TheDepthsWorldGen.InDepths(player))
			{
				var SkyEntity = typeof(AmbientSky).GetNestedType("SkyEntity", BindingFlags.NonPublic);
				SkyEntity.GetField("Texture",
					BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Static |
					BindingFlags.Instance
					).SetValue(
					self,
					ModContent.Request<Texture2D>("TheDepths/Backgrounds/Ambient/AlbinoBat" + Main.rand.Next(1, 4)));
			}
		}
		#endregion

		#region MakeSomeTilesWindyInTheDepths
		private float On_TileDrawing_GetWindCycle(On_TileDrawing.orig_GetWindCycle orig, TileDrawing self, int x, int y, double windCounter)
		{
			if (TheDepthsWorldGen.InDepths(Main.LocalPlayer))
			{
				if (!Main.SettingsEnabled_TilesSwayInWind)
				{
					return 0f;
				}
				float num = (float)x * 0.5f + (float)(y / 100) * 0.5f;
				float num2 = (float)Math.Cos(windCounter * 6.2831854820251465 + (double)num) * 0.5f;
				if (Main.remixWorld)
				{
					if (!((double)y > Main.worldSurface) && (y <= Main.UnderworldLayer))
					{
						return 0f;
					}
					num2 += Main.WindForVisuals;
				}
				else
				{
					if (!((double)y < Main.worldSurface) && (y <= Main.UnderworldLayer))
					{
						return 0f;
					}
					num2 += Main.WindForVisuals;
				}
				float lerpValue = Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), clamped: true);
				return num2 * lerpValue;
			}
			else
			{
				return orig.Invoke(self, x, y, windCounter);
			}
		}
		#endregion

		#region VineWindTileLength
		private void On_TileDrawing_DrawMultiTileVinesInWind(On_TileDrawing.orig_DrawMultiTileVinesInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
		{
			if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.DepthsVanityBanners>() || Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.DepthsBanners>())
			{
				sizeY = 3;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.NightwoodChandelier>() || Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.QuartzChandelier>() || Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.PetrifiedWoodChandelier>())
			{
				sizeX = 3;
				sizeY = 3;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<NightwoodLantern>() || Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<QuartzLantern>() || Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<PetrifiedWoodLantern>())
			{
				sizeX = 1;
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.HangingShadowShrub>())
			{
				sizeX = 2;
				sizeY = 3;
			}
			orig.Invoke(self, screenPosition, offSet, topLeftX, topLeftY, sizeX, sizeY);
		}

		private void On_TileDrawing_PostDrawTiles(On_TileDrawing.orig_PostDrawTiles orig, TileDrawing self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets)
		{
			orig.Invoke(self, solidLayer, forRenderTargets, intoRenderTargets);
			if (!solidLayer && !intoRenderTargets)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
				DrawChandeliers();
				DrawLanterns();
				Main.spriteBatch.End();
			}
		}
		private void DrawLanterns()
		{
			for (int i = 0; i < ModContent.GetInstance<PetrifiedWoodLantern>().Coordinates.Count; i++)
			{
				ModContent.GetInstance<PetrifiedWoodLantern>().DrawMultiTileVines(ModContent.GetInstance<PetrifiedWoodLantern>().Coordinates[i].X, ModContent.GetInstance<PetrifiedWoodLantern>().Coordinates[i].Y, Main.spriteBatch);
			}
		}
		private void DrawChandeliers()
		{
			for (int i = 0; i < ModContent.GetInstance<PetrifiedWoodChandelier>().Coordinates.Count; i++)
			{
				ModContent.GetInstance<PetrifiedWoodChandelier>().DrawMultiTileVines(ModContent.GetInstance<PetrifiedWoodChandelier>().Coordinates[i].X, ModContent.GetInstance<PetrifiedWoodChandelier>().Coordinates[i].Y, Main.spriteBatch);
			}
		}
		#endregion

		#region NEWWorldIcondetour
		private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch)
		{
			bool data = uiItem.Data.TryGetHeaderData(ModContent.GetInstance<TheDepthsWorldGen>(), out var _data);
			UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
			WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
			WorldIcon.RemoveAllChildren();
			#region UnopenedWorldIcon
			if (!data)
			{
				if (!Data.DrunkWorld && !Data.DefeatedMoonlord)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (!Data.DrunkWorld && Data.DefeatedMoonlord)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-7f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (Data.DrunkWorld)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (Data.DrunkWorld && Data.DefeatedMoonlord)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-7f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
			}
			#endregion
			else
			{
				bool NotDuelCores = !_data.GetBool("DrunkDepthsRight") && !_data.GetBool("DrunkDepthsLeft");

				#region Normal
				if (_data.GetBool("HasDepths") && NotDuelCores && !Data.RemixWorld && !Data.DrunkWorld && !Data.DefeatedMoonlord)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepths"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				if (_data.GetBool("HasDepths") && NotDuelCores && !Data.RemixWorld && !Data.DrunkWorld && Data.DefeatedMoonlord)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepths"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-7f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (!_data.GetBool("HasDepths") && NotDuelCores && !Data.DrunkWorld && !Data.DefeatedMoonlord)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (!_data.GetBool("HasDepths") && NotDuelCores && !Data.DrunkWorld && Data.DefeatedMoonlord)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-7f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region DrunkSeedIcon
				else if (Data.DrunkWorld && !Data.DefeatedMoonlord && (_data.GetBool("DrunkDepthsLeft") && _data.GetBool("DrunkDepthsRight") || !_data.GetBool("DrunkDepthsLeft") && !_data.GetBool("DrunkDepthsRight")))
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (Data.DrunkWorld && Data.DefeatedMoonlord && (_data.GetBool("DrunkDepthsLeft") && _data.GetBool("DrunkDepthsRight") || !_data.GetBool("DrunkDepthsLeft") && !_data.GetBool("DrunkDepthsRight")))
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-7f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}

				else if (!Data.DefeatedMoonlord && _data.GetBool("DrunkDepthsLeft") && !_data.GetBool("DrunkDepthsRight"))
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkLeft"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (Data.DefeatedMoonlord && _data.GetBool("DrunkDepthsLeft") && !_data.GetBool("DrunkDepthsRight"))
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkLeft"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-7f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}

				else if (!Data.DefeatedMoonlord && !_data.GetBool("DrunkDepthsLeft") && _data.GetBool("DrunkDepthsRight"))
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkRight"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (Data.DefeatedMoonlord && !_data.GetBool("DrunkDepthsLeft") && _data.GetBool("DrunkDepthsRight"))
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkRight"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-7f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}

				#endregion

				#region RemixSeedIcon
				else if (_data.GetBool("HasDepths") && Data.HasCrimson && Data.RemixWorld && !Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCrimson"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (_data.GetBool("HasDepths") && Data.HasCrimson && Data.RemixWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCrimsonHallow"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (_data.GetBool("HasDepths") && Data.HasCorruption && Data.RemixWorld && !Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCorruption"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				else if (_data.GetBool("HasDepths") && Data.HasCorruption && Data.RemixWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCorruptionHallow"))
					{
						Top = new StyleDimension(-10f, 0f),
						Left = new StyleDimension(-6f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion
			}
		}
		#endregion

		#region QuicksilverfallGlowmaskRemover
		private Color On_WaterfallManager_StylizeColor(On_WaterfallManager.orig_StylizeColor orig, float alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache, Color aColor)
		{
			if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
			{
				return aColor;
			}
			else
			{
				return orig.Invoke(alpha, maxSteps, waterfallType, y, s, tileCache, aColor);
			}
		}
		#endregion

		#region LavaSensorDetour
		private bool On_TELogicSensor_GetState(On_TELogicSensor.orig_GetState orig, int x, int y, TELogicSensor.LogicCheckType type, TELogicSensor instance)
		{
			if (type == TELogicSensor.LogicCheckType.Lava && ((Worldgen.TheDepthsWorldGen.isWorldDepths && !TheDepthsWorldGen.DrunkDepthsLeft && !TheDepthsWorldGen.DrunkDepthsRight) || (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(x) < Main.maxTilesX / 2 || Worldgen.TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(x) > Main.maxTilesX / 2)))
			{
				return false;
			}
			return orig.Invoke(x, y, type, instance);
		}
		#endregion

		#region MercuryBugCatchingPunishmentDetour
		private Rectangle On_Player_ItemCheck_CatchCritters(On_Player.orig_ItemCheck_CatchCritters orig, Player self, Item sItem, Rectangle itemRectangle)
		{
			orig.Invoke(self, sItem, itemRectangle);
			bool flag = sItem.type == ModContent.ItemType<Items.QuicksilverproofBugNet>() || sItem.type == ItemID.FireproofBugNet;
			for (int i = 0; i < 200; i++)
			{
				if (!Main.npc[i].active || Main.npc[i].catchItem <= 0)
				{
					continue;
				}
				Rectangle value = new Rectangle((int)Main.npc[i].position.X, (int)Main.npc[i].position.Y, Main.npc[i].width, Main.npc[i].height);
				if (!itemRectangle.Intersects(value))
				{
					continue;
				}
				if (!flag && ItemID.Sets.IsLavaBait[Main.npc[i].catchItem])
				{
					if (Main.myPlayer == Main.LocalPlayer.whoAmI/* && Player.Hurt(PlayerDeathReason.ByNPC(i), 1, (Main.npc[i].Center.X < Main.LocalPlayer.Center.X) ? 1 : (-1), pvp: false, quiet: false, -1, false, 3f) > 0.0*/ && !Main.LocalPlayer.dead)
					{
						if (Main.npc[i].type == ModContent.NPCType<NPCs.EnchantedNightmareWorm>() || Main.npc[i].type == ModContent.NPCType<NPCs.AlbinoRat>() || Main.npc[i].type == ModContent.NPCType<NPCs.QuartzCrawler>())
						{
							Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.MercuryBoiling>(), 300, quiet: false);
							Main.LocalPlayer.ClearBuff(BuffID.OnFire);
						}
					}
				}
			}
			return itemRectangle;
		}
		#endregion

		#region OuterLowerDepthsProgressBar
		private void ProgressBarEdit(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchCallvirt<SpriteBatch>("Draw"), i => i.MatchLdarg1(), i => i.MatchLdarg0(), i => i.MatchLdfld<UIGenProgressBar>("_texOuterLower"));
			c.EmitDelegate((Asset<Texture2D> texture) => (!WorldGen.drunkWorldGen && TheDepthsWorldGen.isWorldDepths || (WorldGen.drunkWorldGen && Main.rand.NextBool(2))) ? ModContent.Request<Texture2D>("TheDepths/Assets/Loading/Depths_Outer_Lower") : texture);
		}
		#endregion

		#region DepthsBackgroundILEdit
		private void UWBGInsert(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdcI4(0), i => i.MatchCall<Main>("DrawUnderworldBackground"));
			c.EmitDelegate(() => {
				DrawUnderworldBackground(false);
			});
		}

		private void UWBGInsertCapture(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdcI4(1), i => i.MatchCall<Main>("DrawUnderworldBackground"));
			c.EmitDelegate(() => {
				DrawUnderworldBackground(true);
			});
		}

		public int UnderworldStyleCalc()
		{
			if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
			{
				return 1;
			}
			return 0;
		}

		protected void DrawUnderworldBackground(bool flat)
		{
			if (!(Main.screenPosition.Y + (float)Main.screenHeight < (float)(Main.maxTilesY - 220) * 16f))
			{
				UWBGStyle = UnderworldStyleCalc();
				for (var i = 0; i < 2; i++)
				{
					if (UWBGStyle != i)
					{
						UWBGAlpha[i] = Math.Max(UWBGAlpha[i] - 0.05f, 0f);
					}
					else
					{
						UWBGAlpha[i] = Math.Min(UWBGAlpha[i] + 0.05f, 1f);
					}
				}
				Vector2 screenOffset = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
				float pushUp = (Main.GameViewMatrix.Zoom.Y - 1f) * 0.5f * 200f;
				SkyManager.Instance.ResetDepthTracker();
				for (int num = 4; num >= 0; num--)
				{
					bool flag = false;
					for (int j = 0; j < 2; j++)
					{
						if (UWBGAlpha[j] > 0f && j != UWBGStyle)
						{
							DrawUnderworldBackgroudLayer(flat, screenOffset, pushUp, num, j, flat ? 1f : UWBGAlpha[j]);
							flag = true;
						}
					}
					DrawUnderworldBackgroudLayer(flat, screenOffset, pushUp, num, UWBGStyle, flag ? UWBGAlpha[UWBGStyle] : 1f);
				}
				if (!Main.mapFullscreen)
				{
					SkyManager.Instance.DrawRemainingDepth(Main.spriteBatch);
				}
				//Main.DrawSurfaceBG_DrawChangeOverlay(12);
			}
		}

		private void DrawUnderworldBackgroudLayer(bool flat, Vector2 screenOffset, float pushUp, int layerTextureIndex, int Style, float Alpha)
		{
			if (Style == 0)
			{
				return;
			}
			int num = Main.underworldBG[layerTextureIndex];
			Asset<Texture2D> asset = UWBGTexture[Style][num];
			if (!asset.IsLoaded)
			{
				Main.Assets.Request<Texture2D>(asset.Name);
			}
			Texture2D value = asset.Value;
			Vector2 vec = new Vector2((float)value.Width, (float)value.Height) * 0.5f;
			float num7 = (flat ? 1f : ((float)(layerTextureIndex * 2) + 3f));
			Vector2 vector = new(1f / num7);
			Rectangle value2 = new(0, 0, value.Width, value.Height);
			float num8 = 1.3f;
			Vector2 zero = Vector2.Zero;
			int num9 = 0;
			switch (num)
			{
				case 1:
					{
						int num14 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
						value2 = new ((num14 >> 1) * (value.Width >> 1), num14 % 2 * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
						vec *= 0.5f;
						zero.Y += 175f;
						break;
					}
				case 2:
					zero.Y += 100f;
					break;
				case 3:
					zero.Y += 75f;
					break;
				case 4:
					num8 = 0.5f;
					zero.Y -= 0f;
					break;
				case 5:
					zero.Y += num9;
					break;
				case 6:
					{
						int num13 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
						value2 = new(num13 % 2 * (value.Width >> 1), (num13 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
						vec *= 0.5f;
						zero.Y += num9;
						zero.Y += -60f;
						break;
					}
				case 7:
					{
						int num12 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
						value2 = new(num12 % 2 * (value.Width >> 1), (num12 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
						vec *= 0.5f;
						zero.Y += num9;
						zero.X -= 400f;
						zero.Y += 90f;
						break;
					}
				case 8:
					{
						int num11 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
						value2 = new(num11 % 2 * (value.Width >> 1), (num11 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
						vec *= 0.5f;
						zero.Y += num9;
						zero.Y += 90f;
						break;
					}
				case 9:
					zero.Y += num9;
					zero.Y -= 30f;
					break;
				case 10:
					zero.Y += 250f * num7;
					break;
				case 11:
					zero.Y += 100f * num7;
					break;
				case 12:
					zero.Y += 20f * num7;
					break;
				case 13:
					{
						zero.Y += 20f * num7;
						int num10 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
						value2 = new(num10 % 2 * (value.Width >> 1), (num10 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
						vec *= 0.5f;
						break;
					}
			}
			if (flat)
			{
				num8 *= 1.5f;
			}
			vec *= num8;
			SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / vector.X);
			if (flat)
			{
				zero.Y += (float)(UWBGTexture[Style][0].Height() >> 1) * 1.3f - vec.Y;
			}
			zero.Y -= pushUp;
			float num2 = num8 * (float)value2.Width;
			int num3 = (int)((float)(int)(screenOffset.X * vector.X - vec.X + zero.X - (float)(Main.screenWidth >> 1)) / num2);
			vec = vec.Floor();
			int num4 = (int)Math.Ceiling((float)Main.screenWidth / num2);
			int num5 = (int)(num8 * ((float)(value2.Width - 1) / vector.X));
			Vector2 vec2 = (new Vector2((float)((num3 - 2) * num5), (float)Main.UnderworldLayer * 16f) + vec - screenOffset) * vector + screenOffset - Main.screenPosition - vec + zero;
			vec2 = vec2.Floor();
			while (vec2.X + num2 < 0f)
			{
				num3++;
				vec2.X += num2;
			}
			for (int i = num3 - 2; i <= num3 + 4 + num4; i++)
			{
				Color color = Color.White;
				float num16 = (float)(int)color.R * Alpha;
				float num17 = (float)(int)color.G * Alpha;
				float num18 = (float)(int)color.B * Alpha;
				float num19 = (float)(int)color.A * Alpha;
				color = new((int)(byte)num16, (int)(byte)num17, (int)(byte)num18, (int)(byte)num19);

				Color color2 = UWBGBottomColor[Style];
				float num116 = (float)(int)color2.R * Alpha;
				float num117 = (float)(int)color2.G * Alpha;
				float num118 = (float)(int)color2.B * Alpha;
				float num119 = (float)(int)color2.A * Alpha;
				color2 = new((int)(byte)num116, (int)(byte)num117, (int)(byte)num118, (int)(byte)num119);

				Main.spriteBatch.Draw(value, vec2, (Rectangle?)value2, color, 0f, Vector2.Zero, num8, (SpriteEffects)0, 0f);
				if (layerTextureIndex == 0)
				{
					int num6 = (int)(vec2.Y + (float)value2.Height * num8);
					Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)vec2.X, num6, (int)((float)value2.Width * num8), Math.Max(0, Main.screenHeight - num6)), color2);
				}
				vec2.X += num2;
			}
		}
		#endregion

		#region QuicksilverTilePlacement
		private bool On_Player_PlaceThing_Tiles_CheckLavaBlocking(On_Player.orig_PlaceThing_Tiles_CheckLavaBlocking orig, Player self)
		{
			orig.Invoke(self);
			bool result = false;
			if (Worldgen.TheDepthsWorldGen.InDepths(self))
			{
				result = false;
			}
			return result;
		}
		#endregion

		#region DepthsSilverfallLightRemover
		private void On_WaterfallManager_AddLight(On_WaterfallManager.orig_AddLight orig, int waterfallType, int x, int y)
		{
			if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
			{
				if (waterfallType == 1)
				{
					float r = 0f;
					float g = 0f;
					float b = 0f;
					Lighting.AddLight(x, y, r, g, b);
					return;
				}
			}
			orig.Invoke(waterfallType, x, y);
		}
		#endregion

		#region QuicksilverCombinations
		private void On_Liquid_GetLiquidMergeTypes(On_Liquid.orig_GetLiquidMergeTypes orig, int thisLiquidType, out int liquidMergeTileType, out int liquidMergeType, bool waterNearby, bool lavaNearby, bool honeyNearby, bool shimmerNearby)
		{
			orig.Invoke(thisLiquidType, out liquidMergeTileType, out liquidMergeType, waterNearby, lavaNearby, honeyNearby, shimmerNearby);
			if ((Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) && Main.netMode == NetmodeID.SinglePlayer) || (Worldgen.TheDepthsWorldGen.isWorldDepths && Main.netMode != NetmodeID.SinglePlayer))
			{
				liquidMergeTileType = ModContent.TileType<Tiles.Quartz>();
				liquidMergeType = thisLiquidType;
				if (thisLiquidType != 0 && waterNearby)
				{
					switch (thisLiquidType)
					{
						case 1:
							liquidMergeTileType = ModContent.TileType<Tiles.Quartz>();
							break;
						case 2:
							liquidMergeTileType = 229;
							break;
						case 3:
							liquidMergeTileType = 659;
							break;
					}
					liquidMergeType = 0;
				}
				if (thisLiquidType != 1 && lavaNearby)
				{
					switch (thisLiquidType)
					{
						case 0:
							liquidMergeTileType = ModContent.TileType<Tiles.Quartz>();
							break;
						case 2:
							liquidMergeTileType = ModContent.TileType<Tiles.GlitterBlock>();
							break;
						case 3:
							liquidMergeTileType = 659;
							break;
					}
					liquidMergeType = 1;
				}
				if (thisLiquidType != 2 && honeyNearby)
				{
					switch (thisLiquidType)
					{
						case 0:
							liquidMergeTileType = 229;
							break;
						case 1:
							liquidMergeTileType = ModContent.TileType<Tiles.GlitterBlock>();
							break;
						case 3:
							liquidMergeTileType = 659;
							break;
					}
					liquidMergeType = 2;
				}
				if (thisLiquidType != 3 && shimmerNearby)
				{
					switch (thisLiquidType)
					{
						case 0:
							liquidMergeTileType = 659;
							break;
						case 1:
							liquidMergeTileType = 659;
							break;
						case 2:
							liquidMergeTileType = 659;
							break;
					}
					liquidMergeType = 3;
				}
			}
		}
		#endregion

		#region SilverfallTextureDetour
		private void On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects(On_WaterfallManager.orig_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects orig, WaterfallManager self, int waterfallType, int x, int y, float opacity, Vector2 position, Rectangle sourceRect, Color color, SpriteEffects effects)
		{
			if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
			{
				if (waterfallType == 1)
				{
					Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TheDepths/Assets/Lava/Quicksilver_Silverfall", (AssetRequestMode)1), position, sourceRect, color, 0f, default(Vector2), 1f, effects, 0f);
					return;
				}
			}
			orig.Invoke(self, waterfallType, x, y, opacity, position, sourceRect, color, effects);
		}
		#endregion

		#region OtherworldlyMusicDetour
		private void Main_UpdateAudio_DecideOnTOWMusic(Terraria.On_Main.orig_UpdateAudio_DecideOnTOWMusic orig, Main self)
		{
			orig.Invoke(self);
			if (!Main.gameMenu)
			{
				if (Main.newMusic == MusicLoader.GetMusicSlot(this, "Sounds/Music/Depths"))
				{
					Main.newMusic = MusicLoader.GetMusicSlot(this, "Sounds/Music/DepthsOtherworldly");
				}
			}
		}
		#endregion

		#region DepthsNoSkyLightDetour/StainedGlassDetour
		private void TileLightScanner_ApplyHellLight(Terraria.Graphics.Light.On_TileLightScanner.orig_ApplyHellLight orig, TileLightScanner self, Tile tile, int x, int y, ref Vector3 lightColor)
		{
			orig.Invoke(self, tile, x, y, ref lightColor);
			float finalR = 0f;
			float finalG = 0f;
			float finalB = 0f;
			float num4 = 0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.08f;
			if ((!tile.HasTile || tile.IsHalfBlock || !Main.tileNoSunLight[tile.TileType]) && tile.WallType == ModContent.WallType<Walls.BlackStainedGlass>() && tile.LiquidAmount < 255)
			{
				finalR = num4;
				finalG = num4 * 0.6f;
				finalB = num4 * 0.2f;
				if (tile.WallType == ModContent.WallType<Walls.BlackStainedGlass>())
				{
					finalR *= 0.58f;
					finalG *= 0.61f;
					finalB *= 0.6f;
				}
			}
			if (lightColor.X < finalR)
			{
				lightColor.X = finalR;
			}
			if (lightColor.Y < finalG)
			{
				lightColor.Y = finalG;
			}
			if (lightColor.Z < finalB)
			{
				lightColor.Z = finalB;
			}
			Vector3 depthslightcolor = new Vector3(0f, 0f, 0.01f);
			if (TheDepthsWorldGen.InDepths(Main.LocalPlayer))

			{
				if ((!tile.HasTile || !Main.tileNoSunLight[tile.TileType] || ((tile.Slope != 0 || tile.IsHalfBlock) && Main.tile[x, y - 1].LiquidAmount == 0 && Main.tile[x, y + 1].LiquidAmount == 0 && Main.tile[x - 1, y].LiquidAmount == 0 && Main.tile[x + 1, y].LiquidAmount == 0)) && (Main.wallLight[tile.WallType] || tile.WallType == 73 || tile.WallType == 227) && tile.LiquidAmount < 200 && (!tile.IsHalfBlock || Main.tile[x, y - 1].LiquidAmount < 200))
				{
					lightColor = depthslightcolor;
				}
				if ((!tile.HasTile || tile.IsHalfBlock || !Main.tileNoSunLight[tile.TileType]) && tile.LiquidAmount < byte.MaxValue)
				{
					lightColor = depthslightcolor;
				}
				lightColor = depthslightcolor;
			}
		}

		private void On_TileLightScanner_ApplySurfaceLight(On_TileLightScanner.orig_ApplySurfaceLight orig, TileLightScanner self, Tile tile, int x, int y, ref Vector3 lightColor)
		{
			orig.Invoke(self, tile, x, y, ref lightColor);
			float finalR = 0f;
			float finalG = 0f;
			float finalB = 0f;
			float num6 = Main.tileColor.R / 255f;
			float num7 = Main.tileColor.G / 255f;
			float num8 = Main.tileColor.B / 255f;
			if ((!tile.HasTile || tile.IsHalfBlock || !Main.tileNoSunLight[tile.TileType]) && tile.WallType == ModContent.WallType<Walls.BlackStainedGlass>() && tile.LiquidAmount < 255)
			{
				finalR = num6;
				finalG = num7;
				finalB = num8;
				if (tile.WallType == ModContent.WallType<Walls.BlackStainedGlass>())
				{
					finalR *= 0.58f;
					finalG *= 0.61f;
					finalB *= 0.6f;
				}
			}
			float num3 = 1f - Main.shimmerDarken;
			finalR *= num3;
			finalG *= num3;
			finalB *= num3;
			if (lightColor.X < finalR)
			{
				lightColor.X = finalR;
			}
			if (lightColor.Y < finalG)
			{
				lightColor.Y = finalG;
			}
			if (lightColor.Z < finalB)
			{
				lightColor.Z = finalB;
			}
		}
		#endregion

		#region QuicksilverNoLightDetour
		private void On_TileLightScanner_ApplyLiquidLight(On_TileLightScanner.orig_ApplyLiquidLight orig, TileLightScanner self, Tile tile, ref Vector3 lightColor)
		{
			orig.Invoke(self, tile, ref lightColor);
			if (tile.LiquidType == 1 && Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
				lightColor = Vector3.Zero;
		}
		#endregion

		#region DepthsNoHeatDistortionILEdit
		private void HeatRemoval(ILContext il)
		{
			ILCursor c = new ILCursor(il); //Make a cursor
			c.GotoNext(MoveType.After, 
				i => i.MatchLdloc0(), 
				i => i.MatchLdfld<Point>("Y"), 
				i => i.MatchLdsfld<Main>("maxTilesY"), 
				i => i.MatchLdcI4(320), 
				i => i.MatchSub(), 
				i => i.MatchCgt());
			//Finds the Flag7 Bool that controles the heat Y level
			c.EmitDelegate<Func<bool, bool>>(currentBool => currentBool && !Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer)); //Adds ontop of the bool with our own
		}
		#endregion

		#region NoEvaporationILEdit
		private void Evaporation(ILContext il)
		{
			ILCursor c = new(il);
			ILLabel IL_011e = null;
			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdarg0(),
				i => i.MatchLdfld<Liquid>("y"),
				i => i.MatchCall<Main>("get_UnderworldLayer"),
				i => i.MatchBle(out IL_011e)))
			{
				ModContent.GetInstance<TheDepths>().Logger.Debug("The Depths: Could not locate the Water Evaporation Code");
				return;
			}
			if (IL_011e == null) return;
			c.EmitLdarg0();//Terraria.Liquid instance
			c.EmitLdfld(typeof(Liquid).GetField("x", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)); //X position variable used in the delegate
			c.EmitDelegate((int x) =>
			{
				return (Worldgen.TheDepthsWorldGen.isWorldDepths && !TheDepthsWorldGen.DrunkDepthsLeft && !TheDepthsWorldGen.DrunkDepthsRight) 
				|| (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(x) < Main.maxTilesX / 2 
				|| Worldgen.TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(x) > Main.maxTilesX / 2);
			});
			c.EmitBrtrue(IL_011e);
		}
		#endregion
	}

	public static class TheDepthsWindUtilities
	{
		public static void Load()
		{
			_addSpecialPointSpecialPositions = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialPositions", BindingFlags.NonPublic | BindingFlags.Instance);
			_addSpecialPointSpecialsCount = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialsCount", BindingFlags.NonPublic | BindingFlags.Instance);
			_addVineRootPositions = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_vineRootsPositions", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static void Unload()
		{
			_addSpecialPointSpecialPositions = null;
			_addSpecialPointSpecialsCount = null;
			_addVineRootPositions = null;
		}

		public static FieldInfo _addSpecialPointSpecialPositions;
		public static FieldInfo _addSpecialPointSpecialsCount;
		public static FieldInfo _addVineRootPositions;

		public static void AddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int x, int y, int type)
		{
			if (_addSpecialPointSpecialPositions.GetValue(tileDrawing) is Point[][] _specialPositions)
			{
				if (_addSpecialPointSpecialsCount.GetValue(tileDrawing) is int[] _specialsCount)
				{
					_specialPositions[type][_specialsCount[type]++] = new Point(x, y);
				}
			}
		}

		public static void CrawlToTopOfVineAndAddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int j, int i)
		{
			if (_addVineRootPositions.GetValue(tileDrawing) is List<Point> _vineRootsPositions)
			{
				int y = j;
				for (int num = j - 1; num > 0; num--)
				{
					Tile tile = Main.tile[i, num];
					if (WorldGen.SolidTile(i, num) || !tile.HasTile)
					{
						y = num + 1;
						break;
					}
				}
				Point item = new(i, y);
				if (!_vineRootsPositions.Contains(item))
				{
					_vineRootsPositions.Add(item);
					Main.instance.TilesRenderer.AddSpecialPoint(i, y, 6);
				}
			}
		}
	}
}