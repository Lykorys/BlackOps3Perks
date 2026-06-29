using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
namespace BlackOps3.Content.Players
{
    public abstract class Perk
    {
        
        public string perkName => GetType().Name;
        public int tier = 1;
        public abstract int maxTier {get;}
        public abstract int[] prices {get;}
        public abstract Texture2D perkLogo {get;}
        public abstract void ApplyEffect(PlayerPerks playerPerks);
        public virtual void OnHitNPCWithProj(PlayerPerks playerPerks, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void OnHitNPCWithItem(PlayerPerks playerPerks, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void OnHurt(PlayerPerks playerPerks, Player.HurtInfo hurtInfo) { }
        public virtual void OnKillNPC(PlayerPerks playerPerks, NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual bool PreKill(PlayerPerks playerPerks, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGlowDust, ref PlayerDeathReason damageSource) => true;
        public virtual void OnReloadStart(PlayerPerks playerPerks) { }
        public virtual void DuringReload(PlayerPerks playerPerks) { }
    }
    public class DoubleTap : Perk
    {
        /*
        Tier 1 : 30% fire rate
        Tier 2 : 10% dmg increase
        Tier 3 : 10% crit chance
        Tier 4 : +10 def pene
        Tier 5 : double all projectile
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/DoubleTapLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.GetAttackSpeed(DamageClass.Generic) += 0.3f;
            if(tier>=2)perkPlayer.Player.GetDamage(DamageClass.Generic) += 0.1f;
            if(tier>=3)perkPlayer.Player.GetCritChance(DamageClass.Generic) += 0.1f;
            if(tier>=4)perkPlayer.Player.GetArmorPenetration(DamageClass.Generic) += 10;
            if(tier>=5)perkPlayer.doubleAllProjectiles = true;//TODO
            
        }
    }

    public class ElectricCherry : Perk
    {
        /*
        Tier 1 : Electric Aura
        Tier 2 : Defense boost during reload
        Tier 3 : Electric debuff to ennemies on hit DOT
        Tier 4 : Stun ennemies
        Tier 5 : Aura can chain lightning up to 3 ennemies TODO
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/ElectricCherryLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer){}
        public override void OnHitNPCWithProj(PlayerPerks playerPerks, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) => HandleOnHit(playerPerks, target);
        public override void OnHitNPCWithItem(PlayerPerks playerPerks, Item item, NPC target, NPC.HitInfo hit, int damageDone) => HandleOnHit(playerPerks, target);

        private void HandleOnHit(PlayerPerks perkPlayer, NPC target)
        {
            if (tier >= 3) target.AddBuff(BuffID.Electrified, 300); 
            if (tier >= 4 && Main.rand.NextBool(5)) target.AddBuff(ModContent.BuffType<ElectricCherryStunDebuff>(), 120);
        }
        public override void DuringReload(PlayerPerks playerPerks)
        {
            if (tier >= 2) 
            {
                playerPerks.Player.statDefense += 10;
            } 
        }
        public override void OnReloadStart(PlayerPerks perkPlayer)
        {
            if (tier >= 1)
            {
                int baseDamage = (tier >= 5) ? 250 : 150;
                Player player = perkPlayer.Player;

                int boltCount = Main.rand.Next(3, 5);

                for (int i = 0; i < boltCount; i++)
                {
                    float randomXOffset = Main.rand.Next(-40, 40);
                    float randomYOffset = Main.rand.Next(-10, 40);
                    Vector2 spawnPosition = player.Center + new Vector2(randomXOffset, randomYOffset)- new Vector2(4f, 20f);

                    int projIndex = Projectile.NewProjectile(
                        player.GetSource_FromThis(),
                        spawnPosition,
                        Vector2.Zero,
                        ModContent.ProjectileType<ElectricAuraProj>(),
                        0,
                        0f,
                        player.whoAmI,
                        player.whoAmI,
                        randomXOffset
                    );

                    if (projIndex != Main.maxProjectiles)
                    {
                        Main.projectile[projIndex].localAI[0] = randomYOffset;
                    }
                }

                float damageRadius = 200f;
                SpawnRadiusVisual(player.Center, damageRadius);

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.damage > 0 && !npc.dontTakeDamage)
                    {
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance <= damageRadius)
                        {
                            NPC.HitInfo hitInfo = new NPC.HitInfo
                            {
                                Damage = baseDamage,
                                Knockback = 4f,
                                HitDirection = (npc.Center.X > player.Center.X) ? 1 : -1,
                                Crit = Main.rand.Next(1, 101) <= player.GetCritChance(DamageClass.Ranged)
                            };

                            npc.StrikeNPC(hitInfo);

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendStrikeNPC(npc, hitInfo);
                            }
                        }
                    }
                }
            }
        }

        private void SpawnRadiusVisual(Vector2 center, float radius)
        {
            int dustCount = 40;
            for (int i = 0; i < dustCount; i++)
            {
                float angle = MathHelper.TwoPi * i / dustCount;
                Vector2 dustPos = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;

                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Electric, Vector2.Zero, 0, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.velocity = Vector2.Zero;
            }
        }
    }

    public class Juggernog : Perk
    {
        /*
        Tier 1 : +5 defense
        Tier 2 : +25 hp
        Tier 3 : +10% life regen increase
        Tier 4 : no kb
        Tier 5 : +15% de DR
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/JuggernogLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.statDefense+=5;
            if(tier>=2)perkPlayer.Player.statLifeMax2+=25;
            if(tier>=3)perkPlayer.Player.lifeRegen+= (int)(1.10f *perkPlayer.Player.lifeRegen);
            if(tier>=4)perkPlayer.Player.noKnockback=true;
            if(tier>=5)perkPlayer.Player.endurance+=0.15f;
        }
    }

    public class MuleKick : Perk
    {
        /*
        Tier 1 : Faster mining speed
        Tier 2 : NPCs cost less
        Tier 3 : Increase interaction range
        Tier 4 : Pylons don't need npcs anymore
        Tier 5 : Upon death coins are kept
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/MuleKickLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.moveSpeed+=0.15f;
            if(tier>=2)perkPlayer.Player.discountAvailable = true;
            if(tier>=3)perkPlayer.Player.blockRange += 3;;
            if(tier>=4)perkPlayer.NoNPCPylons = true;
            if(tier>=5)perkPlayer.keepCoinsOnDeath = true;
        }
    }

    public class QuickRevive : Perk
    {  
        /*
        Tier 1 : One revive
        Tier 2 : Temporary speed boost after revive
        Tier 3 : Temporary hp boost upon revive
        Tier 4 : revive at full hp
        Tier 5 : keep all perks upon revival
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/QuickReviveLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            
        }
        public override bool PreKill(PlayerPerks playerPerks, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGlowDust, ref PlayerDeathReason damageSource)
        {
            
            if(tier>=1)
            {
                if(tier>=2) playerPerks.Player.AddBuff(BuffID.Swiftness, 600);
                if(tier>=3) playerPerks.Player.AddBuff(BuffID.Lifeforce, 300);
                if(tier>=4) playerPerks.Player.statLife=playerPerks.Player.statLifeMax2;
                if(tier<5)  playerPerks.ClearPerks();
                playerPerks.ActivePerks.Remove("QuickRevive");
                return false;
            }
            return true;
        }
        
    }

    public class SpeedCola : Perk
    {
        /*
        Tier 1 : Quicker reload
        Tier 2 : Chance to not consume ammo upon reload
        Tier 3 : Speed boost upon reload
        Tier 4 : Bigger mag cap
        Tier 5 : Upon reload drop the mag and create a mine at the player pos || Upon kill chance to gain back ammo
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo =>ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/SpeedColaLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.reloadSpeed+=0.15f;
            if(tier>=2)perkPlayer.ammoSaveChance += 0.20f;
            if(tier>=3 && perkPlayer.isReloading)perkPlayer.Player.moveSpeed += 0.20f;
            if(tier>=4)perkPlayer.magSizeMult += 0.3f;
            if(tier>=5);//TODO decide
        }
    }

    public class StaminUp : Perk
    {
        /*
        Tier 1 : 10% speed boost
        Tier 2 : 10% acceleration 
        Tier 3 : Immune to slow debuff
        Tier 4 : longer dash size
        Tier 5 : Quicker dash reset
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/StaminUpLogo").Value;

        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.moveSpeed+=0.15f;
            if(tier>=2)perkPlayer.Player.runAcceleration+=0.10f;
            if(tier>=3)
            {
                perkPlayer.Player.buffImmune[BuffID.Slow] = true;
                perkPlayer.Player.buffImmune[BuffID.Chilled] = true;
            }
            if(tier>=4)perkPlayer.dashLengthMod += 0.25f;
            if(tier>=5)perkPlayer.dashCooldownMod -= 0.20f;
            
        }
    }

    public class WidowsWine : Perk
    {
        /*
        Tier 1 : x% de chance de spawn une araignée qui ralenti et empoisonné les ennemies (4 ou 5 max et faut que ça soit comme les araignée du spider staff)
        Tier 2 : +1 max spawn et +x% de taux de spawn
        Tier 3 : donne l'abilité de s'attacher au mur et ameliore la vitesse de rétractation du grapin 
        Tier 4 : +2 max spawn 
        Tier 5 : ameliore l'effet de poison et de ralentissement. Si vous utilisez une armure summon gagnez +0.2hp/s pour chaque araignée vivante 
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/ElectricCherryLogo").Value;
        
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1);
            if(tier>=2);
            if(tier>=3);
            if(tier>=4);
            if(tier>=5);
        }
    }
}