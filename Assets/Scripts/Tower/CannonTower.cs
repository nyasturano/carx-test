using UnityEngine;

public class CannonTower : Tower
{
    [SerializeField] private float _rotationSpeed = 1.0f;
    [SerializeField] private float _projectileSpeed = 0.2f;
    [SerializeField] private float _targetingThreshold = 1f; 
    
    [SerializeField] private GameObject _cannon;
    [SerializeField] private GameObject _cannonHub;

    private Vector3 _direction;
    private Vector3 _offsetDirection;
    private Vector3 _axis;
    private float _speedRatio;

    private bool _isTargeted = false;

    protected override void Start()
    {
        base.Start();

        _vision.OnNewTarget += () => {
            _isTargeted = false;
            if (_vision.CurrentTarget)
            {
                RecalculateConstants();
            }
        };

    }

    private void Update()
    {
        if (_vision.CurrentTarget)
        {
            Calculations();
            SmoothRotation();
            CheckTargeting();
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
            _offsetDirection = Vector3.zero;
        }
    }

    private void SmoothRotation()
    {
        Quaternion cannonRotation = Quaternion.LookRotation(_offsetDirection);
        Quaternion cannonHubRotation = Quaternion.LookRotation(new Vector3(_offsetDirection.x, 0, _offsetDirection.z));
        
        _cannon.transform.rotation = Quaternion.Lerp(
            _cannon.transform.rotation,
            cannonRotation,
            Time.deltaTime * _rotationSpeed
         );

        _cannonHub.transform.rotation = Quaternion.Lerp(
            _cannonHub.transform.rotation, 
            cannonHubRotation, 
            Time.deltaTime * _rotationSpeed
         );

    }

    private void CheckTargeting()
    {
        if (!_isTargeted && Vector3.Angle(_offsetDirection, _cannon.transform.forward) < _targetingThreshold)
        {
            _isTargeted = true;
            TryShoot();
        }
    }


    private void RecalculateConstants()
    {
        _speedRatio = _vision.CurrentTarget.Speed / _projectileSpeed;
        _axis = Vector3.Cross(_shootPoint.transform.position - _vision.CurrentTarget.transform.position, _vision.CurrentTarget.transform.forward).normalized;
    }
    
    protected override bool ShootCondition()
    {
        return base.ShootCondition() && _isTargeted;
    }

    protected override void EmitProjectile()
    {
        Rigidbody body = Instantiate(_projectilePrefab, _shootPoint.transform.position, Quaternion.identity)
            .GetComponent<Rigidbody>();
        body.AddForce(_offsetDirection * _projectileSpeed * body.mass, ForceMode.Impulse);
    }
}
