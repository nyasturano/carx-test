using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerVision : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private SphereCollider _collider;

    private List<Enemy> _targets = new List<Enemy>();
    private Dictionary<Enemy, Action> _removeActions = new Dictionary<Enemy, Action>();

    public event Action OnNewTarget;

    public bool IsEmpty { get => _targets.Count == 0; private set { } }
    public Enemy CurrentTarget { get; private set; }

    private void Start()
    {
        _collider.radius = _radius;
        CurrentTarget = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Enemy.Tag))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            
            if (enemy)
            {
                _targets.Add(enemy);
                _removeActions.Add(enemy, null);
                SetCurrentTarget();

                _removeActions[enemy] = () =>
                {
                    _targets.Remove(enemy);
                    _removeActions.Remove(enemy);
                    SetCurrentTarget();
                };

                enemy.OnDeath += _removeActions[enemy];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Enemy.Tag))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.OnDeath -= _removeActions[enemy];
                _removeActions[enemy]?.Invoke();
            }
        }
    }

    private void SetCurrentTarget()
    {
        if (IsEmpty)
        {
            CurrentTarget = null;
            return;
        }

        if (CurrentTarget != _targets[0])
        {
            CurrentTarget = _targets[0];
            OnNewTarget?.Invoke();
        }
    }
}
