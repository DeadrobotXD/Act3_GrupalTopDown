using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2D;            // Rigidbody2D del Enemy (raíz)
    [SerializeField] Animator animator;           // Animator de este visual
    [SerializeField] SpriteRenderer spriteRenderer;

    [Tooltip("Velocidad mínima para considerar que se está moviendo.")]
    [SerializeField] float movingThreshold = 0.05f;

    private void Awake()
    {
        // Auto-asignación por si olvidas arrastrar algo en el Inspector.
        if (rb2D == null) rb2D = GetComponentInParent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 velocity = rb2D.linearVelocity;

        bool isMoving = velocity.sqrMagnitude > (movingThreshold * movingThreshold);
        animator.SetBool("IsMoving", isMoving);

        
        if (Mathf.Abs(velocity.x) > movingThreshold)
        {
            spriteRenderer.flipX = velocity.x > 0f;
        }
    }
}