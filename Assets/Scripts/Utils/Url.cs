public static class Url
{
  private static string outgame = "http://185.81.165.107:31488";
  private static string ingame = "http://185.81.165.107:32762";
  private static string wsIngame = "ws://185.81.165.107:32762";

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

  public static string WsSub(string channelName, int playerNum)
  {
    return wsIngame + "/sub" + "?channel_id=" + channelName + "&player_num=" + playerNum.ToString();
  }

  public static string WsSub(string channelName)
  {
    return wsIngame + "/sub" + "?channel_id=" + channelName;
  }

  public static string Version()
  {
    return "https://raw.githubusercontent.com/bamboooo-dev/waiwai-privacy-policy/master/version.json";
  }
}

