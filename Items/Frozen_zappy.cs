using Microsoft.Xna.Framework;
using rail;
using System;
//using System.Numerics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace zapitanor.Items
{
	public class Frozen_zappy : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Frozen zapinator"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting Projectile line.
			// Tooltip.SetDefault("This gun makes the games balance tremble");
		}
		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.width = 11;
			Item.height = 5;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1.8f;
			Item.mana = 15;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.ZapinatorLaser;
			Item.shootSpeed = 3f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity*5, ProjectileID.ZapinatorLaser, 40, 1f, player.whoAmI);
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6,0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(2503, 100);
            recipe.AddIngredient(5070, 1);
            recipe.AddIngredient(593, 100);
			recipe.AddIngredient(664, 100);
            recipe.AddTile(4);
			recipe.AddTile(161);
            recipe.Register();
        }
    }
}