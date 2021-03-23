using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class GameFieldManager : MonoBehaviour
{
  public string channelName;
  public int playerCount;
  private int myNumber;
  private int myIndex;
  private GameObject selectedPlayer;
  public Text mainText;
  private WebSocket ws;
  public bool isHost;
  private int[] playerOrder;
  public int nowIndex = 0;
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
    else if (response.type == "next")
    {
      if (isHost)
      {
        judgeNext(response.player_id);
      }
    }
    else if (response.type == "next_result")
    {
      if (response.result == "OK")
      {
        SetResult("成功！");
      }
      else if (response.result == "NG")
      {
        SetResult("失敗・・・");
        Invoke(nameof(LoadVote), 3f);
      }
      else if (response.result == "FIN")
      {
        SetResult("ワイワイ！");
        Invoke(nameof(LoadVote), 3f);
      }
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
    playerOrder = CalculateOrder(numbers);
    string saveDirPath = Application.temporaryCachePath + "/" + channelName + "/";
    if (!Directory.Exists(saveDirPath))
    {
      Directory.CreateDirectory(saveDirPath);
    }
    string numbersFilePath = saveDirPath + "numbers.json";
    File.WriteAllText(numbersFilePath, JsonUtility.ToJson(numbers));
  }

  private int[] CalculateOrder(int[] numbers)
  {
    int[] sortNumbers = numbers.Clone() as int[];
    Array.Sort(sortNumbers);
    int[] order = new int[numbers.Length];
    for (int i = 0; i < numbers.Length; i++)
    {
      int index = Array.IndexOf(numbers, sortNumbers[i]);
      order[i] = index;
    }
    return order;
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

  private void SetMyNumber(int number)
  {
    myNumber = number;
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

  public void SendPredict()
  {
    int[] predicts = new int[playerCount];
    predicts[myIndex] = myNumber;
    for (int i = 0; i < playerCount; i++)
    {
      if (i == myIndex) { continue; }
      string objectName = "Emo" + i.ToString();
      if (i >= 4)
      {
        objectName += "(Clone)";
      }
      predicts[i] = int.Parse(GameObject.Find(objectName).transform.Find("Predict").GetComponent<InputField>().text);
    }
    PredictMessage predictMessage = new PredictMessage();
    predictMessage.type = "predict";
    predictMessage.predicts = predicts;
    predictMessage.predictor_index = myIndex;
    var json = JsonUtility.ToJson(predictMessage);
    ws.Send(json);
  }

  public void OnSelectPlayer(GameObject player)
  {
    EventSystem.current.SetSelectedGameObject(null);
    selectedPlayer = player;
    GameObject selectedPlayerName = GameObject.Find("SelectedPlayerName");
    selectedPlayerName.GetComponent<Text>().text = player.transform.Find("Name").GetComponent<Text>().text;
  }

  public void SendNextPlayer()
  {
    int playerID = extractPlayerID(selectedPlayer);
    NextPlayerMessage message = new NextPlayerMessage("next", playerID);
    ws.Send(JsonUtility.ToJson(message));
  }

  public int extractPlayerID(GameObject player)
  {
    string name = player.name;
    // 4番目が ID となっている
    return int.Parse(name.Substring(3, 1));
  }

  private void SetResult(string result)
  {
    mainText.text = result;
  }

  private void LoadVote()
  {
    SceneManager.sceneLoaded += WaitingRoomSceneLoaded;
    SceneManager.LoadScene("Vote");
  }

  private void judgeNext(int player_id)
  {
    int nextID = playerOrder[nowIndex];
    if (player_id == nextID)
    {
      SendNextResult("OK");
      nowIndex++;
    }
    else
    {
      SendNextResult("NG");
    }
  }

  private void SendNextResult(string result)
  {

  }

  private void WaitingRoomSceneLoaded(Scene next, LoadSceneMode mode)
  {
    var voteSceneManager = GameObject.FindWithTag("VoteSceneManager").GetComponent<VoteSceneManager>();
    voteSceneManager.channelName = channelName;

    SceneManager.sceneLoaded -= WaitingRoomSceneLoaded;
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
  public string result;
  public int player_id;
}

[Serializable]
public class PredictMessage
{
  public string type;
  public int[] predicts;
  public int predictor_index;
}

[Serializable]
public class NextPlayerMessage
{
  public string type;
  public int player_id;
  public NextPlayerMessage(string type, int player_id)
  {
    this.type = type;
    this.player_id = player_id;
  }
}

[Serializable]
public class NextResultMessage
{
  public string type;
  public string result;
  public NextResultMessage(string result)
  {
    this.type = "next_result";
    this.result = result;
  }
}
