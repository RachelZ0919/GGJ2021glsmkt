using UnityEngine;
using System.Collections.Generic;


public class MultiplePointWeapon : Weapon
{
    /// <summary>
    /// 发射点,旋转(0,0,0)的时候默认向右
    /// </summary>
    [SerializeField] private List<Transform> shootingPoints;

    public override bool Shoot(Vector2 direction)
    {
        if (!isReloading && Time.time - lastShootingTime > weaponData.shootingInter)
        {
            for (int i = 0; i < shootingPoints.Count; i++)
            {
                Projectile projectile = GetAProjectile();
                //float angle = shootingPoints[i].eulerAngles.z * Mathf.Deg2Rad;
                //Vector2 directionVec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                Vector2 shootdir = shootingPoints[i].localRotation * direction;

                projectile.Launch(shootingPoints[i].position, shootdir);

                if (effectName != null) EffectPool.instance.PlayEffect(effectName, shootingPoints[i].position, shootdir);
            }

            //后坐力
            ApplyRecoilForce();

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
