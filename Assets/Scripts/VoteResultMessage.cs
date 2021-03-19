using System;

[Serializable]
public class VoteResultMessage
{
  public string type = "vote_type";
  public int mvp_id;
  public int mwp_id;
  public int near_player_id;
  public int far_player_id;
  public int[] points;

  public VoteResultMessage(int mvp_id, int mwp_id, int near_player_id, int far_player_id, int[] points)
  {
    this.mvp_id = mvp_id;
    this.mwp_id = mwp_id;
    this.near_player_id = near_player_id;
    this.far_player_id = far_player_id;
    this.points = points;
  }
}
