using UnityEngine;
using UnityEngine.UI;

public class ExpectDialogContainer : MonoBehaviour
{
  void Start()
  {
    string[] colors = new string[] { "Green", "Blue", "Brown", "Purple", "Orange", "Ocher" };
    for (int i = 0; i < colors.Length; i++)
    {
      Transform row = gameObject.transform.Find("DialogBody").Find(colors[i]);
      if (i >= Cycle.names.Length)
      {
        row.gameObject.SetActive(false);
        continue;
      }
      row.Find("Name").GetComponent<Text>().text = Cycle.names[i];
      row.Find("Frame").Find("AnswerNumber").GetComponent<Text>().text = Cycle.numbers[i].ToString();
      int right = 1200 * (100 - Cycle.numbers[i]) / 100;
      row.Find("Frame").Find("Date").Find("RealDate").GetComponent<RectTransform>().offsetMax = new Vector2(-right, 0);
      for (int j = 0; j < colors.Length; j++)
      {
        if (j == i) continue;
        if (j >= Cycle.names.Length)
        {
          row.Find("Frame").Find("Date").Find(colors[j]).gameObject.SetActive(false);
          continue;
        }
        Vector2 pos = row.Find("Frame").Find("Date").Find(colors[j]).GetComponent<RectTransform>().anchoredPosition;
        pos.x = 1200 * Cycle.predicts[j][i] / 100;
        row.Find("Frame").Find("Date").Find(colors[j]).GetComponent<RectTransform>().anchoredPosition = pos;
      }
    }
  }

  public void HideDialog()
  {
    gameObject.SetActive(false);
  }
}
