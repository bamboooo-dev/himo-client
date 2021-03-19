using System;

[Serializable]
public class VoteSceneResponse
{
  public string type;
  public int mvp_id;
  public int mwp_id;
  public int near_player_id;
  public int far_player_id;
  public int[] points;
}
