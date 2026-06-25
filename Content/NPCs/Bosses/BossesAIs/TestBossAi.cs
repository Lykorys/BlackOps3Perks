using System; 
using System.Collections.Generic;
using BlackOps3.Content.Utils.Effects;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Config;

namespace BlackOps3.Content.NPCs.Bosses.BossesAIs
{
    public class TestBossAI
    {
        public static BossConfig config = ModContent.GetInstance<BossConfig>();
        private static bool switchedPhase = false;
        private static bool hasTPd= false;
        private static float phaseSwitchTreshold = 0.5f; 
        private static bool hasHitPlayer= false;
        private static bool disabledContactDmg= false;

        /*--Attack Stats--*/
        private static float phaseDelayMultiplier = 1;
        private static float phaseSpeedMultiplier = 1;
        /*-First Stage*/
        private static float speedFirstStage = 15f;
        /*Dash attack*/
        private static float dashSpeed =25f;
        private static int dashGracePeriod = 180; 
        private static int dashAttackPeriod = dashGracePeriod +dashGracePeriod /6;
        /*Dash attack*/
        private static float slamSpeed =30f;
        private static int slamGracePeriod = 180; 
        private static int slamAttackPeriod = slamGracePeriod + slamGracePeriod /6;
        /*Grab attack*/
        private static int grabGracePeriod = 180;
        private static int grabAttackPeriod = grabGracePeriod+grabGracePeriod/6;
        private static int grabFinisherPeriod = grabAttackPeriod+60;
        /*-Second Stage*/
        private static float speedSecondStage = 20f;
        /*PFC*/
        private static float PFCDistanceFromPlayer = 400f;
        public static void VanillaTestBossAI(NPC npc,Mod mod){
            // Get a target
            if (npc.target < 0 || npc.target == Main.maxPlayers || Main.player[npc.target].dead || !Main.player[npc.target].active){
				npc.TargetClosest();
			}
                
            // Target variable
            Player player = Main.player[npc.target];
			bool canSwitch= config.forcedPhaseHand == 0;
			if (player.dead) {
				// If the targeted player is dead, flee
				npc.velocity.Y -= 0.04f;
				// This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
				npc.EncourageDespawn(10);
				return;
			}
            if (config.forcedPhaseHand == 1)
            {
                switchedPhase=false;
            }
            else if(config.forcedPhaseHand==2)
            {
                switchedPhase=true;                
            }
            if (!switchedPhase && npc.life < npc.lifeMax * phaseSwitchTreshold && npc.ai[1] == 0 && canSwitch)
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
            else
            {
                doSecondStage(player,npc);
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
            boss.ai[0]++;
            if (boss.ai[3] == 0)
            { 
                boss.ai[3] = Main.rand.NextBool() ? 1 : -1;
            }

            if (config.ForcedAttackHandFP != 0)
            {
                boss.ai[1]=config.ForcedAttackHandFP;
            }
            switch (boss.ai[1])
            {
                case 0://hover
                    // Calculate the target position (200 units above the player)
                    Vector2 targetPosition = player.Center + new Vector2(0, -300);
                    
                    // Calculate the direction vector from boss to target
                    Vector2 direction = targetPosition - boss.Center;

                    
                    float inertia = 20f;
                    
                    // Normalize the direction and apply speed
                    if (direction.Length() > speedFirstStage)
                    {
                        direction.Normalize();
                        direction *= speedFirstStage;
                    }
                    
                    // Smooth movement using inertia
                    boss.velocity = (boss.velocity * (inertia - 1) + direction) / inertia;
                    
                    // Random wait time between 15-20 seconds (1050-300 ticks)
                    //int waitTime = Main.rand.Next(1050, 1500);
                    int waitTime = 200;
                    if (boss.ai[0] >= waitTime)
                    {
                        if(config.ForcedAttackHandFP != 0) boss.ai[1]=config.ForcedAttackHandFP;
                        else  boss.ai[1] = Main.rand.Next(1,3);//attack that he does
                        boss.ai[0] = 0; // Reset timer
                    }
                    
                break;
            
                case 1://dashatk
                    sideDashAttack(player,boss);
                    break;
                case 2://slam
                    slamAttack(player,boss);
                    break; 
            }
        }


        public static void sideDashAttack(Player player,NPC boss)
        {
            if(boss.ai[0] < dashGracePeriod * phaseDelayMultiplier)
            {
                Vector2 targetPosition = new Vector2(player.Center.X + 375 * boss.ai[3], player.Center.Y);
                float targetRotation = MathHelper.PiOver2*boss.ai[3];
                if (targetPosition != boss.position)
                {
                    boss.Center=targetPosition;
                    if (!hasTPd)
                    {
                        hasTPd=true;
                        Dust.NewDust(boss.position, boss.width, boss.height, DustID.Shadowflame);
                    }
                }

                boss.rotation = targetRotation;
            }
            else if (boss.ai[0] < dashAttackPeriod * phaseDelayMultiplier)
            {
                if (boss.ai[0] == dashGracePeriod * phaseDelayMultiplier)//Dash
                {
                    Vector2 dashDirection = (player.Center - boss.Center )*2;
                    dashDirection.Normalize();
                    boss.velocity = dashDirection * dashSpeed * phaseSpeedMultiplier; // Dash speed
                }
                
            }
            else
            {
                boss.ai[0]=0;
                boss.ai[1]=0;
                boss.ai[3]=0;
                boss.rotation=0f;
                hasTPd=false;
            }
        }


        public static void slamAttack(Player player,NPC boss)
        {
            if (boss.ai[0] < slamGracePeriod *phaseDelayMultiplier)
            {
                Vector2 targetPosition = new Vector2(player.Center.X, player.Center.Y - 500f);
                Vector2 move = targetPosition - boss.Center;
                if (move.Length() > speedFirstStage)
                {
                    move.Normalize();
                    move *= speedFirstStage;
                }

                boss.velocity = move;
            }
            else if (boss.ai[0] < slamAttackPeriod *phaseDelayMultiplier)
            {
                if (boss.ai[0] < slamGracePeriod *phaseDelayMultiplier)
                {
                    boss.velocity=Vector2.Zero;
                }
                Point playerTilePos = player.Center.ToTileCoordinates();
                int groundY = playerTilePos.Y;
                
                // Search downward for solid tile
                for (int y = playerTilePos.Y; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[playerTilePos.X, y];
                    if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType])
                    {
                        groundY = y;
                        break;
                    }
                }
                if (boss.ai[0] == 180 * phaseDelayMultiplier)
                {
                    if (boss.Bottom.Y < groundY*16)
                    {
                        // Accelerate downward
                        boss.velocity.X = 0;
                        boss.velocity.Y = slamSpeed * phaseSpeedMultiplier;
                    }
                }

                if(boss.Bottom.Y >= groundY * 16 )//atteint le sol
                {
                    
                    float projectileSpeed = 12f;
                    int projectileType = ProjectileID.SpikedSlimeSpike;
                    int projectileDamage =40;
                    for( int i = 0; i < 13; i++)
                    {
                        float angle = MathHelper.Lerp(MathHelper.TwoPi,MathHelper.Pi, i / 12f);
                        Vector2 projectileVelocity = new Vector2(
                            (float)Math.Cos(angle) * projectileSpeed,
                            (float)Math.Sin(angle) * projectileSpeed
                        );
                        Projectile.NewProjectile(boss.GetSource_FromAI(),boss.Center,projectileVelocity,projectileType,projectileDamage,2f);
                    }
                    int proj1 = Projectile.NewProjectile(boss.GetSource_FromAI(),boss.Center,new Vector2(16f,0),ProjectileID.StarCannonStar,projectileDamage,2f,-1,0,1f);
                    int proj2 = Projectile.NewProjectile(boss.GetSource_FromAI(),boss.Center,new Vector2(-16f,0),ProjectileID.StarCannonStar,projectileDamage,2f,-1,0,1f);
                    if (proj1 >= 0 && proj1 < Main.maxProjectiles)
                    {
                        Main.projectile[proj1].hostile = true;
                        Main.projectile[proj1].friendly = false;
                    }
                    if (proj2 >= 0 && proj2 < Main.maxProjectiles)
                    {
                        Main.projectile[proj2].hostile = true;
                        Main.projectile[proj2].friendly = false;
                    }
                    boss.ai[0]=211;
                    boss.velocity= Vector2.Zero;
                    ModContent.GetInstance<ScreenShake>().startShake(10, 15f);
                }
                
                
            }
            else
            {
                boss.ai[0]=0;
                boss.ai[1]=0;
                boss.ai[3]=0;
            }
            
        }

