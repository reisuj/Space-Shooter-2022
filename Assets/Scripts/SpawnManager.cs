using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;

    [SerializeField]
    private float _enemySpawnDelay = 5.0f;

    private bool _playerIsAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_playerIsAlive)
        {            
            SpawnEnemy();
            yield return new WaitForSeconds(_enemySpawnDelay);
        }        
    }
    
    void SpawnEnemy()
    {
        Vector3 enemyPosition = new Vector3(Random.Range(-9.5f, 9.5f), 10.0f, 0);
        GameObject newEnemy =  Instantiate(_enemy, enemyPosition, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_playerIsAlive)
        {
            SpawnPowerup();
            yield return new WaitForSeconds(Random.Range(3, 8));
        }        
    }

    void SpawnPowerup()
    {
        Vector3 powerupPosition = new Vector3(Random.Range(-9.5f, 9.5f), 10.0f, 0);
        GameObject newPowerup = Instantiate(_tripleShotPrefab, powerupPosition, Quaternion.identity);
        newPowerup.transform.parent = _powerupContainer.transform;
    }

    public void StopSpawning()
    {
        _playerIsAlive = false;
    }
}
