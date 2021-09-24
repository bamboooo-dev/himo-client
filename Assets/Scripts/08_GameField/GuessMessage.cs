using System;

[Serializable]
public class GuessMessage
{
  public string type;
  public int[] numbers;
  public int playerIndex;
  public int cycleIndex;
  public int[][] predicts;
  public Reaction reaction;

  public GuessMessage(string type, int[] numbers, int playerIndex, int cycleIndex, int[][] predicts, Reaction reaction = default(Reaction))
  {
    this.type = type;
    this.numbers = numbers;
    this.playerIndex = playerIndex;
    this.cycleIndex = cycleIndex;
    this.predicts = predicts;
    this.reaction = reaction;
  }
}
