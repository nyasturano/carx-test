using System;
using UnityEngine;

public class CannonTower : Tower
{
    public event Action OnTargeted;

    [SerializeField] private GameObject _cannon;

    [SerializeField] private float _rotationSpeed = 1.0f;
    [SerializeField] private float _projectileSpeed = 0.2f;
    [SerializeField] private float _targetingThreshold = 1f;

    private Vector3 _direction;
    private Vector3 _offsetDirection;
    private float _speedRatio;
    private Vector3 _axis;

    protected override void Start()
    {
        base.Start();
        _vision.OnNewTarget += NewTargetCalculations;
    }

    private void Update()
    {
        if (_vision.CurrentTarget)
        {
            Calculations();
            SmoothRotation();
        }

        Debug.DrawLine(_shootPoint.transform.position, _shootPoint.transform.position + _direction * 10, Color.red);
        Debug.DrawLine(_shootPoint.transform.position, _shootPoint.transform.position + _offsetDirection * 10, Color.blue);
    }

    private void Calculations()
    {
        if (_speedRatio <= 1)
        {
            _direction = (_vision.CurrentTarget.transform.position - _shootPoint.transform.position).normalized;


            float angleB = Vector3.SignedAngle(-_direction, _vision.CurrentTarget.transform.forward, _axis);
            float angleA = Mathf.Asin(Mathf.Sin(angleB * Mathf.Deg2Rad) * _speedRatio);
            
            _offsetDirection = Quaternion.AngleAxis(-angleA * Mathf.Rad2Deg, _axis) * _direction;
        }
        else
        {
            // снаряд не успеет долететь
            _offsetDirection = Vector3.zero;
        }
    }

    private void SmoothRotation()
    {
        _cannon.transform.rotation = Quaternion.Slerp(
            _cannon.transform.rotation, 
            Quaternion.LookRotation(ToHorizontal(_offsetDirection)), 
            Time.deltaTime * _rotationSpeed
         );
    }

    private void NewTargetCalculations()
    {
        _speedRatio = _vision.CurrentTarget.Speed / _projectileSpeed;
        _axis = Vector3.Cross(_shootPoint.transform.position - _vision.CurrentTarget.transform.position, _vision.CurrentTarget.transform.forward).normalized;
    }

    private Vector3 ToHorizontal(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    protected override void EmitProjectile()
    {
        Rigidbody body = Instantiate(_projectilePrefab, _shootPoint.transform.position, Quaternion.identity)
            .GetComponent<Rigidbody>();
        body.AddForce(_offsetDirection * _projectileSpeed * body.mass, ForceMode.Impulse);
    }
}
