using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _fireTime = -1;
    private bool _canFire = true;

    private int movementTypeID;


    // Start is called before the first frame update
    void Start()
    {
        GetHandles();
        NullChecking();

        movementTypeID = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _fireTime && _canFire == true)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _fireTime = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            LaserBehaviour[] lasers = enemyLaser.GetComponentsInChildren<LaserBehaviour>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        switch (movementTypeID)
        {
            case 1:
                transform.Translate((Vector3.down + Vector3.left) * (_enemySpeed / 2) * Time.deltaTime);
                break;
            case 2:
                transform.Translate((Vector3.down + Vector3.right) * (_enemySpeed / 2) * Time.deltaTime);
                break;
            case 3:
                transform.Translate(Vector3.down * Time.deltaTime * _enemySpeed);
                break;
            default:
                break;
        }

        

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
            _canFire = false;
            _player.Damage();
            EnemyDeath();
        }        
        
        if (other.gameObject.tag == "Laser")
        {
            _canFire = false;
            Destroy(other.gameObject);            
            _player.AddScore(10);            
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        Destroy(GetComponent<Collider2D>());
        _animator.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0.0f;
        _audioSource.Play();        
        Destroy(this.gameObject, 2.5f);
    }

    private void GetHandles()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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
