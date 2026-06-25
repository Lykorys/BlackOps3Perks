using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlackOps3.Content
{
    public class AmmoFinderSystem : ModSystem
    {
        /// <summary>
        /// Finds all inventory slots containing a specific ammo type.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <param name="ammoType">The AmmoID (e.g., AmmoID.Bullet).</param>
        /// <returns>A list of integers representing slot indexes (0-57).</returns>
        public static int[] GetAllBulletSlots(Player player) {
            int[] foundSlots = [];
            int[] scanOrder = new int[58];
            for (int i = 0; i < 4; i++) scanOrder[i] = 54 + i; // Ammo Slots
            for (int i = 0; i < 54; i++) scanOrder[i + 4] = i; // Main Inventory

            foreach (int slot in scanOrder) {
                Item item = player.inventory[slot];
                if (!item.IsAir && item.ammo == AmmoID.Bullet && item.stack > 0) {
                    foundSlots.Append(slot);
                }
            }
            return foundSlots;
        }
        public static int GetFirstBulletSlot(Player player)
        {
            int[] scanOrder = new int[58];
            for (int i = 0; i < 4; i++) scanOrder[i] = 54 + i; // Ammo Slots
            for (int i = 0; i < 54; i++) scanOrder[i + 4] = i; // Main Inventory

            foreach (int slot in scanOrder) {
                Item item = player.inventory[slot];
                if (!item.IsAir && item.ammo == AmmoID.Bullet && item.stack > 0) {
                    return slot;
                }
            }
            return -1;
        }
    }
}