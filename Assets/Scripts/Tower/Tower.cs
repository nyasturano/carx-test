using System;
using System.Collections;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
	public event Action OnRecharged;

	[SerializeField] private float _shootInterval = 0.5f;
	[SerializeField] protected TowerVision _vision;
	[SerializeField] protected GameObject _projectilePrefab;
	[SerializeField] protected GameObject _shootPoint;

	private bool _isReady = true;

    protected virtual void Start()
    {
        OnRecharged += TryShoot;
		_vision.OnNewTarget += TryShoot;
	}

	private IEnumerator Recharge()
	{
		yield return new WaitForSecondsRealtime(_shootInterval);
		_isReady = true;
		OnRecharged?.Invoke();
	}

	protected void TryShoot()
	{
		if (ShootCondition())
		{
			EmitProjectile();
			_isReady = false;
			StartCoroutine(Recharge());
		}
	}

	protected virtual bool ShootCondition()
    {
		return _isReady && _vision.CurrentTarget;
	}

	protected abstract void EmitProjectile();
}
