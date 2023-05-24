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

namespace TheDepths
{
    public class TheDepthsModSystem : ModSystem
    {
        //public static bool NotLavaDestroyable;

        public override void PostUpdateEverything()
        {
            /*string twld = Path.ChangeExtension(Main.worldPathName, ".twld");
            if (File.ReadAllText(ConfigManager.ModConfigPath + "/AltLibrary_AltLibraryConfig.json").Contains(twld))
            {
                Main.NewText("Found World");
            }*/
        }

        /*public override void PreUpdateWorld()
        {
            //if (TheDepthsWorldGen.InDepths)
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
                    recipe.AddCondition(Language.GetOrRegister(""), () => TheDepthsWorldGen.InDepths);
                }
            }
        }

        public override void OnWorldUnload()
        {
            Gemforge.RubyRelicIsOnForge = 1;
        }

        public override void PostUpdateWorld()
        {
            if (Main.desiredWorldTilesUpdateRate == 0)
                return;
            float num1 = (float)(3E-05f * Main.desiredWorldTilesUpdateRate);
            float num3 = Main.maxTilesX * Main.maxTilesY * num1;
            int num4 = 151;
            int maxValue2 = (int)MathHelper.Lerp(num4, num4 * 2.8f, MathHelper.Clamp((float)(Main.maxTilesX / 4200.0 - 1.0), 0.0f, 1f));
            for (int index1 = 0; index1 < num3; ++index1)
            {
                if (Main.rand.Next(100) == 0 && Main.rand.Next(maxValue2) == 0)
                    PlantAlch();
            }
        }

        public static void PlantAlch()
        {
            int index1 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
            int index2 = WorldGen.genRand.Next(40) != 0 ? (WorldGen.genRand.Next(10) != 0 ? WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 20) : WorldGen.genRand.Next(0, Main.maxTilesY - 20)) : WorldGen.genRand.Next((int)(Main.rockLayer + (double)Main.maxTilesY) / 2, Main.maxTilesY - 20);
            while (index2 < Main.maxTilesY - 20 && !Main.tile[index1, index2].HasTile)
                ++index2;
            if (!Main.tile[index1, index2].HasUnactuatedTile || Main.tile[index1, index2 - 1].HasTile || Main.tile[index1, index2 - 1].LiquidAmount != 0)
                return;
            int num1 = 15;
            int num2 = 5;
            int num3 = 0;
            int num4 = (int)(num1 * (Main.maxTilesX / 4200.0));
            int num5 = Utils.Clamp(index1 - num4, 4, Main.maxTilesX - 4);
            int num6 = Utils.Clamp(index1 + num4, 4, Main.maxTilesX - 4);
            int num7 = Utils.Clamp(index2 - num4, 4, Main.maxTilesY - 4);
            int num8 = Utils.Clamp(index2 + num4, 4, Main.maxTilesY - 4);
            for (int index3 = num5; index3 <= num6; ++index3)
            {
                for (int index4 = num7; index4 <= num8; ++index4)
                {
                    if (Main.tileAlch[Main.tile[index3, index4].TileType])
                        ++num3;
                }
            }
            if (num3 >= num2)
                return;
            if (Main.tile[index1, index2].TileType == ModContent.TileType<ShaleBlock>())
                WorldGen.PlaceAlch(index1, index2 - 1, 5);
            if (!Main.tile[index1, index2 - 1].HasTile || Main.netMode != NetmodeID.Server)
                return;
            NetMessage.SendTileSquare(-1, index1, index2 - 1, 1, TileChangeType.None);
        }

        private static bool PlaceAlch(int x, int y, int style)
        {
            var tile = Main.tile[x, y];
            var tileplusone = Main.tile[x, y + 1];
            if (Main.tile[x, y] == null)
                tile = new Tile();
            if (Main.tile[x, y + 1] == null)
                tileplusone = new Tile();
            if (!Main.tile[x, y].HasTile && Main.tile[x, y + 1].HasUnactuatedTile && (!Main.tile[x, y + 1].IsHalfBlock && (int)Main.tile[x, y + 1].Slope == 0))
            {
                bool flag = false;
                if (style == 0)
                {
                    if ((int)Main.tile[x, y + 1].TileType != 2 && (int)Main.tile[x, y + 1].TileType != 78 && ((int)Main.tile[x, y + 1].TileType != 380 && (int)Main.tile[x, y + 1].TileType != 109))
                        flag = true;
                    if ((int)Main.tile[x, y].LiquidAmount > 0)
                        flag = true;
                }
                else if (style == 1)
                {
                    if ((int)Main.tile[x, y + 1].TileType != 60 && (int)Main.tile[x, y + 1].TileType != 78 && (int)Main.tile[x, y + 1].TileType != 380)
                        flag = true;
                    if ((int)Main.tile[x, y].LiquidAmount > 0)
                        flag = true;
                }
                else if (style == 2)
                {
                    if ((int)Main.tile[x, y + 1].TileType != 0 && (int)Main.tile[x, y + 1].TileType != 59 && ((int)Main.tile[x, y + 1].TileType != 78 && (int)Main.tile[x, y + 1].TileType != 380))
                        flag = true;
                    if ((int)Main.tile[x, y].LiquidAmount > 0)
                        flag = true;
                }
                else if (style == 3)
                {
                    if ((int)Main.tile[x, y + 1].TileType != 203 && (int)Main.tile[x, y + 1].TileType != 199 && ((int)Main.tile[x, y + 1].TileType != 23 && (int)Main.tile[x, y + 1].TileType != 25) && ((int)Main.tile[x, y + 1].TileType != 78 && (int)Main.tile[x, y + 1].TileType != 380))
                        flag = true;
                    if ((int)Main.tile[x, y].LiquidAmount > 0)
                        flag = true;
                }
                else if (style == 4)
                {
                    if ((int)Main.tile[x, y + 1].TileType != 53 && (int)Main.tile[x, y + 1].TileType != 78 && ((int)Main.tile[x, y + 1].TileType != 380 && (int)Main.tile[x, y + 1].TileType != 116))
                        flag = true;
                    if ((int)Main.tile[x, y].LiquidAmount > 0 && (Main.tile[x, y].LiquidType == LiquidID.Lava))
                        flag = true;
                }
                else if (style == 5)
                {
                    if ((int)Main.tile[x, y + 1].TileType != ModContent.TileType<ShaleBlock>() && (int)Main.tile[x, y + 1].TileType != 78 && (int)Main.tile[x, y + 1].TileType != ModContent.TileType<ShadowShrubPlanterBox>())
                        flag = true;
                    if ((int)Main.tile[x, y].LiquidAmount > 0 && !(Main.tile[x, y].LiquidType == LiquidID.Lava))
                        flag = true;
                }
                else if (style == 6)
                {
                    if ((int)Main.tile[x, y + 1].TileType != 78 && (int)Main.tile[x, y + 1].TileType != 380 && ((int)Main.tile[x, y + 1].TileType != 147 && (int)Main.tile[x, y + 1].TileType != 161) && ((int)Main.tile[x, y + 1].TileType != 163 && (int)Main.tile[x, y + 1].TileType != 164 && (int)Main.tile[x, y + 1].TileType != 200))
                        flag = true;
                    if ((int)Main.tile[x, y].LiquidAmount > 0 && (Main.tile[x, y].LiquidType == LiquidID.Lava))
                        flag = true;
                }
                if (!flag)
                {
                    tile.HasTile = true;
                    Main.tile[x, y].TileType = (ushort)82;
                    Main.tile[x, y].TileFrameX = (short)(18 * style);
                    Main.tile[x, y].TileFrameY = (short)0;
                    return true;
                }
            }
            return false;
        }
    }
}