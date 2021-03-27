using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class FinalResultManager : MonoBehaviour
{
  [SerializeField] private Text firstPlaceText;
  [SerializeField] private Text secondPlaceText;
  [SerializeField] private Text thirdPlaceText;
  [SerializeField] private Button finishButton;
  private string[] sortedNames;

  void Start()
  {
    // DEBUG
    // RoomStatus.points = new int[] { 3, 2, 1, 4, 5 };
    // Cycle.names = new string[] { "a", "b", "c", "d", "e" };

    SortNames();
    Invoke(nameof(ShowThirdPlace), 1.0f);
    Invoke(nameof(ShowSecondPlace), 2.0f);
    Invoke(nameof(ShowFirstPlace), 3.0f);
  }

  private void ShowThirdPlace()
  {
    thirdPlaceText.text = this.sortedNames[2];
  }

  private void ShowSecondPlace()
  {
    secondPlaceText.text = this.sortedNames[1];
  }
  private void ShowFirstPlace()
  {
    firstPlaceText.text = this.sortedNames[0];
    finishButton.gameObject.SetActive(true);
  }

  private void SortNames()
  {
    string[] names = new string[RoomStatus.points.Length];
    int[] sortedPoints = RoomStatus.points.OrderByDescending(x => x).ToArray();
    for (int i = 0; i < RoomStatus.points.Length; i++)
    {
      names[Array.IndexOf(sortedPoints, RoomStatus.points[i])] = Cycle.names[i];
    }
    this.sortedNames = names;
  }
}
