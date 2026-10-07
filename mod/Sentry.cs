using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Engineer's sentry gun: bolted to the ground, spots the player, spits bullets and beeps.</summary>
public class Sentry : ModNPC
{
	private int timer;

	public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 1;

	public override void SetDefaults()
	{
		NPC.width = 36; NPC.height = 36;
		NPC.lifeMax = 260; NPC.damage = 15; NPC.defense = 10; NPC.knockBackResist = 0f; NPC.value = 800;
		NPC.aiStyle = -1;
		NPC.HitSound = SoundID.NPCHit4; NPC.DeathSound = SoundID.Item14;
	}

	public override void AI()
	{
		NPC.timeLeft = 600;
		NPC.velocity.X = 0;
		NPC.TargetClosest(true);
		Player t = Main.player[NPC.target];
		NPC.spriteDirection = NPC.direction = t.Center.X < NPC.Center.X ? 1 : -1;   // art faces left
		if (!t.active || Vector2.Distance(t.Center, NPC.Center) > 28 * 16) return;
		if (++timer % 9 != 0) return;
		Vector2 from = NPC.Center + new Vector2(-NPC.spriteDirection * 22f + 0, -4f);
		if (!Collision.CanHitLine(NPC.position, NPC.width, NPC.height, t.position, t.width, t.height)) return;
		Mix.Shoot(ProjectileID.Bullet, from, Mix.Aim(from, t.Center, 10f).RotatedByRandom(0.05f), 10, 1f, hostile: true);
		SoundEngine.PlaySound(SoundID.Item11, NPC.Center);
		Dust.NewDustPerfect(from, DustID.YellowTorch, new Vector2(-NPC.spriteDirection * 2f, 0), 0, default, 1.5f).noGravity = true;
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		for (int i = 0; i < 6; i++)
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, hit.HitDirection * 2f, -2f, 0, default, 1.2f);
		for (int i = 0; i < 4; i++)
			Dust.NewDustPerfect(NPC.Center, DustID.YellowTorch, Main.rand.NextVector2Circular(4, 4), 0, default, 1.2f).noGravity = true;
		if (NPC.life <= 0) {
			for (int i = 0; i < 40; i++) Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 2), 0, default, 2.5f);
			for (int i = 0; i < 20; i++) Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-4, 0), 100, default, 2f);
			Mix.Shake(4, 0.2);
		}
	}

	public override void OnKill()
	{
		Mix.Drop(ItemID.GoldCoin, NPC.Center, 2);
		Mix.Popup(NPC.Top, "Sentry destroyed!", Color.OrangeRed);
	}
}
