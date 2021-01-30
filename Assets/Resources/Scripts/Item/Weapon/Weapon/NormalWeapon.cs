using UnityEngine;
using System.Collections;



public class NormalWeapon : Weapon
{
    [SerializeField] protected Transform shootingPoint; //子弹生成点的Transform

    public override bool Shoot(Vector2 direction)
    {
        if (!isReloading &&  Time.time - lastShootingTime > weaponData.shootingInter)
        {
            Projectile projectile = GetAProjectile();
            projectile.Launch(shootingPoint.position, direction);

            //后坐力
            ApplyRecoilForce();

            if (effectName != null) EffectPool.instance.PlayEffect(effectName, shootingPoint.position, direction);

            lastShootingTime = Time.time;
            projectileLeft--;
            if (projectileLeft <= 0)
            {
                Reload();
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
