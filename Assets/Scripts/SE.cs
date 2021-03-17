using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{

    public AudioClip sound1;
    AudioSource audioSource;

    void Start()
    {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // タップ
        if (Input.GetMouseButtonDown(0))
        {
            //音(sound1)を鳴らす
            audioSource.PlayOneShot(sound1);
        }
    }
}