using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoidForces
{
	[SerializeField]
	float _repulsionDistance = 5f;
	List<Boid> _repulsionList = new List<Boid>();

	[SerializeField]
	float _alignementDistance = 25f;
	List<Boid> _alignementList = new List<Boid>();

	[SerializeField]
	float _attractionDistance = 600f;
	List<Boid> _attractionList = new List<Boid>();

	Vector3 _separationForce;
	Vector3 _alignmentForce;
	Vector3 _attractionForce;
	public Vector3 SeparationForce { get { return _separationForce; } }
	public Vector3 AlignmentForce { get { return _alignmentForce; } }
	public Vector3 AttractionForce { get { return _attractionForce; } }

	float _separationMagnitude;
	float _alignmentMagnitude;
	float _attractionMagnitude;
	public float SeparationMagnitude { get { return _separationMagnitude; } }
	public float AlignmentMagnitude { get { return _alignmentMagnitude; } }
	public float AttractionMagnitude { get { return _attractionMagnitude; } }

	Boid _boid;

	public void Init(Boid boid)
	{
		_boid = boid;
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

			float distanceSQ = (boid.transform.position - position).magnitude;

			if(distanceSQ <= _repulsionDistance)
			{
				_repulsionList.Add(boid);
			}
			else
				if(distanceSQ <= _alignementDistance)
			{
				_alignementList.Add(boid);
			}
			else
				if(distanceSQ <= _attractionDistance)
			{
				_attractionList.Add(boid);
			}
		}

		_separationForce = Vector3.zero;
		_separationMagnitude = 0f;

		_alignmentForce = Vector3.zero;
		_alignmentMagnitude = 0f;

		_attractionForce = Vector3.zero;
		_attractionMagnitude = 0f;

		bool isOk = Separation(out _separationForce);
		_separationMagnitude = _separationForce.magnitude;
		_separationForce.Normalize();
		if(!isOk)
		{
			isOk = Alignment(out _alignmentForce);
			_alignmentMagnitude = _alignmentForce.magnitude;
			_alignmentForce.Normalize();
		}
		if(!isOk)
		{
			isOk = Attraction(out _attractionForce);
			_attractionMagnitude = _attractionForce.magnitude;
			_attractionForce.Normalize();
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
