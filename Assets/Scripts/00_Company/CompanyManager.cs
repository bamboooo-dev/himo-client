﻿using UnityEngine;

public class CompanyManager : MonoBehaviour
{
  private float step_time;
  // Start is called before the first frame update
  void Start()
  {
    step_time = 0.0f;
#if UNITY_IOS
    ShowAttDialog.RequestIDFA();
#endif
  }

  // Update is called once per frame
  void Update()
  {
    // 経過時間をカウント
    step_time += Time.deltaTime;

    // 1秒後に画面遷移( Title へ移動)
    if (step_time >= 1.0f)
    {
      FadeManager.Instance.LoadScene("Scenes/Title", 0.2f);
    }
  }

  void OnDestroy()
  {
    AudioManager.GetInstance().PlayBGM(0);
  }
}
