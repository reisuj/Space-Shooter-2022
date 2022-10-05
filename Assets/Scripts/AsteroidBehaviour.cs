using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    private float _rotateSpeed;
    [SerializeField]
    private GameObject _explosion;
    private SpriteRenderer _sprite;
    [SerializeField]
    private SpawnManager _spawnmanager;

    // Start is called before the first frame update
    void Start()
    {
        _rotateSpeed = -50.0f;
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _sprite.sortingOrder = -1;
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnmanager.StartGame();
            Destroy(this.gameObject, .45f);
        }
    }
}
