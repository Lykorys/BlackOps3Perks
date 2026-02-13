using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoneTest.Content.Items.Consumables
{
	public class DayTimeChanger : ModItem
	{

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
			if (player.whoAmI == Main.myPlayer) {
				// If the player using the item is the client
				// (explicitly excluded serverside here)
				SoundEngine.PlaySound(SoundID.CoinPickup, player.position);
                if (Main.dayTime)
                {
                    Main.NewText("Il fait nuit", Color.Yellow);
                    Main.dayTime= false;
                    Main.time=0;//on passe a la nuit
                }
                else
                {
                    Main.NewText("Il fait jour", Color.AntiqueWhite);
                    Main.dayTime= true;
                    Main.time=Main.dayLength/2;
                }
                // Sync of world data on the server in MP
                if (Main.netMode == NetmodeID.Server) {
                    NetMessage.SendData(MessageID.WorldData);
                }
			}
            Main.NewText(Main.dayTime, Color.Gray);
            Main.NewText(Main.time, Color.Red);
			return true;
		}
	}
}
