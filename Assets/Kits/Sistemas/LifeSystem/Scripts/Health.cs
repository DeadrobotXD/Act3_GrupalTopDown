using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class Health : MonoBehaviour
{
    [SerializeField] float startLife = 1f;
    [SerializeField] float hitDamage = 0.2f;

    HurtCollider hurtCollider;
    PlayerCollect playerCollect;
    Inventory inventory;

    public UnityEvent<float, float> onLifeChanged;
    public UnityEvent<float> onLifeDepleted;

    float currentLife;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        hurtCollider = GetComponent<HurtCollider>();
        currentLife = startLife;
        playerCollect = GetComponent<PlayerCollect>();
        inventory = GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        hurtCollider.onHitRecieved.AddListener(OnHitRecieved);
        playerCollect?.onCollectedObjectDirectUsage.AddListener(OnCollectedObject);
        inventory?.onObjectUsed.AddListener(OnObjectUse);
    }

    private void OnDisable()
    {
        hurtCollider.onHitRecieved.RemoveListener(OnHitRecieved);
        playerCollect?.onCollectedObjectDirectUsage.RemoveListener(OnCollectedObject);
        inventory?.onObjectUsed.RemoveListener(OnObjectUse);
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
                onLifeDepleted.Invoke(startLife);//cuando llega a 0
            }
        }
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

    private void OnCollectedObject(CollectableObject collectable)
    {
        InventoryInfo info = collectable.inventoryInfo;
        UseInventoryInfo(info);
    }

    private void OnObjectUse(InventoryInfo info)
    {
        UseInventoryInfo(info);
    }

    private void UseInventoryInfo(InventoryInfo info)
    {
        if (info.type == InventoryInfo.InventoryObjectType.Health)
        {
            currentLife += info.recovery;
            onLifeChanged.Invoke(currentLife, startLife);
        }
    }
}

