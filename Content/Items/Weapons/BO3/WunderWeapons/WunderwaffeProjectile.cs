using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace BlackOps3.Content.Items.Weapons.BO3.WunderWeapons
{
    public class WunderwaffeProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MartianTurretBolt;
        private List<int> hittedTargets = new List<int>();
        private int maxChains = 10;
        private float maxDistanceRange = 400;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true; // Allows it to hit enemies
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 10; // Disappears after 1 hit
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hittedTargets.Add(target.whoAmI);
            if (hittedTargets.Count < maxChains)
            {
                goToNextTarget();
            }
            else
            {
                Main.NewText("Max Chains Reached!", Color.Red);
                Projectile.Kill();
            }
        }
        private void goToNextTarget()
        {
            NPC closest = null;
            float closestDist = maxDistanceRange;
            foreach(NPC npc in Main.npc)
            {
                if(npc.CanBeChasedBy()&& !hittedTargets.Contains(npc.whoAmI))
                {
                    float dist = Vector2.Distance(Projectile.Center,npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist= dist;
                        closest=npc;
                    }
                }
            }
            if (closest != null)
            {
                Vector2 direction = closest.Center - Projectile.Center;
                direction.Normalize();
                Projectile.velocity = direction * 15f;
                SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
            }
            else
            {
                Projectile.Kill();
            }
        }
    }
}

