using UnityEngine;
using UnityEngine.UI;
public class HealthBarController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Health targetHealth;         // Kéo vào trong Inspector
    public Slider healthSlider;         // Kéo Slider vào đây

    void start()
    {
        if (targetHealth == null)
            targetHealth = GetComponentInParent<Health>();
    }
    void Update()
    {
        if (targetHealth != null && healthSlider != null)
        {
            healthSlider.value = targetHealth.GetHealthPercent();
        }
    }
}
