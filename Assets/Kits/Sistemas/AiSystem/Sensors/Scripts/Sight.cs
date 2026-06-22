
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class Sight : MonoBehaviour
{
    
    [SerializeField] private float radius = 3f;
    [SerializeField] List<Ivisible.Side> attendedSides;

    public List<Ivisible> visiblesInSight = new();
    void Update()
    {
        visiblesInSight.Clear();
        Collider2D[] potentialVisibles = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D c in potentialVisibles)
        {
            Ivisible visible = c.GetComponent<Ivisible>();
            if ((visible != null) && attendedSides.Contains(visible.GetSide()))
            {
                visiblesInSight.Add(visible);
            }
        }
    }
}
