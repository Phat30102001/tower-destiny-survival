using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameplayManager gameplayManager;
        
    private void Start()
    {
        resourceManager.AddResource(ResourceConstant.COIN, 0);
        resourceManager.AddResource(ResourceConstant.GEM, 0);

        gameplayManager.ActiveGameplay(resourceManager);
    }

}
