using BlackOps3.Content.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlackOps3.Content.Items.Weapons.BO3.Shotguns
{
	public class Brecci : ReloadableGun
	{
		public override string Texture => "Terraria/Images/Item_"+ItemID.Boomstick;
		public override void SetDefaults() {
			Item.width = 44; 
			Item.height = 18; 
			Item.rare = ItemRarityID.Green;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 6;
			Item.knockBack = 6f;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.None;
            magCapacity = 12;
            reloadTime = (int)(60 * 1.5);
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
            whenToPlaySound = Item.useAnimation / Item.useTime;
            LoadBullets();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			const int NumProjectiles = 8;
			for (int i = 0; i < NumProjectiles; i++) {
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
				newVelocity *= 1f - Main.rand.NextFloat(0.3f);
				if (loadedBullets.Count > 0) {
					Projectile.NewProjectile(source, position, newVelocity, loadedBullets[0], damage, knockback, player.whoAmI);
				}
            }
			removeBullets();
			playSound();
			return false;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-2f, -2f);
		}
	}
}