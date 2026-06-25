using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using BlackOps3.Content.Players;

namespace BlackOps3.Content.Systems
{
    public class ZombieMoney : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            PlayerPerks perks = Main.LocalPlayer.GetModPlayer<PlayerPerks>();

            string moneyText = $"{perks.zombieMoney}";

            Terraria.Utils.DrawBorderStringFourWay(
                spriteBatch,
                FontAssets.MouseText.Value,
                moneyText,
                20f,
                Main.screenHeight * 0.95f,
                Color.Yellow,
                Color.Black,
                Vector2.Zero
            );
        }
    }
}