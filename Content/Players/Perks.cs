using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
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
            if(tier>=5);
            
        }
    }

    public class ElectricCherry : Perk
    {
        /*
        Tier 1 : Electric Aura
        Tier 2 : Speed boost after reload
        Tier 3 : Electric debuff to ennemies on hit DOT
        Tier 4 : Stun ennemies
        Tier 5 : Aura can chain lightning up to 3 ennemies
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/ElectricCherryLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if (tier >= 2) if (perkPlayer.isReloading)
            {
                perkPlayer.Player.AddBuff(ModContent.BuffType<ElectricCherryBuff>(),5);
                Main.NewText("boost");
            } 
            if(tier>=3);
            if(tier>=4);
            if(tier>=5);
        }
    }

    public class Juggernog : Perk
    {
        /*
        Tier 1 : +5 defense
        Tier 2 : +50% kb resistance
        Tier 3 : +10% life regen increase
        Tier 4 : +100hp
        Tier 5 : +15% de DR
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/JuggernogLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.statDefense+=5;
            if(tier>=2);
            if(tier>=3)perkPlayer.Player.lifeRegen+= (int)(1.10f *perkPlayer.Player.lifeRegen);
            if(tier>=4)perkPlayer.Player.statLifeMax2+=100;
            if(tier>=5)perkPlayer.Player.endurance+=0.15f;
            
        }
    }

    public class MuleKick : Perk
    {
        /*
        Tier 1 : Faster mining speed
        Tier 2 : NPCs cost less
        Tier 3 : Increase interaction range
        Tier 4 : Bigger pylon range 
        Tier 5 : Upon death coins are kept
        */
        public override int maxTier { get; } = 5;
        public override int[] prices { get; } = [500, 1500, 3000, 4500, 6000];
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/MuleKickLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.moveSpeed+=0.15f;
            if(tier>=2)perkPlayer.Player.runAcceleration+=0.10f;
            if(tier>=3);
            if(tier>=4);
            if(tier>=5);
            perkPlayer.magSizeMult += 0.3f;
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
            if(tier>=1);
            if(tier>=2);
            if(tier>=3);
            if(tier>=4);
            if(tier>=5);
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
            if(tier>=2)perkPlayer.Player.runAcceleration+=0.10f;
            if(tier>=3);
            if(tier>=4);
            if(tier>=5);
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
            if(tier>=3);
            if(tier>=4);
            if(tier>=5);
            
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