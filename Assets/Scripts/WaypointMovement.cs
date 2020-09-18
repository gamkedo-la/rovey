using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    public List<Transform> waypoints;
    [Tooltip("Transition speed curve between waypoints")]
    public AnimationCurve transitionCurve;
    [Tooltip("Transition time in seconds between waypoints")]
    public float transitionTime;
    [Tooltip("Time spent at each waypoint before transitioning to the next")]
    public float waypointTransitionDelay = 1.0f;

    private int currentWaypointIndex = 0;
    private int targetWaypointIndex = 1;
    private float currentTransitionTime = 0f;
    private bool waitingToTransition = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentWaypointPosition = waypoints[currentWaypointIndex].position;
        var targetWaypointPosition = waypoints[targetWaypointIndex].position;
        currentTransitionTime += Time.deltaTime;

        transform.position = Vector3.Lerp(currentWaypointPosition, targetWaypointPosition,
            transitionCurve.Evaluate(currentTransitionTime / transitionTime));
        var sqrDistanceToTarget = (targetWaypointPosition - transform.position).sqrMagnitude;

        // Transition to next waypoint once maximum transition time is reached.
        if (currentTransitionTime > transitionTime && !waitingToTransition)
        {
            waitingToTransition = true;
            StartCoroutine(DelayedWaypointTransition());
        }
    }

    private IEnumerator DelayedWaypointTransition()
    {
        yield return new WaitForSeconds(waypointTransitionDelay);

        currentWaypointIndex = ++currentWaypointIndex % waypoints.Count;
        targetWaypointIndex = ++targetWaypointIndex % waypoints.Count;
        currentTransitionTime = 0f;
        waitingToTransition = false;
    }
}
