using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using BlackOps3.Content.Utils.Functions;
using Terraria.DataStructures;
using BlackOps3.Content.Config;
using System;

namespace BlackOps3.Content.Items.Weapons
{
	public class SuperShotgun : ModItem
	{
        private bool isGrappling = false;
        private int hookType = ProjectileID.GemHookDiamond;
        private int hookNumber = -1;
        private int hookTTL=20;
        public override string Texture => "Terraria/Images/Item_" + ItemID.Boomstick;
		SoundStyle shootSound = new SoundStyle("BlackOps3/Content/Sound/Weapons/MR6shoot") {
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
                gun.maxAmmo = 2;
                gun.reloadTime = (int)(60 *  1.77);
                gun.reloadSound = SoundID.GuitarAm;
                gun.shootSound= shootSound;
                gun.whenToPlaySound= Item.useAnimation/Item.useTime;
            }
        }

        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player) {
            if (Gun.isReloading) return false;
            if (Gun.ammo <= 0 && player.altFunctionUse !=2) {
                SoundEngine.PlaySound(SoundID.MenuTick, player.position);
                return true;
            }

            return true;
        }
        public override void HoldItem(Player player)
        {
            if (isGrappling)
            {
                hookTTL--;
                if (hookTTL == 0)
                {
                    isGrappling=!isGrappling;
                    Main.projectile[hookNumber].Kill();
                    hookTTL=20;
                }
            }
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            if(player.altFunctionUse== 2)
            {
                if (isGrappling)
                {
                    Main.projectile[hookNumber].Kill();
                }
                else{
                    hookNumber = Projectile.NewProjectile(source, position, velocity, hookType, 0, 0, player.whoAmI);
                }
                isGrappling=!isGrappling;
            }        
            else if (Gun.loadedBullets.Count > 0) {
                int nbPellets = 8;
                float angle = 10f;
                for(int _ = 0; _ < 2; _++)
                {
                    for(int i = 0; i < nbPellets/2; i++)
                    {
                        Vector2 newSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(angle));
                        Projectile.NewProjectile(source, position, newSpeed, Gun.loadedBullets[0], damage, knockback, player.whoAmI);
                    }
                    Gun.loadedBullets.RemoveAt(0);
                    Gun.ammo--;
                }
                SoundEngine.PlaySound(shootSound, player.position);
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