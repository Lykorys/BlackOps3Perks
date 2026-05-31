using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BoneTest.Content.Config
{
    public class MyVideoMenu : ModMenu
    {
        // Replace with your actual frame count after resizing!
        private const int TotalFrames = 48; 
        private int frameTimer = 0;
        private int currentFrame = 0;

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            // Load your vertical strip from your mod's 'Assets' folder
            Texture2D texture = ModContent.Request<Texture2D>("BoneTest/Content/Menu").Value;

            // Calculate height of a single frame
            int frameHeight = texture.Height / TotalFrames;

            // Animation Logic: Change frame every 5 ticks (60 ticks = 1 second)
            frameTimer++;
            if (frameTimer >= 5) {
                currentFrame = (currentFrame + 1) % TotalFrames;
                frameTimer = 0;
            }

            // The 'Source Rectangle' picks exactly ONE frame from your long strip
            Rectangle sourceRect = new Rectangle(0, currentFrame * frameHeight, texture.Width, frameHeight);

            // Draw to fill the entire screen
            spriteBatch.Draw(texture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), sourceRect, Color.White);

            return true; // Return true to keep the Terraria Logo visible on top
        }
    }
}