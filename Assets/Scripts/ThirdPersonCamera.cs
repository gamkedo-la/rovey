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

    [Range(0f, 3f)] public float minCameraZoomRange = 0.5f;
    [Range(0f, 3f)] public float maxCameraZoomRange = 3f;
    [Range(0.01f, 1.0f)] public float mouseWheelSensitivity = 0.1f;
    private float zoomFactor = 0;

    private float[] rigRadii;

    void Start()
    {
        freeLook = (CinemachineFreeLook) GetComponentInChildren<CinemachineBrain>().ActiveVirtualCamera;
        defaultHorizontalAxis = freeLook.m_XAxis.m_InputAxisName;
        defaultVerticalAxis = freeLook.m_YAxis.m_InputAxisName;
        
        rigRadii = new float[freeLook.m_Orbits.Length];
        for (var i = 0; i < rigRadii.Length; i++)
        {
            rigRadii[i] = freeLook.m_Orbits[i].m_Radius;
        }
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
        
        // Adjust camera distance with mouse wheel.
        var mouseWheelInput = -Input.mouseScrollDelta.y;
        if (mouseWheelInput > 0 || mouseWheelInput < 0)
        {
            var zoomDelta = mouseWheelInput * mouseWheelSensitivity;
            zoomFactor = Mathf.Clamp(zoomFactor + zoomDelta, minCameraZoomRange, maxCameraZoomRange);
            AdjustRigRadii();
        }
    }

    private void AdjustRigRadii()
    {
        for (var i = 0; i < rigRadii.Length; i++)
        {
            if (freeLook.m_Orbits.Length > i)
            {
                freeLook.m_Orbits[i].m_Radius = rigRadii[i] * zoomFactor;
            }
        }
    }
}
