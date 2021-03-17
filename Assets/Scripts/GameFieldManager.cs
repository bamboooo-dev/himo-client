using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System;
using System.IO;

public class GameFieldManager : MonoBehaviour
{
  public string channelName;
  private WebSocket ws;
  void Start()
  {
    ws = SetupWebSocket();
    ws.Connect();
  }

  void Update() { }

  private WebSocket SetupWebSocket()
  {
    ws = new WebSocket("ws://localhost:8000/");
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
    };

    ws.OnMessage += (sender, e) =>
    {
      ProcessData(e.Data);
    };

    ws.OnError += (sender, e) =>
    {
      Debug.Log("WebSocket Error Message: " + e.Message);
    };

    ws.OnClose += (sender, e) =>
    {
      Debug.Log("WebSocket Close");
    };
    return ws;
  }

  private void ProcessData(string data)
  {
    var response = JsonUtility.FromJson<Response>(data);
    if (response.type == "answer")
    {
      SetAnswer(response);
    }
    else if (response.type == "predict")
    {
      SetPredict(response);
    }
  }

  private void SetAnswer(Response response)
  {
    SaveNumbers(response.numbers);
    SetNames(response.names);
    SetMyNumber(response.numbers[response.your_index]);
  }

  private void SetPredict(Response response)
  {
    SavePredicts(response.predictor_index, response.predicts);
    SetComplete(response.predictor_index);
  }

  private void SaveNumbers(int[] numbers)
  {
    string saveDirPath = Application.temporaryCachePath + "/" + channelName + "/";
    if (!Directory.Exists(saveDirPath))
    {
      Directory.CreateDirectory(saveDirPath);
    }
    string numbersFilePath = saveDirPath + "numbers.json";
    File.WriteAllText(numbersFilePath, JsonUtility.ToJson(numbers));
  }

  private void SetNames(string[] name)
  {

  }

  private void SetMyNumber(int myNumber)
  {
    GameObject.FindWithTag("MyNumber").GetComponent<Text>().text = myNumber.ToString();
  }

  private void SavePredicts(int index, int[] predicts)
  {
    string saveDirPath = Application.temporaryCachePath + "/" + channelName + "/";
    if (!Directory.Exists(saveDirPath))
    {
      Directory.CreateDirectory(saveDirPath);
    }
    string predictsFilePath = saveDirPath + index.ToString() + "_predicts.json";
    File.WriteAllText(predictsFilePath, JsonUtility.ToJson(predicts));
  }

  private void SetComplete(int index) { }
}

[Serializable]
public class Response
{
  public string type;
  public int[] numbers;
  public string[] names;
  public int[] predicts;
  public int your_index;
  public int predictor_index;
}
