using UnityEngine;
using UnityEngine.UI;

public class GameFieldManager : MonoBehaviour
{
  public Text themeText;
  void Start()
  {
    themeText.text = RoomStatus.themes[RoomStatus.cycleIndex].Sentence;
  }
}
