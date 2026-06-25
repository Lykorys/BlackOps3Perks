using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
namespace BlackOps3.Content.Players
{
    public abstract class Perk
    {
        
        public string perkName => GetType().Name;
        public int tier = 1;
        public abstract Texture2D perkLogo {get;}
        public abstract void ApplyEffect(PlayerPerks playerPerks);
    }
    public class DoubleTap : Perk
    {
        /*
        Tier 1 : 
        Tier 2 :
        Tier 3 : 
        Tier 4 : 
        Tier 5 : 
        */
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/DoubleTapLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.Player.GetAttackSpeed(DamageClass.Generic) += 0.3f;
        }
    }

    public class ElectricCherry : Perk
    {
        /*
        Tier 1 : Electric Aura
        Tier 2 : Speed boost after reload
        Tier 3 : Electric debuff to ennemies on hit DOT
        Tier 4 : Stun ennemies
        Tier 5 : 
        */
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/ElectricCherryLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {   
            
        }
    }

    public class Juggernog : Perk
    {
        /*
        Tier 1 : +5 defense
        Tier 2 : +50% kb resistance
        Tier 3 : 
        Tier 4 : +100hp
        Tier 5 : +15% de DR
        */
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/JuggernogLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.endurance+=0.15f;
            if (tier >= 2)
            {
                perkPlayer.Player.moveSpeed+=5f;
            } 
            
        }
    }

    public class MuleKick : Perk
    {
        /*
        Tier 1 : 
        Tier 2 :
        Tier 3 : 
        Tier 4 : 
        Tier 5 : 
        */
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/MuleKickLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.magSizeMult += 0.3f;
        }
    }

    public class QuickRevive : Perk
    {  
        /*
        Tier 1 : 
        Tier 2 :
        Tier 3 : 
        Tier 4 : 
        Tier 5 : 
        */
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/QuickReviveLogo").Value;
        


        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
        }
    }

    public class SpeedCola : Perk
    {
        /*
        Tier 1 : 
        Tier 2 :
        Tier 3 : 
        Tier 4 : 
        Tier 5 : 
        */
        public override Texture2D perkLogo =>ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/SpeedColaLogo").Value;

        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.Player.moveSpeed += 0.5f;
        }
    }

    public class StaminUp : Perk
    {
        /*
        Tier 1 : 10% speed boost
        Tier 2 : 10% acceleration 
        Tier 3 : 10% max speed
        Tier 4 : longer dash size
        Tier 5 : 
        */
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/StaminUpLogo").Value;

        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.Player.moveSpeed *= 1.21f;
        }
    }

    public class WidowsWine : Perk
    {
 
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BlackOps3/Content/Players/PerksLogo/ElectricCherryLogo").Value;
        
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
        }
    }
}