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
using Terraria.Localization;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class OnyxRobe : ModItem
	{
	    public override void SetStaticDefaults()
		{
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 14;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 300000;
			Item.defense = 16;
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes) {
			robes = true;
			equipSlot = EquipLoader.GetEquipSlot(Mod, "OnyxRobe_Legs", EquipType.Legs);
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Type && (head.type == ItemID.WizardHat || head.type == ItemID.MagicHat);
		}
		public override void UpdateArmorSet(Player player)
		{
			if (player.head == 14)
			{
				player.setBonus = Language.GetTextValue("Mods.TheDepths.SetBonus.OnyxRobeWizard");
				player.GetCritChance(DamageClass.Magic) += 18;
				player.statDefense += 2;
			}
			else if (player.head == 159)
			{
				player.setBonus = Language.GetTextValue("Mods.TheDepths.SetBonus.OnyxRobeMagic");
				player.statManaMax2 += 100;
				player.statDefense += 3;
			}
		}

		public override void UpdateEquip(Player player)
		{
			player.manaCost -= 0.22f;
			player.statManaMax2 += 60;
			player.hasGemRobe = true;
		}
	}
}
