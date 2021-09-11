using System;

[Serializable]
public class FinalResultMessage
{
  public string type;
  public int playerIndex;

  public FinalResultMessage(string type, int playerIndex)
  {
    this.type = type;
    this.playerIndex = playerIndex;
  }
}
