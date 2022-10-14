using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.0f;
    
    private float _yMaxPosition = 0.0f, _yMinPosition = -5.0f, _xMaxPosition = 11.3f, _xMinPosition = -11.3f;

    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject _playerLaser;
    [SerializeField]
    private GameObject _tripleShot;

    private SpawnManager _spawnManager;
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _nextFire = 0.0f;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserAudio;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GetHandles();
        NullChecking();
        transform.position = new Vector3(0, -4.0f, 0);        
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
        Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y, 0);
        _nextFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShot, laserPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(_playerLaser, laserPosition, Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _playerShield.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedUpActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedUpPowerDownRoutine());
    }

    IEnumerator SpeedUpPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _playerShield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }

    private void GetHandles()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void NullChecking()
    {
        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource is NULL");
        }
    }
}
