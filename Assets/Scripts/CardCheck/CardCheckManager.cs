using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CardCheckManager : MonoBehaviour
{
  public Text myNumberText;
  void Start()
  {
    int myNumber = ExtractMyNumber();
    myNumberText.text = myNumber.ToString();
  }

  private int ExtractMyNumber()
  {
    string nickname = FetchNickname();
    int index = Array.IndexOf(Cycle.names, nickname);
    return Cycle.numbers[index];
  }

  private string FetchNickname()
  {
    return File.ReadAllText(SavePath.nickname);
  }
}
