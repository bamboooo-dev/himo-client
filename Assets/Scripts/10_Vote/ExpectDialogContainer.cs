using UnityEngine;
using UnityEngine.UI;

public class ExpectDialogContainer : MonoBehaviour
{
  void Start()
  {
    string[] colors = new string[] { "Green", "Blue", "Brown", "Purple", "Orange", "Ocher" };
    for (int i = 0; i < Cycle.names.Length; i++)
    {
      Transform row = gameObject.transform.Find("DialogBody").Find(colors[i]);
      row.Find("Name").GetComponent<Text>().text = Cycle.names[i];
      row.Find("Frame").Find("AnswerNumber").GetComponent<Text>().text = Cycle.numbers[i].ToString();
      int right = 1200 * (100 - Cycle.numbers[i]) / 100;
      row.Find("Frame").Find("Date").Find("RealDate").GetComponent<RectTransform>().offsetMax = new Vector2(-right, 0);
      for (int j = 0; j < Cycle.names.Length; j++)
      {
        if (j == i) continue;
        Vector2 pos = row.Find("Frame").Find("Date").Find(colors[j]).GetComponent<RectTransform>().anchoredPosition;
        pos.x = 1200 * Cycle.predicts[i][j] / 100;
        row.Find("Frame").Find("Date").Find(colors[j]).GetComponent<RectTransform>().anchoredPosition = pos;
      }
    }
  }

  public void HideDialog()
  {
    gameObject.SetActive(false);
  }
}
