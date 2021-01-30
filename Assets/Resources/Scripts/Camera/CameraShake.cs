using UnityEngine;
using System.Collections;


/// <summary>
/// 抖屏管理，singleton
/// </summary>
public class CameraShake : MonoBehaviour
{
    static public CameraShake instance;

    private float shakeTimeRemaining;//剩余抖屏时长
    private float shakeIntensity; //抖屏强度
    private float shakeFadeSpeed; //抖屏减弱速度
    private float shakeRotation; //抖屏旋转强度
    private Vector3 originPosigion; //原位置

    public bool isShaking
    {
        get
        {
            return shakeTimeRemaining > 0;
        }
    }

    /// <summary>
    /// 是否要旋转
    /// </summary>
    public bool allowRotation = true;
    /// <summary>
    /// 旋转强度
    /// </summary>
    public float rotationMultiplier = 15f;

    private void Awake()
    {
        if (instance == null)
        {
            originPosigion = transform.position;
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            float xAmount = Random.Range(-1f, 1f) * shakeIntensity;
            float yAmount = Random.Range(-1f, 1f) * shakeIntensity;
            //随时间减弱抖屏强度
            shakeIntensity = Mathf.MoveTowards(shakeIntensity, 0f, shakeFadeSpeed * Time.deltaTime);
            //随时间减弱旋转强度
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeSpeed * rotationMultiplier * Time.deltaTime);
            //抖屏
            transform.position = originPosigion + new Vector3(xAmount, yAmount, 0);
            if (allowRotation) transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
        }
        else
        {
            transform.position = originPosigion;
        }
    }

    /// <summary>
    /// 抖动屏幕
    /// </summary>
    /// <param name="intensity">抖动强度</param>
    /// <param name="time">抖动持续时间</param>
    public void ShakeScreen(float time, float intensity)
    {
        shakeTimeRemaining = time;
        shakeIntensity = intensity;
        shakeFadeSpeed = intensity / time;
        shakeRotation = intensity * rotationMultiplier;
    }
}
