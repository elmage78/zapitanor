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
            dust.scale *= 1.5f; // Multiplies the dust's initial scale by 1.5.
            dust.alpha *= 1;
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