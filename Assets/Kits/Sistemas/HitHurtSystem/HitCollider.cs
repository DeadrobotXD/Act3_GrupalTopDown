using UnityEngine;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HurtCollider hurtCollider = collision.GetComponent<HurtCollider>();
        
        if (hurtCollider == null) return; //evitamos NullReferenceException cuando golpeamos algo
        hurtCollider.NotifyHit(this);
    }

   
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
