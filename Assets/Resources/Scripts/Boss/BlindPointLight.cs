using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class BlindPointLight : MonoBehaviour
{
    static public BlindPointLight instance;
    private Transform player;
    private Vector3 dampVel = Vector3.zero;
    private float dampSpeed;
    private bool isFollowingPlayer = false;
    private Light2D light;

    public float normalRadius = 9.77f;
    public float followRadius = 3.2f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            light = GetComponent<Light2D>();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (isFollowingPlayer)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.position, ref dampVel, 0.1f);
            light.pointLightOuterRadius = Mathf.SmoothDamp(light.pointLightOuterRadius, followRadius, ref dampSpeed, 0.5f);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, Vector3.zero, ref dampVel, 1.7f);
            light.pointLightOuterRadius = Mathf.SmoothDamp(light.pointLightOuterRadius, normalRadius, ref dampSpeed, 0.5f);
        }

    }

    public void StartBlind()
    {
        isFollowingPlayer = true;
    }

    public void EndBlind()
    {
        isFollowingPlayer = false;
    }
}
