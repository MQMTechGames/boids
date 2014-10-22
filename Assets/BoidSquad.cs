using UnityEngine;
using System.Collections.Generic;

public class BoidSquad : MonoBehaviour
{
	Boid [] _boids = new Boid[0];
	public Boid [] Boids
	{
		get { return _boids; } 
	}

	void Awake()
	{
		_boids = GetComponentsInChildren<Boid>();
	}
}
