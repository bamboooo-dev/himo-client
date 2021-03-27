using System;

[Serializable]
public class OrderMessage
{
  public string type = "order";
  public string result;
  public int player_index;

  public OrderMessage(string result, int playerIndex)
  {
    this.result = result;
    this.player_index = playerIndex;
  }
}
