using UnityEngine;
using UnityEngine.UI;

public class GameFieldManager : MonoBehaviour
{
  public Text themeText;
  public Player player;
  [SerializeField] private GameObject playerParent;
  [SerializeField] private Sprite[] sprites;

  void Start()
  {
    themeText.text = RoomStatus.themes[RoomStatus.cycleIndex].Sentence;
    // Cycle.numbers = new int[] { 1, 2, 3, 4, 5 };
    // Cycle.names = new string[] { "kohei", "ashashun", "mari", "monaka", "bayashi" };
    // Cycle.myIndex = 3;
    InstantiatePlayers(Cycle.numbers, Cycle.names, Cycle.myIndex);
  }

  private void InstantiatePlayers(int[] numbers, string[] names, int myIndex)
  {
    string[] colors = new string[] { "#018D50", "#0178C2", "#FD8016", "#C4453F", "#714A9B", "#C031A5" };
    int count = names.Length;
    for (int i = 0; i < count; ++i)
    {
      int x = -750 + i * 1500 / (count - 1);
      var _player = Instantiate(player, new Vector3(x, 0, 0), Quaternion.identity);
      _player.transform.SetParent(playerParent.transform.transform, false);
      _player.transform.Find("Name").GetComponent<Text>().text = names[i];
      Color newCol;
      ColorUtility.TryParseHtmlString(colors[i], out newCol);
      _player.transform.Find("Name").GetComponent<Text>().color = newCol;
      _player.transform.Find("InputField").Find("Text").GetComponent<Text>().color = newCol;
      newCol.a = 0.25f;
      _player.transform.Find("InputField").Find("Placeholder").GetComponent<Text>().color = newCol;
      _player.transform.Find("EmoImage").GetComponent<Image>().sprite = sprites[i];
      if (i == myIndex)
      {
        _player.transform.Find("InputField").GetComponent<InputField>().text = numbers[myIndex].ToString();
        _player.transform.Find("InputField").GetComponent<InputField>().readOnly = true;
      }
    }
  }


}
