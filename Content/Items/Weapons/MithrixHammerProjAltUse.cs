using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
namespace BoneTest.Content.Items.Weapons
{
    public class MithrixHammerProjAltUse : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.PhoenixBlaster;
        private int chargeTime = 0;
        private const int maxChargeTime = 60;
        
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = maxChargeTime + 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            // Stick to player
            Projectile.Center = player.Center;
            
            // Check if still channeling
            if (!player.channel || !player.HeldItem.channel)
            {
                ReleaseCharge(player);
                Projectile.Kill();
                return;
            }
            
            chargeTime++;
            
            // Charging effects
            if (chargeTime % 5 == 0)
            {
                Dust.NewDust(player.position, player.width, player.height, 
                    DustID.Electric, 0f, 0f, 100, default(Color), 1f + (chargeTime / 30f));
            }
            
            if (chargeTime == maxChargeTime)
            {
                SoundEngine.PlaySound(SoundID.MaxMana, player.position);
            }
            
            // Auto-release at max charge
            if (chargeTime >= maxChargeTime)
            {
                ReleaseCharge(player);
                Projectile.Kill();
            }
        }

        private void ReleaseCharge(Player player)
        {
            if (chargeTime < 10) return; // Minimum charge
            
            float chargePercent = (float)chargeTime / maxChargeTime;
            
            SoundEngine.PlaySound(SoundID.Item1, player.position);
            
            // Create powerful slam projectile
            Vector2 direction = player.DirectionTo(Main.MouseWorld);
            
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                player.Center,
                direction * (12f + chargePercent * 8f),
                ProjectileID.ShadowBeamFriendly, // Replace with slam effect
                (int)(50 * (1f + chargePercent * 1.5f)),
                10f * (1f + chargePercent),
                player.whoAmI
            );
        }
    }
}

