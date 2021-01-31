using UnityEngine;
using System.Collections;


public class ForceDamage : Damage
{
    public override void DealDamage(HitBehavior hitBehavior, Stats stat, Vector2 direction, bool isTouchDamage = false)
    {
        hitBehavior.GetHit(damage, knockbackForce, direction, isTouchDamage, false);
    }
}