        public static void grabAttack(Player player,NPC boss)
        {
            int bdmg=boss.damage;
            boss.damage=0;
            int gGPdelayed= (int)(grabGracePeriod *phaseDelayMultiplier);
            int gAPdelayed= (int)(grabAttackPeriod *phaseDelayMultiplier);
            if (boss.ai[0] < gGPdelayed)
            {
                Vector2 targetPosition = new Vector2(player.Center.X + 450 * boss.ai[3], player.Center.Y-300);
                float targetRotation = MathHelper.PiOver2*boss.ai[3];
                

                if (targetPosition != boss.position)
                {
                    boss.Center=targetPosition;
                    if (!hasTPd)
                    {
                        hasTPd=true;
                    }
                }

                boss.rotation = targetRotation;
            }
            else if (boss.ai[0] < gAPdelayed)
            {
                Rectangle bossHitbox = new Rectangle((int)boss.position.X, (int)boss.position.Y, boss.width, boss.height);
                Rectangle playerHitbox = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                
                if (bossHitbox.Intersects(playerHitbox))
                {
                    hasHitPlayer = true;
                }
                if (boss.ai[0] == grabGracePeriod * phaseDelayMultiplier)//Dash
                {
                    Vector2 dashDirection = (player.Center - boss.Center )*2;
                    dashDirection.Normalize();
                    boss.velocity = dashDirection * dashSpeed * phaseSpeedMultiplier; // Dash speed
                    
                }
                if (hasHitPlayer)
                    {
                        player.position = boss.position;
                    }
            }
            else if (hasHitPlayer && boss.ai[0] < grabFinisherPeriod)
            {
                boss.damage=bdmg;
                player.velocity=Vector2.Zero;
                Vector2 move = player.Center - boss.Center;
                if (move.Length() > 60f)
                {
                    move.Normalize();
                    move *= 60f;
                }
                boss.velocity = move;
            }
            else
            {
                hasHitPlayer=false;
                boss.ai[0]=0;
                boss.ai[1]=0;
                boss.ai[3]=0;
                boss.rotation=0f;
                hasTPd=false;
            }
            /*
            le boss tape un laser ou dash dans le joeur immobile 
            attaque qui fait bcp de degats
            */
        }
/*
#############################################
#        Second Stage Functions             #
#                                           #
#############################################
*/
        public static void doSecondStage(Player player,NPC boss)
        {
            if (boss.ai[3] == 0)
            { 
                boss.ai[3] = Main.rand.NextBool() ? 1 : -1;
            }
            
        
            boss.ai[0]++;
            switch (boss.ai[1])
            {
                case 0://hover
                    // Calculate the target position (200 units above the player)
                    Vector2 targetPosition = player.Center + new Vector2(0, -300);
                    
                    // Calculate the direction vector from boss to target
                    Vector2 direction = targetPosition - boss.Center;
                    
                    float inertia = 20f;
                    
                    // Normalize the direction and apply speedSecondStage
                    if (direction.Length() > speedSecondStage)
                    {
                        direction.Normalize();
                        direction *= speedSecondStage;
                    }
                    
                    // Smooth movement using inertia
                    boss.velocity = (boss.velocity * (inertia - 1) + direction) / inertia;
                    
                    // Random wait time between 15-20 seconds (1050-300 ticks)
                    //int waitTime = Main.rand.Next(1050, 1500);
                    int waitTime = 200;
                    if (boss.ai[0] >= waitTime)
                    {
                        if(config.ForcedAttackHandSP != 0) boss.ai[1]=config.ForcedAttackHandSP;
                        else  boss.ai[1] = Main.rand.Next(1,7);//attack that he does
                        boss.ai[0] = 0; // Reset timer
                    }
                    
                break;
                case 1://dashatk
                    sideDashAttack(player,boss);
                    break;
                case 2://slam
                    slamAttack(player,boss);
                    break; 
                case 3:
                    pistolAttack(player,boss);
                    break;
                case 4:
                case 5:
                case 6: 
                    if(boss.ai[3]==0) boss.ai[3] = Main.rand.Next(1,4);
                    pfcAttack(player,boss);
                    break;
            }
        }


