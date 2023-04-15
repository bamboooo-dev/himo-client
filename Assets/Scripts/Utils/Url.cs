public static class Url
{
  // prod
  private static string outgame = "http://138.2.43.212:30837";
  // dev
  // private static string outgame = "http://168.138.198.236:5502";
  // prod
  private static string ingame = "http://138.2.43.212:30687";
  // dev
  // private static string ingame = "http://168.138.198.236";

  // prod
  private static string wsIngame = "ws://138.2.43.212:30687";
  // dev
  // private static string wsIngame = "ws://168.138.198.236";


  public static string OutgameIP()
  {
    return outgame.Replace("http://", "");
  }
  public static string Sub(string channelName)
  {
    return outgame + "/sub" + "/" + channelName;
  }

  public static string Enter()
  {
    return ingame + "/enter";
  }

  public static string Room()
  {
    return ingame + "/room";
  }

  public static string Group(string channelName)
  {
    return ingame + "/group" + "?channel_id=" + channelName;
  }

  public static string Pub(string channelName)
  {
    return ingame + "/pub" + "?channel_id=" + channelName;
  }

  public static string Start()
  {
    return ingame + "/start";
  }

  public static string Update()
  {
    return ingame + "/update";
  }

  public static string WsSub(string channelName, int playerNum)
  {
    return wsIngame + "/sub" + "?channel_id=" + channelName + "&player_num=" + playerNum.ToString();
  }

  public static string WsSub(string channelName)
  {
    return wsIngame + "/sub" + "?channel_id=" + channelName;
  }

  public static string Status()
  {
    return "https://raw.githubusercontent.com/bamboooo-dev/waiwai-privacy-policy/master/status.json";
  }
}
