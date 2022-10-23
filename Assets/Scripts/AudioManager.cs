using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosion;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayExplosion()
    {
        _audioSource.PlayOneShot(_explosion);
    }
}
