using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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

  public static int[] RandomThemeIDs(int categoryID)
  {
    List<int> numbers = new List<int>();

    switch (categoryID)
    {
      case 0: // 一般向け: 1~40, 121~180
        numbers = (Enumerable.Range(1, 40).Concat(Enumerable.Range(121, 60))).ToList();
        break;
      case 1: // エンジニア: 41~80
        numbers = Enumerable.Range(41, 40).ToList();
        break;
      case 2: // 18禁: 81~120
        numbers = Enumerable.Range(81, 40).ToList();
        break;
    }

    int count = 3;
    int[] ids = new int[count];

    UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    for (int i = 0; i < count; i++)
    {
      int index = UnityEngine.Random.Range(0, numbers.Count);
      int ransu = numbers[index];
      ids[i] = ransu;
      numbers.RemoveAt(index);
    }
    return ids;
  }
}
