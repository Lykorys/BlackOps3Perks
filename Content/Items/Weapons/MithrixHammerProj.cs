using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class RainbowArcProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 4;
        Projectile.height = 4;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 120;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.alpha = 255; // Start invisible
    }

    public override void AI()
    {
        // Apply gravity for arc motion
        Projectile.velocity.Y += 0.3f; // Gravity
        
        // Fade in
        if (Projectile.alpha > 0)
            Projectile.alpha -= 15;
        
        // Create rainbow dust trail
        if (Main.rand.NextBool(2))
        {
            float rainbowHue = 0.65f; // Use ai[1] for color offset
            Color rainbowColor = Main.hslToRgb(rainbowHue, 1f, 0.5f);
            Dust dust = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.RainbowMk2,
                Vector2.Zero,
                100,
                rainbowColor,
                1.5f
            );
            dust.noGravity = true;
            dust.velocity *= 0.5f;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        // Draw rainbow trail
        float rainbowHue = Projectile.ai[1];
        rainbowHue += Main.GlobalTimeWrappedHourly * 0.5f;
        rainbowHue %= 1f;
        
        Color trailColor = Main.hslToRgb(rainbowHue, 1f, 0.5f);
        
        // You can draw a custom trail here if needed
        return true;
    }
}