using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] InputActionReference attack;
    [SerializeField] GameObject attackHitbox;   // hijo con HitCollider, desactivado por defecto
    [SerializeField] AudioClip attackSound;

    [Header("Tiempos")]
    [Tooltip("Cuánto tiempo permanece activa la hitbox por golpe (segundos).")]
    [SerializeField] float attackActiveTime = 0.15f;
    [Tooltip("Tiempo mínimo entre ataques (segundos).")]
    [SerializeField] float attackCooldown = 0.4f;

    float cooldownTimer = 0f;

    private void OnEnable()
    {
        attack.action.Enable();
        attack.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        attack.action.performed -= OnAttack;
        attack.action.Disable();
    }

    private void Start()
    {
        // Aseguramos que la hitbox empiece apagada.
        if (attackHitbox != null) attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        // No atacamos si todavía estamos en cooldown.
        if (cooldownTimer > 0f) return;
        if (attackHitbox == null)
        {
            Debug.LogWarning("PlayerAttack: falta asignar 'attackHitbox' en el Inspector.", this);
            return;
        }

        cooldownTimer = attackCooldown;
        StartCoroutine(DoAttack());
        AudioSource.PlayClipAtPoint(attackSound, transform.position);
    }

    private System.Collections.IEnumerator DoAttack()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(attackActiveTime);
        attackHitbox.SetActive(false);
    }
}
