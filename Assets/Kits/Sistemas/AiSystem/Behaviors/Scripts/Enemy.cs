using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Guarding,
        Wandering,
        Patrolling,
        Seeking,
        Attacking,
        BeingHit,
        Dead,
    };

    [SerializeField] Transform target;

    [SerializeField] float attackRange = 1f;          // distancia para pasar de Seeking a Attacking
    [SerializeField] float hitStaggerDuration = 0.4f; // cuanto se detiene al recibir un golpe

    [SerializeField] float deathDuration = 1.2f;

    State currentState = State.Guarding;
    CharacterController characterController;
    Sight sight;
    Health health;
    HurtCollider hurtCollider;
    Animator animator;

    float stateTimer;

    private void Awake()
    {
        sight = GetComponent<Sight>();
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        hurtCollider = GetComponent<HurtCollider>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (hurtCollider != null) hurtCollider.onHitRecieved.AddListener(OnHitReceived);
        if (health != null) health.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnDisable()
    {
        if (hurtCollider != null) hurtCollider.onHitRecieved.RemoveListener(OnHitReceived);
        if (health != null) health.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    /// <summary>
    /// Asigna el objetivo al que este enemigo debe perseguir.
    /// Llamado por EnemyGenerator al instanciar el prefab.
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// Devuelve el Transform del objetivo actual, priorizando los visibles
    /// detectados por Sight y cayendo en el target asignado si no hay ninguno.
    /// </summary>
    private Transform GetCurrentTarget()
    {
        if (sight != null && sight.visiblesInSight.Count > 0)
        {
            return sight.visiblesInSight[0].GetTransform();
        }
        return target;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Guarding:
                // Si hay un visible en rango de Sight o tenemos un target asignado, perseguir
                if (sight.visiblesInSight.Count > 0 || target != null)
                {
                    currentState = State.Seeking;
                }
                else { characterController.SetRawMove(Vector2.zero); }
                break;

            case State.Wandering:
                break;

            case State.Patrolling:
                break;

            case State.Seeking:
                Transform seekTarget = GetCurrentTarget();
                if (seekTarget == null)
                {
                    // Sin visibles y sin target asignado: volver a guardia
                    currentState = State.Guarding;
                }
                else
                {
                    float distance = Vector2.Distance(transform.position, seekTarget.position);

                    if (distance <= attackRange)
                    {
                        currentState = State.Attacking;
                    }
                    else
                    {
                        characterController.SetRawMove((seekTarget.position - transform.position).normalized);
                    }
                }
                break;

            case State.Attacking:
                Transform atkTarget = GetCurrentTarget();
                if (atkTarget == null)
                {
                    currentState = State.Guarding;
                    break;
                }

                float distToTarget = Vector2.Distance(transform.position, atkTarget.position);

                characterController.SetRawMove(Vector2.zero);

                if (distToTarget > attackRange)
                {
                    currentState = State.Seeking;
                }
                break;

            case State.BeingHit:
                characterController.SetRawMove(Vector2.zero);
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = State.Seeking;
                }
                break;

            case State.Dead:
                characterController.SetRawMove(Vector2.zero);
                break;
        }
    }

    private void OnHitReceived()
    {
        if (currentState == State.Dead) return;

        stateTimer = hitStaggerDuration;
        currentState = State.BeingHit;
    }

    private void OnLifeDepleted(float startLife)
    {
        if (currentState == State.Dead) return; // evita disparar la muerte dos veces

        currentState = State.Dead;

        // Dispara la animacion de muerte (Trigger "Die" en el Animator).
        if (animator != null) animator.SetTrigger("Die");

        // Desactivamos los colliders para que el cadaver no siga golpeando
        // al player ni recibiendo mas hits mientras se reproduce la animacion.
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // Destruye el zombi cuando termina la animacion.
        Destroy(gameObject, deathDuration);
    }
}
