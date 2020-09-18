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

    private int targetWaypointIndex;
    private int currentWaypointIndex;
    private float currentTransitionTime;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypointIndex = 0;
        currentTransitionTime = 0;
        targetWaypointIndex = 1;
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        var currentWaypointPosition = waypoints[currentWaypointIndex].position;
        var targetWaypointPosition = waypoints[targetWaypointIndex].position;
        currentTransitionTime += Time.deltaTime;

        transform.position = Vector3.Lerp(currentWaypointPosition, targetWaypointPosition,
            transitionCurve.Evaluate(currentTransitionTime / transitionTime));
        var sqrDistanceToTarget = (targetWaypointPosition - transform.position).sqrMagnitude;

        if (sqrDistanceToTarget < 0.1f)
        {
            currentWaypointIndex = ++currentWaypointIndex % waypoints.Count;
            currentTransitionTime = 0f;
        }
    }
}
