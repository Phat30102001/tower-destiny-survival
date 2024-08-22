using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractEnemy: MonoBehaviour,IEnemy
{
    [SerializeField] private float speed = 500f;
    private Transform objectTransform;
    private int state = 0;
    [SerializeField] private float attackDistance = 10;
    [SerializeField] private DamageSender damageSender;
    CoroutineHandle handle;



    public void ActiveAction(Transform _target)
    {
        if(!objectTransform) objectTransform = transform;
        Timing.KillCoroutines(handle);
        switch (state)
        {
            case 0:
                handle = Timing.RunCoroutine(Moving(_target));
                break;
            case 1:
                handle = Timing.RunCoroutine(Attacking(_target));
                break;
        }

    }
    private IEnumerator<float> Moving(Transform _target)
    {
        while (state == 0)
        {
            if (Vector3.Distance(objectTransform.position, _target.position) <= attackDistance)
            {
                state++;
            }
            objectTransform.position = Vector3.MoveTowards(objectTransform.position, _target.position, speed * Time.deltaTime);
            yield return Timing.WaitForOneFrame;
        }
        ActiveAction(_target);
    }
    private IEnumerator<float> Attacking(Transform _target)
    {

        while (state == 1)
        {
            if (Vector3.Distance(objectTransform.position, _target.position) > attackDistance)
            {
                state--;
            }
            damageSender.gameObject.SetActive(false);

            yield return Timing.WaitForSeconds(1f);
            damageSender.gameObject.SetActive(true);

        }
        ActiveAction(_target);
    }

}