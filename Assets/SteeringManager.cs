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
	ObstacleAvoidance _obstacleAvoidance;

	[SerializeField]
	float _seekFactor = 1f;
	[SerializeField]
	float _obstacleAvoidanceFactor = 1f;
	[SerializeField]
	float _separationForceFactor = 1f;
	[SerializeField]
	float _alignmentForceFactor = 1f;
	[SerializeField]
	float _attractionForceFactor = 1f;

	[SerializeField]
	float _rotationSpeed = 1f;

	void Awake()
	{
		Boid boid = GetComponent<Boid>();

		_boidForces.Init(boid);
		_obstacleAvoidance.Init(boid);
	}

	public void Seek(Vector3 target)
	{
		_state = State.SEEK;
		_target = target;
	}

	void Update()
	{
		Vector3 steeringForce = Vector3.zero;
		float steeringMagnitude = 0f;

		switch(_state)
		{
		case State.SEEK:
			DoSeek(out steeringForce, out steeringMagnitude);
			break;
		
		default:
			break;
		}

		Vector3 newVel = Vector3.zero;

		_obstacleAvoidance.UpdateForces();
		_boidForces.UpdateForces();

		// Movement
		newVel += _obstacleAvoidance.SteeringForce * _obstacleAvoidanceFactor * Time.deltaTime;
		newVel += _boidForces.SeparationForce * _boidForces.SeparationMagnitude * _separationForceFactor * Time.deltaTime;
		newVel += _boidForces.AlignmentForce * _boidForces.AlignmentMagnitude * _alignmentForceFactor * Time.deltaTime;
		newVel += _boidForces.AttractionForce * _boidForces.AttractionMagnitude * _attractionForceFactor * Time.deltaTime;
		newVel += steeringForce * Mathf.Max(steeringMagnitude - _boidForces.SeparationMagnitude, 0f) * _seekFactor * Time.deltaTime;
		rigidbody.velocity = rigidbody.velocity + newVel;

		// Rotation
		if(rigidbody.velocity.sqrMagnitude > 1f)
		{
			Quaternion quat = Quaternion.LookRotation(rigidbody.velocity);
			quat = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * _rotationSpeed );
			transform.rotation = quat;
//			transform.LookAt(transform.position + rigidbody.velocity);
		}
	}

	void DoSeek(out Vector3 steeringForce, out float steeringMagnitude)
	{
		Vector3 desiredVelocity = _target - transform.position;
		Vector3 velocityDir = rigidbody.velocity;
		
		steeringForce = desiredVelocity - velocityDir;
		steeringMagnitude = steeringForce.magnitude;
		steeringForce.Normalize();
	}
}
