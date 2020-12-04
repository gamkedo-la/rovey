using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPlatform : MonoBehaviour
{
    public float springPower = 120.0f;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            PlayerController pcScript = collider.GetComponent<PlayerController>();
            CharacterController ccScript = collider.GetComponent<CharacterController>();
            Vector3 moveBy = transform.position + transform.up * 1.5f - ccScript.transform.position;
            ccScript.Move(moveBy);
            pcScript.PinballPush(transform.up, springPower);
        }
    }



}
