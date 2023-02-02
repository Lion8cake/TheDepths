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
			IL.Terraria.Liquid.Update += Evaporation;
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(this, "TheDepths/Items/Armor/OnyxRobe_Legs", EquipType.Legs, name: "OnyxRobe_Legs");
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
                    if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome" && liqTile.LiquidType == LiquidID.Water) {
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

        public override void Unload() {
			IL.Terraria.Liquid.Update -= Evaporation;
			IL.Terraria.Player.UpdateBiomes -= NoHeap;
			IL.Terraria.Main.DrawUnderworldBackgroudLayer -= ILMainDrawUnderworldBackground;
			livingFireBlockList = null;
        }
    }
}