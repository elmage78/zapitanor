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
	public class Coherent : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Coherent"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting Projectile line.
            // Tooltip.SetDefault("This gun makes the games balance tremble");
		}
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.width = 84;
			Item.height = 90;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 40;
			Item.useAnimation = 40*3;
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
            int NumProjectiles = 3;
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
                Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velX,velY)*0.000000001f, ModContent.ProjectileType<CoherentShot>(), 73, 1f, player.whoAmI);
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
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.BlackLens, 1);
			recipe.AddIngredient(ItemID.MechanicalLens, 1);
			recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}