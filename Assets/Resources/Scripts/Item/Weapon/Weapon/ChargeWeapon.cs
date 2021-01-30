using UnityEngine;
using System.Collections;


public class ChargeWeapon : Weapon
{
    [SerializeField] protected Transform shootingPoint; //子弹生成点的Transform
    private float chargeTime;//蓄力时间
    private bool isCharging = false; //是否在蓄力
    private bool thisFrameIsShooting = false; //当前帧是否蓄力
    private float shootingStartTime; //蓄力开始时间

    private void Start()
    {
        chargeTime = weaponData.shootingInter;
    }

    public override bool Shoot(Vector2 direction)
    {
        if (!isReloading)
        {
            thisFrameIsShooting = true;

            if (isCharging)
            {
                if (Time.time - shootingStartTime >= chargeTime)
                {
                    //发射子弹
                    Projectile projectile = GetAProjectile();
                    projectile.Launch(shootingPoint.position, direction);

                    //后坐力
                    transform.position = Vector3.zero;
                    ApplyRecoilForce();

                    //特效
                    if (effectName != null) EffectPool.instance.PlayEffect(effectName, shootingPoint.position, direction);

                    //换弹逻辑
                    lastShootingTime = Time.time;
                    projectileLeft--;
                    if (projectileLeft <= 0)
                    {
                        Reload();
                    }

                    //刷新蓄力
                    isCharging = false;

                    return true;
                }
                else
                {
                    //Debug.Log("isCharging");
                    float xAmount = Random.Range(-1f, 1f) * 0.05f;
                    float yAmount = Random.Range(-1f, 1f) * 0.05f;
                    transform.localPosition = new Vector3(xAmount, yAmount, 0);
                }
            }
        }

        return false;
    }

    private void LateUpdate()
    {
        if (thisFrameIsShooting && !isCharging)
        {
            shootingStartTime = Time.time;
            animator.SetBool("isCharging", true);
        }
        else if (!thisFrameIsShooting && isCharging)
        {
            animator.SetBool("isCharging", false);
        }

        isCharging = thisFrameIsShooting;
        thisFrameIsShooting = false;
    }
}
