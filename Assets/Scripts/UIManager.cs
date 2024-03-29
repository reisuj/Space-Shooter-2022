using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to Text
    [SerializeField]
    private Text _scoreText;
    private int _currentScore = 0;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Text _ammoDepletedText;
    [SerializeField]
    private Sprite[] _liveSprites;
    private int _maxAmmo = 15;

    private bool _isGameOver;
    private bool _ammoDepleted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + _currentScore;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _isGameOver = false;
        UpdateAmmo(_maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        StartCoroutine(GameOverFlicker());
        _restartText.gameObject.SetActive(true);
        _isGameOver = true;
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateAmmo(int currentAmmo)
    {
        _ammoCountText.text = "Ammo: " + currentAmmo + "/" + _maxAmmo;
        if (currentAmmo < 1)
        {
            AmmoDepleted();
        }
        else
        {
            _ammoDepleted = false;
        }
    }

    public void AmmoDepleted()
    {
        _ammoDepleted = true;
        StartCoroutine(AmmoDepletedRoutine());
    }

    IEnumerator AmmoDepletedRoutine()
    {
        while (_ammoDepleted == true)
        {
            _ammoDepletedText.text = "AMMO DEPLETED!";
            yield return new WaitForSeconds(0.5f);
            _ammoDepletedText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
