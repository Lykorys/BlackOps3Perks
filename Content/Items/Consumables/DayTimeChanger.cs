using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoneTest.Content.Items.Consumables
{
	public class DayTimeChanger : ModItem
	{
		private bool midDayMode = false;

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

		public override bool AltFunctionUse(Player player) => true;
        
		public override bool? UseItem(Player player) {
			if(player.altFunctionUse == 2)
			{
				midDayMode= !midDayMode;
				SoundEngine.PlaySound(SoundID.Roar,player.position);
			}
			
			else
			{
				if (player.whoAmI == Main.myPlayer) {
					if (midDayMode)
					{
						if (Main.dayTime)
						{
							Main.dayTime= false;
							Main.time=Main.nightLength/2;//on passe a la nuit
						}
						else
						{
							Main.dayTime= true;
							Main.time=Main.dayLength/2;
						}
					}
					else
					{
						if (Main.dayTime)
						{
							Main.dayTime= false;
							Main.time=0;//on passe a la nuit
						}
						else
						{
							Main.dayTime= true;
							Main.time=0;
						}
					}
					SoundEngine.PlaySound(SoundID.CoinPickup, player.position);
					// Sync of world data on the server in MP
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
			return true;
		}
	}
}
