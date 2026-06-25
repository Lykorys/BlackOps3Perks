using BlackOps3.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlackOps3.Content.NPCs
{
    public class ZombieMoney : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (!NPCID.Sets.Zombies[npc.type]) return;
            if (!npc.AnyInteractions()) return;

            Player player = Main.player[npc.lastInteraction];
            PlayerPerks perks = player.GetModPlayer<PlayerPerks>();
            perks.zombieMoney+=100;
            Main.NewText("You killed a zombie!", 50, 255, 50);
        }
    }
}