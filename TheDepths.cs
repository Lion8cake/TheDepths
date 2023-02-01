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
using AltLibrary.Common.AltBiomes;
using System.Reflection;

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
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(this, "TheDepths/Items/Armor/OnyxRobe_Legs", EquipType.Legs, name: "OnyxRobe_Legs");
            }
            IL.Terraria.Liquid.Update += ILEvaporateWatrer;
        }

        private void ILEvaporateWatrer(ILContext il)
        {
            ILCursor c = new ILCursor(il);
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

                c.EmitDelegate((byte goingToEvaporateBy) => {
                    Console.WriteLine("What the fuck is a brain");
                    if (WorldBiomeManager.WorldHell == "TheDepths/DepthsBiome")
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

        public override void Unload()
        {
            livingFireBlockList = null;
        }
    }
}