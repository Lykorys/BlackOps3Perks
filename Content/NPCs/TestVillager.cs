using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace BoneTest.Content.NPCs
{
    [AutoloadHead]
    public class TestVillager : ModNPC
    {
        public const string ShopName = "Shop";
		public int NumberOfTimesTalkedTo = 0;
        public static LocalizedText UpgradedText { get; private set; }
        public override LocalizedText DeathMessage => this.GetLocalization("DeathMessage");
		private static Profiles.StackedNPCProfile NPCProfile;

        public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}
        
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackType[Type] = -1; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like) // Example Person prefers the forest.
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike) // Example Person dislikes the snow.
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) // Loves living near the dryad.
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Likes living near the guide.
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike) // Dislikes living near the merchant.
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Hates living near the demolitionist.
			; // < Mind the semicolon!


			ContentSamples.NpcBestiaryRarityStars[Type] = 3; // We can override the default bestiary star count calculation by setting this.

			UpgradedText = this.GetLocalization("Upgraded");
		}
        // The PreDraw hook is useful for drawing things before our sprite is drawn or running code before the sprite is drawn
		// Returning false will allow you to manually draw your NPC
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			// This code slowly rotates the NPC in the bestiary
			// (simply checking NPC.IsABestiaryIconDummy and incrementing NPC.Rotation won't work here as it gets overridden by drawModifiers.Rotation each tick)
			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers)) {
				drawModifiers.Rotation += 0.001f;

				// Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}

			return true;
		}
        
        
		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
        /*
			if (TownNPCRespawnSystem.unlockedExamplePersonSpawn) {
				// If Example Person has spawned in this world before, we don't require the user satisfying the ExampleItem/ExampleBlock inventory conditions for a respawn.
				return true;
			}

			foreach (var player in Main.ActivePlayers) {
				// Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
				if (player.inventory.Any(item => item.type == ModContent.ItemType<ExampleItem>() || item.type == ModContent.ItemType<Items.Placeable.ExampleBlock>())) {
					return true;
				}
			}*/

			return true;
		}


		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Zeruels",
				"Muda",
				"Lycoris"
			};
		}

		public override string GetChat() {
			return "blablabla";
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28"); // This is the key to the word "Shop"
			button2 = "Awesomeify";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				// We want 3 different functionalities for chat buttons, so we use HasItem to change button 1 between a shop and upgrade action.
				shop = ShopName; // Name of the shop tab we want to open.
			}
		}

		// Not completely finished, but below is what the NPC will sell
		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName);
			npcShop.Register(); // Name of this shop tab
		}

		public override void ModifyActiveShop(string shopName, Item[] items) {
			foreach (Item item in items) {
				// Skip 'air' items and null items.
				if (item == null || item.type == ItemID.None) {
					continue;
				}
			}
		}

		public override bool ModifyDeathMessage(ref NetworkText customText, ref Color color) {
			return true;
		}


		public override void LoadData(TagCompound tag) {
			NumberOfTimesTalkedTo = tag.GetInt("numberOfTimesTalkedTo");
		}

		public override void SaveData(TagCompound tag) {
			tag["numberOfTimesTalkedTo"] = NumberOfTimesTalkedTo;
		}
    }
}