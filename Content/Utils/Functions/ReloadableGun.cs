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
{
    public class ReloadableGun : GlobalItem
    {
        
        private PlayerPerks playerPerks;
        private int chargeTimer = 0;
        public int reloadTime;
        public int ammo = 0;
        public int maxAmmo;
        private int maxDefaultAmmo;
        public bool isReloading = false;
        public List<int> loadedBullets = new List<int>();
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
            maxDefaultAmmo=maxAmmo;
        }
        public override void SaveData(Item item, TagCompound tag) {
            if (IsReloadable) {
                tag["ammo"] = ammo;
                tag["bullets"] = loadedBullets;
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.ContainsKey("ammo")) {
                ammo = tag.GetInt("ammo");
            }
            if (tag.ContainsKey("bullets")) {
                loadedBullets = (List<int>)tag.GetList<int>("bullets");
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            playerPerks = player.GetModPlayer<PlayerPerks>();
            if(maxAmmo==maxDefaultAmmo && playerPerks.HasPerk("MuleKick")) maxAmmo=(int)(maxAmmo*playerPerks.magSizeMult);
            if (KeybindSystem.Reload.JustPressed) {
                if (!isReloading && ammo <maxAmmo) {
                    reload(player); 
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
                playerPerks.isReloading=false;
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
        public void removeBullets()
        {
            loadedBullets.RemoveAt(0);
            ammo--;
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
        public void reload(Player player)
        {
            if (!IsReloadable) return;
            isReloading=true;
            int ammoToRemove = maxAmmo-ammo;
            shootSoundNumber=whenToPlaySound;
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
                    if(bullet.consumable) bullet.stack-=1;
                }
            }
        }
        public override void PostDrawInInventory(Item item,SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if (!IsReloadable) return;
            Player player = Main.LocalPlayer;
            int totalReserves = 0;
            foreach (Item invItem in player.inventory) {
                if (!invItem.IsAir && invItem.ammo == AmmoID.Bullet) {
                    totalReserves += invItem.stack;
                }
            }
            string magText = $"{ammo}";
            string reserveText = $"{totalReserves}";
            float textScale = scale * 1.1f; 
            float ratio = (float)ammo / maxAmmo;
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