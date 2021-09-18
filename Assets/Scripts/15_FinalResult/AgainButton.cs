using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;

public class AgainButton : MonoBehaviour
{
  public async void OnClick()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("AgainRoom");
  }
}
