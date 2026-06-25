using BlackOps3.Content.NPCs.Bosses.BossesAIs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
//DD2SquireSonicBoom
namespace BlackOps3.Content.NPCs.Bosses
{
    [AutoloadBossHead]
    public class TestBoss : ModNPC
    {
        /*
        ai = new float[maxAI]
 	    An array with 4 slots used for any sort of data storage, 
        which is occasionally synced from the server to clients. 
        Each vanilla NPCAIStyleID uses these slots for different purposes.
        */
        /*Use to initiate the total max life*/
        public override void SetStaticDefaults(){
            NPCID.Sets.MPAllowedEnemies[Type]=true; //To be summoned in multiplayer
             Main.npcFrameCount[Type] = 6;  // Total number of frame in the sprite
        }
        public override void SetDefaults(){
            NPC.width = 110;
			NPC.height = 110;
			NPC.damage = 60;
			NPC.defense = 10;
			NPC.lifeMax = 5600;
            NPC.npcSlots = 6f; // 6 is recommended for bosses
            NPC.value = Item.buyPrice(0, 10, 0, 0);// plat,gold,silver,copper
            //double HPBoost = CalamityServerConfig.Instance.BossHealthBoost * 0.01;
            /*NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);*/
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }
        public override void FindFrame(int frameHeight)
        {
            int startFrame= 0;
            int finalFrame = 2;
            int frameSpeed = 5;
			NPC.frameCounter += 0.5f;
			NPC.frameCounter += NPC.velocity.Length() / 10f; // Make the counter go faster with more movement speed
			if (NPC.frameCounter > frameSpeed) {
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;

				if (NPC.frame.Y > finalFrame * frameHeight) {
					NPC.frame.Y = startFrame * frameHeight;
				}
			}
        }
        public override void AI(){
            TestBossAI.VanillaTestBossAI(NPC,Mod);         
        }
        
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
            target.AddBuff(BuffID.Darkness, 150, true);
        }
    }
}