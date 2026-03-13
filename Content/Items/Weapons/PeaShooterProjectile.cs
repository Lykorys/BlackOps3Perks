using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
namespace BoneTest.Content.Items.Weapons
{
    public class PeaShooterProjectile : ModProjectile
    {
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true; // Allows it to hit enemies
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1; // Disappears after 1 hit
            Projectile.tileCollide = true; // Collides with walls/floors
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 600; 
            Projectile.aiStyle = 0;
            Projectile.spriteDirection = 1;
        }
    }
}

