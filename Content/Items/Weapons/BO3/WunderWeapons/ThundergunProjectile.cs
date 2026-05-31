using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BoneTest.Content.Items.Tiles.Perks;
using Microsoft.Build.Evaluation;
namespace BoneTest.Content.Items.Weapons.BO3.WunderWeapons
{
    public class ThundergunProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BeachBall;
        private int counterDistance=0;
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true; // Allows it to hit enemies
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 300;
            Projectile.tileCollide = true; // Collides with walls/floors
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 600; 
            Projectile.aiStyle = 0;
            Projectile.spriteDirection = 1;
            Projectile.knockBack=500f;
            Projectile.scale=2f;
        }
        
        public override void AI()
        {
            Main.NewText(counterDistance);
            counterDistance++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if(counterDistance > 20)Projectile.damage= 15;
            if(counterDistance>=120) Projectile.Kill();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (counterDistance > 20)
            {
                Projectile.damage= 0;
            }
        }

    }
}

