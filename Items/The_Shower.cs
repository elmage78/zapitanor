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
	public class The_Shower : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("The Shower"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting Projectile line.
			// Tooltip.SetDefault("This gun makes the games balance tremble");
		}
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.width = 42;
			Item.height = 45;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 10;
			Item.useAnimation = 10*6;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
            Item.scale = 0.7f;
			Item.knockBack = 1.8f;
			Item.mana = 40;
			Item.value = 6900420;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.ZapinatorLaser;
			Item.shootSpeed = 0f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.MouseWorld;
            int NumProjectiles = 5;
			float Xrandom;
			float Yrandom;
            for (int i = 0; i < NumProjectiles; i++)
			{
                if (Main.rand.Next(0, 2) == 0)
                {
                    Yrandom = Main.rand.NextFloat(-10f, 1f) * 16;
                }
                else
                {
                    Yrandom = Main.rand.NextFloat(0f, 11f) * 16;
                }
                if (Main.rand.Next(0, 2) == 0)
                {
                    Xrandom = Main.rand.NextFloat(-10f, 1f) * 16;
                }
                else
                {
                    Xrandom = Main.rand.NextFloat(0f, 11f) * 16;
                }
                float posX = mousePosition.X + Xrandom;			// We multiply by 16 because a tile is 16 x 16 pixels.
                float posY = mousePosition.Y + Yrandom;
                float velX = -posX + mousePosition.X;
                float velY = -posY + mousePosition.Y;
                Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velX,velY), ModContent.ProjectileType<ShowerShot>(), 100, 1f, player.whoAmI);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6,0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.OrichalcumBar, 12);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.Pearlwood, 100);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.SoulofMight, 5);
            recipe2.AddIngredient(ItemID.MythrilBar, 12);
            recipe2.AddIngredient(ItemID.SpellTome, 1);
            recipe2.AddIngredient(ItemID.Pearlwood, 100);
            recipe2.AddTile(TileID.Bookcases);
            recipe2.Register();
        }
    }
}