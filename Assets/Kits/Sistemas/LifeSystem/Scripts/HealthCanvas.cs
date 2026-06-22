using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AdaptivePerformance;

public class HealthCanvas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Health health;
    [SerializeField] Image mask;

    private void OnEnable()
    {
        health.onLifeChanged.AddListener(OnLifeChanged);
        //health.onLifeDepleted.AddListener(OnLifeDepleted);

    }

    private void OnDisable()
    {
        health.onLifeChanged.RemoveListener(OnLifeChanged);
        //health.onLifeDepleted.AddListener(OnLifeDepleted);

    }

    private void OnLifeChanged(float currentHealth, float startHealth)
    {
        mask.fillAmount = currentHealth / startHealth;
    }

    

    //private void OnLifeDepleted(float startHealth)
    //{
    //    throw new NotImplementedException();
    //}
}
