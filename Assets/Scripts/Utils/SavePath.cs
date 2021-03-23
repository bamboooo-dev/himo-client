using UnityEngine;

public static class SavePath
{
  public static string token = Application.persistentDataPath + "/access-token.jwt";
  public static string nickname = Application.persistentDataPath + "/nickname.txt";

  public static string TmpDir(string channelName)
  {
    return Application.temporaryCachePath + "/" + channelName + "/";
  }

}
