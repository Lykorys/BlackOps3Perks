using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
namespace BlackOps3.Content.Utils.Effects
{
    public class ScreenShake : ModSystem
    {
        private int shakeTime;
        private float shakeIntensity;
        
        public void startShake(int time, float intensity)
        {
            shakeTime = time;
            shakeIntensity = intensity;
        }
        
        public override void ModifyScreenPosition()
        {
            if (shakeTime > 0)
            {
                Main.screenPosition += new Vector2(
                    Main.rand.NextFloat(-shakeIntensity, shakeIntensity),
                    Main.rand.NextFloat(-shakeIntensity, shakeIntensity)
                );
                shakeTime--;
            }
        }
    }
}
