using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VoteResultManager : MonoBehaviour
{
  [SerializeField] private VoteResultPlayer voteResultPlayer;
  [SerializeField] private GameObject voteResultPlayerParent;
  [SerializeField] private Sprite[] sprites;
  private VoteResultPlayer[] players;

  void Start()
  {
    // DEBUG
    // RoomStatus.points = new int[] { 10, 10, 10, 10, 10, 10 };
    // Cycle.names = new string[] { "しゅんこりん", "しゅんこりん", "しゅんこりん", "しゅんこりん", "しゅんこりん", "しゅんこりん" };
    // Cycle.mvpIndex = 1;
    // Cycle.mwpIndex = 2;

    players = new VoteResultPlayer[Cycle.names.Length];
    InstantiateOrderPlayers(RoomStatus.points, Cycle.names);

    // 同じ人になりうるため MVP の発表と MWP の発表を分ける
    Invoke(nameof(ShowMVP), 1.0f);
    Invoke(nameof(ShowMVPPoints), 2.0f);
    Invoke(nameof(ShowMVPAddedPoints), 3.0f);

    Invoke(nameof(ShowMWP), 5.0f);
    Invoke(nameof(ShowMWPPoints), 6.0f);
    Invoke(nameof(ShowMWPAddedPoints), 7.0f);

    // 画面遷移は自動で行う
    Invoke(nameof(MoveToExpectResult), 10.0f);
  }

  private void InstantiateOrderPlayers(int[] points, string[] names)
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
      var _player = Instantiate(voteResultPlayer, new Vector3(x, 0, 0), Quaternion.identity);
      _player.transform.SetParent(voteResultPlayerParent.transform.transform, false);
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

  private void ShowMVP()
  {
    players[Cycle.mvpIndex].transform.Find("MVPImage").GetComponent<Image>().gameObject.SetActive(true);
  }

  private void ShowMWP()
  {
    players[Cycle.mwpIndex].transform.Find("MWPImage").GetComponent<Image>().gameObject.SetActive(true);
  }

  private void ShowMVPPoints()
  {
    players[Cycle.mvpIndex].transform.Find("MVPText").GetComponent<Text>().gameObject.SetActive(true);
  }

  private void ShowMWPPoints()
  {
    players[Cycle.mwpIndex].transform.Find("MWPText").GetComponent<Text>().gameObject.SetActive(true);
  }

  private void ShowMVPAddedPoints()
  {
    players[Cycle.mvpIndex].transform.Find("MVPText").GetComponent<Text>().gameObject.SetActive(false);
    AddPoints(Cycle.mvpIndex, 3);
    players[Cycle.mvpIndex].transform.Find("MVPImage").GetComponent<Image>().gameObject.SetActive(false);
  }

  private void ShowMWPAddedPoints()
  {
    players[Cycle.mwpIndex].transform.Find("MWPText").GetComponent<Text>().gameObject.SetActive(false);
    AddPoints(Cycle.mwpIndex, -2);
    players[Cycle.mwpIndex].transform.Find("MWPImage").GetComponent<Image>().gameObject.SetActive(false);
  }

  private void AddPoints(int index, int point)
  {
    RoomStatus.points[index] += point;
    if (RoomStatus.cycleIndex != 2)
    {
      players[index].transform.Find("Point").Find("Text").GetComponent<Text>().text = RoomStatus.points[index].ToString();
    }
  }

  private void MoveToExpectResult()
  {
    SceneManager.LoadScene("ExpectResult");
  }
}
