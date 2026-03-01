using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BoneTest.Content.Items.Consumables
{
	public class CamLockTest : ModItem
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
            // Toggle the lock
            CameraSystem.IsCameraLocked = !CameraSystem.IsCameraLocked;
            if (CameraSystem.IsCameraLocked) {
                // Set the lock point to the player's current center
                CameraSystem.LockPosition = player.Center;
            }
            return true;
        }
	}
}
