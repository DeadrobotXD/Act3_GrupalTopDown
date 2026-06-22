using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent onHitRecieved;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    internal void NotifyHit(HitCollider hitCollider)
    {
        onHitRecieved.Invoke();

    }
}
