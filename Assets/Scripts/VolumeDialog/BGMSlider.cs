using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
  private void Awake()
  {
    this.gameObject.GetComponent<Slider>().value = AudioManager.GetInstance().BGMVolume;
  }
}
