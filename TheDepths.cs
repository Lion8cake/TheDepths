using System;
using System.Collections.Generic;
using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Tiles;
using ReLogic.Content;

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
            IL.Terraria.Main.Update += ILWaterEvaporation;
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(this, "TheDepths/Items/Armor/OnyxRobe_Legs", EquipType.Legs, name: "OnyxRobe_Legs");
            }
        }

        private void ILMainDrawUnderworldBackground(ILContext il)
        {
            ILCursor c = new(il);
            /*c.GotoNext(MoveType.After, i => i.MatchStloc(2));
                c.Index -= 2;
            c.Emit(OpCodes.Ldloc, 1);
            c.Emit(OpCodes.Ldloc, 0);
            c.EmitDelegate<Func<Asset<Texture2D>, int, Asset<Texture2D>>>((orig, index) =>
            {
                if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
                    return texture[index];
                return orig;
            });
            c.Emit(OpCodes.Stloc, 1);*/

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

        private void ILWaterEvaporation(ILContext il)
        {
            ILCursor c = new(il);
            try
            {
                int b = 0;
                c.GotoNext(MoveType.After,
                    i => i.MatchLdcI4(out _),
                i => i.MatchStloc(out b),
                i => i.MatchLdloca(b),
                i => i.MatchCall(out _),
                i => i.MatchLdindU1(),
                i => i.MatchLdloc(b),
                i => i.MatchBge(out _));

                c.GotoNext(MoveType.After, i => i.MatchLdloc(b));

                c.EmitDelegate((byte goingToEvaporateBye) =>
                {
                    if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
                    {
                        return 0;
                    }
                    return goingToEvaporateBy;
                });
            }
            catch
            {
            }
        }

        public override void Unload()
        {
            livingFireBlockList = null;
        }
    }
}