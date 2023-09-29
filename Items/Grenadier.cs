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
	public class Grenadier : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Grenadier"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting Projectile line.
			// Tooltip.SetDefault("This grenade pouch makes the games balance tremble");
		}
		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.width = 11;
			Item.height = 5;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 100;
			Item.useAnimation = 100;
			Item.scale = 1.5f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1.8f;
			Item.mana = 55;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GrenadeMod>();
			Item.shootSpeed = 10f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6,0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ExplosivePowder, 100);
            recipe.AddIngredient(ModContent.ItemType<Molten_gun>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Frozen_zappy>(), 1);
            if (ModLoader.TryGetMod("CalamityMod", out Mod exampleMod) && exampleMod.TryFind("Stardust", out ModItem Dustitem))
			{
				recipe.AddIngredient(Dustitem.Type, 10);
			}
            recipe.AddTile(TileID.MythrilAnvil); ///Mythril Anvil && Orichalcum anvil
            recipe.Register();
        }
    }
}