        public static void pistolAttack(Player player,NPC boss)
        {
            if(boss.ai[0] < 210)
            {
                Vector2 targetPosition = new Vector2(player.Center.X + 500 * boss.ai[3], player.Center.Y);
                Vector2 direction = targetPosition - boss.Center;
                float targetRotation = MathHelper.PiOver2*boss.ai[3];

                if (targetPosition != boss.Center)
                {
                    boss.Center=targetPosition;
                    if (!hasTPd)
                    {
                        hasTPd=true;
                        Dust.NewDust(boss.position, boss.width, boss.height, DustID.Shadowflame);
                        Dust.NewDust(boss.position, boss.width+1, boss.height, DustID.Shadowflame);
                        Dust.NewDust(boss.position, boss.width+1, boss.height+1, DustID.Shadowflame);
                        Dust.NewDust(boss.position, boss.width, boss.height+1, DustID.Shadowflame);
                    }
                }
                float rotationDifference = targetRotation - boss.rotation;
                boss.rotation += rotationDifference * 1f;
                int projectileDamage = 40;
                float projectileSpeed =30f;
                int proj =-1;
                List<float> firingCadence = new List<float>{125,140,155,170,185,200};
                if (firingCadence.Contains(boss.ai[0])){//Fire
                    {
                        proj = Projectile.NewProjectile(boss.GetSource_FromAI(),boss.Center,new Vector2(projectileSpeed*-boss.ai[3],0),ProjectileID.StarCannonStar,projectileDamage,2f,-1,0,1f);
                    }
                    if (proj >= 0 && proj < Main.maxProjectiles)
                    {
                        Main.projectile[proj].hostile = true;
                        Main.projectile[proj].friendly = false;
                    }
                }
            }
            else
            {
                boss.ai[0]=0;
                boss.ai[1]=0;
                boss.ai[3]=0;
                boss.rotation=0f;
                hasTPd=false;
            }
        }


