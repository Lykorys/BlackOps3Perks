using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Players;
using System.Security.Permissions;
using Terraria.GameContent.ObjectInteractions;
using System.Linq;

namespace BlackOps3.Content.Utils.Functions
{
    public abstract class PerkMachine : ModTile
    {
        public abstract Perk perk {get;}
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            PlayerPerks modPlayer = player.GetModPlayer<PlayerPerks>();
            if (modPlayer.HasPerk(perk.perkName))
            {
                Perk playerPerk =modPlayer.ActivePerks[perk.perkName];
                if(playerPerk.tier < perk.prices.Length && modPlayer.zombieMoney >= perk.prices[playerPerk.tier])
                {
                    modPlayer.zombieMoney-=perk.prices[playerPerk.tier];
                    playerPerk.tier++;
                    if(playerPerk.tier==playerPerk.maxTier) Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37, player.position);
                    else Terraria.Audio.SoundEngine.PlaySound(SoundID.Tink, player.position);
                    Main.NewText(playerPerk.tier,Color.Blue);
                }
            }
            else
            {
                if(modPlayer.ActivePerks.Count == modPlayer.maxPerks) return false;
                if(modPlayer.ActivePerks.Count < modPlayer.maxPerks && modPlayer.zombieMoney >= perk.prices[0] )
                {
                    modPlayer.AddPerk(perk);
                    modPlayer.zombieMoney-=perk.prices[0];
                    modPlayer.ActivePerks[perk.perkName].tier=1;
                    Main.NewText(modPlayer.ActivePerks[perk.perkName].tier,Color.Black);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item3, player.position);
                }
            }
            return true;
        }

        
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) 
        {
            return true;
        }
        public override void MouseOver(int i, int j) {
            Player player = Main.LocalPlayer;
            player.noThrow = 2; 
        }
    }
}