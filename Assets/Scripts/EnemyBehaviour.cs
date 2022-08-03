using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _enemySpeed);

        if (transform.position.y < -8.0f)
        {
            float _newPositionX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(_newPositionX, 10.0f, 0);
        }
    }
}
