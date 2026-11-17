using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnRate;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);

            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
