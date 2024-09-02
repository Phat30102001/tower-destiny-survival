using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // Dictionary to store resource counts with string keys
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    // Called when the script instance is being loaded
    void Start()
    {
        // Initialize resources
        resources[ResourceConstant.COIN] = 0;
        resources[ResourceConstant.GEM] = 0;
    }

    public void AddResource(string resourceType, int amount)
    {
        if (amount <= 0) return;

        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] += amount;
        }
        else
        {
            resources[resourceType] = amount;
        }

        Debug.Log($"{resourceType} added. New count: {resources[resourceType]}");
    }

    public bool ConsumeResource(string resourceType, int amount)
    {
        if (amount <= 0) return false;

        if (resources.ContainsKey(resourceType) && resources[resourceType] >= amount)
        {
            resources[resourceType] -= amount;
            Debug.Log($"{resourceType} consumed. Remaining count: {resources[resourceType]}");
            return true;
        }
        else
        {
            Debug.Log($"{resourceType} not enough or doesn't exist.");
            return false;
        }
    }


    public int GetResourceValue(string resourceType)
    {
        if (resources.ContainsKey(resourceType))
        {
            return resources[resourceType];
        }
        else
        {
            return 0;
        }
    }
}
public static class  ResourceConstant
{
    public static string COIN = "Coin";
    public static string GEM = "Gem";
}
