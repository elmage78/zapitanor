using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using static System.Formats.Asn1.AsnWriter;

namespace zapitanor.Content.Projectiles
{
    public class ShotgunZapinator : ModProjectile
    {

        public override void SetDefaults()
        {   
            Projectile.width = 6;
            Projectile.localNPCHitCooldown = 0;
            Projectile.height = 6;
            Projectile.alpha = 255;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.scale = 1.4f;
            Projectile.timeLeft = 3600;
            Projectile.DamageType = DamageClass.Magic;
            
        }
        public override void AI()
        {
            Projectile.aiStyle = 0;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.tileCollide = true;
            }
            else
            {
                Projectile.tileCollide = false;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.alpha < 200)
            {
                return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
            }
            return Color.Transparent;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, base.Projectile.position);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 position = base.Projectile.position;
            if (Main.rand.Next(20) == 0)
            {
                Projectile.tileCollide = false;
                base.Projectile.position.X += Main.rand.Next(-256, 257);
            }
            if (Main.rand.Next(20) == 0)
            {
                Projectile.tileCollide = false;
                base.Projectile.position.Y += Main.rand.Next(-256, 257);
            }
            if (Main.rand.Next(2) == 0)
            {
                Projectile.tileCollide = false;
            }
            if (Main.rand.Next(3) != 0)
            {
                Projectile.position = base.Projectile.position;
                base.Projectile.position -= Projectile.velocity * (float)Main.rand.Next(0, 40);
                if (Projectile.tileCollide && Collision.SolidTiles(base.Projectile.position, Projectile.width, Projectile.height))
                {
                    base.Projectile.position = Projectile.position;
                    base.Projectile.position -= Projectile.velocity * (float)Main.rand.Next(0, 40);
                    if (Projectile.tileCollide && Collision.SolidTiles(base.Projectile.position, Projectile.width, Projectile.height))
                    {
                        base.Projectile.position = Projectile.position;
                    }
                }
            }
            Projectile.velocity *= 0.6f;
            if (Main.rand.Next(5) == 0)
            {
                Projectile.velocity.X += (float)Main.rand.Next(30, 31) * 0.01f;
            }
            if (Main.rand.Next(5) == 0)
            {
                Projectile.velocity.Y += (float)Main.rand.Next(30, 31) * 0.01f;
            }
            Projectile.damage = (int)((double)Projectile.damage * 0.9);
            Projectile.knockBack *= 0.9f;
            if (Main.rand.Next(20) == 0)
            {
                Projectile.knockBack *= 10f;
            }
            if (Main.rand.Next(50) == 0)
            {
                Projectile.damage *= 10;
            }
            if (Main.rand.Next(500) == 0)
            {
                Projectile.damage *= 100;
            }
            if (Main.rand.Next(7) == 0)
            {
                Projectile.position = base.Projectile.position;
                base.Projectile.position.X += Main.rand.Next(-64, 65);
                if (Projectile.tileCollide && Collision.SolidTiles(base.Projectile.position, Projectile.width, Projectile.height))
                {
                    base.Projectile.position = Projectile.position;
                }
            }
            if (Main.rand.Next(7) == 0)
            {
                Projectile.position = base.Projectile.position;
                base.Projectile.position.Y += Main.rand.Next(-64, 65);
                if (Projectile.tileCollide && Collision.SolidTiles(base.Projectile.position, Projectile.width, Projectile.height))
                {
                    base.Projectile.position = Projectile.position;
                }
            }
            if (Main.rand.Next(14) == 0)
            {
                Projectile.velocity.X *= -1f;
            }
            if (Main.rand.Next(14) == 0)
            {
                Projectile.velocity.Y *= -1f;
            }
            if (Main.rand.Next(10) == 0)
            {
                Projectile.velocity *= (float)Main.rand.Next(1, 201) * 0.0005f;
            }
            if (Projectile.tileCollide)
            {
                Projectile.ai[1] = 0f;
            }
            else
            {
                Projectile.ai[1] = 1f;
            }
            Projectile.aiStyle = 1;
           
        }
    }
}