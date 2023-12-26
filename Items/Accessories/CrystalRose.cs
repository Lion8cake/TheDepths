using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using TheDepths.Buffs;

namespace TheDepths.Items.Accessories
{
	[AutoloadEquip(new EquipType[] { EquipType.Face })]
	public class CrystalRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Face.Sets.DrawInFaceHeadLayer[Item.faceSlot] = true;
			//ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[Item.faceSlot] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Pink;
			Item.width = 20;
			Item.accessory = true;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.height = 26;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<TheDepthsPlayer>().stoneRose = true;
			player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<StoneRose>());
			recipe.AddIngredient(ModContent.ItemType<CrystalSkull>());
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}