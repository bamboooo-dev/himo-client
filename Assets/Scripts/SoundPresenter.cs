using UnityEngine;
using UnityEngine.UI;

public class SoundPresenter : MonoBehaviour
{
  [SerializeField] Slider bgmSlider;//BGMMenuViewのsliderを取得
  [SerializeField] Slider seSlider;//SEMenuViewのsliderを取得

  void Start() { }

  //BGMMenuViewのSliderを動かしたときに呼び出す関数を作成
  public void OnChangedBGMSlider()
  {
    //Sliderの値に応じてBGMを変更
    AudioManager.GetInstance().BGMVolume = bgmSlider.value;
  }

  //SEMenuViewのSliderを動かしたときに呼び出す関数を作成
  public void OnChangedSESlider()
  {
    //Sliderの値に応じてSEを変更
    AudioManager.GetInstance().SEVolume = seSlider.value;
  }

  //Buttonを押したときに呼ばれる関数
  public void OnPushButton()
  {
    //SEを再生
    AudioManager.GetInstance().PlaySound(0);
  }
}
