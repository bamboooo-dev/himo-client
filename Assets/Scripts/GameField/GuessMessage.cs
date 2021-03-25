using System;

[Serializable]
public class GuessMessage
{
  public string type = "guess";
  public int[] numbers;
  public int playerIndex;

  public GuessMessage(int[] numbers, int playerIndex)
  {
    this.numbers = numbers;
    this.playerIndex = playerIndex;
  }
}
