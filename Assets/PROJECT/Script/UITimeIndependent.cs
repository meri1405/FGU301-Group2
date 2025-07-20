using UnityEngine;

/// <summary>
/// Script này ??m b?o các animation UI v?n ho?t ??ng ngay c? khi game b? pause (Time.timeScale = 0)
/// G?n script này vào các UI objects c?n animation khi pause
/// </summary>
public class UITimeIndependent : MonoBehaviour
{
    private Animator animator;
    private Canvas canvas;

    void Start()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();

        // Set animator ?? không b? ?nh h??ng b?i Time.timeScale
        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        // ??m b?o canvas render trên cùng khi pause
        if (canvas != null)
        {
            canvas.sortingOrder = 999;
        }
    }

    void Update()
    {
        // Có th? thêm logic khác n?u c?n thi?t
        // Ví d?: tween animations s? d?ng unscaled time
    }
}