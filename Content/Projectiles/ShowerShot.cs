﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using static System.Formats.Asn1.AsnWriter;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace zapitanor.Content.Projectiles
{
    public class ShowerShot : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 7;
            Projectile.localNPCHitCooldown = 0;
            Projectile.height = 7;
            Projectile.alpha = 255;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.scale = 1f;
            Projectile.timeLeft = 10*60*3;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
            for (int num163 = 0; num163 < 10; num163++) // Spawns 10 dust every ai update (I have projectile.extraUpdates = 1; so it may actually be 20 dust per ai update)
            {
                Projectile.tileCollide = true;
                float x2 = Projectile.Center.X;// - Projectile.velocity.X / 10f * (float)num163;
                float y2 = Projectile.Center.Y;// - Projectile.velocity.Y / 10f * (float)num163;
                int num164 = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.SpectreStaff, 0f, 0f, 0, Color.White, 0.7f);
                Main.dust[num164].alpha = Projectile.alpha;
                Main.dust[num164].position.X = x2;
                Main.dust[num164].position.Y = y2+20;
                Main.dust[num164].velocity *= Projectile.velocity;
                Main.dust[num164].noGravity = true;
                Main.dust[num164].fadeIn *= 0.5f;
                Main.dust[num164].scale = 1f;
            }
            for (int i = 0; i < 200; i++)
            {
                //Enemy NPC variable being set
                NPC target = Main.npc[i];

                //Getting the shooting trajectory
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y + (float)target.height *0.5f - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                //If the distance between the projectile and the live target is active
                if (distance < 480f && !target.friendly && target.active)
                {
                    if (Projectile.ai[0] > 0.5f * 60f*3f) //Assuming you are already incrementing this in AI outside of for loop
                    {
                        Projectile.aiStyle = 1;
                        //Dividing the factor of 3f which is the desired velocity by distance
                        distance = 3f / distance;

                        //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                        shootToX *= distance * 5;
                        shootToY *= distance * 5;

                        Projectile.velocity *= 1.75f;

                        //Shoot projectile and set ai back to 0
                        Projectile.position = new Vector2(Projectile.Center.X, Projectile.Center.Y);
                        Projectile.velocity = new Vector2(shootToX, shootToY);
                        SoundEngine.PlaySound(SoundID.Item10, base.Projectile.position); //Bullet noise
                        Projectile.ai[0] = 0f;
                    }
                }
                if (Projectile.ai[0] > 8f) //Assuming you are already incrementing this in AI outside of for loop
                {
                    Projectile.velocity *= 1.0001f;
                }
            }
            Projectile.ai[0] += 1f;
            /*
             * Separation
             */
            if (Projectile.ai[1] == 0f)
            {
                Projectile.tileCollide = false;
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
            Projectile.netUpdate = true;
            Projectile.aiStyle = 1;
        }
    }
}