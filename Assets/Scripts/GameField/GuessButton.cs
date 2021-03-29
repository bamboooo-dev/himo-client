using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GuessButton : MonoBehaviour
{
  public Text messageText;

  public async void OnClick()
  {
    Player[] players = GameObject.Find("GameFieldManager").GetComponent<GameFieldManager>().players;

    (int[] numbers, bool isValid) = Validate(players);
    if (!isValid) { return; }

    // 自分の予想を記憶しておく
    Cycle.predicts[Cycle.myIndex] = numbers;

    await PostGuess(numbers);

    this.gameObject.SetActive(false);
    messageText.text = "みんなの予想が終わるまで待ってね！";

    // 自分が予想完了したことを示す
    players[Cycle.myIndex].transform.Find("GuessedImage").gameObject.SetActive(true);

    for (int i = 0; i < Cycle.names.Length; i++)
    {
      if (Cycle.predicts[i][0] == 0)
      {
        return;
      }
    }
    SceneManager.LoadScene("Ordering");
  }

  private (int[], bool) Validate(Player[] players)
  {
    int[] validatedNumbers = new int[players.Length];
    for (int i = 0; i < players.Length; i++)
    {
      string input = players[i].transform.Find("InputField").GetComponent<InputField>().text;
      if (input == "")
      {
        messageText.text = "すべての予想を入力してね";
        return (validatedNumbers, false);
      }
      else if (Int32.Parse(input) <= 0 | Int32.Parse(input) >= 101)
      {
        messageText.text = "1から100で入力してね";
        return (validatedNumbers, false);
      }
      validatedNumbers[i] = Int32.Parse(input);
    }
    return (validatedNumbers, true);
  }

  private IEnumerator PostGuess(int[] numbers)
  {
    GuessMessage message = new GuessMessage(numbers, Cycle.myIndex);
    string json = JsonUtility.ToJson(message);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
    var request = new UnityWebRequest(Url.Pub(RoomStatus.channelName), "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
  }
}
