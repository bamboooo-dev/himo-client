using UnityEngine;
using UnityEngine.UI;

public class CopyIDButton : MonoBehaviour
{
  public Text idText;
  public GameObject copyText;
  void Start() { }

  void Update() { }

  public void CopyID()
  {
    GUIUtility.systemCopyBuffer = idText.text;
    copyText.SetActive(true);
  }
}
