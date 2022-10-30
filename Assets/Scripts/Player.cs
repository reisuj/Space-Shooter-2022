using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private float _baseSpeed = 5.0f;
    [SerializeField]
    private float _thrusterSpeed = 10.0f;
    
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
    private int _shieldStrength;
    private Renderer _shieldColor;
    private Color _shieldDefaultColor;

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
    [SerializeField]
    private AudioClip _powerupAudio;
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
        ThrusterControl();
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

    void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = _thrusterSpeed;
        }
        else
        {
            _speed = _baseSpeed;
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
            _shieldStrength--;
            ShieldCheck();
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
        PlayPowerupSound();
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
        PlayPowerupSound();
        StartCoroutine(SpeedUpPowerDownRoutine());
    }

    IEnumerator SpeedUpPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        
        PlayPowerupSound();
        _playerShield.SetActive(true);
    }

    public void ShieldPickedUp()
    {
        if (_shieldStrength < 3)
        {
            _shieldStrength++;
        }
        _isShieldActive = true;
        _playerShield.SetActive(true);
        ShieldCheck();
    }

    public void ShieldCheck()
    {
        switch (_shieldStrength)
        {
            case 0:
                _isShieldActive = false;
                _playerShield.SetActive(false);
                break;
            case 1:
                _shieldColor.material.SetColor("_Color", Color.red);
                break;
            case 2:
                _shieldColor.material.SetColor("_Color", Color.yellow);
                break;
            case 3:
                _shieldColor.material.SetColor("_Color", _shieldDefaultColor);
                break;
        }
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
        _shieldColor = _playerShield.GetComponent<Renderer>();
        _shieldDefaultColor = _playerShield.GetComponent<Renderer>().material.GetColor("_Color");
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

    private void PlayPowerupSound()
    {
        _audioSource.PlayOneShot(_powerupAudio);
    }
}
