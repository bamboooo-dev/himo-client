using System;

[Serializable]
public class GuessMessage
{
  public string type = "guess";
  public int[] numbers;
  public int playerIndex;
  public int cycleIndex;

  public GuessMessage(int[] numbers, int playerIndex, int cycleIndex)
  {
    this.numbers = numbers;
    this.playerIndex = playerIndex;
    this.cycleIndex = cycleIndex;
  }
}
