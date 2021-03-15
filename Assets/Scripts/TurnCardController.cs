using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TurnCardController : MonoBehaviour
{
  private Image img;
  private Sprite ura;
  private Sprite omote;
  public Text text;
  void Start()
  {
    img = GetComponent<Image>();
    // 裏面の画像(Assets/Resource/Sprites/ura.png)
    ura = Resources.Load<Sprite>("Sprites/ura");

    // 表面の画像
    omote = Resources.Load<Sprite>("Sprites/hosi");

    // 最初は裏面を表示する
    transform.eulerAngles = new Vector3(0, 0, 0);

    cardOpen(100f, true);
  }

  void Update()
  {

  }

  public async Task cardOpen(float speed, bool toOmote)
  {
    float angle = -180f;
    transform.SetAsFirstSibling();


    //回転させる
    while (angle < 0f)
    {
      if (angle >= -90f)
      {
        img.sprite = toOmote ? omote : ura;
        text.text = "39";
      }
      else
      {
        img.sprite = toOmote ? ura : omote;
      }
      angle += speed * Time.deltaTime;
      transform.eulerAngles = new Vector3(0, angle, 0);
      await Task.Delay(TimeSpan.FromSeconds(0.01f));
    }

    transform.eulerAngles = new Vector3(0, 0, 0);
  }

}
