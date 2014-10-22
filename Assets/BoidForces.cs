using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoidForces
{
	[SerializeField]
	float _repulsionDistance = 5f;
	float _repulsionDistanceSQ ;
	List<Boid> _repulsionList = new List<Boid>();

	[SerializeField]
	float _alignementDistance = 25f;
	float _alignementDistanceSQ;
	List<Boid> _alignementList = new List<Boid>();

	[SerializeField]
	float _attractionDistance = 600f;
	float _attractionDistanceSQ;
	List<Boid> _attractionList = new List<Boid>();

	Vector3 _separationForce;
	Vector3 _alignmentForce;
	Vector3 _attractionForce;
	public Vector3 SeparationForce { get { return _separationForce; } }
	public Vector3 AlignmentForce { get { return _alignmentForce; } }
	public Vector3 AttractionForce { get { return _attractionForce; } }

	Boid _boid;

	public void Init(Boid boid)
	{
		_boid = boid;

		_repulsionDistanceSQ = _repulsionDistance * _repulsionDistance;
		_alignementDistanceSQ = _alignementDistance * _alignementDistance;
		_attractionDistanceSQ = _attractionDistance * _attractionDistance;
	}

	public void UpdateForces()
	{
		Boid [] boids = _boid.Squad.Boids;
		Vector3 position = _boid.transform.position;
			
		_repulsionList.Clear();
		_alignementList.Clear();
		_attractionList.Clear();

		for (int i = 0; i < boids.Length; ++i)
		{
			Boid boid = boids[i];

			if(boid == _boid)
			{
				continue;
			}

			float distanceSQ = (boid.transform.position - position).sqrMagnitude;

			if(distanceSQ <= _repulsionDistanceSQ)
			{
				_repulsionList.Add(boid);
			}
			//else
				if(distanceSQ <= _alignementDistanceSQ)
			{
				_alignementList.Add(boid);
			}
			//else
				if(distanceSQ <= _attractionDistanceSQ)
			{
				_attractionList.Add(boid);
			}
		}

		_separationForce = Vector3.zero;
		_alignmentForce = Vector3.zero;
		_attractionForce = Vector3.zero;

		bool isOk = Separation(out _separationForce);
		if(!isOk)
		{
			isOk = Alignment(out _alignmentForce);
		}
		if(!isOk)
		{
			isOk = Attraction(out _attractionForce);
        }
	}

	bool Separation(out Vector3 force)
	{
		force = Vector3.zero;

		if(_repulsionList.Count == 0)
		{
			return false;
		}

		Vector3 position = _boid.transform.position;

		Vector3 avgPosition = Vector3.zero;
		for (int i = 0; i < _repulsionList.Count; ++i)
		{
			avgPosition += _repulsionList[i].transform.position;
		}
		avgPosition /= (_repulsionList.Count);

		Vector3 desiredVelocity = _boid.transform.position - avgPosition;
		force = desiredVelocity - _boid.transform.rigidbody.velocity;

		return true;
	}

	bool Alignment(out Vector3 force)
	{
		force = Vector3.zero;
		
		if(_alignementList.Count == 0)
		{
			return false;
		}
		
		Vector3 position = _boid.transform.position;

		Vector3 avgVelocity = Vector3.zero;
		for (int i = 0; i < _alignementList.Count; ++i)
		{
			avgVelocity += _alignementList[i].rigidbody.velocity;
		}
		
		avgVelocity /= (float)_alignementList.Count;
		force = avgVelocity - _boid.transform.rigidbody.velocity;

		return true;
	}

	bool Attraction(out Vector3 force)
	{
		force = Vector3.zero;
		
		if(_attractionList.Count == 0)
		{
			return false;
		}
		
		Vector3 position = _boid.transform.position;

		Vector3 avgPosition = Vector3.zero;
		for (int i = 0; i < _attractionList.Count; ++i)
		{
			avgPosition += _attractionList[i].transform.position;
		}
		avgPosition /= (_attractionList.Count);

		Vector3 desiredVelocity = avgPosition - _boid.transform.position;
		force = desiredVelocity - _boid.transform.rigidbody.velocity;

		return true;
	}
}
