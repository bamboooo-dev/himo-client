using UnityEngine;
using System.Threading.Tasks;
using System;
using Grpc.Core;
using Himo.V1;

class GetRoomTest
{
  // Can be run from commandline.
  // Example command:
  // "/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -nographics -executeMethod GetRoom.RunGetRoom -logfile"
  public static void RunGetRoom()
  {
    Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

    Debug.Log("==============================================================");
    Debug.Log("Starting tests");
    Debug.Log("==============================================================");

    Debug.Log("Application.platform: " + Application.platform);
    Debug.Log("Environment.OSVersion: " + Environment.OSVersion);

    var response = GetRoom(1);
    Debug.Log("Table Name: " + response.Table.Name);

    Debug.Log("==============================================================");
    Debug.Log("Tests finished successfully.");
    Debug.Log("==============================================================");
  }

  public static ContentResponse GetRoom(ulong id)
  {
    // 接続するチャンネルを生成
    // ここでは、Golang の gRPC サーバーは dev サーバーの5502番ポートに建っているのでそう指定
    Channel channel = new Channel("168.138.198.236:5502", ChannelCredentials.Insecure);

    var client = new Room.RoomClient(channel);

    // GetContent メソッドを実行する
    var response = client.GetContent(new ContentRequest { Id = id });

    channel.ShutdownAsync().Wait();

    return response;
  }
}
