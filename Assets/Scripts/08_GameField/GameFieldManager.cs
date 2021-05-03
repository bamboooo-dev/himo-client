using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using WebSocketSharp;

public class GameFieldManager : MonoBehaviour
{
  public Text themeText;
  public Player player;
  public WebSocket ws;
  public Player[] players; // GuessButton が取得する用に定義
  [SerializeField] private GameObject playerParent;
  [SerializeField] private Sprite[] sprites;

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

    themeText.text = RoomStatus.themes[RoomStatus.cycleIndex].Sentence;
    players = new Player[Cycle.names.Length];
    InstantiatePlayers(Cycle.numbers, Cycle.names, Cycle.myIndex);
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

  private void ProcessData(string data, SynchronizationContext context)
  {
    Debug.Log(data);
    var message = JsonUtility.FromJson<GuessMessage>(data);
    if (message.type != "guess" | message.cycleIndex != RoomStatus.cycleIndex) return;
    Cycle.predicts[message.playerIndex] = message.numbers;
    int index = message.playerIndex;
    context.Post(state =>
    {
      players[Int32.Parse(state.ToString())].transform.Find("GuessedImage").gameObject.SetActive(true);
      for (int i = 0; i < Cycle.names.Length; i++)
      {
        if (Cycle.predicts[i][0] == 0)
        {
          return;
        }
      }
      SceneManager.LoadScene("Ordering");
    }, index);
  }
}
