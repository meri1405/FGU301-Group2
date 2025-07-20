using UnityEngine;

/// <summary>
/// Script n�y ??m b?o c�c animation UI v?n ho?t ??ng ngay c? khi game b? pause (Time.timeScale = 0)
/// G?n script n�y v�o c�c UI objects c?n animation khi pause
/// </summary>
public class UITimeIndependent : MonoBehaviour
{
    private Animator animator;
    private Canvas canvas;

    void Start()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();

        // Set animator ?? kh�ng b? ?nh h??ng b?i Time.timeScale
        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        // ??m b?o canvas render tr�n c�ng khi pause
        if (canvas != null)
        {
            canvas.sortingOrder = 999;
        }
    }

    void Update()
    {
        // C� th? th�m logic kh�c n?u c?n thi?t
        // V� d?: tween animations s? d?ng unscaled time
    }
}