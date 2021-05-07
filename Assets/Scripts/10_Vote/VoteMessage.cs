using System;

[Serializable]
public class VoteMessage
{
  public string type;
  public int mvpIndex;
  public int mwpIndex;
  public int nearIndex;
  public int farIndex;
  public int cycleIndex;
  public int playerIndex;

  public VoteMessage(string type, int mvpIndex, int mwpIndex, int nearIndex, int farIndex, int cycleIndex, int playerIndex)
  {
    this.type = type;
    this.mvpIndex = mvpIndex;
    this.mwpIndex = mwpIndex;
    this.nearIndex = nearIndex;
    this.farIndex = farIndex;
    this.cycleIndex = cycleIndex;
    this.playerIndex = playerIndex;
  }
}
