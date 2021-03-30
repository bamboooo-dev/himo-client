using UnityEngine;
using UnityEngine.UI;

public class CopyIDButton : MonoBehaviour
{
  public Text idText;
  void Start() { }

  void Update() { }

  public void CopyID()
  {
    GUIUtility.systemCopyBuffer = idText.text;
  }
}
