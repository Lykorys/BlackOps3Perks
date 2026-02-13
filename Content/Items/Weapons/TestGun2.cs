using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoneTest.Content.Items.Weapons
{
    public class TestGun2 : ModItem{
		public override string Texture => "Terraria/Images/Item_" + ItemID.PhoenixBlaster;
        public override void SetDefaults(){
            
			// Common Properties
			Item.width = 4; // Hitbox width of the item.
			Item.height = 3; // Hitbox height of the item.
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 8; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 8; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

			// The sound that this item plays when used.
			
			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 150; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 500f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.

			// Gun Properties
			Item.shoot = ProjectileID.Boulder;
			Item.shootSpeed = 75f; // The speed of the projectile (measured in pixels per frame.) 
			Item.useAmmo = AmmoID.None; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.
		}

        public override void AddRecipes(){
			CreateRecipe()
			.AddIngredient(ItemID.DirtBlock,1)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int proj = Projectile.NewProjectile(source,position,velocity,type,damage,knockback,Main.myPlayer,0,0f);
			if (proj >= 0 && proj < Main.maxProjectiles)
                {
                    Main.projectile[proj].hostile = false;
                    Main.projectile[proj].friendly = true;
                }
			return false;
        }
	}
}