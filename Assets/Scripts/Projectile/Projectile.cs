using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int _damage = 10;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Enemy.Tag))
        {
            other.gameObject.GetComponent<Enemy>()?.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
