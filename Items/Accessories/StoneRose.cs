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

namespace TheDepths.Items.Accessories
{
	[AutoloadEquip(new EquipType[] { EquipType.Face })]
	class StoneRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[Item.faceSlot] = true;
			DisplayName.SetDefault("Stone Rose");
			Tooltip.SetDefault("Reduced damage from Mercury debuffs");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.width = 10;
			Item.accessory = true;
			Item.value = 20000;
			Item.height = 10;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<TheDepthsPlayer>().stoneRose = true;
		}
	}
}