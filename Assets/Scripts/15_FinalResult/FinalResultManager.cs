using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FinalResultManager : MonoBehaviour
{
  [SerializeField] private Text firstPlaceText;
  [SerializeField] private Text secondPlaceText;
  [SerializeField] private Text thirdPlaceText;
  [SerializeField] private Text firstPointText;
  [SerializeField] private Text secondPointText;
  [SerializeField] private Text thirdPointText;
  [SerializeField] private Button finishButton;
  private Result[] results;

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
    thirdPlaceText.text = this.results[2].name;
    thirdPointText.text = this.results[2].point.ToString() + "pt";
  }

  private void ShowSecondPlace()
  {
    secondPlaceText.text = this.results[1].name;
    secondPointText.text = this.results[1].point.ToString() + "pt";
  }
  private void ShowFirstPlace()
  {
    firstPlaceText.text = this.results[0].name;
    firstPointText.text = this.results[0].point.ToString() + "pt";
    finishButton.gameObject.SetActive(true);
  }

  private void SortNames()
  {
    Result[] results = new Result[RoomStatus.points.Length];
    for (int i = 0; i < RoomStatus.points.Length; i++)
    {
      results[i] = new Result(RoomStatus.points[i], Cycle.names[i]);
    }
    this.results = results.OrderByDescending(x => x.point).ToArray();
  }
}
