using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class ExpectResultManager : MonoBehaviour
{
  [SerializeField] private ExpectResultPlayer expectResultPlayer;
  [SerializeField] private GameObject expectResultPlayerParent;
  [SerializeField] private Sprite[] sprites;
  private ExpectResultPlayer[] players;
  public WebSocket ws;

  void Start()
  {
    // DEBUG
    // RoomStatus.points = new int[] { 10, 13, 10, 7, 10 };
    // Cycle.names = new string[] { "a", "b", "c", "d", "e" };
    Cycle.nearIndex = 1;
    Cycle.farIndex = 0;

    players = new ExpectResultPlayer[Cycle.names.Length];
    InstantiateExpectPlayers(RoomStatus.points, Cycle.names);

    // Near を表示する
    Invoke(nameof(ShowNearPlayer), 1.0f);
    Invoke(nameof(ShowNearPoint), 2.0f);
    Invoke(nameof(ShowNearAddedPoint), 3.0f);

    Invoke(nameof(ShowFarPlayer), 5.0f);
    Invoke(nameof(ShowFarPoint), 6.0f);
    Invoke(nameof(ShowFarAddedPoint), 7.0f);

    Invoke(nameof(MoveToNext), 10.0f);
  }

  private void InstantiateExpectPlayers(int[] points, string[] names)
  {
    int count = names.Length;

    string[] colors = new string[] { "#018D50", "#0178C2", "#FD8016", "#C4453F", "#714A9B", "#C031A5" };

    // 人数分インスタンス化する
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
      var _player = Instantiate(expectResultPlayer, new Vector3(x, 0, 0), Quaternion.identity);
      _player.transform.SetParent(expectResultPlayerParent.transform.transform, false);
      _player.transform.Find("Name").GetComponent<Text>().text = names[i];

      // 色をつける
      Color newCol;
      ColorUtility.TryParseHtmlString(colors[i], out newCol);
      _player.transform.Find("Name").GetComponent<Text>().color = newCol;
      _player.transform.Find("Point").Find("Text").GetComponent<Text>().color = newCol;

      if (RoomStatus.cycleIndex != 2)
      {
        _player.transform.Find("Point").Find("Text").GetComponent<Text>().text = RoomStatus.points[i].ToString();
      }
      else
      {
        _player.transform.Find("Point").Find("Text").GetComponent<Text>().text = "?";
      }
      _player.transform.Find("EmoImage").GetComponent<Image>().sprite = sprites[i];

      players[i] = _player;
    }
  }

  private void ShowNearPlayer()
  {
    players[Cycle.nearIndex].transform.Find("NearImage").GetComponent<Image>().gameObject.SetActive(true);
  }

  private void ShowFarPlayer()
  {
    players[Cycle.farIndex].transform.Find("FarImage").GetComponent<Image>().gameObject.SetActive(true);
  }

  private void ShowNearPoint()
  {
    players[Cycle.nearIndex].transform.Find("NearText").GetComponent<Text>().gameObject.SetActive(true);
  }

  private void ShowFarPoint()
  {
    players[Cycle.farIndex].transform.Find("FarText").GetComponent<Text>().gameObject.SetActive(true);
  }

  private void ShowNearAddedPoint()
  {
    players[Cycle.nearIndex].transform.Find("NearText").GetComponent<Text>().gameObject.SetActive(false);
    players[Cycle.nearIndex].transform.Find("NearImage").GetComponent<Image>().gameObject.SetActive(false);
    AddPoints(Cycle.nearIndex, 4);
  }

  private void ShowFarAddedPoint()
  {
    players[Cycle.farIndex].transform.Find("FarText").GetComponent<Text>().gameObject.SetActive(false);
    players[Cycle.farIndex].transform.Find("FarImage").GetComponent<Image>().gameObject.SetActive(false);
    AddPoints(Cycle.farIndex, -5);
  }

  private void AddPoints(int index, int point)
  {
    RoomStatus.points[index] += point;
    if (RoomStatus.cycleIndex != 2)
    {
      players[index].transform.Find("Point").Find("Text").GetComponent<Text>().text = RoomStatus.points[index].ToString();
    }

  }

  private void MoveToNext()
  {
    switch (RoomStatus.cycleIndex)
    {
      case 0:
        RoomStatus.cycleIndex++;
        SceneManager.LoadScene("Round2");
        break;
      case 1:
        RoomStatus.cycleIndex++;
        SceneManager.LoadScene("Round3");
        break;
      case 2:
        SceneManager.LoadScene("Calculating");
        break;
    }
  }
}
