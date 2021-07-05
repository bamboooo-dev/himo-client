using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinalResultManager : MonoBehaviour
{
  [SerializeField] private Text firstPlaceText;
  [SerializeField] private Text secondPlaceText;
  [SerializeField] private Text thirdPlaceText;
  [SerializeField] private Text lastPlaceText;
  [SerializeField] private Text firstPointText;
  [SerializeField] private Text secondPointText;
  [SerializeField] private Text thirdPointText;
  [SerializeField] private Text lastPointText;
  [SerializeField] private Button finishButton;
  [SerializeField] private Place placePrefab;
  [SerializeField] private GameObject placeParent;
  [SerializeField] private Sprite[] placeSprites;
  private Result[] results;

  void Start()
  {
    // DEBUG
    // RoomStatus.points = new int[] { 3, 2, 1, 4, 5, 6 };
    // Cycle.names = new string[] { "a", "b", "c", "jdk", "しゅんこりん", "あさしゅん" };

    SortNames();

    switch (this.results.Length)
    {
      case 1:
        StartCoroutine(ShowPlace(3.0f, 0, 1));
        break;
      case 2:
        StartCoroutine(ShowPlace(3.0f, 1, 2));
        StartCoroutine(ShowPlace(5.0f, 0, 2));
        break;
      case 3:
        StartCoroutine(ShowPlace(3.0f, 2, 3));
        StartCoroutine(ShowPlace(4.0f, 1, 3));
        StartCoroutine(ShowPlace(5.0f, 0, 3));
        break;
      case 4:
        StartCoroutine(ShowPlace(3.0f, 2, 4));
        StartCoroutine(ShowPlace(4.0f, 1, 4));
        StartCoroutine(ShowPlace(6.0f, 0, 4));
        StartCoroutine(ShowPlace(6.0f, 3, 4));
        break;
      case 5:
        StartCoroutine(ShowPlace(3.0f, 3, 5));
        StartCoroutine(ShowPlace(4.0f, 2, 5));
        StartCoroutine(ShowPlace(5.0f, 1, 5));
        StartCoroutine(ShowPlace(7.0f, 0, 5));
        StartCoroutine(ShowPlace(7.0f, 4, 5));
        break;
      case 6:
        StartCoroutine(ShowPlace(3.0f, 4, 6));
        StartCoroutine(ShowPlace(4.0f, 3, 6));
        StartCoroutine(ShowPlace(5.0f, 2, 6));
        StartCoroutine(ShowPlace(6.0f, 1, 6));
        StartCoroutine(ShowPlace(8.0f, 0, 6));
        StartCoroutine(ShowPlace(8.0f, 5, 6));
        break;
      default:
        break;
    }
  }

  private IEnumerator ShowPlace(float waitTime, int place, int maxPlace)
  {
    yield return new WaitForSeconds(waitTime);
    int y;
    y = 450 - (place + 1) * 900 / (maxPlace + 1);
    var _place = Instantiate(placePrefab, new Vector3(0, y, 0), Quaternion.identity);
    _place.transform.SetParent(placeParent.transform.transform, false);
    _place.transform.Find("Point").GetComponent<Text>().text = results[place].point.ToString() + "pt";
    _place.transform.Find("Name").GetComponent<Text>().text = results[place].name;
    if (place >= 3 && place == maxPlace - 1)
    {
      place = placeSprites.Length - 1;
    }
    _place.transform.Find("PlaceImage").GetComponent<Image>().sprite = placeSprites[place];
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
