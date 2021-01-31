using UnityEngine;
using UnityEngine.UI;


public class HealthView : MonoBehaviour
{
    private Stats stats;
    private Image image;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<Stats>().OnStatsChanged += UpdateHealth;
        image = GetComponent<Image>();
    }

    private void UpdateHealth(Stats stat)
    {
        float fill = stat.health / stat.maxHealth;
        image.fillAmount = fill;
    }
}
