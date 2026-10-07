using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class Minigun : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 44; Item.height = 26;
		Item.damage = 11; Item.DamageType = DamageClass.Ranged; Item.knockBack = 1.5f;
		Item.useTime = 4; Item.useAnimation = 4; Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true; Item.autoReuse = true;
		Item.UseSound = SoundID.Item11;
		Item.rare = ItemRarityID.Orange; Item.value = Item.buyPrice(gold: 5);
		Item.shoot = ProjectileID.Bullet; Item.shootSpeed = 14f;
	}

	public override Vector2? HoldoutOffset() => new Vector2(-6f, 0f);

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		Vector2 dir = velocity.SafeNormalize(Vector2.UnitX);
		position += dir * 32f;
		velocity = velocity.RotatedByRandom(0.07f);
		Dust flash = Dust.NewDustPerfect(position, DustID.YellowTorch, dir * 2f + Main.rand.NextVector2Circular(1, 1), 0, default, 1.6f);
		flash.noGravity = true;
		Dust shell = Dust.NewDustPerfect(player.Center, DustID.GoldCoin, new Vector2(-dir.X * 2f, -2f) + Main.rand.NextVector2Circular(1, 1), 0, default, 0.8f);
		shell.noGravity = false;
	}
}
