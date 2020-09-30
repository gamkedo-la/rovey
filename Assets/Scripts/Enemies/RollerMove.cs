using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RollerMove : MonoBehaviour
{
    public bool chasing;
    [SerializeField]
    private float DistanceTillDestination;
    [SerializeField]
    private bool moving;
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Transform[] _patrolPoint;
    [SerializeField]
    private float pauseTime;
    [SerializeField]
    private int lastPatrolPoint;


    [SerializeField]
     private Transform _destination;
    [SerializeField]
     private NavMeshAgent _navMeshAgent;
    

    void Start()
    {
        if (_navMeshAgent == null)
        {
            GetComponent<NavMeshAgent>();
        }
        SetDestination();
    }

    private void SetDestination()
    {
        if (_destination != null)
        {
            Debug.Log("Destination set to" + _destination);
            Vector3 targetVector = _destination.transform.position;
            moving = true;
            _navMeshAgent.SetDestination(targetVector);
        }
    }
    void Update()
    {
//        if ((_navMeshAgent.isStopped) && (!chasing))
        if ((_navMeshAgent.remainingDistance < 2) && (!chasing) && (moving))
        {
            Debug.Log("Stop Detected");
            moving = false;
            GetNextDestination();
        }

        if (chasing)
        {
            _destination = _player;
            SetDestination();
        }
    }


    public void SpotPlayer()
    {
        StopCoroutine(Pause());
        chasing = true;
        moving = true;
        
    }

    public void LostPlayer()
    {
        chasing = false;
        _destination = _patrolPoint[0];
    }

    void GetNextDestination()
    {
        Debug.Log("GetNextDestinationTriggered");
        if (lastPatrolPoint < (_patrolPoint.Length))
        {
            Debug.Log("Patrol point incremented");
            lastPatrolPoint++;
            StartCoroutine(Pause());

        }
        if (lastPatrolPoint == (_patrolPoint.Length))
        {
            Debug.Log("Patrol point set back to 0");
            lastPatrolPoint = 0;
            StartCoroutine(Pause());
        }
    }

    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(pauseTime);
        _destination = _patrolPoint[lastPatrolPoint];
        SetDestination();
    }
}
