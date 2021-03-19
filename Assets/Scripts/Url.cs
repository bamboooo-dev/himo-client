public static class Url
{
  private static string ingame = "http://168.138.198.236:5502";
  public static string getSub(string channelName)
  {
    return ingame + "/sub?channel_id=" + channelName;
  }
}

