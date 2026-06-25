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
namespace BlackOps3.Content.Utils.Functions
{//todo fix you can reload with no bullet and reload exced max ammo
    public class WunderWeapon : GlobalItem
    {
        private PlayerPerks playerPerks;
        private int chargeTimer = 0;
        public int reloadTime;
        public int ammo = 0;
        public int magCapacity;
        private int maxDefaultAmmo;
        public int ammoReserve = 0;
        public bool isReloading = false;
        public override bool InstancePerEntity => true;
        public bool IsReloadable = false;
        public SoundStyle? reloadSound;
        public SoundStyle shootSound;
        private bool hasPlayedReloadSound = false; 
        //Use it if your sound is a burst and not a single shot if single shot value= 1 and values should never be negative
        public int whenToPlaySound=1;
        private int shootSoundNumber;
        public override void SetDefaults(Item entity)
        {
            shootSoundNumber=whenToPlaySound;
            maxDefaultAmmo=magCapacity;
        }
        public override void SaveData(Item item, TagCompound tag) {
            if (IsReloadable) {
                tag["ammo"] = ammo;
                tag["ammoReserve"] = ammoReserve;
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.ContainsKey("ammo")) {
                ammo = tag.GetInt("ammo");
            }
            if (tag.ContainsKey("ammoReserve")) {
                ammoReserve = tag.GetInt("ammoReserve");
            }
        }
        public override void HoldItem(Item item, Player player)
        {

            if (KeybindSystem.Reload.JustPressed) {
                playerPerks ??= player.GetModPlayer<PlayerPerks>();
                if(magCapacity==maxDefaultAmmo && playerPerks.HasPerk("MuleKick")) magCapacity= (int)(magCapacity*playerPerks.magSizeMult);
                if (!isReloading && ammo <magCapacity) {
                    reload(); 
                }
            }
            if (!IsReloadable||!isReloading) return;
            if (isReloading)
            {
                playerPerks ??= player.GetModPlayer<PlayerPerks>();
                if(chargeTimer==5) playerPerks.isReloading=true;
                if (reloadSound.HasValue&& ! hasPlayedReloadSound) {
                    hasPlayedReloadSound=true;
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
                    isReloading = false;
                    hasPlayedReloadSound=false;
                }
            }
            else
            {
                if (playerPerks != null) playerPerks.isReloading=false;
                chargeTimer = 0;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (isReloading) return false;
            if (ammo <= 0) {
                SoundEngine.PlaySound(SoundID.MenuTick, player.position);
                return true;
            }

            return true;
        }
        public void removeBullets(int numberToRemove)
        {
            for(int _ = 0; _ < numberToRemove; _++)
            {
                ammo--;
            }
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
        public void reload()
        {
            if (!IsReloadable) return;
            isReloading=true;
            int ammoToRemove = magCapacity-ammo;
            shootSoundNumber=whenToPlaySound;
            while (ammoToRemove > 0 && ammoReserve>0) 
            {
                ammoReserve--;
                ammoToRemove--;
                ammo++;
            }
        }
        public override void PostDrawInInventory(Item item,SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if (!IsReloadable) return;
            string magText = $"{ammo}";
            string reserveText = $"{ammoReserve}";
            float textScale = scale * 1.5f; 
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