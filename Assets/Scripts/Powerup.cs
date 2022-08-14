using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField] // 0 = TripleShot, 1 = Speed, 2 = Shields
    private int _powerupID;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -7.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            switch (_powerupID)
            {
                case 0:
                    Debug.Log("TripleShot Enabled");
                    player.TripleShotActive();
                    break;
                case 1:
                    Debug.Log("Speed Enabled");
                    player.SpeedUpActive();
                    break;
                case 2:
                    Debug.Log("Shields Enabled");
                    break;
                default:
                    break;
            }

            Destroy(this.gameObject);
        }
    }
}
