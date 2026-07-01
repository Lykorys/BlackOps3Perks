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
namespace BlackOps3.Content.Systems
{
    public abstract class WunderWeapon : Reloadable
    {
        public int ammoReserve;
        
        public override void SetDefaults() => shootSoundNumber = whenToPlaySound; 
        public override bool canReload(Player player) => !playerPerks.isReloading && ammoReserve>0 && ammo < magCapacity;
         
        public void removeBullets(int numberToRemove)
        {
            for(int _ = 0; _ < numberToRemove; _++)
            {
                ammo--;
            }
        }
        public override void reload(Player player)
        {
            playerPerks = player.GetModPlayer<PlayerPerks>();
            playerPerks.isReloading = true;
            int ammoToRemove = magCapacity - ammo;
            shootSoundNumber = whenToPlaySound;
            while (ammoToRemove > 0 && ammoReserve > 0) 
            {
                ammoReserve--;
                ammoToRemove--;
                ammo++;
            }
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
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
        public override void SaveData( TagCompound tag) { 
            tag["ammo"] = ammo;
            tag["ammoReserve"] = ammoReserve;
        }

        public override void LoadData( TagCompound tag) {
            if (tag.ContainsKey("ammo")) {
                ammo = tag.GetInt("ammo");
            }
            if (tag.ContainsKey("ammoReserve")) {
                ammoReserve = tag.GetInt("ammoReserve");
            }
        }

    }
}