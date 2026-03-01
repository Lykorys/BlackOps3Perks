
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO; // Required for TagCompound

namespace BoneTest.Content.Utils.Functions
{
    public class ReloadableGun : GlobalItem
    {
        public int chargeTimer = 0;
        public int reloadTime = (int)(60*1.9);
        public int ammo = 0;
        public int maxAmmo= 8;
        public bool isReloading = false;
        public List<int> loadedBullets = new List<int>();
        public override bool InstancePerEntity => true;
        public bool IsReloadable = false;
        public SoundStyle? reloadSound;
        public override void SaveData(Item item, TagCompound tag) {
        // Only save data if the item is actually one of our guns
        if (IsReloadable) {
            tag["ammo"] = ammo;
            tag["bullets"] = loadedBullets;
        }
        }

        public override void LoadData(Item item, TagCompound tag) {
            // Check if the keys exist before loading to avoid errors
            if (tag.ContainsKey("ammo")) {
                ammo = tag.GetInt("ammo");
            }

            if (tag.ContainsKey("bullets")) {
                // Cast the saved list back into our local list
                loadedBullets = (List<int>)tag.GetList<int>("bullets");
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            if (!IsReloadable||!isReloading) return;
            if (isReloading)
            {
                // Set itemTime and itemAnimation to 2 every frame to prevent the player 
                // from starting a new 'Use' (shooting) while the reload is active.
                player.itemTime = 2;
                player.itemAnimation = 2;

                if (chargeTimer < reloadTime)
                {
                    chargeTimer++;
                }
                else
                {
                    chargeTimer = 0;
                    isReloading = false;
                    if (reloadSound.HasValue) {
                        SoundEngine.PlaySound(reloadSound.Value, player.position);
                    }
                }
            }
            else
            {
                chargeTimer = 0;
            }
        }
        public void reload(Player player)
        {
            if (!IsReloadable) return;
            isReloading=true;
            int ammoToRemove = maxAmmo-ammo;
            int slot = AmmoFinderSystem.GetFirstBulletSlot(player);
            Item bullet = player.inventory[slot];
            while (ammoToRemove != 0 && slot!=-1) 
            {
                
                if (bullet.stack == 0)
                {
                    bullet.TurnToAir();
                    slot = AmmoFinderSystem.GetFirstBulletSlot(player);
                    bullet = player.inventory[slot];
                }
                else
                {
                    ammoToRemove-=1;
                    ammo++;
                    loadedBullets.Insert(0,bullet.shoot);
                    bullet.stack-=1;
                }
            }
        }
        public override void PostDrawInInventory(Item item,SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if (!IsReloadable) return;
            Player player = Main.LocalPlayer;

            // 1. Calculate total reserve ammo
            int totalReserves = 0;
            foreach (Item invItem in player.inventory) {
                if (!invItem.IsAir && invItem.ammo == AmmoID.Bullet) {
                    totalReserves += invItem.stack;
                }
            }

            // 2. Setup text and scale
            string magText = $"{ammo}";
            string reserveText = $"{totalReserves}";
            float textScale = scale * 1.1f; 

            // Color logic
            float ratio = (float)ammo / maxAmmo;
            Color magColor = Color.Lerp(new Color(150, 0, 0), Color.White, ratio);

            // 3. Find the Slot Corners
            // Since 'position' is often the sprite center, we offset based on the standard 52px slot size
            float slotWidth = 52f * scale;
            Vector2 slotTopLeft = position - (new Vector2(26f, 26f) * scale); // Center - HalfWidth

            // 4. Draw Current Ammo (Bottom-Left)
            // Offset 4px from left, 10px UP from the bottom edge
            Vector2 magPos = slotTopLeft + new Vector2(4f * scale, 34f * scale); 
            Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch, 
                FontAssets.ItemStack.Value, 
                magText, 
                magPos, 
                magColor, 
                0f, 
                Vector2.Zero, 
                new Vector2(textScale)
            );

            textScale = scale * 0.8f; 
            // 5. Draw Reserves (Bottom-Right)
            Vector2 reserveSize = FontAssets.ItemStack.Value.MeasureString(reserveText) * textScale;
            
            // Position at Right Edge - String Width - 4px padding
            Vector2 reservePos = slotTopLeft + new Vector2(slotWidth - reserveSize.X - 4f * scale, 34f * scale);
            
            Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch, 
                FontAssets.ItemStack.Value, 
                reserveText, 
                reservePos, 
                Color.White * 0.9f, 
                0f, 
                Vector2.Zero, 
                new Vector2(textScale)
            );
        }
    }
}