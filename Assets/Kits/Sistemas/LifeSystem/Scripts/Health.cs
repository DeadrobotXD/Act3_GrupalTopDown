using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float startLife = 1f;
    [SerializeField] float hitDamage = 0.2f;

    HurtCollider hurtCollider;
    public UnityEvent<float, float> onLifeChanged;
    public UnityEvent<float> onLifeDepleted;

    float currentLife;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        hurtCollider = GetComponent<HurtCollider>();
        
        currentLife = startLife;
    }

    private void OnEnable()
    {
        hurtCollider.onHitRecieved.AddListener(OnHitRecieved);
    }

    private void OnDisable()
    {
        hurtCollider.onHitRecieved.RemoveListener(OnHitRecieved);
    }
    private void OnHitRecieved()
    {
        if(currentLife > 0)
        {
            currentLife -= hitDamage;
            onLifeChanged.Invoke(currentLife, startLife);
            if (currentLife <= 0f)
            {
                currentLife = 0f;
                onLifeDepleted.Invoke(startLife);
            }
        }
    }

    public void Heal(float healthAmount)
    {
        currentLife += healthAmount;
        onLifeChanged.Invoke(currentLife, startLife);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Restart()
    {
        currentLife = startLife;
        onLifeChanged.Invoke(currentLife, startLife);

    }
}
