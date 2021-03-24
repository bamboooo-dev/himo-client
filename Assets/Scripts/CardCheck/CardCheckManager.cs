using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardCheckManager : MonoBehaviour
{
  public Text myNumberText;
  private float step_time;

  void Start()
  {
    int myNumber = ExtractMyNumber();
    myNumberText.text = myNumber.ToString();
  }

  void Update()
  {
    step_time += Time.deltaTime;

    if (step_time >= 3.0f)
    {
      SceneManager.LoadScene("GameField");
    }
  }

  private int ExtractMyNumber()
  {
    string nickname = FetchNickname();
    Cycle.myIndex = Array.IndexOf(Cycle.names, nickname);
    return Cycle.numbers[Cycle.myIndex];
  }

  private string FetchNickname()
  {
    return File.ReadAllText(SavePath.nickname);
  }
}
