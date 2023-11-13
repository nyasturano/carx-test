using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHP = 30;
    [SerializeField] private float _speed = 0.1f;

    private int _hp;

    public static readonly string Tag = "Enemy";
    public event Action OnDeath;
 
    public float Speed { get => _speed; private set { } }

    private void Start()
    {
        _hp = _maxHP;
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

}
