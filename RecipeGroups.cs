using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Items.Accessories;
using TheDepths.Items.Placeable;

namespace TheDepths
{
    internal class RecipeGroups : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.TerrasparkBoots);
                recipe.AddIngredient(ItemID.FrostsparkBoots)
                .AddIngredient(ModContent.ItemType<SilverSlippers>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            Recipe recipe2 = Recipe.Create(ItemID.LogicSensor_Liquid);
                recipe2.AddIngredient(ItemID.Cog, 5)
                .AddIngredient(ItemID.MagicWaterDropper)
                .AddIngredient(ModContent.ItemType<MagicQuicksilverDropper>())
                .AddIngredient(ItemID.MagicHoneyDropper)
                .AddIngredient(ItemID.Wire)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe recipe3 = Recipe.Create(ItemID.DryBomb);
                recipe3.AddIngredient(ModContent.ItemType<Items.Weapons.QuicksilverBomb>())
                .Register();
            Recipe recipe4 = Recipe.Create(ItemID.DryRocket);
                recipe4.AddIngredient(ModContent.ItemType<Items.Weapons.QuicksilverRocket>())
                .Register();
            Recipe recipe5 = Recipe.Create(ItemID.SeafoodDinner);
                recipe5.AddIngredient(ModContent.ItemType<Items.ShadowFightingFish>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
            Recipe recipe6 = Recipe.Create(ItemID.SeafoodDinner);
                recipe6.AddIngredient(ModContent.ItemType<Items.QuartzFeeder>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
            Recipe recipe7 = Recipe.Create(ItemID.PotionOfReturn);
                recipe7.AddIngredient(ModContent.ItemType<Items.QuartzFeeder>())
                .AddIngredient(ItemID.RecallPotion)
                .AddTile(TileID.AlchemyTable)
                .Register();

            Recipe recipe8 = Recipe.Create(ItemID.NightsEdge);
                recipe8.AddIngredient(ItemID.LightsBane)
                .AddIngredient(ItemID.Muramasa)
                .AddIngredient(ItemID.BladeofGrass)
                .AddIngredient(ModContent.ItemType<Items.Weapons.Terminex>())
                .AddTile(TileID.DemonAltar)
                .Register();
            Recipe recipe9 = Recipe.Create(ItemID.NightsEdge);
                recipe9.AddIngredient(ItemID.BloodButcherer)
                .AddIngredient(ItemID.Muramasa)
                .AddIngredient(ItemID.BladeofGrass)
                .AddIngredient(ModContent.ItemType<Items.Weapons.Terminex>())
                .AddTile(TileID.DemonAltar)
                .Register();
            Recipe recipe10 = Recipe.Create(ItemID.GenderChangePotion);
                recipe10.AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.Daybloom)
                .AddIngredient(ItemID.Moonglow)
                .AddIngredient(ItemID.Blinkroot)
                .AddIngredient(ItemID.Waterleaf)
                .AddIngredient(ItemID.Deathweed)
                .AddIngredient(ItemID.Shiverthorn)
                .AddIngredient(ModContent.ItemType<Items.ShadowShrub>())
                .AddTile(TileID.Bottles)
                .Register();
            Recipe recipe11 = Recipe.Create(ItemID.GarlandHat);
                recipe11.AddIngredient(ItemID.Daybloom)
                .AddIngredient(ItemID.Moonglow)
                .AddIngredient(ItemID.Blinkroot)
                .AddIngredient(ItemID.Waterleaf)
                .AddIngredient(ItemID.Deathweed)
                .AddIngredient(ItemID.Shiverthorn)
                .AddIngredient(ModContent.ItemType<Items.ShadowShrub>())
                .Register();
            Recipe recipe12 = Recipe.Create(ItemID.GravitationPotion);
                recipe12.AddIngredient(ItemID.BottledWater)
                .AddIngredient(ModContent.ItemType<Items.ShadowShrub>())
                .AddIngredient(ItemID.Deathweed)
                .AddIngredient(ItemID.Blinkroot)
                .AddIngredient(ItemID.Feather)
                .AddTile(TileID.Bottles)
                .Register();
            Recipe recipe13 = Recipe.Create(ItemID.TeleportationPotion);
                recipe13.AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.ChaosFish)
                .AddIngredient(ModContent.ItemType<Items.ShadowShrub>())
                .AddTile(TileID.Bottles)
                .Register();
            Recipe recipe14 = Recipe.Create(ItemID.BiomeSightPotion);
                recipe14.AddIngredient(ItemID.BottledWater)
                .AddIngredient(ModContent.ItemType<Items.ShadowShrub>())
                .AddIngredient(ItemID.Blinkroot)
                .AddIngredient(ItemID.Moonglow)
                .AddIngredient(ItemID.GrassSeeds, 5)
                .AddTile(TileID.Bottles)
                .Register();
            Recipe recipe15 = Recipe.Create(ItemID.DrillContainmentUnit);
                recipe15.AddIngredient(ItemID.LunarBar, 40)
                .AddIngredient(ItemID.ChlorophyteBar, 40)
                .AddIngredient(ItemID.ShroomiteBar, 40)
                .AddIngredient(ItemID.SpectreBar, 40)
                .AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 40)
                .AddIngredient(ItemID.MeteoriteBar, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe recipe16 = Recipe.Create(ItemID.AdamantiteForge);
                recipe16.AddIngredient(ItemID.AdamantiteOre, 30)
                .AddIngredient(ModContent.ItemType<Items.Placeable.Gemforge>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe recipe17 = Recipe.Create(ItemID.TitaniumForge);
                recipe17.AddIngredient(ItemID.TitaniumOre, 30)
                .AddIngredient(ModContent.ItemType<Items.Placeable.Gemforge>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}