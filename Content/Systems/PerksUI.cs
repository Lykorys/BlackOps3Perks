using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using BlackOps3.Content.Players;
using Terraria.DataStructures;
using System.Linq;

namespace BlackOps3.Content.Systems
{
    public class PerksUI : ModSystem
    {
        
        private int numberOfPerks = 0;
        private int nbOfPerksInALine = 8;//TODO fix ui for more than this nb of perks
        private int offsetLeft = 150;
        private int offsetBottom = 150;
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            
            PlayerPerks perks = Main.LocalPlayer.GetModPlayer<PlayerPerks>();
            numberOfPerks=0;
            foreach (var perk in perks.ActivePerks.Values)
            {
                if(perk.perkLogo != null)
                {
                    drawPerks(perk.perkLogo, spriteBatch);
                    numberOfPerks++;
                }
            }
        }
        public void drawPerks( Texture2D img, SpriteBatch spriteBatch)
        {
            if (numberOfPerks > nbOfPerksInALine)
            {
                numberOfPerks-=nbOfPerksInALine; 
                offsetBottom-=150;
            } 
            
            Vector2 position = new Vector2(offsetLeft+numberOfPerks*32,Main.screenHeight-offsetBottom);

            spriteBatch.Draw(img, position, null, Color.White, 0f, img.Size() / 2f, 1f, SpriteEffects.None, 0f);

            
        }
    }
}