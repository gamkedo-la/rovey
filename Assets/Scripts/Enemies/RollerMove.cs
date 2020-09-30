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
        GetNextDestination();
        //_destination = _patrolPoint[0];
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        //if (CheckLOS())
        //{
        //    SpotPlayer();
        //}
        SpotPlayer();
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (CheckLOS())
    //    {
    //        SpotPlayer();
    //    }

    //}

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        //if (!CheckLOS())
        //{
        //    LostPlayer();
        //}
        LostPlayer();

    }

    private bool CheckLOS()
    {
        RaycastHit Hit;
        Debug.DrawRay(transform.position, _destination.position - transform.position, Color.blue);
        if (Physics.Raycast(transform.position, _destination.position - transform.position, out Hit) && Hit.transform.tag == "Terrain")
        {
            Debug.Log("Terrain Hit");
            return false;
        }
        if (Physics.Raycast(transform.position, _destination.position - transform.position, out Hit) && Hit.transform.tag == "Player")
        {
            Debug.Log("Player Hit");
            return true;
        }
        else
        {
            Debug.Log("Cast failed");
            return false;
        }
    }

#if UNITY_EDITOR
    //Code here for Editor only.
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        foreach (Transform trans in _patrolPoint)
        {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(trans.position, 1);

        }
        // Draw a red sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_destination.position, 1.5f);
    }

#endif
}
