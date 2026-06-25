using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BlackOps3.Content
{
    public class CameraSystem : ModSystem
    {
        // These variables store whether the camera is locked and where
        public static bool IsCameraLocked = false;
        public static Vector2 LockPosition;

        public override void ModifyScreenPosition() {
            if (IsCameraLocked) {
                // This forces the screen to stay at the LockPosition
                // We subtract half the screen width/height to center the point
                Main.screenPosition = LockPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            }
        }
    }
}