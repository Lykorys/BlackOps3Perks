using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using BoneTest.Content.Players;
using Terraria.DataStructures;

namespace BoneTest.Content.Systems
{
    public class PerksUI : ModSystem
    {
        
        private int numberOfPerks = 0;
        private int nbOfPerksInALine = 8;
        private int offsetLeft = 150;
        private int offsetBottom = 150;
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            PlayerPerks perks = Main.LocalPlayer.GetModPlayer<PlayerPerks>();
            if(perks.hasJug) drawPerks(ModContent.Request<Texture2D>("BoneTest/Content/Items/Tiles/Perks/Jug64").Value,spriteBatch);
            
        }
        public void drawPerks( Texture2D img, SpriteBatch spriteBatch)
        {
            if (numberOfPerks > 8)
            {
                numberOfPerks=0; 
                offsetBottom+=150;
            } 
            
            Vector2 position = new Vector2(offsetLeft+numberOfPerks*150,Main.screenHeight-offsetBottom);

            spriteBatch.Draw(img, position, null, Color.White, 0f, img.Size() / 2f, 1f, SpriteEffects.None, 0f);

            
        }
    }
}