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
    private List<int> cacheMilestones;
    private int currentMilestone = -1;
    private Action<int> onSpawnEnemy;

    public void GetMilestone(List<int> _milestone)
    {
        cacheMilestones = _milestone;
    }
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
                if ((currentMilestone + 1) >= cacheMilestones.Count) yield break;
                    //Debug.Log($"[PHAT] progress: {progress}");
                if (cacheMilestones.Count>0&& progress == cacheMilestones[currentMilestone + 1])
                {
                    currentMilestone++;
                    //Debug.Log($"[PHAT] increase milestone, current milestone: {currentMilestone}");
                    onSpawnEnemy?.Invoke(currentMilestone);
                    if (currentMilestone >= cacheMilestones.Count - 1)
                    {
                        //Debug.Log($"[phat] Final wave");
                        yield break;
                    }
                }
                progress += progressIncreaseRate;
            }
            yield return Timing.WaitForOneFrame;
        }
    }
    public void AssignEvent(Func<bool> _onCheckEnemyInRange, Action<int> _onSpawnEnemy)
    {
        onCheckEnemyInRange = _onCheckEnemyInRange;
        onSpawnEnemy=_onSpawnEnemy;
    }
    public void ResetData()
    {
        progress = 0;
        currentMilestone = -1;
    }
}
