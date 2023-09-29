using Microsoft.Xna.Framework;
using rail;
using System;
//using System.Numerics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using zapitanor.Content.Projectiles;

namespace zapitanor.Items
{
	public class Molten_gun : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Molten gun"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting Projectile line.
			// Tooltip.SetDefault("This gun makes the games balance tremble");
		}
		public override void SetDefaults()
		{
			Item.damage = 75;
			Item.width = 11;
			Item.height = 5;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1.8f;
			Item.mana = 40;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ZapinatorChanged>();
            Item.shootSpeed = 20f;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6,0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(4347, 1);
            recipe.AddIngredient(175, 15);
            recipe.AddIngredient(75, 10);
			recipe.AddTile(16);
            recipe.Register();
        }
    }
}