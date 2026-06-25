using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Players;
using System.Security.Permissions;
using Terraria.GameContent.ObjectInteractions;

namespace BlackOps3.Content.Utils.Functions
{
    public abstract class PerkMachine : ModTile
    {
        public abstract Perk perk {get;}
        public abstract int[] prices {get;}
        private int priceIndex = 0;
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            PlayerPerks modPlayer = player.GetModPlayer<PlayerPerks>();
            if(priceIndex < prices.Length)
            {
                if (modPlayer.zombieMoney >= prices[priceIndex])
                {
                    if (modPlayer.HasPerk(perk.perkName))
                    {
                        modPlayer.ActivePerks[perk.perkName].tier++;
                        modPlayer.zombieMoney-=prices[priceIndex];
                        priceIndex++;
                        Main.NewText(priceIndex,Color.Blue);
                        
                    }
                    else
                    {
                        modPlayer.AddPerk(perk);
                        modPlayer.zombieMoney-=prices[priceIndex];
                        priceIndex=1;
                        Main.NewText(priceIndex,Color.Black);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item3, player.position);
                    }
                }
            }
            else
            {
                if (!modPlayer.HasPerk(perk.perkName))
                {
                    priceIndex=0;
                    if (modPlayer.zombieMoney >= prices[priceIndex])
                    {
                        modPlayer.AddPerk(perk);
                        modPlayer.zombieMoney-=prices[priceIndex];
                        priceIndex=1;
                        Main.NewText(priceIndex,Color.Black);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item3, player.position);
                    }
                } 
            }
            Main.NewText(priceIndex,Color.Red);
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