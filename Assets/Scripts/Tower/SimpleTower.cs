using UnityEngine;

public class SimpleTower : Tower
{
	protected override void EmitProjectile()
    {
		Instantiate(_projectilePrefab, _shootPoint.transform.position, Quaternion.identity)
			.GetComponent<GuidedProjectile>()
			.Target = _vision.CurrentTarget.gameObject;
    }
}
