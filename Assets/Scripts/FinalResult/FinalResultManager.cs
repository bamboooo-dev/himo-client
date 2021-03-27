using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class FinalResultManager : MonoBehaviour
{
  [SerializeField] private Text firstPlaceText;
  [SerializeField] private Text secondPlaceText;
  [SerializeField] private Text thirdPlaceText;
  private string[] sortedNames;

  void Start()
  {
    Invoke(nameof(ShowThirdPlace), 1.0f);
    Invoke(nameof(ShowSecondPlace), 1.0f);
    Invoke(nameof(ShowFirstPlace), 1.0f);
  }

  private void ShowThirdPlace()
  {
    thirdPlaceText.text = this.sortedNames[2];
  }

  private void ShowSecondPlace()
  {
    thirdPlaceText.text = this.sortedNames[1];
  }
  private void ShowFirstPlace()
  {
    thirdPlaceText.text = this.sortedNames[0];
  }

  private void SortNames()
  {
    string[] names = new string[RoomStatus.points.Length];
    int[] sortedPoints = RoomStatus.points.OrderByDescending(x => x).ToArray();
    for (int i = 0; i < RoomStatus.points.Length; i++)
    {
      names[i] = Cycle.names[Array.IndexOf(sortedPoints, RoomStatus.points[i])];
    }
    this.sortedNames = names;
  }
}
