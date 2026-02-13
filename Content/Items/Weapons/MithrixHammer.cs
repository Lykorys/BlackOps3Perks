using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Build.Evaluation;

namespace BoneTest.Content.Items.Weapons
{
    public class MithrixHammer : ModItem
    {
        public int chargeTimer = 0;
        public const int MaxCharge = 60;
        private bool isCharged = false;
        private int baseDamage = 80;

        public override void SetDefaults()
        {
            Item.damage = baseDamage;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.shoot=ProjectileID.HallowBossLastingRainbow;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            // If we are charged and Left Clicking (Standard Use)
            if (isCharged && player.altFunctionUse == 2)
            {
                chargedAttack(player);
                isCharged = false; // Reset after use
                chargeTimer = 0;
                return true; 
            }
            else
            {
                return true;
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            // Position and Rotate the hammer behind the head during charging
            if (player.controlUseTile && !isCharged)
            {
                // Move pivot to shoulder height
                player.itemLocation = player.MountedCenter + new Vector2(-4f * player.direction, -12f);
                
                // Rotation: -2.0f is pointing back and up
                player.itemRotation = -2.0f * player.direction;
            }
        }

        public override void HoldItem(Player player)
        {
            // Handle Right Click charging
            if (player.controlUseTile && !player.mouseInterface && !isCharged)
            {
                chargeTimer++;

                // Keep player in "using" state visually
                player.itemTime = 2;
                player.itemAnimation = 2;

                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustDirect(player.position, player.width, player.height, DustID.Electric, 0, 0, 100, default, 0.7f).noGravity = true;
                }

                if (chargeTimer >= MaxCharge)
                {
                    isCharged = true;
                    chargeTimer=0;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, player.position);
                    
                    // Visual "Pop" to show it's ready
                    for(int i = 0; i < 10; i++) 
                        Dust.NewDustDirect(player.position, player.width, player.height, DustID.Electric, 0, 0, 100, default, 1.2f).noGravity = true;
                }
            }
            else
            {
                chargeTimer=0;
            }
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            // Ensure the hammer head itself doesn't do damage if we don't want it to
            // This is optional since you are shooting bullets.
            if (player.altFunctionUse == 2) 
            {
                modifiers.FinalDamage *= 0;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        { 
           for(int i = 0; i < 4; i++)
            {
                Projectile.NewProjectile(
                    source,
                    player.Center,
                    arcVelocity,
                    ModContent.ProjectileType<RainbowArcProjectile>(), // Use your custom projectile
                    damage,
                    knockback,
                    player.whoAmI,
                    0
                );
            }
            return false;
        }
        private void chargedAttack(Player player)
        {
            Vector2 velocity = Main.MouseWorld - player.Center;
            velocity.Normalize();
            // Use baseDamage * 3 so it scales correctly
            Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, velocity * 14f, ProjectileID.LunarFlare, baseDamage * 3, 10f, player.whoAmI);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, player.position);
        }
    }
}