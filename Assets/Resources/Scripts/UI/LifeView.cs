using UnityEngine;
using UnityEngine.UI;


public class LifeView : MonoBehaviour
{
    [SerializeField] Sprite[] lifeImages;
    private Image lifeImage;

    // Use this for initialization
    void Start()
    {
        lifeImage = GetComponent<Image>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>().OnStatsChanged += UpdateLife;
    }

    private void UpdateLife(Stats stat)
    {
        int life = Mathf.RoundToInt(Mathf.Clamp(stat.health, 0, 3));
        lifeImage.sprite = lifeImages[life];
    }
}
