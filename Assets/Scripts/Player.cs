using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _baseSpeed = 5.0f;
    private float _speedMultiplier = 2.0f;       
    private float _yMaxPosition = 0.0f, _yMinPosition = -5.0f, _xMaxPosition = 11.3f, _xMinPosition = -11.3f;

    [Header("Player Weapon Settings")]
    [SerializeField]
    private int _maxAmmo = 15;
    [SerializeField]
    private int _currentAmmo;
    [SerializeField]
    private GameObject _playerLaser;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _multiShotActive = false;
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _nextFire = 0.0f;

    [Header("Player Shield Settings")]
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private bool _isShieldActive = false;
    private int _shieldStrength;
    private Renderer _shieldColor;
    private Color _shieldDefaultColor;

    [Header("Player Audio Settings")]
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _powerupAudio;
    private AudioSource _audioSource;

    [Header("Player Misc Settings")]
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [Header("Boost Bar Settings")]
    [SerializeField]
    private BoostBar _boostbar;
    [SerializeField]
    private float _thrusterSpeed = 10.0f;
    [SerializeField]
    private int _thrusterLevel = 1000;
    [SerializeField]
    private bool _thrustAvailable;
    private bool _thrusterRegen = false;
    private int _thrusterMax = 1000;


    void Start()
    {
        GetHandles();
        NullChecking();
        _currentAmmo = _maxAmmo;
        _thrustAvailable = true;
        _boostbar.SetStartBooster(_thrusterMax);
        _boostbar.SetStartBooster(_thrusterMax);
        _thrusterLevel = _thrusterMax;
        transform.position = new Vector3(0, -4.0f, 0);      
    }

    void Update()
    {
        PlayerMovement();
        ThrusterControl();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _currentAmmo > 0)
        {
            FireLaser();            
        }
        else if (_currentAmmo < 1)
        {
            _uiManager.AmmoDepleted();
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

    void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrustAvailable == true)
        {
            StartCoroutine(ThrusterActivated());
        }
    }

    private IEnumerator ThrusterActivated()
    {
        _speed = _thrusterSpeed;
        while (Input.GetKey(KeyCode.LeftShift) && _thrusterLevel > 0)
        {
            yield return new WaitForSeconds(0.5f);
            _thrusterLevel = _thrusterLevel - 1;
            _boostbar.SetBooster(_thrusterLevel);
            if (_thrusterLevel < 1)
            {
                _thrustAvailable = false;
            }
        }
        _speed = _baseSpeed;
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(ThrusterRecharge());
    }

    private IEnumerator ThrusterRecharge()
    {
        _thrusterRegen = true;
        yield return new WaitForSeconds(1.0f);
        while (_thrusterLevel < _thrusterMax && _thrusterRegen == true)
        {
            _thrusterLevel++;
            _boostbar.SetBooster(_thrusterLevel);
            yield return new WaitForSeconds(1.0f);
        }
        _thrustAvailable = true;
        _thrusterRegen = false;
    }


    void FireLaser()
    {
        _currentAmmo--;
        _uiManager.UpdateAmmo(_currentAmmo);
        Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y, 0);
        _nextFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShot, laserPosition, Quaternion.identity);
        }
        else if(_multiShotActive == true)
        {
            MultiShot();
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

        CameraControl cameraControl = Camera.main.GetComponent<CameraControl>();
        if (cameraControl != null)
        {
            cameraControl.CamShake();
        }

        _lives--;
        CheckEngineDamage(_lives);
        _uiManager.UpdateLives(_lives);        

        if (_lives < 1)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _currentAmmo += 5;
        _uiManager.UpdateAmmo(_currentAmmo);
        _tripleShotActive = true;
        PlayPowerupSound();
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void MultiShotActive()
    {
        _tripleShotActive = false;
        _multiShotActive = true;
        AmmoCollected(5);
        MultiShotPowerDownRoutine();
    }

    public void MultiShot()
    {
        for (int fireAngle = -67; fireAngle < 83; fireAngle += 15)
        {
            for (int fireangle = -67; fireangle < 83; fireangle += 15)
            {
                GameObject newBullet = Instantiate(_playerLaser, (new Vector3(transform.position.x, transform.position.y, 0)), Quaternion.identity);
                newBullet.transform.eulerAngles = Vector3.forward * fireangle;
            }
        }
    }

    IEnumerator MultiShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _multiShotActive = false;
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

    private void CheckEngineDamage(int lives)
    {
        if (lives == 3)
        {
            _rightEngine.SetActive(false);
        }
        if (lives == 2)
        {
            _rightEngine.SetActive(true);
            _leftEngine.SetActive(false);
        }
        else if (lives == 1)
        {
            _leftEngine.SetActive(true);
        }
    }

    public void AmmoCollected(int ammoAmount)
    {
        _currentAmmo += ammoAmount;        
        if (_currentAmmo > _maxAmmo)
        {
            _currentAmmo = _maxAmmo;
        }
        _uiManager.UpdateAmmo(_currentAmmo);
    }

    public void HealthCollected()
    {
        _lives++;
        if (_lives > 3)
        {
            _lives = 3;
        }
        CheckEngineDamage(_lives);
        _uiManager.UpdateLives(_lives);
    }

    public void AddScore(int points)
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }

    private void PlayPowerupSound()
    {
        _audioSource.PlayOneShot(_powerupAudio);
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
            Debug.LogError("The SpawnManager is NULL!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource is NULL!");
        }

        if (_shieldColor == null)
        {
            Debug.LogError("Shield Color is NULL!");
        }

        if (_shieldDefaultColor == null)
        {
            Debug.LogError("Default Shield Color is NULL!");
        }
    }    
}