        public static void pfcAttack(Player player,NPC boss)
        {
            if (boss.ai[0] < 180)
            {
                switch (boss.ai[3])
                {
                    case 1://Pierre
                        boss.velocity=Vector2.Zero;
                        Main.NewText("Pierre");
                        int projectileDamage=40;
                        if (boss.ai[0] % 20 == 0)
                        {
                            int spawnX=Main.rand.Next((int)player.Center.X-400,(int)player.Center.X+400);
                            int proj= Projectile.NewProjectile(boss.GetSource_FromAI(),new Vector2(spawnX,player.Center.Y-700),new Vector2(0,20f),ProjectileID.Boulder,projectileDamage,2f);
                            if (proj >= 0 && proj < Main.maxProjectiles)
                            {
                                Main.projectile[proj].hostile = true;
                                Main.projectile[proj].friendly = false;
                            }
                        }
                        break;
                    case 2://Feuille
                        Main.NewText("Feuille");
                        break;
                    case 3://Ciseaux
                        Main.NewText("Ciso");
                        List<float> firingCadence = new List<float>{80,95,110,125,140,155,170};
                        float rotationSpeed = 0.05f; 
                        double angle = boss.ai[0] * rotationSpeed;
                        float radius = PFCDistanceFromPlayer; 
                        Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                        Vector2 targetPosition = player.Center + offset;
                        boss.position = targetPosition;
                        boss.rotation = (player.Center - boss.Center).ToRotation() - (float)Math.PI / 2;
                        if (firingCadence.Contains(boss.ai[0]))
                        {
                            Vector2 shotDirection = player.Center - boss.Center;
                            shotDirection.Normalize();
                            shotDirection *= 10f; // Projectile speed
                            int proj= Projectile.NewProjectile(boss.GetSource_FromAI(), boss.Center, shotDirection, ProjectileID.DeathLaser, 16, 1f);
                            if (proj >= 0 && proj < Main.maxProjectiles)
                            {
                                Main.projectile[proj].hostile = true;
                                Main.projectile[proj].friendly = false;
                            }
                        }
                        break;
                }   
            }
            else
            {
                boss.ai[0]=0;
                boss.ai[3]=0;
            }
        }
    }
}
