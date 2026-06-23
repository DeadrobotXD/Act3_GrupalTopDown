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

    [Header("Combate")]
    [SerializeField] float attackRange = 1f;          // distancia para pasar de Seeking a Attacking
    [SerializeField] float hitStaggerDuration = 0.4f; // cuánto se detiene al recibir un golpe

    [Header("Muerte")]
    [Tooltip("Segundos que dura la animación de muerte antes de destruir el zombi.")]
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

    private void Update()
    {
        switch (currentState)
        {
            case State.Guarding:
                if (sight.visiblesInSight.Count > 0)
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
                if (sight.visiblesInSight.Count <= 0)
                {
                    currentState = State.Guarding;
                }
                else
                {
                    Transform target = sight.visiblesInSight[0].GetTransform();
                    float distance = Vector2.Distance(transform.position, target.position);

                    if (distance <= attackRange)
                    {
                        currentState = State.Attacking;
                    }
                    else
                    {
                        characterController.SetRawMove((target.position - transform.position).normalized);
                    }
                }
                break;

            case State.Attacking:
                if (sight.visiblesInSight.Count <= 0)
                {
                    currentState = State.Guarding;
                    break;
                }

                Transform attackTarget = sight.visiblesInSight[0].GetTransform();
                float distToTarget = Vector2.Distance(transform.position, attackTarget.position);

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

        // Dispara la animación de muerte (Trigger "Die" en el Animator).
        if (animator != null) animator.SetTrigger("Die");

        // Desactivamos los colliders para que el cadáver no siga golpeando
        // al player ni recibiendo más hits mientras se reproduce la animación.
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // Destruye el zombi cuando termina la animación.
        Destroy(gameObject, deathDuration);
    }
}