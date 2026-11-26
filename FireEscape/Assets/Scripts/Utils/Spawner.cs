using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _checkOccupiedRadius;
    [SerializeField] private LayerMask _obstacleLayer;

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
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            bool isOccupied = Physics.CheckSphere(
                spawnPoint.position,
                _checkOccupiedRadius,
                _obstacleLayer
            );

            if (isOccupied) continue;

            Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_spawnPoints == null) return;

        Gizmos.color = Color.red;

        foreach (Transform point in _spawnPoints)
        {
            if (point != null)
                Gizmos.DrawWireSphere(point.position, _checkOccupiedRadius);
        }
    }
}
