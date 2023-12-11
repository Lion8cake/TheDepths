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

        public override void OnWorldUnload()
        {
            Gemforge.RubyRelicIsOnForge = 1;
        }

		public override void Load()
		{
            IL_Player.ItemCheck_ManageRightClickFeatures += IL_Player_ItemCheck_ManageRightClickFeatures;
		}

		public override void Unload()
		{
            IL_Player.ItemCheck_ManageRightClickFeatures -= IL_Player_ItemCheck_ManageRightClickFeatures;
        }

		#region ShellphoneILEdit
		private void IL_Player_ItemCheck_ManageRightClickFeatures(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(out _), i => i.MatchBr(out _), i => i.MatchLdcI4(ItemID.ShellphoneHell)))
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TheDepths>(), il, null);
            }

            c.EmitDelegate<Func<int, int>>(static shellphone => {
                if (Worldgen.TheDepthsWorldGen.depthsorHell)
                    shellphone = ModContent.ItemType<ShellPhoneDepths>();
                return shellphone;
            });

            MonoModHooks.DumpIL(ModContent.GetInstance<TheDepths>(), il);
        }
		#endregion
	}
}