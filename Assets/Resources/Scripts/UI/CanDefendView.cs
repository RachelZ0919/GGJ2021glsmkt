using UnityEngine;
using UnityEngine.UI;


public class CanDefendView : MonoBehaviour
{
    [SerializeField]
    private Sprite CanDefend;
    [SerializeField]
    private Sprite CanNotDefend;

    private Image baseImage;
    private Image fillImage;

    private TadpoleGroup tadpole;

    private void Awake()
    {
        baseImage = transform.Find("CanDefend").GetComponent<Image>();
        fillImage = transform.Find("DefendReady").GetComponent<Image>();
        tadpole = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponentInChildren<TadpoleGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tadpole.CanDefend)
        {
            baseImage.sprite = CanNotDefend;
            fillImage.fillAmount = 0;
        }else
        {
            baseImage.sprite = CanDefend;
            fillImage.fillAmount = tadpole.CoolingDownTime;
        }
    }
}
