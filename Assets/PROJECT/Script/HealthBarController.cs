using UnityEngine;
using UnityEngine.UI;
public class HealthBarController : MonoBehaviour
{
    
    public Health targetHealth;         
    public Slider healthSlider;         

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
