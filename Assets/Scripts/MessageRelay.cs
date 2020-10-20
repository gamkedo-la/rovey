using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageRelay : MonoBehaviour
{
	public GameObject target;

	public void RelayMessage(string message)
	{
		target.SendMessage(message);
	}
}
