using System;

[Serializable]
public class Theme
{
  public int ID;
  public string Sentence;

  public Theme(int id, string sentence)
  {
    this.ID = id;
    this.Sentence = sentence;
  }
}
