using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;

    
    private float _yMaxPosition = 0.0f, _yMinPosition = -5.0f, _xMaxPosition = 11.3f, _xMinPosition = -11.3f;

    [SerializeField]
    private GameObject _playerLaser;

    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private float _laserOffset = 1.05f;
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _nextFire = 0.0f;

    [SerializeField]
    private int _lives = 3;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    void PlayerMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);

        float yPosition = Mathf.Clamp(transform.position.y, _yMinPosition, _yMaxPosition);
        transform.position = new Vector3(transform.position.x, yPosition, 0);

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);
        if (transform.position.x < _xMinPosition)
        {
            transform.position = new Vector3(_xMaxPosition, transform.position.y, 0);
        }
        else if (transform.position.x > _xMaxPosition)
        {
            transform.position = new Vector3(_xMinPosition, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y + _laserOffset, 0);
        _nextFire = Time.time + _fireRate;
        Instantiate(_playerLaser, laserPosition, Quaternion.identity);
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }
}
