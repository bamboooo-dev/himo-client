using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPresenter : MonoBehaviour
{
    //BGM
    [SerializeField] Text bgmVolumeText;//BGMMenuViewのvolumeTextを取得
    [SerializeField] Slider bgmSlider;//BGMMenuViewのsliderを取得

    //SE
    [SerializeField] Text seVolumeText;//SEMenuViewのvolumeTextを取得
    [SerializeField] Slider seSlider;//SEMenuViewのsliderを取得

    void Start()
    {

    }

    //BGMMenuViewのSliderを動かしたときに呼び出す関数を作成
    public void OnChangedBGMSlider()
    {
        //volumeTextの値をSliderのvalueに変更
        bgmVolumeText.text = string.Format("{0:0}", bgmSlider.value * 100);
    }

    //SEMenuViewのSliderを動かしたときに呼び出す関数を作成
    public void OnChangedSESlider()
    {
        //volumeTextの値をSliderのvalueに変更
        seVolumeText.text = string.Format("{0:0}", seSlider.value * 100);
    }

    //Buttonを押したときに呼ばれる関数
    public void OnPushButton()
    {
        Debug.Log("Buttonを押しました");
    }
}
