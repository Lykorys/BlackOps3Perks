using System; 
using System.Collections.Generic;
using BoneTest.Content.Utils.Effects;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using BoneTest.Content.Config;

namespace BoneTest.Content.NPCs.Bosses.BossesAIs
{
    public class MithrixAi
    {
        private static BossConfig config = ModContent.GetInstance<BossConfig>();
        private static bool switchedPhase = false;
        private static bool hasTPd= false;
        private static float phaseSwitchTreshold = 0f; 
        private static bool hasHitPlayer= false;
        private static bool disabledContactDmg= false;
        private static float speedFirstStage = 15f;
        /*--Attack Stats--*/
        private static float phaseDelayMultiplier = 1;
        private static float phaseSpeedMultiplier = 1;
        private static float speedSecondStage = 20f;

        private static int[] projList = new int[5];
        public static void Ai(NPC npc,Mod mod){
            Main.rainTime = 18000; // Sets rain duration (ticks)
            Main.raining = true;   // Turns rain on
            Main.maxRaining = 1f;  // Sets the intensity (0.0 to 1.0)
            // Get a target
            if (npc.target < 0 || npc.target == Main.maxPlayers || Main.player[npc.target].dead || !Main.player[npc.target].active){
				npc.TargetClosest();
			}
                
            // Target variable
            Player player = Main.player[npc.target];
			
			if (player.dead) {
				// If the targeted player is dead, flee
				npc.velocity.Y -= 0.04f;
				// This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
				npc.EncourageDespawn(10);
                Main.rainTime = 0;
                Main.raining = false;
                Main.maxRaining = 0f;
				return;
			}
            if (!switchedPhase && npc.life < npc.lifeMax * phaseSwitchTreshold && npc.ai[1] == 0)
            {
                switchedPhase=true;
                phaseDelayMultiplier=0.8f;
                phaseSpeedMultiplier=1.4f;
                SoundEngine.PlaySound(SoundID.DrumCymbal1, player.position);
            }
            if(!switchedPhase)
            {
                doFirstStage(player,npc);
            }

            
        }
		


/*
#############################################
#        First Stage Functions              #
#                                           #
#############################################
*/
        public static void doFirstStage(Player player,NPC boss)
        {
            // 2. Stay on Ground and Maintain Distance
            float targetDistance = 300f; // The "Fixed Distance" you want
            float movementSpeed = 0.2f;  // How fast he accelerates
            float maxSpeed = 6f;         // Top walking speed

            // Determine which side of the player the boss should be on
            Vector2 targetPos = player.Center;
            targetPos.X += (boss.Center.X < player.Center.X) ? -targetDistance : targetDistance;

            // Horizontal Movement
            if (boss.Center.X < targetPos.X) {
                boss.velocity.X += movementSpeed;
                if (boss.velocity.X < 0) boss.velocity.X += movementSpeed; // Friction assist
            } else {
                boss.velocity.X -= movementSpeed;
                if (boss.velocity.X > 0) boss.velocity.X -= movementSpeed;
            }

            // Cap horizontal speed and apply gravity
            boss.velocity.X = MathHelper.Clamp(boss.velocity.X, -maxSpeed, maxSpeed);
            boss.velocity.Y += 0.4f; // Standard gravity
            boss.ai[0]++;
            if (boss.ai[3] == 0)
            { 
                boss.ai[3] = Main.rand.NextBool() ? 1 : -1;
            }
            if(config.forcedPhaseMithrix!=0)
            {
                boss.ai[1]=config.ForcedAttackMithrix;
            }
            switch (boss.ai[1])
            {
                case 0:
                    int waitTime = 200;
                    if (boss.ai[0] >= waitTime)
                    {
                        boss.ai[1] = Main.rand.Next(1,3);//attack that he does
                        boss.ai[0] = 0; // Reset timer
                    }
                    
                break;
            
                case 1://dashatk
                    pizzaCutter(player,boss);
                    break;
                case 2://slam
                    hammerSlam(player,boss);
                    break; 
            }
        }
        public static void hammerSlam(Player player,NPC boss)
        {
            boss.ai[0]++;
            if (boss.ai[0] < 180)
            {
                if (projList.Length < 6)
                {
                    int proj = Projectile.NewProjectile(boss.GetSource_FromAI(),player.Center,new Vector2(-16f,0),ProjectileID.StarCannonStar,15,2f);
                    projList.Append(proj);
                }
                foreach(int elm in projList){
                    Projectile proj = Main.projectile[projList[elm]];
                    bool offScreenX = proj.position.X < Main.screenPosition.X || proj.position.X > Main.screenPosition.X + Main.screenWidth;
                    if(offScreenX) proj.position = new Vector2(Main.screenPosition.X,player.Center.Y);
                }   
            }
            else
            {
                boss.ai[0]=0;
                boss.ai[1]=0;
                boss.ai[3]=0;
            }
        }
        public static void pizzaCutter(Player player,NPC boss)
        {
            boss.ai[0]++;
            if (boss.ai[0] > 30 && boss.ai[0] <180)
            {
                
            }

            /*
            afk
            4 a 5 zones safe et le restes les pilliers de mithrix
            
            */
        }
    }
}
