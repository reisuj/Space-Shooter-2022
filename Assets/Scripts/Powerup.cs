using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    //ID for Powerups
    //0 = Triple Shot
    //1 = Speed
    //2 = Shields
    [SerializeField]
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

            if (player != null)
            {
                player.TripleShotActive();
            }

            // add switch statement

            //Condition 0 is Triple Shot
            //Consition 1 is SpeedUp
            //Condition 2 is Shields
            //Add default condition if no other true

            Destroy(this.gameObject);
        }
    }
}
