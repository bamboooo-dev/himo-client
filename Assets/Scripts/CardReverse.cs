using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class a : MonoBehaviour
{
    private Image img;
    private Sprite ura;
    private Sprite omote;
    void Start()
    {
        img = GetComponent<Image>();
        // 裏面の画像(Assets/Resource/Sprites/ura.png)
        ura = Resources.Load<Sprite>("Sprites/ura");

        // 表面の画像
        omote = Resources.Load<Sprite>("Sprites/omote");

        // 最初は裏面を表示する
        img.sprite = ura;

        cardOpen(100f);
    }

    void Update()
    {

    }

    public async Task cardOpen(float speed)
    {
        float angle = -180f;
        transform.SetAsFirstSibling();

        //回転させる
        while (angle < 0f)
        {
            angle += speed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, angle, 0);
            await Task.Delay(TimeSpan.FromSeconds(0.01f));
        }

        transform.eulerAngles = new Vector3(0, 0, 0);
    }
}