using System;
using System.Linq;
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
    Debug.Log("CardCheck Scene started");

    // ゲーム開始にともなって各パラメータの初期化をここで行う
    myNumberText.text = "";
    RoomInit();
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

  private void RoomInit()
  {
    Cycle.orderIndices = SortIndices(Cycle.numbers);
    Cycle.nextIndex = 0;
    Cycle.predicts = new int[Cycle.names.Length][];
    for (int i = 0; i < Cycle.predicts.Length; i++)
    {
      Cycle.predicts[i] = new int[Cycle.names.Length];
    }
  }

  private int[] SortIndices(int[] numbers)
  {
    int[] indices = new int[numbers.Length];
    int[] sortedNumbers = numbers.OrderBy(x => x).ToArray();
    for (int i = 0; i < numbers.Length; i++)
    {
      indices[i] = Array.IndexOf(sortedNumbers, numbers[i]);
    }
    return indices;
  }
}
