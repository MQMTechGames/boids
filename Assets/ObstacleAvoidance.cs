using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObstacleAvoidance
{
	Boid _boid;

	[SerializeField]
	float _rayLength = 10f;

	Vector3 _steeringForce;
	public Vector3 SteeringForce { get { return _steeringForce; } }

	Vector3 debugPos;
	float debugRadius = -1;

	public void Init(Boid boid)
	{
		_boid = boid;
	}

	public void UpdateForces()
	{
		Vector3 position = _boid.transform.position;
		Vector3 rayDir = _boid.rigidbody.velocity.normalized;
		Ray ray = new Ray(position, rayDir);

		_steeringForce = Vector3.zero;

		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, _rayLength, 1 << LayerMask.NameToLayer("obstacle")))
		{
			float radius = hit.collider.bounds.size.x;
			radius = radius < hit.collider.bounds.size.z ? hit.collider.bounds.size.z : radius;
			radius *= 0.5f;

			debugRadius = radius;
			debugPos = hit.collider.transform.position;

			Vector3 obstacleAvoidanceForce = (hit.point - hit.collider.transform.position).normalized * radius;
			_steeringForce = obstacleAvoidanceForce - _boid.rigidbody.velocity;
			_steeringForce.y = 0f;
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		Vector3 position = _boid.transform.position;
		Vector3 rayDir = _boid.rigidbody.velocity.normalized * _rayLength;

		Gizmos.DrawLine(position, position + rayDir);

		Gizmos.DrawSphere(debugPos, debugRadius);
	}
}
