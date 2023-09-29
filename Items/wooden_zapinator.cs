using Microsoft.Xna.Framework;
using System;
//using System.Numerics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace zapitanor.Items
{
	public class wooden_zapinator : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wooden Zapinator"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting Projectile line.
			// Tooltip.SetDefault("This gun makes the games balance tremble");
		}
		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.width = 23;
			Item.height = 11;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0.8f;
			Item.mana = 8;
			Item.value = 10;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.ZapinatorLaser;
			Item.shootSpeed = 2f;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6,0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddIngredient(ItemID.Acorn, 5);
            recipe.AddIngredient(ItemID.Moonglow, 1);
            recipe.AddIngredient(ItemID.WoodenBow, 1);
            recipe.AddIngredient(75, 5);
			recipe.AddIngredient(705, 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}