using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnemy: MonoBehaviour,IEnemy
{
    [SerializeField] private float speed = 500f;
    public void ActiveAction(Transform _target)
    {
        if (_target != null)
        {
            Debug.Log($"[phat] {_target.name} is active");
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
            

        }
    }
    
}

