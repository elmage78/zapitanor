﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using static System.Formats.Asn1.AsnWriter;
using Mono.Cecil;
using zapitanor.Items;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using System.Drawing.Text;
using System;
using System.Diagnostics;

namespace zapitanor.Content.Projectiles
{
    public class TerraShot : ModProjectile
    {
        NPC shootA;
        Vector2 NewVelocity;
        int check;
        int a;
        private int tracing;
        private int Fire2;
        private int Fire3;
        private Terraria.DataStructures.IEntitySource source;
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 50;
            Projectile.light = 0.75f;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 100000;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            if (AltUse.Alt == 1)
            {
                Fire2 = 1;
                AltUse.Alt = 0;
            }
            else
            {
                Fire2 = 0;
            }
            if (Fire2 == 1)
            {
                Projectile.damage = 600;
            }
            else
            {
                Projectile.damage = 250;
            }
            if (AltUse.Omega == 1)
            {
                Fire3 = 1;
                AltUse.Omega = 0;
            }
            else {Fire3 = 0; }
            if (Fire3 == 1)
            {
                //Dusts
                for (int i = 0; i < 5000; i++) 
                {
                    Vector2 position = Main.LocalPlayer.Center;
                    int dust = Dust.NewDust(position, 1, 1, 306, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity.Normalize();
                    Main.dust[dust].velocity *= new Vector2(Main.rand.NextFloat(40f, 44f), Main.rand.NextFloat(40f, 44f));
                    Main.dust[dust].velocity *= 0.4f;
                }
                //Shots
                int NumShots = 10;
                int numArrows = 15;
                for (int i = 0; i < NumShots; i++) // Coherent + Shower
                {
                    Vector2 velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(23));
                    int e623 = Projectile.NewProjectile(source, Projectile.Center, velocity, ModContent.ProjectileType<ShowerShot>(), Projectile.damage - 100, 1f, Main.myPlayer);
                    Main.projectile[e623].usesLocalNPCImmunity = true;
                    Main.projectile[e623].localNPCHitCooldown = 0;
                    int e621 = Projectile.NewProjectile(source, Projectile.position, velocity * 1.25f, ModContent.ProjectileType<CoherentShot>(), Projectile.damage - 200, 1f, Main.myPlayer);
                    Main.projectile[e621].usesLocalNPCImmunity = true;
                    Main.projectile[e621].localNPCHitCooldown = 0;
                }
                for (int i = 0; i < numArrows; i++) // Shotgun
                {
                    Vector2 NewVelocity = Projectile.velocity;
                    Vector2 RotatedVelocity = NewVelocity.RotatedByRandom(MathHelper.ToRadians(18));
                    RotatedVelocity *= Main.rand.NextFloat(0.25f, 1f);
                    int num69 = Projectile.NewProjectile(source, Projectile.Center, RotatedVelocity * 0.70f, ModContent.ProjectileType<ShotgunZapinator>(), 40, 3f, Main.myPlayer);
                    Main.projectile[num69].usesLocalNPCImmunity = true;
                    Main.projectile[num69].localNPCHitCooldown = 0;
                    Main.projectile[num69].penetrate = 30;
                }
                for (int i = 0; i < 4; i++) // Molten shot
                {
                    Vector2 NewVelocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(45));
                    int e621 = Projectile.NewProjectile(source, Projectile.Center, NewVelocity, ModContent.ProjectileType<ZapinatorChanged>(), 300, 3f, Main.myPlayer);
                    Main.projectile[e621].usesLocalNPCImmunity = true;
                    Main.projectile[e621].localNPCHitCooldown = 0;
                }
                for (int i = 0; i < 4; ++i) // MORE TERRAS
                {
                    int e621 = Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)), ModContent.ProjectileType<TerraShot>(), 40, 3f, Main.myPlayer);
                    Main.projectile[e621].usesLocalNPCImmunity = true;
                    Main.projectile[e621].localNPCHitCooldown = 0;
                }
                float Yvel = Math.Abs(Projectile.velocity.Y)*1.5f;
                Projectile.NewProjectile(source, Projectile.Center, new Vector2(0f,Yvel), ModContent.ProjectileType<GrenadeMod>(), 1000, 3f, Main.myPlayer);
            }
        }
        public override void AI()
        {
            if (tracing == 1)
            {
                if (a == 110)
                {
                    tracing = 0;
                    a = 0;
                    //Getting the shooting trajectory
                    float shootToX = shootA.position.X + (float)shootA.width * 0.5f - Projectile.Center.X;
                    float shootToY = shootA.position.Y + (float)shootA.height * 0.5f - Projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                    //If the distance between the projectile and the live target is active
                    if (!shootA.friendly && shootA.active && !shootA.dontTakeDamage)
                    {
                        //Dividing the factor of 3f which is the desired velocity by distance
                        distance = 3f / distance;

                        //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                        shootToX *= distance * 5;
                        shootToY *= distance * 5;
                        Vector2 shootTo = new Vector2(shootToX, shootToY);
                        shootTo.Normalize();
                        shootTo *= (float)Math.Abs(Math.Sqrt(Projectile.oldVelocity.X * Projectile.oldVelocity.X + Projectile.oldVelocity.Y * Projectile.oldVelocity.Y));
                        //Shoot projectile and set ai back to 0
                        Projectile.position = new Vector2(Projectile.Center.X, Projectile.Center.Y);
                        Projectile.velocity = shootTo*2f;
                    }
                }
                else
                {
                    a++;
                }
            }
            //Dusts
            for (int num163 = 0; num163 < 3; num163++) // Spawns 10 dust every ai update (I have projectile.extraUpdates = 1; so it may actually be 20 dust per ai update)
            {
                float x2 = Projectile.Center.X - Projectile.velocity.X / 6f * (float)num163;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 6f * (float)num163;
                int num164 = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.CrystalSerpent_Pink, Projectile.velocity.X, Projectile.velocity.Y, 255, Color.FloralWhite, 1f);
                Main.dust[num164].alpha = Projectile.alpha;
                Main.dust[num164].position.X = x2;
                Main.dust[num164].position.Y = y2;
                Main.dust[num164].velocity *= Main.rand.NextFloat(-1.5f,1.5f);
                Main.dust[num164].noGravity = true;
                Main.dust[num164].fadeIn *= 1.8f;
                Main.dust[num164].scale = 1f;
            }
            // Separation
            Lighting.AddLight(Projectile.position, 0.1f, 0.5f, 1f);
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
            if (Main.rand.Next(3)  == 0)
            {
                tracing = 1;
                shootA = target;
            }
            if (Main.rand.Next(5) == 0)
            {
                Projectile.velocity.Y += (float)Main.rand.Next(30, 31) * 0.01f;
            }
            if (Main.rand.Next(30) == 0)
            {
                Projectile.velocity.X *= Main.rand.NextFloat(1f, 2.5f);
            }
            if (Main.rand.Next(30) == 0)
            {
                Projectile.velocity.Y *= Main.rand.NextFloat(1f, 5f);
            }
            if (Main.rand.Next(100) == 0)
            {
                Projectile.velocity.X *= Main.rand.NextFloat(1f, 5f);
            }
            if (Main.rand.Next(100) == 0)
            {
                Projectile.velocity *= Main.rand.NextFloat(1f, 2f);
            }
            if (Main.rand.Next(10) == 0)
            {
                Projectile.ghostHeal((int)(Projectile.damage * Main.rand.NextFloat(0f, 8f)), Projectile.Center, target);
            }
            if (Main.rand.Next(5) == 0)
            {
                Projectile.vampireHeal((int)((Projectile.damage-100) * Main.rand.NextFloat(1f, 1.25f)), Projectile.Center, target);
            }
            if (Main.rand.Next(100) == 0)
            {
                Projectile.aiStyle = 1;
            }
            if (Main.rand.Next(20) == 0)
            {
                Projectile.aiStyle = 0;
            }
            if (Main.rand.Next(100) == 0)
            {
                Projectile.aiStyle = 8;
            }
            if (Main.rand.Next(20) == 0)
            {
                Projectile.width = 20;
                Projectile.height = 20;
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
            // Alt fire custom on hit's (extra luck and etc)
            if (Fire2 == 1)
            {
                if (Main.rand.Next(12) == 0)
                {
                    //Terra shots
                    Projectile.damage *= 10;
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                    Projectile.NewProjectile(source, Projectile.Center, velocity * 0.5f, ModContent.ProjectileType<TerraShot>(), Projectile.damage, 1f, Main.myPlayer);
                }
                if (Main.rand.Next(20) == 0)
                {
                    //Coherent shots
                    int NumShots = 6;
                    for (int i = 0; i < NumShots; i++)
                    {
                        Vector2 velocity = new Vector2(Projectile.velocity.X, Main.rand.NextFloat(-3f, 3f));
                        Vector2 Pos = new Vector2(Projectile.Center.X, Projectile.Center.Y - Main.rand.NextFloat(-2f, 2f));
                        int e621 = Projectile.NewProjectile(source, Pos, velocity * 6.25f, ModContent.ProjectileType<CoherentShot>(), Projectile.damage - 160, 1f, Main.myPlayer);
                        Main.projectile[e621].ghostHeal((int)(40 * Main.rand.NextFloat(0f, 8f)), Projectile.Center, target);
                    }
                }
                if (Main.rand.Next(17) == 0)
                {
                    //grenade shots
                    int e621 = Projectile.NewProjectile(source, Projectile.Center, new Vector2(0f,-1.5f), ModContent.ProjectileType<GrenadeMod>(), Projectile.damage, 1f, Main.myPlayer);
                    Main.projectile[e621].scale *= 1.05f;
                    Main.projectile[e621].rotation *= 1.1f;
                    Main.projectile[e621].ghostHeal((int)(40 * Main.rand.NextFloat(0f, 8f)), Projectile.Center, target);
                }
                if (Main.rand.Next(20) == 0)
                {
                    int numArrows = 15;
                    for (int e = 0; e < 200; e++)
                    {
                        //Enemy NPC variable being set
                        NPC trg = Main.npc[e];

                        //Getting the shooting trajectory
                        float shootToX = trg.position.X + (float)trg.width * 0.5f - Projectile.Center.X;
                        float shootToY = trg.position.Y + (float)trg.height * 0.5f - Projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                        //If the distance between the projectile and the live target is active
                        if (distance < 730f && !trg.friendly && trg.active && !trg.dontTakeDamage)
                        {
                            check = 1;
                            distance = 3f / distance;
                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 5f;
                            shootToY *= distance * 5f;
                            Vector2 Vel = new Vector2(shootToX, shootToY);
                            NewVelocity = Vel;
                        }
                    }
                    if (check == 1)
                    {
                        for (int i = 0; i < numArrows; i++)
                        {
                            Vector2 RotatedVelocity = NewVelocity.RotatedByRandom(MathHelper.ToRadians(18));
                            RotatedVelocity *= Main.rand.NextFloat(0.05f, 1f);
                            int num69 = Projectile.NewProjectile(source, Projectile.Center, RotatedVelocity * 0.70f, ModContent.ProjectileType<ShotgunZapinator>(), 66, 3f, Main.myPlayer);
                            Main.projectile[num69].usesLocalNPCImmunity = true;
                            Main.projectile[num69].penetrate = 30;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numArrows; i++)
                        {
                            Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                            int num69 = Projectile.NewProjectile(source, Projectile.Center, velocity * 0.70f, ModContent.ProjectileType<ShotgunZapinator>(), 40, 3f, Main.myPlayer);
                            Main.projectile[num69].usesLocalNPCImmunity = true;
                            Main.projectile[num69].penetrate = 30;
                            Main.projectile[num69].ghostHeal((int)(40 * Main.rand.NextFloat(0f, 8f)), Projectile.Center, target);
                        }
                    }
                } // Shzappy shots
                if (Main.rand.Next(20) == 0)
                {
                    //Shower shots
                    int NumShots = 7;
                    for (int i = 0; i < NumShots; i++)
                    {
                        int e621 = Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity * new Vector2(Main.rand.NextFloat(-1.2f, 1.2f)), ModContent.ProjectileType<ShowerShot>(), Projectile.damage - 100, 1f, Main.myPlayer);
                        Main.projectile[e621].usesLocalNPCImmunity = true;
                        Main.projectile[e621].ghostHeal((int)(40 * Main.rand.NextFloat(0f, 8f)), Projectile.Center, target);
                    }
                }
                if (Main.rand.Next(40) == 0)
                {
                    //The other 2 shots
                    int e621 = Projectile.NewProjectile(source, Projectile.Center, new Vector2(0f, -1.5f), ModContent.ProjectileType<GrenadeMod>(), Projectile.damage, 1f, Main.myPlayer);
                    Main.projectile[e621].scale *= 1.05f;
                    Main.projectile[e621].rotation *= 1.1f;
                    Main.projectile[e621].ghostHeal((int)(40 * Main.rand.NextFloat(0f, 8f)), Projectile.Center, target);
                    int numArrows = 15;
                    for (int e = 0; e < 200; e++)
                    {
                        //Enemy NPC variable being set
                        NPC trg = Main.npc[e];

                        //Getting the shooting trajectory
                        float shootToX = trg.position.X + (float)trg.width * 0.5f - Projectile.Center.X;
                        float shootToY = trg.position.Y + (float)trg.height * 0.5f - Projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                        //If the distance between the projectile and the live target is active
                        if (distance < 730f && !trg.friendly && trg.active && !trg.dontTakeDamage)
                        {
                            check = 1;
                            distance = 3f / distance;
                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 5f;
                            shootToY *= distance * 5f;
                            Vector2 Vel = new Vector2(shootToX, shootToY);
                            NewVelocity = Vel;
                        }
                    }
                    if (check == 1)
                    {
                        for (int i = 0; i < numArrows; i++)
                        {
                            Vector2 RotatedVelocity = NewVelocity.RotatedByRandom(MathHelper.ToRadians(18));
                            RotatedVelocity *= Main.rand.NextFloat(0.05f, 1f);
                            int num69 = Projectile.NewProjectile(source, Projectile.Center, RotatedVelocity * 0.70f, ModContent.ProjectileType<ShotgunZapinator>(), 40, 3f, Main.myPlayer);
                            Main.projectile[num69].usesLocalNPCImmunity = true;
                            Main.projectile[num69].penetrate = 30;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numArrows; i++)
                        {
                            Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                            int num69 = Projectile.NewProjectile(source, Projectile.Center, velocity * 0.70f, ModContent.ProjectileType<ShotgunZapinator>(), 40, 3f, Main.myPlayer);
                            Main.projectile[num69].usesLocalNPCImmunity = true;
                            Main.projectile[num69].penetrate = 30;
                        }
                    }
                }
                if (Main.rand.Next(50) == 0)
                {
                    //Post mechanical bosses shots
                    int NumShots = 6;
                    for (int i = 0; i < NumShots; i++)
                    {
                        int e621 = Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity * new Vector2(Main.rand.NextFloat(-1.2f, 1.2f)), ModContent.ProjectileType<ShowerShot>(), Projectile.damage - 100, 1f, Main.myPlayer);
                        Main.projectile[e621].usesLocalNPCImmunity = true;
                        Vector2 velocity = new Vector2(Projectile.velocity.X, Main.rand.NextFloat(-3f, 3f));
                        Vector2 Pos = new Vector2(Projectile.Center.X, Projectile.Center.Y - Main.rand.NextFloat(-2f, 2f));
                        Projectile.NewProjectile(source, Pos, velocity * 1.25f, ModContent.ProjectileType<CoherentShot>(), Projectile.damage - 200, 1f, Main.myPlayer);
                    }
                }
                if (Main.rand.Next(115) == 0)
                {
                    //ALL THE SHOTS
                    int NumShots = 10;
                    for (int i = 0; i < NumShots; i++)
                    {
                        int e623 = Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity * new Vector2(Main.rand.NextFloat(-1.2f, 1.2f)), ModContent.ProjectileType<ShowerShot>(), Projectile.damage - 100, 1f, Main.myPlayer);
                        Main.projectile[e623].usesLocalNPCImmunity = true;
                        Vector2 velocity = new Vector2(Projectile.velocity.X, Main.rand.NextFloat(-3f, 3f));
                        Vector2 Pos = new Vector2(Projectile.Center.X, Projectile.Center.Y - Main.rand.NextFloat(-2f, 2f));
                        Projectile.NewProjectile(source, Pos, velocity * 1.25f, ModContent.ProjectileType<CoherentShot>(), Projectile.damage - 200, 1f, Main.myPlayer);
                    }
                    int e621 = Projectile.NewProjectile(source, Projectile.Center, new Vector2(0f, -1.5f), ModContent.ProjectileType<GrenadeMod>(), Projectile.damage, 1f, Main.myPlayer);
                    Main.projectile[e621].scale *= 1.05f;
                    Main.projectile[e621].rotation *= 1.1f;
                    int numArrows = 20;
                    for (int e = 0; e < 200; e++)
                    {
                        //Enemy NPC variable being set
                        NPC trg = Main.npc[e];

                        //Getting the shooting trajectory
                        float shootToX = trg.position.X + (float)trg.width * 0.5f - Projectile.Center.X;
                        float shootToY = trg.position.Y + (float)trg.height * 0.5f - Projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                        //If the distance between the projectile and the live target is active
                        if (distance < 730f && !trg.friendly && trg.active && !trg.dontTakeDamage)
                        {
                            check = 1;
                            distance = 3f / distance;
                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 5f;
                            shootToY *= distance * 5f;
                            Vector2 Vel = new Vector2(shootToX, shootToY);
                            NewVelocity = Vel;
                        }
                    }
                    if (check == 1)
                    {
                        for (int i = 0; i < numArrows; i++)
                        {
                            Vector2 RotatedVelocity = NewVelocity.RotatedByRandom(MathHelper.ToRadians(18));
                            RotatedVelocity *= Main.rand.NextFloat(0.05f, 1f);
                            int num69 = Projectile.NewProjectile(source, Projectile.Center, RotatedVelocity * 0.70f, ModContent.ProjectileType<ShotgunZapinator>(), 40, 3f, Main.myPlayer);
                            Main.projectile[num69].usesLocalNPCImmunity = true;
                            Main.projectile[num69].penetrate = 30;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numArrows; i++)
                        {
                            Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                            int num69 = Projectile.NewProjectile(source, Projectile.Center, velocity * 0.70f, ModContent.ProjectileType<ShotgunZapinator>(), 40, 3f, Main.myPlayer);
                            Main.projectile[num69].usesLocalNPCImmunity = true;
                            Main.projectile[num69].penetrate = 30;
                        }
                    }
                }
            }
            // Alt fire & Normal fire extra hit's
            if (Main.rand.Next(40) == 0)
            {
                Projectile.damage *= 10;
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                Projectile.NewProjectile(source, Projectile.Center, velocity * 1.25f, ModContent.ProjectileType<TerraShot>(), Projectile.damage, 1f, Main.myPlayer);
            }
            if (Main.rand.Next(100) == 0)
            {
                //Coherent shots
                int NumShots = 10;
                for (int i = 0; i < NumShots; i++)
                {
                    Vector2 velocity = new Vector2(Projectile.velocity.X, Main.rand.NextFloat(-3f, 3f));
                    Vector2 Pos = new Vector2(Projectile.Center.X, Projectile.Center.Y - Main.rand.NextFloat(-2f, 2f));
                    Projectile.NewProjectile(source, Pos, velocity * 1.25f, ModContent.ProjectileType<CoherentShot>(), Projectile.damage, 1f, Main.myPlayer);
                }
            }
            if (Main.rand.Next(150) == 0)
            {
                int e621 = Projectile.NewProjectile(source, Projectile.Center, new Vector2(0f, 0f), ModContent.ProjectileType<GrenadeMod>(), Projectile.damage * 2, 1f, Main.myPlayer);
                Main.projectile[e621].scale *= 1.05f;
                Main.projectile[e621].rotation *= 1.1f;
            }
            Projectile.netUpdate = true;
            Projectile.aiStyle = 1;
        }
    }
}