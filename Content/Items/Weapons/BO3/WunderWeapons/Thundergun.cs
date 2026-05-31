using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using BoneTest.Content.Utils.Functions;
using BoneTest.Content.Config;

namespace BoneTest.Content.Items.Weapons.BO3.WunderWeapons
{
    public class Thundergun: ModItem{
        public override string Texture => "Terraria/Images/Item_"+ItemID.QuadBarrelShotgun;
        SoundStyle shootSound = new SoundStyle("BoneTest/Content/Sound/Weapons/MR6shoot") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3
        };
        SoundStyle reloadSound = new SoundStyle("BoneTest/Content/Sound/Weapons/MR6reload") {
            Volume = 0.8f,
            Pitch = 0.1f,
            MaxInstances = 3
        };

        private WunderWeapon Gun => Item.GetGlobalItem<WunderWeapon>();
        public override void SetDefaults(){
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.
			Item.useTime = 20; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 20; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 1000; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 500f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 20f; // The speed of the projectile (measured in pixels per frame.) 
			Item.useAmmo = AmmoID.None; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.
            if (Item.TryGetGlobalItem(out WunderWeapon gun)) {
                gun.IsReloadable=true;
                gun.ammo=150;
                gun.maxAmmo = 3;
                gun.ammoReserve=15;
                gun.reloadTime = (int)(60 * 6);
                gun.reloadSound = reloadSound;
                gun.shootSound= shootSound;
                gun.whenToPlaySound= Item.useAnimation/Item.useTime;
            }
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.BoneTest.Items.WunderWaffe.DisplayName", () => "Wunderwaffe DG-2");
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            if (Gun.ammo > 0) {
                Projectile.NewProjectile(source, position, velocity,ModContent.ProjectileType<ThundergunProjectile>() , damage, knockback, player.whoAmI);
                Gun.playSound();
                Gun.removeBullets(1);
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