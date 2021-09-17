using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using WebSocketSharp;

public class FinalResultManager : MonoBehaviour
{
  [SerializeField] private Text firstPlaceText;
  [SerializeField] private Text secondPlaceText;
  [SerializeField] private Text thirdPlaceText;
  [SerializeField] private Text lastPlaceText;
  [SerializeField] private Text firstPointText;
  [SerializeField] private Text secondPointText;
  [SerializeField] private Text thirdPointText;
  [SerializeField] private Text lastPointText;
  [SerializeField] private Place placePrefab;
  [SerializeField] private GameObject placeParent;
  [SerializeField] private Sprite[] placeSprites;
  private Result[] results;

  [SerializeField] private Text waitText;
  [SerializeField] private Button againButton;
  [SerializeField] private Button finishButton;

  public WebSocket ws;
  [SerializeField] private AgainDialog againDialog = default;
  private bool connected;

  void Start()
  {
    // DEBUG
    // RoomStatus.points = new int[] { 3, 2, 1, 4, 5, 6 };
    // Cycle.names = new string[] { "a", "b", "c", "jdk", "しゅんこりん", "あさしゅん" };
    // PlayerStatus.isHost = true;
    // Cycle.myIndex = 0;

    SortNames();
    SetupWebSocket();
    InvokeRepeating("CheckWebsocketConnection", 5.0f, 5.0f);

    switch (this.results.Length)
    {
      case 1:
        StartCoroutine(ShowPlace(3.0f, 0, 1));
        StartCoroutine(ShowButtons(4.0f));
        break;
      case 2:
        StartCoroutine(ShowPlace(3.0f, 1, 2));
        StartCoroutine(ShowPlace(5.0f, 0, 2));
        StartCoroutine(ShowButtons(6.0f));
        break;
      case 3:
        StartCoroutine(ShowPlace(3.0f, 2, 3));
        StartCoroutine(ShowPlace(4.0f, 1, 3));
        StartCoroutine(ShowPlace(5.0f, 0, 3));
        StartCoroutine(ShowButtons(6.0f));
        break;
      case 4:
        StartCoroutine(ShowPlace(3.0f, 2, 4));
        StartCoroutine(ShowPlace(4.0f, 1, 4));
        StartCoroutine(ShowPlace(6.0f, 0, 4));
        StartCoroutine(ShowPlace(6.0f, 3, 4));
        StartCoroutine(ShowButtons(7.0f));
        break;
      case 5:
        StartCoroutine(ShowPlace(3.0f, 3, 5));
        StartCoroutine(ShowPlace(4.0f, 2, 5));
        StartCoroutine(ShowPlace(5.0f, 1, 5));
        StartCoroutine(ShowPlace(7.0f, 0, 5));
        StartCoroutine(ShowPlace(7.0f, 4, 5));
        StartCoroutine(ShowButtons(8.0f));
        break;
      case 6:
        StartCoroutine(ShowPlace(3.0f, 4, 6));
        StartCoroutine(ShowPlace(4.0f, 3, 6));
        StartCoroutine(ShowPlace(5.0f, 2, 6));
        StartCoroutine(ShowPlace(6.0f, 1, 6));
        StartCoroutine(ShowPlace(8.0f, 0, 6));
        StartCoroutine(ShowPlace(8.0f, 5, 6));
        StartCoroutine(ShowButtons(9.0f));
        break;
      default:
        break;
    }
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

  private void ProcessData(string data, SynchronizationContext context)
  {
    var message = JsonUtility.FromJson<FinalResultMessage>(data);
    switch (message.type)
    {
      case "finish":
        context.Post(state =>
        {
          RoomStatus.finished = true;
          SceneManager.LoadScene("Home");
        }, message.type);
        break;

      case "again":
        context.Post(state =>
          {
            againDialog.gameObject.SetActive(true);
            ws.Close();
            ws = null;
          }, message.type);
        break;

      case "ping":
        if (Cycle.myIndex != message.playerIndex) { return; }
        connected = true;
        break;

      default:
        break;
    }
  }

  private IEnumerator ShowPlace(float waitTime, int place, int maxPlace)
  {
    yield return new WaitForSeconds(waitTime);
    int y;
    y = 450 - (place + 1) * 900 / (maxPlace + 1);
    var _place = Instantiate(placePrefab, new Vector3(0, y, 0), Quaternion.identity);
    _place.transform.SetParent(placeParent.transform.transform, false);
    _place.transform.Find("Point").GetComponent<Text>().text = results[place].point.ToString() + "pt";
    _place.transform.Find("Name").GetComponent<Text>().text = results[place].name;
    if (place >= 3 && place == maxPlace - 1)
    {
      place = placeSprites.Length - 1;
    }
    _place.transform.Find("PlaceImage").GetComponent<Image>().sprite = placeSprites[place];
  }

  private void SortNames()
  {
    Result[] results = new Result[RoomStatus.points.Length];
    for (int i = 0; i < RoomStatus.points.Length; i++)
    {
      results[i] = new Result(RoomStatus.points[i], Cycle.names[i]);
    }
    this.results = results.OrderByDescending(x => x.point).ToArray();
  }

  private IEnumerator ShowButtons(float waitTime)
  {
    yield return new WaitForSeconds(waitTime);
    if (PlayerStatus.isHost)
    {
      againButton.gameObject.SetActive(true);
      finishButton.gameObject.SetActive(true);
    }
    else
    {
      waitText.gameObject.SetActive(true);
    }
  }

  private void CheckWebsocketConnection()
  {
    if (!connected)
    {
      try
      {
        ws.Close();
        SetupWebSocket();
      }
      catch (Exception e)
      {
        Debug.Log(e.ToString());
      }
    }
    connected = false;
    StartCoroutine(PostPing());
  }

  private IEnumerator PostPing()
  {
    FinalResultMessage message = new FinalResultMessage("ping", Cycle.myIndex);
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
