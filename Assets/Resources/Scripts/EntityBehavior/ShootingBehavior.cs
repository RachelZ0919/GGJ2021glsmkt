using UnityEngine;

public struct AddtionalProjectileSetting
{
    public float range;
    public float speed;
    public float lastTime;

    public AddtionalProjectileSetting(float r, float s, float l)
    {
        range = r;
        speed = s;
        lastTime = l;
    }
}

public class ShootingBehavior : MonoBehaviour
{
    /// <summary>
    /// 实体当前持有的枪
    /// </summary>
    public Weapon weapon;
    /// <summary>
    /// 额外设置
    /// </summary>
    public AddtionalProjectileSetting setting = new AddtionalProjectileSetting(0, 0, 0);

    #region Visual 
    /// <summary>
    /// 持枪点
    /// </summary>
    public Transform holdingPoint;
    [SerializeField] private Transform shootingVisualTransform;

    public bool enableScreenShake = false;
    public float screenShakeIntensity = 0.05f;
    public float screenShakeTime = 0.1f;

    private Vector3 originScale;
    private Vector3 reverseScale;

    #endregion

    #region Audio

    [SerializeField] private AudioManager audio;
    public bool enableAudio = true;

    #endregion


    private void Awake()
    {
        //视觉
        originScale = shootingVisualTransform.localScale;
        originScale.x = Mathf.Abs(originScale.x);
        reverseScale = new Vector3(-originScale.x, originScale.y, originScale.z);
    }

    private void Start()
    {
        if(weapon != null)
        {
            RegisterGun(weapon);
        }
    }

    /// <summary>
    /// 使用这把枪
    /// </summary>
    /// <param name="weapon"></param>
    public void RegisterGun(Weapon weapon)
    {
        if(weapon != null)
        {
            weapon.RemoveGun();
        }
        weapon.RegisterGun(transform);
    }

    public void UpdateDirection(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        if (Mathf.Abs(angle) > 90)
        {
            shootingVisualTransform.localScale = reverseScale;
            holdingPoint.rotation = Quaternion.Euler(0, 0, angle - 180);

        }
        else
        {
            shootingVisualTransform.localScale = originScale;
            holdingPoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    /// <summary>
    /// 让枪朝指定方向射出子弹
    /// </summary>
    /// <param name="direction">射击方向</param>
    /// <returns>是否发射成功</returns>
    public bool Shoot(Vector2 direction)
    {
        if (weapon == null)
        {
            return false;
        }

        UpdateDirection(direction);

        bool hasShot = weapon.Shoot(direction, setting);
        if (hasShot)
        {
            //抖屏
            if (enableScreenShake) CameraShake.instance.ShakeScreen(screenShakeTime, screenShakeIntensity);
            //音效
            if (enableAudio) {
                if(audio == null)
                {
                    Debug.LogError($"Please assign a AudioManager to {gameObject.name}.ShootingBehavior Component.");
                }
                else
                {
                    if(weapon.shootingAudio != null) 
                    {
                        audio.PlayAudio(weapon.shootingAudio);
                    }
                    else
                    {
                        Debug.LogError($"Please assign a shooting audio in {weapon.name}'s scriptable object data.");
                    }

                }

            } 
        }

        return hasShot;
    }
}


