using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Boid))]
public class SteeringManager : MonoBehaviour
{
	enum State
	{
		IDLE,
		SEEK,
		FLEE,
	}
	State _state = State.IDLE;

	Vector3 _target;

	[SerializeField]
	BoidForces _boidForces;

	[SerializeField]
	float _seekFactor = 1f;
	[SerializeField]
	float _separationForceFactor = 1f;
	[SerializeField]
	float _alignmentForceFactor = 1f;
	[SerializeField]
	float _attractionForceFactor = 1f;

	void Awake()
	{
		Boid boid = GetComponent<Boid>();

		_boidForces.Init(boid);
	}

	public void Seek(Vector3 target)
	{
		_state = State.SEEK;
		_target = target;
	}

	void Update()
	{
		Vector3 steeringForce = Vector3.zero;

		switch(_state)
		{
		case State.SEEK:
			steeringForce = DoSeek();
			break;
		
		default:
			break;
		}

		Vector3 newVel = steeringForce * _seekFactor * Time.deltaTime;

		Vector3 boidForces = Vector3.zero;

		_boidForces.UpdateForces();

		boidForces += _boidForces.SeparationForce * _separationForceFactor * Time.deltaTime;
		boidForces += _boidForces.AlignmentForce * _alignmentForceFactor * Time.deltaTime;
		boidForces += _boidForces.AttractionForce * _attractionForceFactor * Time.deltaTime;

		newVel += boidForces;

		rigidbody.velocity = rigidbody.velocity + newVel;
	}

	Vector3 DoSeek()
	{
		Vector3 desiredVelocity = _target - transform.position;
		Vector3 velocityDir = rigidbody.velocity;
		
		Vector3 steeringForce = desiredVelocity - velocityDir;

		return steeringForce;
	}
}
