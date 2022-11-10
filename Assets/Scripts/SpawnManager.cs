using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _powerupContainer;

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private float _enemySpawnDelay = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;    

    private bool _playerIsAlive = true;

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_playerIsAlive)
        {            
            SpawnEnemy();
            yield return new WaitForSeconds(_enemySpawnDelay);
        }        
    }
    
    void SpawnEnemy()
    {
        Vector3 enemyPosition = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
        GameObject newEnemy =  Instantiate(_enemy, enemyPosition, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_playerIsAlive)
        {
            SpawnPowerup();
            yield return new WaitForSeconds(Random.Range(3, 8));
        }        
    }

    private void PowerUpSelector()
    {
        int weight = Random.Range(1, 111);


    }

    void SpawnPowerup()
    {
        Vector3 powerupPosition = new Vector3(Random.Range(-9.5f, 9.5f), 10.0f, 0);
        int randomPowerUp = Random.Range(0, _powerups.Length);
        GameObject newPowerup = Instantiate(_powerups[randomPowerUp], powerupPosition, Quaternion.identity);
        newPowerup.transform.parent = _powerupContainer.transform;
    }

    public void StopSpawning()
    {
        _playerIsAlive = false;
    }

    public void StartGame()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
}
