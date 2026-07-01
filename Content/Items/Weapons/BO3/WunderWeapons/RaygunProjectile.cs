using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Items.Tiles.Perks;
namespace BlackOps3.Content.Items.Weapons.BO3.WunderWeapons
{
    public class RaygunProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ChlorophyteBullet;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true; // Allows it to hit enemies
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true; // Collides with walls/floors
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 600; 
            Projectile.aiStyle = 0;
            Projectile.spriteDirection = 1;
        }
        
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.position,Vector2.Zero,ProjectileID.MeteorShot,150,0f,-1);
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].friendly = true;
            return true;
        }

    }
}

