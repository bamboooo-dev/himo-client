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
    // RoomStatus.points = new int[] { 10, 10, 10, 10, 10 };
    // Cycle.names = new string[] { "a", "b", "c", "d", "e" };
    // Cycle.mvpIndex = 1;
    // Cycle.mwpIndex = 2;

    players = new VoteResultPlayer[Cycle.names.Length];
    InstantiateOrderPlayers(RoomStatus.points, Cycle.names);

    // 1秒後に MVP・MWP を表示する
    Invoke(nameof(ShowVotedPlayers), 1.0f);

    Invoke(nameof(ShowVotedPoints), 2.0f);

    Invoke(nameof(ShowAddedPoints), 3.0f);

    Invoke(nameof(MoveToExpectResult), 5.0f);
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

      _player.transform.Find("Point").Find("Text").GetComponent<Text>().text = RoomStatus.points[i].ToString();

      _player.transform.Find("EmoImage").GetComponent<Image>().sprite = sprites[i];

      players[i] = _player;
    }
  }

  private void ShowVotedPlayers()
  {
    players[Cycle.mvpIndex].transform.Find("MVPImage").GetComponent<Image>().gameObject.SetActive(true);
    players[Cycle.mwpIndex].transform.Find("MWPImage").GetComponent<Image>().gameObject.SetActive(true);
  }

  private void ShowVotedPoints()
  {
    players[Cycle.mvpIndex].transform.Find("MVPText").GetComponent<Text>().gameObject.SetActive(true);
    players[Cycle.mwpIndex].transform.Find("MWPText").GetComponent<Text>().gameObject.SetActive(true);
  }

  private void ShowAddedPoints()
  {
    AddPoints(Cycle.mvpIndex, Cycle.mwpIndex);
    players[Cycle.mvpIndex].transform.Find("Point").Find("Text").GetComponent<Text>().gameObject.SetActive(true);
    players[Cycle.mwpIndex].transform.Find("Point").Find("Text").GetComponent<Text>().gameObject.SetActive(true);
  }

  private void AddPoints(int mvpIndex, int mwpIndex)
  {
    RoomStatus.points[mvpIndex] += 3;
    RoomStatus.points[mwpIndex] -= 3;
    players[Cycle.mvpIndex].transform.Find("MVPText").GetComponent<Text>().gameObject.SetActive(false);
    players[Cycle.mwpIndex].transform.Find("MWPText").GetComponent<Text>().gameObject.SetActive(false);
    for (int i = 0; i < RoomStatus.points.Length; ++i)
    {
      players[i].transform.Find("Point").Find("Text").GetComponent<Text>().text = RoomStatus.points[i].ToString();
    }
  }

  private void MoveToExpectResult()
  {
    SceneManager.LoadScene("ExpectResult");
  }
}
