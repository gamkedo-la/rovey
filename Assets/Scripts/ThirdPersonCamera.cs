using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThirdPersonCamera : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    // Holding the left mouse button down will temporarily
    // change these to "Mouse X" and "Mouse Y".
    private string defaultHorizontalAxis;
    private string defaultVerticalAxis;

    void Start()
    {
        freeLook = (CinemachineFreeLook) GetComponentInChildren<CinemachineBrain>().ActiveVirtualCamera;
        defaultHorizontalAxis = freeLook.m_XAxis.m_InputAxisName;
        defaultVerticalAxis = freeLook.m_YAxis.m_InputAxisName;
    }

    private void LateUpdate()
    {
        // Adjust camera with mouse while holding left click.
        if (Input.GetMouseButton(0))
        {
            freeLook.m_XAxis.m_InputAxisName = "Mouse X";
            freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
        }

        // Stop camera from getting stuck with last mousex/y axis value.
        if (Input.GetMouseButtonUp(0))
        {
            freeLook.m_XAxis.m_InputAxisName = defaultHorizontalAxis;
            freeLook.m_YAxis.m_InputAxisName = defaultVerticalAxis;
        }
    }
}
