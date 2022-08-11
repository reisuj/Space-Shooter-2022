using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8.0f;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _laserSpeed);

        if (transform.position.y > 6.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
