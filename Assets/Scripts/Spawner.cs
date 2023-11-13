using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	[SerializeField] private GameObject _monsterPrefab;
	[SerializeField] private GameObject _moveTarget;

	[SerializeField] private float _interval = 3f;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
		while (true)
        {
			Instantiate(_monsterPrefab, transform.position, Quaternion.LookRotation(_moveTarget.transform.position - transform.position))
				.GetComponent<Monster>()
				.MoveTarget = _moveTarget;
			
			yield return new WaitForSecondsRealtime(_interval);
		}
    }
}
