using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class VoteSceneManager : MonoBehaviour
{
  public string channelName;
  public int maxNum;
  public bool isHost;
  private WebSocket ws;
  private int[] mvpCount;
  private int[] mwpCount;
  private VoteResult result;
  void Start()
  {
    ws = SetupWebSocket();
    ws.Connect();
  }

  void Update() { }

  private WebSocket SetupWebSocket()
  {
    ws = new WebSocket(Url.Sub(channelName));
    ws.OnMessage += (sender, e) =>
    {
      ProcessData(e.Data);
    };

    ws.OnError += (sender, e) =>
    {
      Debug.Log("WebSocket Error Message: " + e.Message);
    };
    return ws;
  }

  private void ProcessData(string data)
  {
    VoteSceneResponse response = JsonUtility.FromJson<VoteSceneResponse>(data);
    if (response.type == "vote" & isHost)
    {
      ProcessVote(response);
    }
    if (response.type == "vote_result")
    {
      ProcessVoteResult(response);
    }
  }

  public void OnConfirm()
  {
    SendVote();
    SceneManager.LoadScene("MiddleResult");
  }

  void SendVote()
  {
    int[] votes = GetVote();
    VoteMessage message = new VoteMessage(votes[0], votes[1]);
    string json = JsonUtility.ToJson(message);
    ws.Send(json);
  }

  private int[] GetVote()
  {
    return new int[] { 0, 1 };
  }

  private void SendVoteResult(int mvp_id, int mwp_id)
  {
    var (near_player_id, far_player_id, points) = GetProgress(mvp_id, mwp_id);
    VoteResultMessage message = new VoteResultMessage(mvp_id, mwp_id, near_player_id, far_player_id, points);
    string json = JsonUtility.ToJson(message);
    ws.Send(json);
  }

  private void ProcessVote(VoteSceneResponse response)
  {
    mvpCount[response.mvp_id]++;
    mwpCount[response.mwp_id]++;
    if (mvpCount.Sum() == maxNum)
    {
      var mvp_id = mvpCount.Select((x, i) => new { x, i })
    .Aggregate((max, xi) => xi.x > max.x ? xi : max).i;
      var mwp_id = mwpCount.Select((x, i) => new { x, i })
    .Aggregate((min, xi) => xi.x < min.x ? xi : min).i;
      SendVoteResult(mvp_id, mwp_id);
      SceneManager.LoadScene("MiddleResult");
    }
  }

  private (int, int, int[]) GetProgress(int mvp_id, int mwp_id)
  {
    return (0, 2, new int[] { 4, 6, 8, 3, 10 });
  }

  private void ProcessVoteResult(VoteSceneResponse response)
  {
    result = new VoteResult(response.mvp_id, response.mwp_id, response.near_player_id, response.far_player_id, response.points);
    SceneManager.sceneLoaded += MiddleResultSceneLoaded;
    SceneManager.LoadScene("MiddleResult");
  }

  private void MiddleResultSceneLoaded(Scene next, LoadSceneMode mode)
  {
    var middleResultSceneManager = GameObject.FindWithTag("MiddleResultSceneManager").GetComponent<MiddleResultSceneManager>();
    middleResultSceneManager.result = result;

    SceneManager.sceneLoaded -= MiddleResultSceneLoaded;
  }
}
