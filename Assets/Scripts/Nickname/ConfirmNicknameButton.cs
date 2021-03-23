using UnityEngine;
using UnityEngine.UI;
using Grpc.Core;
using Himo.V1;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.SceneManagement;

public class ConfirmNicknameButton : MonoBehaviour
{
  InputField inputField;

  void Start()
  {
    inputField = GameObject.Find("InputField").GetComponent<InputField>();
  }

  void Update() { }

  string GetNickname()
  {
    return inputField.text;
  }

  public AsyncUnaryCall<Himo.V1.SignUpResponse> SignUp(string nickname)
  {
    // 接続するチャンネルを生成
    // ここでは、Golang の gRPC サーバーは dev サーバーの5502番ポートに建っているのでそう指定
    Channel channel = new Channel(Url.OutgameIP(), ChannelCredentials.Insecure);

    UserManager.UserManagerClient client = new UserManager.UserManagerClient(channel);

    AsyncUnaryCall<SignUpResponse> call = client.SignUpAsync(new SignUpRequest { Nickname = nickname });

    channel.ShutdownAsync().Wait();

    return call;
  }

  public void OnClickNicknameButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    string nickname = GetNickname();

    AsyncUnaryCall<SignUpResponse> call = SignUp(nickname);
    Task<Metadata> headers = call.ResponseHeadersAsync;
    string token = headers.Result.GetValue("access-token");

    File.WriteAllText(SavePath.token, token);
    File.WriteAllText(SavePath.nickname, nickname);
    SceneManager.LoadScene("Home");
  }
}
