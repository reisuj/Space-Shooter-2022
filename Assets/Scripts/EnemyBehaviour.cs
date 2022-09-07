using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        GetHandles();
        NullChecking();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player.Damage();
            EnemyDeath();
        }        
        
        if (other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);            
            _player.AddScore(10);            
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        _enemySpeed = 0.0f;
        _animator.SetTrigger("OnEnemyDeath");
        Destroy(this.gameObject, 2.5f);
    }

    private void GetHandles()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void NullChecking()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL!");
        }

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }
    }    
}
