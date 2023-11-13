using UnityEngine;

public class Monster : Enemy {

	[SerializeField] private Rigidbody _rigidbody;

	public GameObject MoveTarget { get; set; } 

    private void Update ()
	{
		if (MoveTarget)
        {
			Movement();
        }
	}

	private void Movement()
    {
		Vector3 direction = (MoveTarget.transform.position - transform.position).normalized;
		Vector3 offset = direction * (Speed * Time.deltaTime);

		_rigidbody.MovePosition(_rigidbody.position + offset);
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject == MoveTarget)
		{
			Death();
		}
	}
}
