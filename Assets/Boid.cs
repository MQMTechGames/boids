using UnityEngine;

[RequireComponent (typeof (SteeringManager))]
public class Boid : MonoBehaviour
{
	public enum SteeringBehavior
	{
		IDLE,
		SEEK,
	}

	[SerializeField]
	SteeringBehavior _steeringBehavior = SteeringBehavior.IDLE;

	[SerializeField]
	Transform _targetTrans;

	[SerializeField]
	BoidSquad _squad;

	public BoidSquad Squad { get { return _squad; } }

	SteeringManager _steeringManager;

	void Awake()
	{
		_steeringManager = GetComponent<SteeringManager>();
	}

	void Init(BoidSquad squad)
	{
		_squad = squad;
	}

	void Update ()
	{
		switch (_steeringBehavior) 
		{
		case SteeringBehavior.SEEK:
			SeekTarget();
			break;
		default:
			break;
		}
	}

	void SeekTarget ()
	{
		if(_targetTrans != null)
		{
			_steeringManager.Seek(_targetTrans.position);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		if(rigidbody.velocity.sqrMagnitude > 1e-1f)
		{
			Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
		}
	}
}
