using UnityEngine;

public class HitCollider : MonoBehaviour
{
    // Se dispara cuando ALGO entra en el trigger (caso normal: el zombi
    // caminando hacia el player, o un proyectil moviéndose).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryHit(collision);
    }

    private void TryHit(Collider2D collision)
    {
        HurtCollider hurtCollider = collision.GetComponent<HurtCollider>();

        // Evitamos NullReferenceException cuando golpeamos algo
        // que no tiene HurtCollider (paredes, props, etc.)
        if (hurtCollider == null) return;

        hurtCollider.NotifyHit(this);
    }
}