using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using static System.Formats.Asn1.AsnWriter;
using Mono.Cecil;
using Microsoft.CodeAnalysis;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using System.IO;
using ReLogic.Content;
using System.Security.Cryptography.X509Certificates;

namespace zapitanor.Content.Projectiles
{
    public class GrenadeMod : ModProjectile
    {
        private Effect screenRef;
        Vector2 NewVelocity;
        int check;
        float distance;
        float shootToX;
        float shootToY;
        //Shader Variables
        private int rippleCount = 3;
        private int rippleSize = 75;
        private int rippleSpeed = 185;
        private float distortStrength = 175f;
        //Normal Variables
        private IEntitySource source;
        int hitground = 1;
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Main.projFrames[Projectile.type] = 3;
            Projectile.height = 8;
            Projectile.aiStyle = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.1f;
        //    Projectile.scale = 0.5f;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }
        //SHADERS SHADERS SHADERS
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> screenRef = new Ref<Effect>(Mod.Assets.Request<Effect>("Assets/Shaders/ShockwaveEffect", AssetRequestMode.ImmediateLoad).Value); // The path to the compiled shader file.
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            float Xsparce = Main.rand.NextFloat(-0.75f, 0.75f);
            float Ysparce = Projectile.Center.Y * Main.rand.Next(-1, 1) + Main.rand.NextFloat(-0.5f, 0.5f);
            int edus = Dust.NewDust(Projectile.Center, 1, 1, DustID.MinecartSpark, Xsparce, Ysparce, 0, Color.Red, 1f);
            Main.dust[edus].noGravity = false;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                hitground = 2;
                Projectile.timeLeft = 10000;
                Projectile.frame = 1;
                Projectile.velocity.X = -oldVelocity.X;
                Projectile.aiStyle = 1;
                Projectile.tileCollide = false;
                Projectile.velocity *= 0.9f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                hitground = 2;
                Projectile.timeLeft = 10000;
                Projectile.frame = 1;
                Projectile.velocity.Y = -oldVelocity.Y;
                Projectile.aiStyle = 1;
                Projectile.tileCollide = false;
                Projectile.aiStyle = 0;
            }
            return false;
        }
        public override void AI()
        {
            if (hitground >= 2)
            {
                Projectile.velocity *= 0.95f;
                if (Projectile.velocity.X >= -2 && Projectile.velocity.X <= 2)
                {
                    if (Projectile.velocity.Y >= -2 && Projectile.velocity.Y <= 2)
                    {
                        Projectile.timeLeft = 100;
                        hitground = -1;
                        SoundEngine.PlaySound(SoundID.Item10, base.Projectile.position);
                        int numArrows = 12;
                        for (int e = 0; e < 200; e++)
                        {
                            //Enemy NPC variable being set
                            NPC target = Main.npc[e];

                            //Getting the shooting trajectory
                            float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                            float shootToY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;
                            float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                            //If the distance between the projectile and the live target is active
                            if (distance < 730f && !target.friendly && target.active && !target.dontTakeDamage)
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
                                 RotatedVelocity *= Main.rand.NextFloat(0.05f,1f);
                                  int num69 = Projectile.NewProjectile(source, Projectile.Center, RotatedVelocity*0.70f, ProjectileID.ZapinatorLaser, 675, 3f, Main.myPlayer);
                                Main.projectile[num69].usesLocalNPCImmunity = true;
                                Main.projectile[num69].penetrate = 30;
                            }
                        } 
                        else
                        {
                            for (int i = 0; i < numArrows; i++)
                            {
                                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                                int num69 = Projectile.NewProjectile(source, Projectile.Center, velocity * 0.70f, ProjectileID.ZapinatorLaser, 675, 3f, Main.myPlayer);
                                Main.projectile[num69].usesLocalNPCImmunity = true;
                                Main.projectile[num69].penetrate = 30;
                            }
                        }
                    }
                }
                if (Projectile.velocity.X >= -5 && Projectile.velocity.X <= 5)
                {
                    if (Projectile.velocity.Y >= -5 && Projectile.velocity.Y <= 5)
                    {
                        Projectile.frame = 2;
                    }
                }
            }
            if (Projectile.timeLeft <= 100f)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.ai[1] = 1; // Set state to exploded
                    Projectile.alpha = 255; // Make the projectile invisible.
                    Projectile.friendly = false; // Stop the bomb from hurting enemies.
                   if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
                   {
                        Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(Projectile.Center);
                   }
                }
                if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                {
                    float progress = (100f - Projectile.timeLeft) / 60f;
                    Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
            {
                Filters.Scene["Shockwave"].Deactivate();
            }
        }
    }
}