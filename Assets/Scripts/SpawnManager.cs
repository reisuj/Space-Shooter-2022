using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private GameObject _enemyContainer;

    private bool _playerIsAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_playerIsAlive)
        {            
            SpawnEnemy();
            yield return new WaitForSeconds(5.0f);
        }        
    }

    void SpawnEnemy()
    {
        Vector3 enemyPosition = new Vector3(Random.Range(-9.5f, 9.5f), 10.0f, 0);
        GameObject newEnemy =  Instantiate(_enemy, enemyPosition, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    public void StopSpawning()
    {
        _playerIsAlive = false;
    }
}
