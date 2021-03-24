using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class GuessButton : MonoBehaviour
{
  private WebSocket ws;
  public Text messageText;

  public void OnClick()
  {
    Player[] players = GameObject.Find("GameFieldManager").GetComponent<GameFieldManager>().players;
    int[] numbers = new int[players.Length];
    for (int i = 0; i < players.Length; i++)
    {
      string input = players[i].transform.Find("InputField").GetComponent<InputField>().text;
      if (input == "")
      {
        messageText.text = "すべての予想を入力してね";
        return;
      }
      numbers[i] = Int32.Parse(input);
    }
    Cycle.predicts[Cycle.myIndex] = numbers;
    GuessMessage message = new GuessMessage(numbers, Cycle.myIndex);
    string json = JsonUtility.ToJson(message);
    ws = GameObject.Find("GameFieldManager").GetComponent<GameFieldManager>().ws;
    ws.Send(json);
    for (int i = 0; i < Cycle.names.Length; i++)
    {
      if (Cycle.predicts[i][0] == 0)
      {
        return;
      }
    }
    SceneManager.LoadScene("HostGeneralConsulting");
    this.gameObject.SetActive(false);
    messageText.text = "みんなの予想が終わるまで待ってね！";
    players[message.playerIndex].transform.Find("GuessedImage").gameObject.SetActive(true);
  }
}
