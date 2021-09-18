using UnityEngine;
using UnityEngine.UI;
using Grpc.Core;
using Himo.V1;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.SceneManagement;

public class ConfirmUpdateNicknameButton : MonoBehaviour
{
  InputField inputField;
  public GameObject errorText;

  void Start()
  {
    inputField = GameObject.Find("InputField").GetComponent<InputField>();
    inputField.text = File.ReadAllText(SavePath.nickname);
  }

  string GetNickname()
  {
    return inputField.text;
  }

  public AsyncUnaryCall<Himo.V1.UpdateUserNameResponse> UpdateNickname(string nickname)
  {
    // 接続するチャンネルを生成
    // ここでは、Golang の gRPC サーバーは dev サーバーの5502番ポートに建っているのでそう指定
    Channel channel = new Channel(Url.OutgameIP(), ChannelCredentials.Insecure);

    UserManager.UserManagerClient client = new UserManager.UserManagerClient(channel);
    var metadata = new Metadata
    {
      { "Authorization", Token.getToken() }
    };

    AsyncUnaryCall<UpdateUserNameResponse> call = client.UpdateUserNameAsync(new UpdateUserNameRequest { Nickname = nickname }, metadata);

    channel.ShutdownAsync().Wait();

    return call;
  }

  public void OnClickNicknameButton()
  {
    AudioManager.GetInstance().PlaySound(0);

    string nickname = GetNickname();
    if (nickname == "")
    {
      errorText.SetActive(true);
      return;
    }

    AsyncUnaryCall<UpdateUserNameResponse> call = UpdateNickname(nickname);
    var result = call.ResponseAsync.Result;

    File.WriteAllText(SavePath.nickname, nickname);
    SceneManager.LoadScene("Home");
  }
}
