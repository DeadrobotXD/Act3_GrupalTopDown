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

    State currentState = State.Guarding;
    CharacterController characterController;
    Sight sight;

    private void Awake()
    {
        sight = GetComponent<Sight>();
        characterController = GetComponent<CharacterController>();
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
                    characterController.SetRawMove((sight.visiblesInSight[0].GetTransform().position - transform.position).normalized);
                }
                break;
            case State.Attacking:
                break;
            case State.BeingHit:
                break;
            case State.Dead:
                break;
        }
    }
}
