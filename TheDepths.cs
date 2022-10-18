using System;
using System.Collections.Generic;
using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Tiles;

namespace TheDepths
{
    public class TheDepths : Mod
    {
        public static readonly Texture2D[] texture = new Texture2D[5];
        public static Mod mod;
        public static List<int> livingFireBlockList;

        public static float DepthsTransition { get; set; }
        public override void Load()
        {
            mod = this;
            livingFireBlockList = new List<int> { 336, 340, 341, 342, 343, 344, ModContent.TileType<LivingFog>() };
            //IL.Terraria.Main.DrawUnderworldBackground += ILMainDrawUnderworldBackground;
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(this, "TheDepths/Items/Armor/OnyxRobe_Legs", EquipType.Legs, name: "OnyxRobe_Legs");
            }
            /*for (int i = 0; i < texture.Length; i++)
                texture[i] = ModContent.Request<Texture2D>("TheDepths/Backgrounds/DepthsUnderworldBG_" + i).Value;*/
        }

        /*private void ILMainDrawUnderworldBackground(ILContext il)
        {
            var c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(4)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdsfld(out _)))
                return;
            c.Index++;
            c.EmitDelegate<Func<Texture2D[], Texture2D[]>>((orig) =>
            {
                if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
                    return texture;
                return orig;
            });
        }*/

        public override void Unload()
        {
            livingFireBlockList = null;
        }
    }
}