using UnityEngine;
using UnityEngine.UI;
using Grpc.Core;
using Himo.V1;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.SceneManagement;

public class NicknameButton : MonoBehaviour
{
  InputField inputField;
  // Start is called before the first frame update
  void Start()
  {
    inputField = GameObject.Find("InputField").GetComponent<InputField>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  string GetNickname()
  {
    return inputField.text;
  }

  public AsyncUnaryCall<Himo.V1.SignUpResponse> SignUp(string nickname)
  {
    // 接続するチャンネルを生成
    // ここでは、Golang の gRPC サーバーは dev サーバーの5502番ポートに建っているのでそう指定
    Channel channel = new Channel("168.138.198.236:5502", ChannelCredentials.Insecure);

    UserManager.UserManagerClient client = new UserManager.UserManagerClient(channel);

    AsyncUnaryCall<SignUpResponse> call = client.SignUpAsync(new SignUpRequest { Nickname = nickname });

    channel.ShutdownAsync().Wait();

    return call;
  }

  public void OnClickNicknameButton()
  {
    string nickname = GetNickname();

    AsyncUnaryCall<SignUpResponse> call = SignUp(nickname);
    Task<Metadata> headers = call.ResponseHeadersAsync;
    string token = headers.Result.GetValue("access-token");

    string tokenPath = Application.persistentDataPath + "/access-token.jwt";
    File.WriteAllText(tokenPath, token);
    SceneManager.LoadScene("Home");
  }
}
