using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BlackOps3.Content.Config
{
    public class MyVideoMenu : ModMenu{
        private const int TotalFrames = 50;   // 54 slots minus the 4 empty ones
        private const int GridColumns = 6;    // 6 columns across
        private const int GridRows = 9;       // 9 rows down
        
        private int frameTimer = 0;
        private int currentFrame = 0;
        private Asset<Texture2D> menuTexture;
        private int currentMusicIndex = 0;
        private int[] musicSlots;
        private bool oldMousePressed = false;
        public override int Music => (musicSlots != null && musicSlots.Length > 0) ? musicSlots[1] : base.Music;
        public override void Load()
        {
            menuTexture = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Config/Menu");
            musicSlots = new int[]
            {
                MusicLoader.GetMusicSlot(Mod, "Music/Menu_Damned"),
                MusicLoader.GetMusicSlot(Mod, "Music/Menu_WaterFront")
            };
        }

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Main.newMusic = musicSlots[currentMusicIndex];
            if (menuTexture == null || !menuTexture.IsLoaded)
                return true;

            Texture2D texture = menuTexture.Value;

            // Divide the total sheet dimensions by your grid layout
            int frameWidth = texture.Width / GridColumns;
            int frameHeight = texture.Height / GridRows;

            // Animation Ticker (Changes frame every 5 ticks)
            frameTimer++;
            if (frameTimer >= 17) {
                currentFrame = (currentFrame + 1) % TotalFrames; // Loops at 50, skipping the 4 empty slots
                frameTimer = 0;
            }
            
            // Math to find the current X (column) and Y (row) position on the grid
            int column = currentFrame % GridColumns;
            int row = currentFrame / GridColumns;

            // Define the source rectangle mapping to the correct grid slot
            Rectangle sourceRect = new Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight);

            // Draw stretched across the screen
            spriteBatch.Draw(texture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), sourceRect, Color.White);

            return true; 
        }
    }
}