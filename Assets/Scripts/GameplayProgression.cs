using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayProgression : MonoBehaviour
{

    private Func<bool> onCheckEnemyInRange;
    // The rate at which progress increases per second when no enemies are in range
    public float progressIncreaseRate = 1f;


    public float progress = 0f;

    public IEnumerator<float> OnCheckEnemyInRange()
    {
        while (true)
        {
            if(onCheckEnemyInRange == null) yield break;
            if (onCheckEnemyInRange())
            {
            }
            else
            {

                progress += progressIncreaseRate * Time.deltaTime;
                Debug.Log($"Progress increased. Current progress: {progress}");
            }
            yield return Timing.WaitForOneFrame;
        }
    }
    public void AssignEvent(Func<bool> _onCheckEnemyInRange)
    {
        onCheckEnemyInRange = _onCheckEnemyInRange;
    }
}
