using Terraria;
using Terraria.ModLoader;

namespace zapitanor.Content.Dusts
{
    public class BulletDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.6f; // Multiply the dust's start velocity by 0.6, slowing it down
            dust.noGravity = false; // Makes the dust have gravity.
            dust.noLight = true; // Makes the dust emit no light.
            dust.scale *= 0.5f; // Multiplies the dust's initial scale by 0.5
            dust.alpha *= 1;
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color lightColor)
        {
            Vector2 zero = Vector2.Zero;
            SpriteEffects effects1 = SpriteEffects.None;
            if (dust.spriteDirection == -1)
            {
                zero.X = (float)Main.dustTexture[dust.type].Width; 
				effects1 = SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw(Main.dustTexture[dust.type], new Vector2(dust.position.X - Main.screenPosition.X +   (float)  (dust.width / 2), dust.position.Y - Main.screenPosition.Y + (float)(dust.height / 2) + dust.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.dustTexture[dust.type].Width, Main.dustTexture[dust.type].Height)), dust.GetAlpha(lightColor), dust.rotation, zero, dust.scale, effects1, 0.0f);
		}

        public override bool Update(Dust dust)
        { // Calls every frame the dust is active
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            if (dust.velocity.Y == 0)
            {
                dust.alpha += 1;
            }
            return false; // Return false to prevent vanilla behavior.
        }
    }
}