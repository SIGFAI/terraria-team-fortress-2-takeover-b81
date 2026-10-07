using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>TF2 flavor for the player: random critical hits with a flashy effect, and flames that really burn.</summary>
public class TFPlayer : ModPlayer
{
	private void Crit(NPC target, ref NPC.HitModifiers modifiers)
	{
		if (Main.rand.NextFloat() < 0.10f) {
			modifiers.SetCrit();
			modifiers.FinalDamage *= 1.6f;
			Mix.Popup(target.Top, "CRITICAL HIT!", Color.Cyan);
			Mix.Burst(target.Center, DustID.MagicMirror, 18, 20f, 5f);
			Mix.Burst(target.Center, DustID.IceTorch, 10, 14f, 4f);
			Mix.Sound("crit", target.Center);
		}
	}

	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) => Crit(target, ref modifiers);

	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) => Crit(target, ref modifiers);

	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (proj.type == ProjectileID.Flames) {
			target.AddBuff(BuffID.OnFire3, 300);
			Mix.Burst(target.Center, DustID.Torch, 6, 14f, 3f);
		}
	}
}
