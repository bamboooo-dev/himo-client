using System;

[Serializable]
public class OrderMessage
{
  public string type;
  public string result;
  public int player_index;
  public int cycleIndex;

  public OrderMessage(string type, string result, int playerIndex, int cycleIndex)
  {
    this.type = type;
    this.result = result;
    this.player_index = playerIndex;
    this.cycleIndex = cycleIndex;
  }
}
