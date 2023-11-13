using UnityEngine;

public class GuidedProjectile : Projectile {

	[SerializeField] private float _speed;
	[SerializeField] private Rigidbody _rigidbody;

	public GameObject Target { get; set; }

    private void Update()
    {
		Movement();
    }

    public void Movement () {
		if (Target == null) {
			Destroy(gameObject);
			return;
		}

		Vector3 direction = (Target.transform.position - transform.position).normalized;
		Vector3 offset = direction * (_speed * Time.deltaTime);

		_rigidbody.MovePosition(_rigidbody.position + offset);
	}
}
