using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BoneTest.Content.Config // Make sure this matches your mod's namespace
{
    public class BossConfig : ModConfig
    {
        // ConfigScope.ServerSide ensures it works in multiplayer
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("BossAIAttackFP")]
        [DefaultValue(0)]
        [Range(0, 4)]
        public int ForcedAttackHandFP;
        [Header("BossAIAttackSP")]
        [DefaultValue(0)]
        [Range(0, 5)]
        public int ForcedAttackHandSP;

        [Header("BossAIStartPhase")]
        [DefaultValue(0)]
        [Range(0, 2)]
        public int forcedPhaseHand;


        [Header("MithrixAISettings")]
        [DefaultValue(0)]
        [Range(0, 2)] // 0 = Random, 1 = Dash, 2 = Slam
        public int ForcedAttackMithrix;
        [Header("MithrixAIStartPhase")]
        [DefaultValue(0)]
        [Range(0, 2)]
        public int forcedPhaseMithrix;
        [Header("projspeed")]
        [Range(0f, 15f)]        // Ensure you use 'f' to specify float
        [Increment(0.25f)]      // This defines how much the slider moves per "tick"
        [DefaultValue(5f)]      // A default of 0 might make the projectiles stationary!
        public float projspeed;
        
        
        [Header("hue")]
        [Range(0f, 1f)]        // Ensure you use 'f' to specify float
        [Increment(0.05f)]      // This defines how much the slider moves per "tick"
        [DefaultValue(0f)]      // A default of 0 might make the projectiles stationary!
        public float hue;


    }
}