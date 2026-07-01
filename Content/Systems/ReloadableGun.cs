using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;
using BlackOps3.Content.Systems;
using BlackOps3.Content.Players;
using Terraria.DataStructures;
namespace BlackOps3.Content.Systems
{
    public abstract class ReloadableGun : Reloadable
    {
        public List<int> loadedBullets = new List<int>();
        public override void SetDefaults() => shootSoundNumber = whenToPlaySound; 

        public void removeBullets()
        {
            loadedBullets.RemoveAt(0);
            ammo--;
        }
        public override void reload(Player player)
        {

            playerPerks.isReloading = true;
            int ammoToRemove = magCapacity-ammo;
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
        public override bool canReload(Player player) => !playerPerks.isReloading && GetTotalReserve(player)>0 && ammo < magCapacity;
        
        
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
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
        public override bool Shoot( Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           if (ammo > 0 && loadedBullets.Count > 0) {
                Projectile.NewProjectile(source, position, velocity, loadedBullets[0], damage, knockback, player.whoAmI);
                playSound();
                removeBullets();
            }
            else SoundEngine.PlaySound(SoundID.MenuTick, player.position);
            return false;
        }
        public override void SaveData(TagCompound tag) {

            tag["ammo"] = ammo;
            tag["bullets"] = loadedBullets;
            
        }

        public override void LoadData(TagCompound tag) {
            if (tag.ContainsKey("ammo")) {
                ammo = tag.GetInt("ammo");
            }
            if (tag.ContainsKey("bullets")) {
                loadedBullets = (List<int>)tag.GetList<int>("bullets");
            }
        }

    }
}