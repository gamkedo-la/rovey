/**
 * Description: Bounce!
 * Authors: Kornel
 * Copyright: © 2020 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Bounce : MonoBehaviour
{
	[SerializeField] private float _cooldown = 1f;

	private Animator _animator = null;
	private float _cooldownLeft = 0;

	void Start( )
	{
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().useGravity = false;
		_animator = GetComponent<Animator>();

		Assert.IsNotNull(_animator, $"Please assign <b>{nameof(_animator)}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	void Update()
	{
		_cooldownLeft -= Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(_cooldownLeft > 0)
			return;

		if(!other.gameObject.CompareTag("Player"))
			return;

		_animator.SetTrigger("Bounce");
		_cooldownLeft = _cooldown;
	}
}
