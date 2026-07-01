using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BlackOps3.Content.Config;
using BlackOps3.Content.Players;
using Humanizer;
using Terraria.DataStructures;
namespace BlackOps3.Content.Systems
{
    public abstract class Reloadable : ModItem
    {
        public PlayerPerks playerPerks;
        public int chargeTimer = 0;
        public int reloadTime;
        public int ammo = 0;
        public int magCapacity;
        public int maxDefaultAmmo;
        public SoundStyle? reloadSound;
        public SoundStyle shootSound;
        public bool hasPlayedReloadSound = false; 
        //Use it if your sound is a burst and not a single shot if single shot value= 1 and values should never be negative
        public int whenToPlaySound=1;
        public int shootSoundNumber;
        public abstract bool canReload(Player player);
        public override void AutoDefaults() => maxDefaultAmmo = magCapacity; 
        public override void HoldItem(Player player)
        {
            playerPerks = player.GetModPlayer<PlayerPerks>();
            if(maxDefaultAmmo==0) maxDefaultAmmo=magCapacity;
            magCapacity = (int)(maxDefaultAmmo * playerPerks.magSizeMult);
            if (KeybindSystem.Reload.JustPressed && !playerPerks.isReloading) {
                if (canReload(player)) {
                    reload(player);
                    chargeTimer = 0;
                }
            }

            if (!playerPerks.isReloading) return;

            if (reloadSound.HasValue && !hasPlayedReloadSound) {
                hasPlayedReloadSound = true;
                SoundEngine.PlaySound(reloadSound.Value, player.position);
            }
            player.itemTime = 2;
            player.itemAnimation = 2;

            if (chargeTimer < reloadTime)
            {
                chargeTimer++;
            }
            else
            {
                chargeTimer = 0;
                playerPerks.isReloading = false;
                hasPlayedReloadSound = false;
            }
        }
        public override bool CanUseItem(Player player)
        {
            playerPerks = player.GetModPlayer<PlayerPerks>();
            if (playerPerks.isReloading) return false;
            return true;
        }
        public void playSound()
        {
            if(shootSoundNumber==whenToPlaySound)
            {
                SoundEngine.PlaySound(shootSound, Main.LocalPlayer.position);
                shootSoundNumber=1;
            }
            else
            {
                shootSoundNumber++;
            }
            
        }
        public abstract void reload(Player player);
        public int GetTotalReserve(Player player)
        {
            int totalReserves = 0;
            foreach (Item invItem in player.inventory) {
                if (!invItem.IsAir && invItem.ammo == AmmoID.Bullet) {
                    totalReserves += invItem.stack;
                }
            }
            return totalReserves;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            Player player = Main.LocalPlayer;
            int totalReserves = GetTotalReserve(player);
            string magText = $"{ammo}";
            string reserveText = $"{totalReserves}";
            float textScale = scale * 1.1f; 
            float ratio = (float)ammo / magCapacity;
            Color magColor = Color.Lerp(new Color(150, 0, 0), Color.White, ratio);
            float slotWidth = 52f * scale;
            Vector2 slotTopLeft = position - (new Vector2(26f, 26f) * scale);
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
            Vector2 reserveSize = FontAssets.ItemStack.Value.MeasureString(reserveText) * textScale;
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