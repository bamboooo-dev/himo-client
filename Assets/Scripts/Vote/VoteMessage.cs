using System;

[Serializable]
public class VoteMessage
{
  public string type;
  public int mvpIndex;
  public int mwpIndex;

  public VoteMessage(string type, int mvpIndex, int mwpIndex)
  {
    this.type = type;
    this.mvpIndex = mvpIndex;
    this.mwpIndex = mwpIndex;
  }
}
