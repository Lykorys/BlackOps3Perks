using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BoneTest.Content.Items.Tiles.Perks;
namespace BoneTest.Content.Items.Weapons.BO3.WunderWeapons
{
    public class RaygunMK2Projectile : ModProjectile
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
    }
}

