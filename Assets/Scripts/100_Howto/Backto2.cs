﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Backto2 : MonoBehaviour
{
  void Start() { }

  void Update() { }

  public void OnClickHowTo()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("Howto2");
  }
}