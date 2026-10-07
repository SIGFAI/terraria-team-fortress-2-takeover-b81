using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Shared behavior of the BLU mercenaries: walking animation, blood, sparks, shooting helper.</summary>
public abstract class MercBase : ModNPC
{
	protected int shotTimer = 60;
	protected virtual float MaxSpeed => 1.2f;
	protected virtual int ShotDelay => 150;
	protected virtual float Range => 20f * 16f;
	protected virtual string[] Barks => new[] { "..." };
	private bool barked;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[Type] = 4;
	}

	protected void Setup(int w, int h, int life, int damage, int defense, float kb, int value)
	{
		NPC.scale = 1.5f;      // bigger than the player so viewers can read them
		NPC.width = (int)(w * NPC.scale); NPC.height = (int)(h * NPC.scale);
		NPC.lifeMax = life; NPC.damage = damage; NPC.defense = defense;
		NPC.knockBackResist = kb; NPC.value = value;
		NPC.aiStyle = NPCAIStyleID.Fighter; AIType = NPCID.Zombie;
		AnimationType = -1;
		NPC.HitSound = SoundID.NPCHit1; NPC.DeathSound = SoundID.NPCDeath1;
	}

	public override void FindFrame(int frameHeight)
	{
		if (NPC.velocity.Y != 0f) { NPC.frame.Y = 1 * frameHeight; return; }
		if (Math.Abs(NPC.velocity.X) < 0.15f) { NPC.frame.Y = 0; NPC.frameCounter = 0; return; }
		NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.9f;
		NPC.frame.Y = (int)(NPC.frameCounter / 6) % 4 * frameHeight;
	}

	public override void AI()
	{
		NPC.timeLeft = Math.Max(NPC.timeLeft, 600);    // vanilla zombie AI would walk away in daylight
		if (!barked) {
			barked = true;
			if (Main.rand.NextBool(4)) Mix.Popup(NPC.Top, Barks[Main.rand.Next(Barks.Length)], Color.LightSkyBlue);
		}
		NPC.TargetClosest(true);
		Player t = Main.player[NPC.target];
		if (Math.Abs(NPC.velocity.X) > MaxSpeed && NPC.velocity.Y == 0f) NPC.velocity.X = Math.Sign(NPC.velocity.X) * MaxSpeed;
		if (!t.active || t.dead) return;
		if (++shotTimer >= ShotDelay && Vector2.Distance(t.Center, NPC.Center) < Range
			&& Collision.CanHitLine(NPC.position, NPC.width, NPC.height, t.position, t.width, t.height)) {
			shotTimer = 0;
			Fire(t);
		}
	}

	protected virtual void Fire(Player target) { }

	protected Vector2 Muzzle(Player t, float reach) =>
		NPC.Center + new Vector2(NPC.direction * reach * NPC.scale, -6f);

	public override void HitEffect(NPC.HitInfo hit)
	{
		for (int i = 0; i < 6; i++)
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection * 2f, -1f, 0, default, 1.2f);
		Dust.NewDustPerfect(NPC.Center, DustID.MagicMirror, Main.rand.NextVector2Circular(3, 3), 0, default, 1f).noGravity = true;
		if (NPC.life <= 0) {
			for (int i = 0; i < 30; i++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection * 3f, -2f, 0, default, 1.6f);
			for (int i = 0; i < 12; i++) {
				Dust d = Dust.NewDustPerfect(NPC.Center, DustID.Smoke, Main.rand.NextVector2Circular(4, 4), 100, default, 1.6f);
				d.noGravity = true;
			}
		}
	}

	public override void OnKill()
	{
		if (Main.rand.NextBool(3)) Mix.Drop(ItemID.SilverCoin, NPC.Center, 2);
		if (Main.rand.NextBool(6)) Mix.Drop<Items.Sandvich>(NPC.Center, 1);
		if (Main.rand.NextBool(12)) Mix.Drop(ItemID.Heart, NPC.Center, 1);
	}
}
