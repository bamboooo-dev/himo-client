using System;

[Serializable]
public class OrderMessage
{
  public string type = "order";
  public string result;
  public int player_index;
  public int cycleIndex;

  public OrderMessage(string result, int playerIndex, int cycleIndex)
  {
    this.result = result;
    this.player_index = playerIndex;
    this.cycleIndex = cycleIndex;
  }
}
