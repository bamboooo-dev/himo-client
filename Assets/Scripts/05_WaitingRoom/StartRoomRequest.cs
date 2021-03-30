using System;

[Serializable]
public class StartRoomRequest
{
  public string channel_name;

  public StartRoomRequest(string channelName)
  {
    this.channel_name = channelName;
  }
}
