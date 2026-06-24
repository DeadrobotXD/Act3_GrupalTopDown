using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenEnemies = 7f;

    private Coroutine generationCoroutine;

    void Start()
    {

        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        generationCoroutine = StartCoroutine(GenerateEnemies());
    }

    IEnumerator GenerateEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenEnemies);
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            if (enemyComponent != null && target != null)
            {
                enemyComponent.SetTarget(target);
            }
        }
    }

    public void StopGenerator()
    {
        if (generationCoroutine != null)
        {
            StopCoroutine(generationCoroutine);
        }

        gameObject.SetActive(false);
    }
}
