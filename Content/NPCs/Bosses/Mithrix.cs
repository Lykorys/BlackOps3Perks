using BoneTest.Content.NPCs.Bosses.BossesAIs;
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

namespace BoneTest.Content.NPCs.Bosses
{
    public class Mithrix : ModNPC
    {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.Deerclops;
        /*
        ai = new float[maxAI]
 	    An array with 4 slots used for any sort of data storage, 
        which is occasionally synced from the server to clients. 
        Each vanilla NPCAIStyleID uses these slots for different purposes.
        */
        /*Use to initiate the total max life*/
        public override void SetStaticDefaults(){
            NPCID.Sets.MPAllowedEnemies[Type]=true; //To be summoned in multiplayer
            Main.npcFrameCount[Type] = 25;  // Total number of frame in the sprite
        }
        public override void SetDefaults(){
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
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }
        public override void AI(){
            MithrixAi.Ai(NPC,Mod);         
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
        public override void OnKill()
        {
            Main.rainTime = 0; 
            Main.raining = false;
            Main.maxRaining = 0f;
        }
    }
}