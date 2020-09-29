using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RollerMove : MonoBehaviour
{

    [SerializeField]
     private Transform _destination;
    [SerializeField]
     private NavMeshAgent _navMeshAgent;

    void Start()
    {
        if (_navMeshAgent == null)
        {
            return;
        }

        else
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }
    void Update()
    {
        
    }
}
