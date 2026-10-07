using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Soldier rocket: fired by the player's launcher (friendly) and by BLU soldiers and tanks (hostile).</summary>
public class Rocket : ModProjectile
{
	public override void SetStaticDefaults()
	{
		ProjectileID.Sets.Explosive[Type] = true;
	}

	public override void SetDefaults()
	{
		Projectile.width = 14;
		Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.timeLeft = 240;
		Projectile.light = 0.6f;
	}

	public override void AI()
	{
		if (Projectile.timeLeft <= 3) {
			Projectile.PrepareBombToBlow();
		}
		else {
			Vector2 tail = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.UnitX) * 10f;
			for (int i = 0; i < 2; i++) {
				Dust fire = Dust.NewDustPerfect(tail + Main.rand.NextVector2Circular(3, 3), DustID.Torch, -Projectile.velocity * 0.1f, 100, default, 2f);
				fire.noGravity = true;
				Dust smoke = Dust.NewDustPerfect(tail, DustID.Smoke, Main.rand.NextVector2Circular(0.6f, 0.6f), 100, default, 1.2f);
				smoke.noGravity = true;
				smoke.fadeIn = 1.2f;
			}
			Lighting.AddLight(Projectile.Center, 1f, 0.6f, 0.2f);
			if (Projectile.velocity.Length() < 14f) Projectile.velocity *= 1.04f;
		}
		if (Projectile.velocity != Vector2.Zero) Projectile.rotation = Projectile.velocity.ToRotation();
	}

	private void Arm()
	{
		if (Projectile.timeLeft > 3) {
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 3;
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Arm();
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => Arm();

	public override void OnHitPlayer(Player target, Player.HurtInfo info) => Arm();

	public override void PrepareBombToBlow()
	{
		Projectile.tileCollide = false;
		Projectile.alpha = 255;
		Projectile.Resize(120, 120);
		Projectile.knockBack = 9f;
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
		Projectile.Resize(22, 22);
		for (int i = 0; i < 30; i++) {
			Dust smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.8f);
			smoke.velocity *= 1.6f;
		}
		for (int i = 0; i < 26; i++) {
			Dust fire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3.5f);
			fire.noGravity = true;
			fire.velocity *= 7f;
			Dust spark = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.6f);
			spark.velocity *= 3.5f;
		}
		for (int i = 0; i < 12; i++) {
			Dust yellow = Dust.NewDustPerfect(Projectile.Center, DustID.YellowTorch, Main.rand.NextVector2CircularEdge(9f, 9f), 0, default, 2f);
			yellow.noGravity = true;
		}
		for (int k = 0; k < 3; k++) {
			Gore g = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
			g.velocity = Main.rand.NextVector2Circular(2, 2);
		}
		if (Projectile.friendly) Mix.Shake(5, 0.2);

		// Rocket jump: the explosion shoves a nearby player away, like the real thing.
		foreach (Player p in Mix.Players) {
			float d = Vector2.Distance(p.Center, Projectile.Center);
			if (Projectile.friendly && d < 90f) {
				Vector2 away = (p.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY);
				p.velocity += away * 6f + new Vector2(0, -3f);
			}
		}
	}
}
