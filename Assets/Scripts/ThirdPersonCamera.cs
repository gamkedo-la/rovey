using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThirdPersonCamera : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    void Start()
    {
        freeLook = (CinemachineFreeLook) GetComponentInChildren<CinemachineBrain>().ActiveVirtualCamera;
        // transform.parent = transform.root;
        Debug.Log(transform.root);
    }

    private void LateUpdate()
    {
        // Adjust camera with mouse while holding left click.
        if (Input.GetMouseButton(0))
        {
            freeLook.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
            freeLook.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");
        }

        // Stop camera from getting stuck with last mousex/y axis value.
        if (Input.GetMouseButtonUp(0))
        {
            freeLook.m_XAxis.m_InputAxisValue = 0;
            freeLook.m_YAxis.m_InputAxisValue = 0;
        }
    }
}
