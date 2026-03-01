using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using BoneTest.Content.Utils.Functions;
using BoneTest.Content.Config;

namespace BoneTest.Content.Items.Weapons
{
    public class MR6 : ModItem{
        SoundStyle shootSound = new SoundStyle("BoneTest/Content/Sound/Weapons/mr6ShootSound") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3 // Prevents the sound from overlapping too many times
        };

        private ReloadableGun Gun => Item.GetGlobalItem<ReloadableGun>();
        public override void SetDefaults(){
            
			// Common Properties
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 8; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 8; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)


			// The sound that this item plays when used.
			
			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 20; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 500f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.

			// Gun Properties
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 20f; // The speed of the projectile (measured in pixels per frame.) 
			Item.useAmmo = AmmoID.None; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.
            if (Item.TryGetGlobalItem(out ReloadableGun gun)) {
                gun.IsReloadable=true;
                gun.maxAmmo = 8;
                gun.reloadTime = (int)(60 * 1.9);
                gun.reloadSound = SoundID.GuitarAm;
            }
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.BoneTest.Items.MR6.DisplayName", () => "MR6");
        }
        public override bool CanUseItem(Player player) {
            if (Gun.isReloading) return false;
            if (Gun.ammo <= 0) {
                SoundEngine.PlaySound(SoundID.MenuTick, player.position);
                return true;
            }

            return true;
        }
        public override void HoldItem(Player player)
        {
            if (KeybindSystem.Reload.JustPressed) {
                if (!Gun.isReloading && Gun.ammo < Gun.maxAmmo) {
                    Gun.reload(player); 
                }
            }
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            if (Gun.loadedBullets.Count > 0) {
                SoundEngine.PlaySound(shootSound, player.position);
                Projectile.NewProjectile(source, position, velocity, Gun.loadedBullets[0], damage, knockback, player.whoAmI);
                
                Gun.loadedBullets.RemoveAt(0);
                Gun.ammo--;
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