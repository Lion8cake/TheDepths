using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Graphics.Effects;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Tiles;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Liquid;
using System.Reflection;
using System.IO;
using System.Text.Json;
using Terraria.ModLoader.Config;
using Terraria.Localization;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.IO;
using Newtonsoft.Json;
using Terraria.ModLoader.IO;
using TheDepths.Worldgen;
using System.Linq;
using TheDepths.Items;

namespace TheDepths
{
    public class TheDepthsModSystem : ModSystem
    {
        //public static bool NotLavaDestroyable;

        /*public override void PreUpdateWorld()
        {
            //if (Worldgen.TheDepthsWorldGen.InDepths)
            //{
                for (int i = 0; i < TileLoader.TileCount; i++)
                {
                    Main.tileLavaDeath[i] = false;
                }
            //}
            //else if (NotLavaDestroyable == false)
            //{
            //    for (int i = 0; i < TileLoader.TileCount; i++)
            //    {
            //        Main.tileLavaDeath[i] = true;
            //    }
            //}
        }*/

        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasCondition(Condition.NearLava))
                {
                    recipe.AddCondition(Language.GetOrRegister(""), () => Worldgen.TheDepthsWorldGen.InDepths);
                }
            }
        }

		public static int? MossConversion(int thisType, int otherType)
		{
            if (Main.tileMoss[thisType] || TileID.Sets.tileMossBrick[thisType])
            {
                if (otherType == 38)
                {
                    return ModContent.TileType<MercuryMossStoneBricks>();
                }
                if (otherType == 1)
                {
                    return ModContent.TileType<MercuryMoss>();
                }
            }
            return null;
		}

		public override void OnWorldUnload()
        {
            Gemforge.RubyRelicIsOnForge = 1;
        }
	}
}