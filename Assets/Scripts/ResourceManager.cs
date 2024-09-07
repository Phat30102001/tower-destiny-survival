using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // Dictionary to store resource counts with string keys
    private Dictionary<string, int> resources = new Dictionary<string, int>();
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

    public int ConsumeResource(ResourceData _price, Action<long> _onSuccess, Action _onFail = null, Func<bool> _callbackBeforeConsume=null)
    {
        if (_price.ResourceValue <= 0) return resources[_price.ResourceId];

        if (resources.ContainsKey(_price.ResourceId) && resources[_price.ResourceId] >= _price.ResourceValue)
        {
            if(_callbackBeforeConsume != null)
            {
                if (_callbackBeforeConsume()){
                    resources[_price.ResourceId] -= _price.ResourceValue;
                    _onSuccess?.Invoke(resources[_price.ResourceId]);
                }
                else
                {
                    _onFail?.Invoke();

                }
               
            }
            else
            {
                resources[_price.ResourceId] -= _price.ResourceValue;
                _onSuccess?.Invoke(resources[_price.ResourceId]);
            }


        }
        else
        {
            _onFail?.Invoke();
        }
        return resources[_price.ResourceId];
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
[Serializable]
public class ResourceData
{
    public string ResourceId;
    public int ResourceValue;

}
public static class  ResourceConstant
{
    public static string COIN = "Coin";
    public static string GEM = "Gem";
}
