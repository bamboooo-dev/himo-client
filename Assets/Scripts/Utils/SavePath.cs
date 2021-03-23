using UnityEngine;

public static class SavePath
{
  public static string TmpDir(string channelName)
  {
    return Application.temporaryCachePath + "/" + channelName + "/";
  }

}
