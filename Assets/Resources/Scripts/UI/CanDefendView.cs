using UnityEngine;
using UnityEngine.UI;


public class CanDefendView : MonoBehaviour
{
    [SerializeField]
    private Sprite CanDefend;
    [SerializeField]
    private Sprite IsCoolingDown;
    [SerializeField]
    private Sprite CanNotDefend;

    private TadpoleGroup tadpole;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        tadpole = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TadpoleGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tadpole.CanDefend)
        {
            image.sprite = CanNotDefend;
        }else if (tadpole.IsCoolingDown)
        {
            image.sprite = IsCoolingDown;
        }
        else
        {
            image.sprite = CanDefend;
        }
    }
}
