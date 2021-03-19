using System;

[Serializable]
public class VoteMessage
{
  public string type;
  public int mvp_id;
  public int mwp_id;

  public VoteMessage(int mvp_id, int mwp_id)
  {
    this.mvp_id = mvp_id;
    this.mwp_id = mwp_id;
  }
}
