using UnityEngine;


public class UITimeIndependent : MonoBehaviour
{
    private Animator animator;
    private Canvas canvas;

    void Start()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();

        
        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        
        if (canvas != null)
        {
            canvas.sortingOrder = 999;
        }
    }

    void Update()
    {
        
    }
}