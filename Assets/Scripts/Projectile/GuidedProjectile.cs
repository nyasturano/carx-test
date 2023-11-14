using UnityEngine;

public class GuidedProjectile : Projectile {

	[SerializeField] private float _speed;
	[SerializeField] private Rigidbody _rigidbody;

	public GameObject Target { get; set; }

    private void Update()
    {
		if (Target) 
		{
			Movement();
		}
		else
		{
			Destroy(gameObject);
		}
    }

    public void Movement () {

		Vector3 direction = (Target.transform.position - transform.position).normalized;
		Vector3 offset = direction * (_speed * Time.deltaTime);

		_rigidbody.MovePosition(_rigidbody.position + offset);
	}
}
