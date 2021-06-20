using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Threading;
using WebSocketSharp;
using System.Collections;
using LitJson;

public class GameFieldManager : MonoBehaviour
{
  public Text themeText;
  public Text messageText;
  public Player player;
  public WebSocket ws;
  public Player[] players; // GuessButton が取得する用に定義
  [SerializeField] private GameObject playerParent;
  [SerializeField] private Sprite[] sprites;
  [SerializeField] private GameObject openFinishGameDialogButton;
  private static GameFieldManager instance;
  private bool connected;

  public static GameFieldManager Instance
  {
    get
    {
      if (null == instance)
      {
        instance = (GameFieldManager)FindObjectOfType(typeof(GameFieldManager));
        if (null == instance)
        {
          Debug.Log("GameFieldManager Instance Error");
        }
      }
      return instance;
    }
  }

  void Start()
  {
    // DEBUG
    // RoomStatus.cycleIndex = 0;
    // RoomStatus.themes = new Theme[] {
    //   new Theme(0, "好きな食べ物は")
    // };
    // Cycle.names = new string[] { "しゅんこりん", "しゅんこりん", "しゅんこりん" };
    // Cycle.numbers = new int[] { 1, 2, 3 };
    // Cycle.myIndex = 0;
    // Cycle.predicts = new int[Cycle.names.Length][];
    // for (int i = 0; i < Cycle.predicts.Length; i++)
    // {
    //   Cycle.predicts[i] = new int[Cycle.names.Length];
    // }

    Cycle.hasGuessed = false;
    themeText.text = RoomStatus.themes[RoomStatus.cycleIndex].Sentence;
    if (PlayerStatus.isHost) { openFinishGameDialogButton.SetActive(true); }
    players = new Player[Cycle.names.Length];

    InstantiatePlayers(Cycle.numbers, Cycle.names, Cycle.myIndex);
    SetupWebsocket();
    // コネクションのチェックを30秒ごとに行う
    InvokeRepeating("CheckWebsocketConnection", 5.0f, 5.0f);
  }

  void OnDestroy()
  {
    ws.Close();
    ws = null;
  }

  private void InstantiatePlayers(int[] numbers, string[] names, int myIndex)
  {
    int count = names.Length;

    string[] colors = new string[] { "#018D50", "#0178C2", "#FD8016", "#C4453F", "#714A9B", "#C031A5" };

    for (int i = 0; i < count; ++i)
    {
      int x;
      if (count == 1)
      {
        x = 0;
      }
      else
      {
        x = -750 + i * 1500 / (count - 1);
      }
      var _player = Instantiate(player, new Vector3(x, 0, 0), Quaternion.identity);
      _player.transform.SetParent(playerParent.transform.transform, false);
      _player.transform.Find("Name").GetComponent<Text>().text = names[i];
      Color newCol;
      ColorUtility.TryParseHtmlString(colors[i], out newCol);
      _player.transform.Find("Name").GetComponent<Text>().color = newCol;
      _player.transform.Find("InputField").Find("Text").GetComponent<Text>().color = newCol;
      newCol.a = 0.25f;
      _player.transform.Find("InputField").Find("Placeholder").GetComponent<Text>().color = newCol;
      _player.transform.Find("EmoImage").GetComponent<Image>().sprite = sprites[i];
      if (i == myIndex)
      {
        _player.transform.Find("InputField").GetComponent<InputField>().text = numbers[myIndex].ToString();
        _player.transform.Find("InputField").GetComponent<InputField>().readOnly = true;
      }
      players[i] = _player;
    }
  }

  private void SetupWebsocket()
  {
    ws = new WebSocket(Url.WsSub(RoomStatus.channelName));
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
    };
    var context = SynchronizationContext.Current;
    ws.OnMessage += (sender, e) =>
    {
      var message = JsonMapper.ToObject(e.Data);

      // 今回の cycle ではないものは除外
      if ((int)message["cycleIndex"] != RoomStatus.cycleIndex) { return; }

      switch ((string)message["type"])
      {
        case "guessProceed":
          for (int i = 0; i < Cycle.names.Length; ++i)
          {
            int[] p = new int[Cycle.names.Length];
            for (int j = 0; j < Cycle.names.Length; ++j)
            {
              p[j] = (int)message["predicts"][i][j];
            }
            Cycle.predicts[i] = p;
          }
          context.Post(state =>
          {
            SceneManager.LoadScene("Ordering");
          }, e.Data);
          break;

        case "guess":
          int[] predict = new int[Cycle.names.Length];
          for (int i = 0; i < Cycle.names.Length; i++)
          {
            predict[i] = (int)message["numbers"][i];
          }
          Cycle.predicts[(int)message["playerIndex"]] = predict;
          int index = (int)message["playerIndex"];
          context.Post(state =>
          {
            players[Int32.Parse(state.ToString())].transform.Find("GuessedImage").gameObject.SetActive(true);
            if (Int32.Parse(state.ToString()) == Cycle.myIndex) { Cycle.hasGuessed = true; }
          }, index);
          // ホストが全員の予想値を受け取れば画面遷移を促す
          if (PlayerStatus.isHost)
          {
            for (int i = 0; i < Cycle.names.Length; i++) { if (Cycle.predicts[i][0] == 0) { return; } }
            context.Post(state => { StartCoroutine(PostGuessProceed()); }, e.Data);
          }
          break;

        case "finishGame":
          context.Post(state =>
          {
            SceneManager.LoadScene("Home");
          }, e.Data);
          break;

        case "ping":
          if (Cycle.myIndex != (int)message["playerIndex"]) { return; }
          connected = true;
          break;

        default:
          break;
      }
    };

    ws.OnClose += (sender, e) =>
      {
        Debug.Log($"Websocket Close. StatusCode: {e.Code} Reason: {e.Reason}");
        if (e.Code == 1006) { ws.Connect(); }
      };
    ws.Connect();
  }

  private IEnumerator PostGuessProceed()
  {
    GuessMessage message = new GuessMessage("guessProceed", new int[Cycle.names.Length], Cycle.myIndex, RoomStatus.cycleIndex, Cycle.predicts);
    string json = JsonMapper.ToJson(message);
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

  private void CheckWebsocketConnection()
  {
    if (!connected)
    {
      try {
        Debug.Log("websocket connect 通ります");
        ws.Close();
        SetupWebsocket();
        Debug.Log("websocket connect 通りました");
      } catch (Exception e) {
        Debug.Log("websocket connect でエラー返ってきてます");
        Debug.Log(e.ToString());
      }
    }
    connected = false;
    StartCoroutine(PostPing());
  }

  private IEnumerator PostPing()
  {
    GuessMessage message = new GuessMessage("ping", new int[Cycle.names.Length], Cycle.myIndex, RoomStatus.cycleIndex, Cycle.predicts);
    string json = JsonMapper.ToJson(message);
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
