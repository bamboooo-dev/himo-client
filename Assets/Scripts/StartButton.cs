using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public class StartButton : MonoBehaviour
{
  // Start is called before the first frame update
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public async void OnClickStartButton()
  {
    await StartRoomRequest();
    SceneManager.sceneLoaded += GameFieldSceneLoaded;
    SceneManager.LoadScene("Gamefield");
  }

  private async UniTask<GroupResponse> StartRoomRequest()
  {
    var request = UnityWebRequest.Get(Url.Start());
    await request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException("failure.");
    }
    else
    {
      GroupResponse res = JsonUtility.FromJson<GroupResponse>(request.downloadHandler.text);
      return res;
    }
  }
  private void GameFieldSceneLoaded(Scene next, LoadSceneMode mode)
  {
    var gameFieldManager = GameObject.FindWithTag("GameFieldManager").GetComponent<GameFieldManager>();
    gameFieldManager.playerCount = RoomStatus.maxNum;

    SceneManager.sceneLoaded -= GameFieldSceneLoaded;
  }
}
