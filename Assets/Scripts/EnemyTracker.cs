using UnityEngine;
using System.Collections.Generic;

public class EnemyTracker : MonoBehaviour
{
    // A set to track enemies currently within the trigger area
    private HashSet<GameObject> enemiesInArea = new HashSet<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TargetConstant.ENEMY))
        {
            // Add the enemy to the set
            enemiesInArea.Add(collision.gameObject);
            Debug.Log($"{collision.gameObject.name} has entered the area.");
        }
    }

    // Called every frame to check for deactivated enemies
    private void Update()
    {
        // Check for and remove deactivated enemies
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var enemy in enemiesInArea)
        {
            if (enemy == null || !enemy.activeInHierarchy)
            {
                toRemove.Add(enemy);
            }
        }

        foreach (var enemy in toRemove)
        {
            enemiesInArea.Remove(enemy);
            Debug.Log($"{enemy.name} has been removed from the area because it is inactive.");
        }
    }


    public bool IsEnemyInArea()
    {
        return enemiesInArea.Count > 0;
    }
}
