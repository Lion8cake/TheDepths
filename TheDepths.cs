using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Graphics.Effects;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using TheDepths.Tiles;
using Terraria.ID;
using MonoMod.Utils;
using Terraria.GameContent.Liquid;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Light;
using Terraria.Utilities;

namespace TheDepths
{
    public class TheDepths : Mod
    {
        public static Asset<Texture2D>[] texture = new Asset<Texture2D>[14];
        public static Mod mod;
        public static List<int> livingFireBlockList;

        public override void Load()
        {
            mod = this;
            for (int i = 0; i < texture.Length; i++)
                texture[i] = ModContent.Request<Texture2D>("TheDepths/Backgrounds/DepthsUnderworldBG_" + i);
            livingFireBlockList = new List<int> { 336, 340, 341, 342, 343, 344, ModContent.TileType<LivingFog>() };
            IL.Terraria.Main.DrawUnderworldBackgroudLayer += ILMainDrawUnderworldBackground;
			IL.Terraria.Player.UpdateBiomes += NoHeap;
			//IL.Terraria.Liquid.Update += Evaporation;
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(this, "TheDepths/Items/Armor/OnyxRobe_Legs", EquipType.Legs, name: "OnyxRobe_Legs");
            }
            On.Terraria.WaterfallManager.LoadContent += WaterfallManager_LoadContent;
            On.Terraria.Graphics.Light.TileLightScanner.ApplyLavaLight += TileLightScanner_ApplyLavaLight;
            On.Terraria.Graphics.Light.TileLightScanner.ApplyHellLight += TileLightScanner_ApplyHellLight;
            On.Terraria.Main.UpdateAudio_DecideOnTOWMusic += Main_UpdateAudio_DecideOnTOWMusic;
            On.Terraria.Dust.NewDust += Dust_NewDust;
        }

        public override void Unload()
        {
            //IL.Terraria.Liquid.Update -= Evaporation;
            IL.Terraria.Player.UpdateBiomes -= NoHeap;
            IL.Terraria.Main.DrawUnderworldBackgroudLayer -= ILMainDrawUnderworldBackground;
            livingFireBlockList = null;
            On.Terraria.WaterfallManager.LoadContent -= WaterfallManager_LoadContent;
            On.Terraria.Graphics.Light.TileLightScanner.ApplyLavaLight -= TileLightScanner_ApplyLavaLight;
            On.Terraria.Graphics.Light.TileLightScanner.ApplyHellLight -= TileLightScanner_ApplyHellLight;
            On.Terraria.Main.UpdateAudio_DecideOnTOWMusic -= Main_UpdateAudio_DecideOnTOWMusic;
            On.Terraria.Dust.NewDust -= Dust_NewDust;
        }


        private int Dust_NewDust(On.Terraria.Dust.orig_NewDust orig, Vector2 Position, int Width, int Height, int Type, float SpeedX, float SpeedY, int Alpha, Color newColor, float Scale)
        {
            int index = orig.Invoke(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale);
            if (Type == DustID.Lava && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
            {
                Main.dust[index].active = false;
            }

            return index;
        }

        private void Main_UpdateAudio_DecideOnTOWMusic(On.Terraria.Main.orig_UpdateAudio_DecideOnTOWMusic orig, Main self)
        {
            orig.Invoke(self);
            Player player = Main.CurrentPlayer;
            if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome" && player.ZoneUnderworldHeight)
            {
                Main.newMusic = 79;
            }
        }

        private void TileLightScanner_ApplyHellLight(On.Terraria.Graphics.Light.TileLightScanner.orig_ApplyHellLight orig, TileLightScanner self, Tile tile, int x, int y, ref Vector3 lightColor)
        {
            orig.Invoke(self, tile, x, y, ref lightColor);
            if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome" && ModContent.GetInstance<TheDepthsClientConfig>().DepthsLightingConfig)
                lightColor = Vector3.Zero;
        }

        private void TileLightScanner_ApplyLavaLight(On.Terraria.Graphics.Light.TileLightScanner.orig_ApplyLavaLight orig, Tile tile, ref Vector3 lightColor)
        {
            orig.Invoke(tile, ref lightColor);
            if (tile.LiquidType == 1 && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
            lightColor = Vector3.Zero;
        }

        private void WaterfallManager_LoadContent(On.Terraria.WaterfallManager.orig_LoadContent orig, WaterfallManager self)
        {
            orig.Invoke(self); //can also be orig.deletgate(params) or smth along those lines
            if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
            {
                object obj = Main.instance.waterfallManager;
                for (int i = 0; i < 26; i++)
                {
                    //typeof(WaterfallManager).GetField("waterfallTexture", BindingFlags.NonPublic | BindingFlags.Instance).GetValue() = ModContent.Request<Texture2D>("TheDepths/Lava/Quicksilver_Silverfall", (AssetRequestMode)2);
                }
            }
        }

        private void NoHeap(ILContext il) {
            var c = new ILCursor(il);
            try {
                c.GotoNext(MoveType.After,
                    i => i.MatchLdstr("HeatDistortion"),
                    i => i.MatchLdsfld<Main>("UseHeatDistortion"));

                c.EmitDelegate((bool useHeatDistortion) => {
                    if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome") {
                        return false;
                    }
                    return useHeatDistortion;
                });
            }
            catch (Exception e) {
                Logger.Error(e.Message);
            }
		}

		private void Evaporation(ILContext il) {
            var c = new ILCursor(il);
            try {
                int b = 0;
                int tile = 0;
				c.GotoNext(MoveType.After,
                    i => i.MatchLdloca(out tile),
                    i => i.MatchCall<Tile>("get_liquid"),
                    i => i.MatchDup(),
                    i => i.MatchLdindU1(),
                    i => i.MatchLdloc(out b),
                    i => i.MatchSub(),
                    i => i.MatchConvU1(),
                    i => i.MatchStindI1());

                c.Index -= 3;
                c.Emit(OpCodes.Ldloca_S, (byte)tile);
                c.EmitDelegate((byte num, ref Tile liqTile) => {
                if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome" && liqTile.LiquidType == LiquidID.Water /*&& ModContent.GetInstance<TheDepthsServerConfig>().DepthsNoWaterVapor*/) {
                        return (byte)0;
                    }
                    return num;
                });
			}
            catch (Exception e) {
                Logger.Error(e.Message);
            }
		}

		private void ILMainDrawUnderworldBackground(ILContext il)
        {
            ILCursor c = new(il);

            int asset = 0, texture = 0;
            c.GotoNext(i => i.MatchLdloc(out asset),
                i => i.OpCode == OpCodes.Callvirt,
                i => i.MatchStloc(out texture));

            c.Emit(OpCodes.Ldloc, asset);
            c.Emit(OpCodes.Ldloc, 0);
            c.EmitDelegate<Func<Asset<Texture2D>, int, Asset<Texture2D>>>((asset, index) =>
            {
                if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
                    return TheDepths.texture[index];
                return asset;
            });
            c.Emit(OpCodes.Stloc, asset);
        }
    }
}