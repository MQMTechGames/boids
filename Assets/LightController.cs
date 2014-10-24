using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class LightController : MonoBehaviour
{
	Animator _animator;

	void Awake()
	{
		_animator = GetComponent<Animator>();

		Activate();
	}

	void Activate()
	{
		_animator.SetBool("activate", true);

		Invoke("Deactivate", 1f);
	}

	void Deactivate()
	{
		_animator.SetBool("activate", false);

		Invoke("Activate", 1f);
	}
}
