using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Systems;

namespace BlackOps3.Content.Items.Weapons.BO3.Snipers
{
    
    public class SVG : ReloadableGun{
        public override string Texture => "Terraria/Images/Item_"+ItemID.SniperRifle;
        public override void SetDefaults(){
            Item.rare = ItemRarityID.Green;
            Item.useTime = 88;
            Item.useAnimation = 88;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 300;
            Item.knockBack = 500f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.None;
            magCapacity = 6;
            reloadTime = (int)(60 * 3.5);
            whenToPlaySound= Item.useAnimation/Item.useTime;
            shootSound = new SoundStyle("BlackOps3/Content/Sound/Weapons/MR6shoot") {
                Volume = 0.8f,
                Pitch = 0.1f,
                MaxInstances = 3
            };
            reloadSound = new SoundStyle("BlackOps3/Content/Sound/Weapons/MR6reload") {
                Volume = 0.8f,
                Pitch = 0.1f,
                MaxInstances = 3
            };
            LoadBullets();
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.BlackOps3.Items.SVG.DisplayName", () => "SVG-100");
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            
            if (loadedBullets.Count > 0) {
                Projectile.NewProjectile(source, position, velocity, loadedBullets[0], damage, knockback, player.whoAmI);
                playSound();
                removeBullets();
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