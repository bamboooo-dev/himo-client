using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System;
using System.IO;

public class GameFieldManager : MonoBehaviour
{
  public string channelName;
  public int playerCount;
  private WebSocket ws;
  void Start()
  {
    SetupEmos(playerCount);
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

  private void SetNames(string[] names)
  {
    for (int i = 0; i < names.Length; i++)
    {
      string objectName = "Emo" + i.ToString();
      if (i >= 4)
      {
        objectName += "(Clone)";
      }
      GameObject.Find(objectName).transform.Find("Name").GetComponent<Text>().text = names[i];
    }
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

  [SerializeField] private GameObject parent = default;
  [SerializeField] private GameObject emo4 = default;
  [SerializeField] private GameObject emo5 = default;
  private void SetupEmos(int playerCount)
  {
    if (playerCount == 5)
    {
      var _emo4 = Instantiate(emo4);
      _emo4.transform.SetParent(parent.transform, false);
    }
    else if (playerCount == 6)
    {
      var _emo4 = Instantiate(emo4);
      _emo4.transform.SetParent(parent.transform, false);
      var _emo5 = Instantiate(emo5);
      _emo5.transform.SetParent(parent.transform, false);
    }
  }
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
