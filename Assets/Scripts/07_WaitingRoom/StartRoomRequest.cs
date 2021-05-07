using System;

[Serializable]
public class StartRoomRequest
{
  public string channel_name;
  public int cycle_index;

  public StartRoomRequest(string channelName, int cycleIndex)
  {
    this.channel_name = channelName;
    this.cycle_index = cycleIndex;
  }
}
