using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using BlackOps3.Content.Players;
namespace BlackOps3.Content.Items.Consumables
{
	public class Moneygiver : ModItem
	{
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool? UseItem(Player player) {
            PlayerPerks modPlayer = player.GetModPlayer<PlayerPerks>();
            modPlayer.zombieMoney+=100000;
            return true;
        }
	}
}
