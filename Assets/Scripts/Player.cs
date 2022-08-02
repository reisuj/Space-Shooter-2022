using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private float yMaxPosition, yMinPosition, xMaxPosition, xMinPosition;

    [SerializeField]
    private GameObject _playerLaser;
    // Start is called before the first frame update
    void Start()
    {
        yMaxPosition = 0.0f;
        yMinPosition = -5.0f;
        xMaxPosition = 11.3f;
        xMinPosition = -11.3f;

        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        FireLaser();
    }

    void PlayerMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);

        float yPosition = Mathf.Clamp(transform.position.y, yMinPosition, yMaxPosition);
        transform.position = new Vector3(transform.position.x, yPosition, 0);

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);
        if (transform.position.x < xMinPosition)
        {
            transform.position = new Vector3(xMaxPosition, transform.position.y, 0);
        }
        else if (transform.position.x > xMaxPosition)
        {
            transform.position = new Vector3(xMinPosition, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_playerLaser, transform.position, Quaternion.identity);
        }
    }
}
