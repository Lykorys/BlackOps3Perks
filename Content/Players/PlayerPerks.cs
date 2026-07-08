using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BlackOps3.Content.Items.Tiles.Perks;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
namespace BlackOps3.Content.Players
{
    public class PlayerPerks : ModPlayer
    {
        public int perkLimit= 4;
        public Dictionary<string, Perk> ActivePerks { get; private set; } = new();
        public int zombieMoney = 10000; // TODO doubles money in event and for every event mob killed should bring money
        public bool isReloading = false;
        private bool wasReloadingLastFrame;

        /*
        #
        #       PERKS 
        #
        */
        public float reloadSpeed = 1f;
        public float magSizeMult= 1f;
        public bool doubleAllProjectiles;
        public bool NoNPCPylons;
        public float ammoSaveChance;
        public float dashLengthMod;
        public float dashCooldownMod;
        public bool keepCoinsOnDeath;
        public bool canClimbWall;

        public bool HasPerk(string perk) => ActivePerks.ContainsKey(perk);
        public void AddPerk(Perk perk) 
        {
            if(ActivePerks.Count<perkLimit)
                if (!ActivePerks.TryGetValue(perk.perkName, out Perk value))
                    ActivePerks.Add(perk.perkName, perk);
                else 
                    if(value.tier < value.maxTier) value.tier++;
        }

        public void ClearPerks() => ActivePerks = new();
        public void PerkAHolic()
        {
            var perkTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Perk)) && !type.IsAbstract);
            perkLimit= perkTypes.Count();
            foreach (Type type in perkTypes)
            {
                if (Activator.CreateInstance(type) is Perk perkInstance)
                {
                    if (!HasPerk(perkInstance.perkName)) 
                    {
                        AddPerk(perkInstance);
                    }
                    ActivePerks[perkInstance.perkName].tier=perkInstance.maxTier;
                }
            }
            Main.NewText("Perkaholic Activated! All perks granted.",Color.Cyan);
        }
        
        public void RemovePerk(string perk) => ActivePerks.Remove(perk);
        public override void ResetEffects()
        {
            magSizeMult = 1f;
            ammoSaveChance = 0f;
            doubleAllProjectiles = false;
            NoNPCPylons = false;
            keepCoinsOnDeath = false;
            dashLengthMod = 1f;
            dashCooldownMod = 1f;
        }

        public override void PostUpdateEquips()
        {
            foreach (var perk in ActivePerks.Values)
            {
                perk.ApplyEffect(this);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var perk in ActivePerks.Values) perk.OnHitNPCWithProj(this, proj, target, hit, damageDone);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var perk in ActivePerks.Values) perk.OnHitNPCWithItem(this, item, target, hit, damageDone);
        }

        public override void OnHurt(Player.HurtInfo hurtInfo)
        {
            foreach (var perk in ActivePerks.Values) perk.OnHurt(this, hurtInfo);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGlowDust, ref PlayerDeathReason damageSource)
        {
            foreach (var perk in ActivePerks.Values)
            {
                if (!perk.PreKill(this, damage, hitDirection, pvp, ref playSound, ref genGlowDust, ref damageSource))
                    return false;
            }
            return true;
        }
        public override void PostUpdate()
        {
            if (isReloading && !wasReloadingLastFrame)
            {
                foreach (var perk in ActivePerks.Values)
                {
                    perk.OnReloadStart(this);
                }
            }
            wasReloadingLastFrame = isReloading;
            if(isReloading)
            {
                foreach (var perk in ActivePerks.Values)
                {
                    perk.DuringReload(this);
                }
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            ClearPerks();
            base.Kill(damage, hitDirection, pvp, damageSource);
        }
        public override void SaveData(TagCompound tag)
        {
            List<TagCompound> savedPerks = new List<TagCompound>();
            foreach (var val in ActivePerks)
            {
                TagCompound perkTag = new TagCompound();
                perkTag["name"] = val.Key;
                perkTag["tier"] = val.Value.tier;
                savedPerks.Add(perkTag);
            }
            tag["ownedPerksList"] = savedPerks;
            tag["perkLimit"] = perkLimit;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("ownedPerksList") && tag.ContainsKey("perkLimit"))
            {
                perkLimit = tag.GetInt("perkLimit");
                var savedPerks = tag.GetList<TagCompound>("ownedPerksList");
                foreach (TagCompound perkTag in savedPerks)
                {
                    string perkName = perkTag.GetString("name");
                    int perkTier = perkTag.GetInt("tier");
                    Perk perkInstance = CreatePerkFromName(perkName);
                    if (perkInstance != null)
                    {
                        perkInstance.tier = perkTier; 
                        AddPerk(perkInstance);
                    }
                }
            }
        }
        

        public void applyPerkBuff(int type, int timeToAdd,float modifier)//todo try 
        {
        }
        private Perk CreatePerkFromName(string name) => name switch
        {
            "DoubleTap" => new DoubleTap(),
            "ElectricCherry" => new ElectricCherry(),
            "Juggernog" => new Juggernog(),
            "MuleKick" => new MuleKick(),
            "QuickRevive" => new QuickRevive(),
            "SpeedCola" => new SpeedCola(),
            "StaminUp" => new StaminUp(),
            "WidowsWine" => new WidowsWine(),
            _ => null
        };

    }
}