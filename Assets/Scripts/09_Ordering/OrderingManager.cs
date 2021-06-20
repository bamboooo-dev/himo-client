using System;
using System.Linq;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using WebSocketSharp;

public class OrderingManager : MonoBehaviour
{
  public Text themeText;
  public OrderPlayer orderPlayer;
  public GameObject orderPlayerParent;
  public OrderPlayer[] players;
  [SerializeField] private Sprite[] sprites;
  private Button[] btns;
  [SerializeField] private Button orderButton;
  [SerializeField] private Text message;
  [SerializeField] private Image successImage;
  [SerializeField] private Image failedImage;
  private WebSocket ws;
  private bool connected;


  void Start()
  {
    // DEBUG
    // RoomStatus.cycleIndex = 0;
    // RoomStatus.themes = new Theme[] {
    //   new Theme(0, "好きな食べ物は")
    // };
    // Cycle.names = new string[] { "a", "b", "c", "d", "e", "f" };
    // Cycle.numbers = new int[] { 1, 2, 3, 4, 5, 6 };
    // Cycle.myIndex = 0;
    // Cycle.predicts = new int[Cycle.names.Length][];
    // for (int i = 0; i < Cycle.predicts.Length; i++)
    // {
    //   Cycle.predicts[i] = new int[Cycle.names.Length];
    // }
    // Cycle.orderIndices = SortIndices(Cycle.numbers);
    // PlayerStatus.isHost = true;
    // DEBUG

    themeText.text = RoomStatus.themes[RoomStatus.cycleIndex].Sentence;

    players = new OrderPlayer[Cycle.names.Length];
    InstantiateOrderPlayers(Cycle.numbers, Cycle.names);
    if (PlayerStatus.isHost)
    {
      PlayerPrefs.SetInt("radio", -1);
      SetBtns();
      orderButton.gameObject.SetActive(true);
    }
    else
    {
      message.gameObject.SetActive(true);
    }
    SetupWebSocket();
    // コネクションのチェックを定期的に行う
    InvokeRepeating("CheckWebSocketConnection", 5.0f, 5.0f);
  }

  private int[] SortIndices(int[] numbers)
  {
    int[] indices = new int[numbers.Length];
    int[] sortedNumbers = numbers.OrderBy(x => x).ToArray();
    for (int i = 0; i < sortedNumbers.Length; i++)
    {
      indices[i] = Array.IndexOf(numbers, sortedNumbers[i]);
    }
    return indices;
  }

  void OnDestroy()
  {
    ws.Close();
    ws = null;
  }

  private void SetupWebSocket()
  {
    ws = new WebSocket(Url.WsSub(RoomStatus.channelName));
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
    };
    var context = SynchronizationContext.Current;
    ws.OnMessage += (sender, e) =>
    {
      ProcessData(e.Data, context);
    };
    ws.OnClose += (sender, e) =>
    {
      Debug.Log($"Websocket Close. StatusCode: {e.Code} Reason: {e.Reason}");
      if (e.Code == 1006) { ws.Connect(); }
    };
    ws.Connect();
  }

  private void InstantiateOrderPlayers(int[] numbers, string[] names)
  {
    int count = names.Length;

    string[] colors = new string[] { "#018D50", "#0178C2", "#FD8016", "#C4453F", "#714A9B", "#C031A5" };

    btns = new Button[names.Length];

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
      var _player = Instantiate(orderPlayer, new Vector3(x, 0, 0), Quaternion.identity);
      _player.transform.SetParent(orderPlayerParent.transform.transform, false);
      _player.transform.Find("Name").GetComponent<Text>().text = names[i];
      Color newCol;
      ColorUtility.TryParseHtmlString(colors[i], out newCol);
      _player.transform.Find("Name").GetComponent<Text>().color = newCol;
      _player.transform.Find("Predict").Find("Text").GetComponent<Text>().color = newCol;

      _player.transform.Find("Predict").Find("Text").GetComponent<Text>().text = Cycle.predicts[Cycle.myIndex][i].ToString();

      _player.transform.Find("EmoImage").GetComponent<Image>().sprite = sprites[i];

      if (PlayerStatus.isHost)
      {
        int tmp = i;
        var button = _player.transform.Find("Button").GetComponent<Button>();
        button.gameObject.SetActive(true);
        button.onClick.AddListener(delegate { OnClick(tmp); });
        btns[tmp] = button;
      }

      players[i] = _player;
    }
  }

  void OnClick(int tmp)
  {
    PlayerPrefs.SetInt("radio", tmp);
    SetBtns();
  }

  void SetBtns()
  {
    int r = PlayerPrefs.GetInt("radio");
    int cnt = 0;
    foreach (Button b in btns)
    {
      b.interactable = (cnt != r);
      cnt++;
    }
  }

  private void ProcessData(string data, SynchronizationContext context)
  {
    var message = JsonUtility.FromJson<OrderMessage>(data);
    if (message.cycleIndex != RoomStatus.cycleIndex) return;
    switch (message.type)
    {
      case "order":
        string result = message.result;
        context.Post(state =>
        {
          if (state.ToString() == "OK")
          {
            successImage.gameObject.SetActive(true);
            Invoke(nameof(EraseSuccessImage), 1.0f);
          }
          else if (state.ToString() == "NG")
          {
            failedImage.gameObject.SetActive(true);
            Invoke(nameof(EraseFailedImage), 1.0f);
          }
          else if (state.ToString() == "FIN")
          {
            successImage.gameObject.SetActive(true);
            Invoke(nameof(Finish), 1.0f);
          }
        }, result);
        int index = message.player_index;
        context.Post(state =>
        {
          players[Int32.Parse(state.ToString())].gameObject.SetActive(false);
        }, index);
        break;

      case "ping":
        if (Cycle.myIndex != message.player_index) { return; }
        connected = true;
        break;

      default:
        break;
    }
  }

  private void CheckWebSocketConnection()
  {
    if (!connected)
    {
      try {
        ws.Close();
        SetupWebSocket();
      } catch (Exception e) {
        Debug.Log(e.ToString());
      }
    }
    connected = false;
    StartCoroutine(PostPing());
  }

  private IEnumerator PostPing()
  {
    OrderMessage message = new OrderMessage("ping", "PING", Cycle.myIndex, RoomStatus.cycleIndex);
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

  private void EraseSuccessImage()
  {
    successImage.gameObject.SetActive(false);
  }

  private void EraseFailedImage()
  {
    failedImage.gameObject.SetActive(false);
    SceneManager.LoadScene("Vote");
  }

  private void Finish()
  {
    successImage.gameObject.SetActive(false);
    SceneManager.LoadScene("Vote");
  }
}
