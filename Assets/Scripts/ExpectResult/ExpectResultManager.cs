using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExpectResultManager : MonoBehaviour
{
  [SerializeField] private ExpectResultPlayer expectResultPlayer;
  [SerializeField] private GameObject expectResultPlayerParent;
  [SerializeField] private Sprite[] sprites;
  private ExpectResultPlayer[] players;

  void Start()
  {
    // DEBUG
    // RoomStatus.points = new int[] { 10, 13, 10, 7, 10 };
    // Cycle.names = new string[] { "a", "b", "c", "d", "e" };
    // Cycle.nearIndex = 1;
    // Cycle.farIndex = 2;

    players = new ExpectResultPlayer[Cycle.names.Length];
    InstantiateExpectPlayers(RoomStatus.points, Cycle.names);

    // 1秒後に Near・Far を表示する
    Invoke(nameof(ShowNearFarPlayers), 1.0f);

    Invoke(nameof(ShowNearFarPoints), 2.0f);

    Invoke(nameof(ShowAddedPoints), 3.0f);

    Invoke(nameof(MoveToNext), 5.0f);
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

  private void ShowNearFarPlayers()
  {
    players[Cycle.nearIndex].transform.Find("NearImage").GetComponent<Image>().gameObject.SetActive(true);
    players[Cycle.farIndex].transform.Find("FarImage").GetComponent<Image>().gameObject.SetActive(true);
  }

  private void ShowNearFarPoints()
  {
    players[Cycle.nearIndex].transform.Find("NearText").GetComponent<Text>().gameObject.SetActive(true);
    players[Cycle.farIndex].transform.Find("FarText").GetComponent<Text>().gameObject.SetActive(true);
  }

  private void ShowAddedPoints()
  {
    AddPoints(Cycle.nearIndex, Cycle.farIndex);
  }

  private void AddPoints(int nearIndex, int farIndex)
  {
    RoomStatus.points[nearIndex] += 4;
    RoomStatus.points[farIndex] -= 5;
    players[Cycle.nearIndex].transform.Find("NearText").GetComponent<Text>().gameObject.SetActive(false);
    players[Cycle.farIndex].transform.Find("FarText").GetComponent<Text>().gameObject.SetActive(false);
    for (int i = 0; i < RoomStatus.points.Length; ++i)
    {
      if (RoomStatus.cycleIndex != 2)
      {
        players[i].transform.Find("Point").Find("Text").GetComponent<Text>().text = RoomStatus.points[i].ToString();
      }
      else
      {
        players[i].transform.Find("Point").Find("Text").GetComponent<Text>().text = "?";
      }
    }
  }

  private void MoveToNext()
  {
    if (RoomStatus.cycleIndex != 2)
    {
      RoomStatus.cycleIndex++;
      SceneManager.LoadScene("CardCheck");
    }
    else
    {
      SceneManager.LoadScene("Calculating");
    }
  }
}
