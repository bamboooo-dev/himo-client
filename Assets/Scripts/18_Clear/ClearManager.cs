using UnityEngine;
using UnityEngine.UI;
using System;

public class ClearManager : MonoBehaviour
{
  [SerializeField] private Sprite[] sprites;
  [SerializeField] private GameObject goToVoteButton;

  void Start()
  {
    // DEBUG
    // Cycle.clearSpriteIndex = 1;

    GameObject.Find("Background").GetComponent<Image>().sprite = sprites[Cycle.clearSpriteIndex];
    Invoke(nameof(ShowGoToVoteButton), 3.0f);
  }

  void ShowGoToVoteButton()
  {
    goToVoteButton.SetActive(true);
  }
}
