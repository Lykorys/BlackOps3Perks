using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using BlackOps3.Content.Players;
namespace BlackOps3.Content.Items.Consumables
{
	public class SelfKiller : ModItem
	{
        public override string Texture => "Terraria/Images/Item_" + ItemID.Pho;
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool? UseItem(Player player) {
			player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{player.name} pressed the big red button.")), 9999, 0);
            player.dead = false;
            player.respawnTimer = 0;
            player.ghost = false;
            PlayerPerks modPlayer = player.GetModPlayer<PlayerPerks>();
            modPlayer.zombieMoney+=100000;
            return true;
        }
	}
}
