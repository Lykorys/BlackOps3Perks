using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Systems;

namespace BlackOps3.Content.Items.Weapons.BO3.WunderWeapons
{
    public class RaygunMK2 : WunderWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 40;
            Item.scale = 2f;
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.
			Item.useTime = 4; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 12; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 1000; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 500f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 20f; // The speed of the projectile (measured in pixels per frame.) 
			Item.useAmmo = AmmoID.None; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.

            
            ammo = 15;
            magCapacity = 15;
            ammoReserve = 150;
            reloadTime = (int)(60 * 2.98);
            shootSound = new SoundStyle("BlackOps3/Content/Sound/Weapons/MR6shoot")
            {
                Volume = 0.8f,
                Pitch = 0.1f,
                MaxInstances = 3
            };
            reloadSound = new SoundStyle("BlackOps3/Content/Sound/Weapons/MR6reload")
            {
                Volume = 0.8f,
                Pitch = 0.1f,
                MaxInstances = 3
            };
            whenToPlaySound = 3;
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.BlackOps3.Items.WunderWaffe.DisplayName", () => "Wunderwaffe DG-2");
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            if (ammo > 0)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<RaygunProjectile>(), damage, knockback, player.whoAmI);
                playSound();
                removeBullets(1);
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