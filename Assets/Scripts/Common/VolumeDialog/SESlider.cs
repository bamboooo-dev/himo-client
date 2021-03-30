using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
{
  private void Awake()
  {
    this.gameObject.GetComponent<Slider>().value = AudioManager.GetInstance().SEVolume;
  }
}
