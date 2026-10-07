using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>The Tank: Mann vs Machine boss. Rolls toward the player, shells him, shakes the ground.</summary>
public class Tank : ModNPC
{
	private int shellTimer = 120;

	public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 1;

	public override void SetDefaults()
	{
		NPC.scale = 1.4f; NPC.width = 140; NPC.height = 78;
		NPC.lifeMax = 1800; NPC.damage = 40; NPC.defense = 10; NPC.knockBackResist = 0f; NPC.value = 20000;
		NPC.aiStyle = -1; NPC.boss = true; NPC.noGravity = false; NPC.npcSlots = 5f;
		NPC.HitSound = SoundID.NPCHit4; NPC.DeathSound = SoundID.Item14;
	}

	public override void AI()
	{
		NPC.timeLeft = 3000;
		NPC.TargetClosest(true);
		Player t = Main.player[NPC.target];
		NPC.spriteDirection = NPC.direction = t.Center.X < NPC.Center.X ? 1 : -1;
		float dx = t.Center.X - NPC.Center.X;
		NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, Math.Sign(dx) * (Math.Abs(dx) > 280f ? 1.2f : 0f), 0.05f);
		if (NPC.velocity.Y == 0f && NPC.collideX) NPC.velocity.Y = -6f;
		if (NPC.velocity.Y == 0f && Math.Abs(NPC.velocity.X) > 0.3f && NPC.ai[0]++ % 8 == 0) {
			Dust.NewDustPerfect(NPC.Bottom + new Vector2(-NPC.direction * 56, -4), DustID.Smoke, new Vector2(-NPC.direction * 1.5f, -1f), 100, default, 1.6f).noGravity = true;
			Dust.NewDustPerfect(NPC.Bottom + new Vector2(Main.rand.Next(-56, 56), -2), DustID.Dirt, new Vector2(0, -1.5f), 0, default, 1.2f);
		}
		bool angry = NPC.life < NPC.lifeMax / 2;
		if (--shellTimer <= 0 && Vector2.Distance(t.Center, NPC.Center) < 40 * 16) {
			shellTimer = angry ? 70 : 110;
			Vector2 from = NPC.Center + new Vector2(-NPC.spriteDirection * 52f, -30f);
			int n = angry ? 3 : 1;
			for (int i = 0; i < n; i++)
				Mix.Shoot<Rocket>(from, Mix.Aim(from, t.Center, 8f).RotatedBy((i - (n - 1) / 2f) * 0.18f), 35, 8f, hostile: true);
			SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
			Mix.Burst(from, DustID.Smoke, 14, 8f, 3f);
			Mix.Burst(from, DustID.Torch, 10, 8f, 5f);
			Mix.Shake(4, 0.25);
		}
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		for (int i = 0; i < 6; i++)
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, hit.HitDirection * 2f, -2f, 0, default, 1.3f);
		Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Circular(56, 28), DustID.YellowTorch, Main.rand.NextVector2Circular(4, 4), 0, default, 1.5f).noGravity = true;
		if (NPC.life <= 0) {
			for (int i = 0; i < 90; i++) {
				Dust f = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Torch, 0, 0, 0, default, 3.5f);
				f.velocity = Main.rand.NextVector2Circular(9, 9); f.noGravity = true;
			}
			for (int i = 0; i < 40; i++) Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-6, 0), 100, default, 2.5f);
			Mix.Shake(14, 1.0);
		}
	}

	public override void OnKill()
	{
		Mix.Drop(ItemID.GoldCoin, NPC.Center, 20);
		Mix.Drop<Items.MannCoCrate>(NPC.Center, 2);
		Mix.Drop<Items.Sandvich>(NPC.Center, 3);
		Mix.Title("TANK DESTROYED", "Mann Co. thanks you for your service", 3);
	}
}
