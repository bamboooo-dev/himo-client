using System;

[Serializable]
public class StartRoomResponse
{
  public string type;
  public int[] numbers;
  public string[] names;
  public int cycle_index;

  public StartRoomResponse(string type)
  {
    this.type = type;
  }
}
