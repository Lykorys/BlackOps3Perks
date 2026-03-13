using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using BoneTest.Content.Utils.Functions;
using BoneTest.Content.Config;
namespace BoneTest.Content.Items.Weapons
{
    public class RK5 : ModItem{
        SoundStyle shootSound = new SoundStyle("BoneTest/Content/Sound/Weapons/RK5burst") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 9
        };
        SoundStyle reloadSound = new SoundStyle("BoneTest/Content/Sound/Weapons/RK5reload") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3
        };
        private ReloadableGun Gun => Item.GetGlobalItem<ReloadableGun>();
        public override void SetDefaults(){
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.
            Item.useTime = 3;           
            Item.useAnimation = 9;      // Total burst duration (6 * 3)
            Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 20; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 500f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 20f; // The speed of the projectile (measured in pixels per frame.) 
			Item.useAmmo = AmmoID.None; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.
            if (Item.TryGetGlobalItem(out ReloadableGun gun)) {
                gun.IsReloadable=true;
                gun.maxAmmo = 15;
                gun.reloadTime = (int)(60 * 1.5);
                gun.reloadSound = reloadSound;
                gun.shootSound = shootSound;
                gun.whenToPlaySound= Item.useAnimation/Item.useTime;
            }
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.BoneTest.Items.RK5.DisplayName", () => "RK5");
        }
        
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
           if (Gun.ammo > 0 && Gun.loadedBullets.Count > 0) {
                Projectile.NewProjectile(source, position, velocity, Gun.loadedBullets[0], damage, knockback, player.whoAmI);
                Gun.playSound();
                Gun.removeBullets();
            }
            return false;
		}
       
        public override void AddRecipes(){
            CreateRecipe()
            .AddIngredient(ItemID.DirtBlock,1)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
        
	}
}