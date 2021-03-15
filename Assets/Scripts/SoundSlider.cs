using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Sliderを使用するために必要

[RequireComponent(typeof(Slider))]
public class SoundSlider : MonoBehaviour
{

    Slider m_Slider;//音量調整用スライダー

    [SerializeField]
    bool m_isInput;//キー入力で調整バーを動かせるようにするか
    [SerializeField]
    float m_ScroolSpeed = 1;//キー入力で調整バーを動かすスピード
    void Awake()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.value = AudioListener.volume;
    }

    private void OnEnable()
    {
        m_Slider.value = AudioListener.volume;
        //スライダーの値が変更されたら音量も変更する
        m_Slider.onValueChanged.AddListener((sliderValue) => AudioListener.volume = sliderValue);
    }

    private void OnDisable()
    {
        m_Slider.onValueChanged.RemoveAllListeners();
    }
    //キー入力による操作　いらないなら削除してもOK
    void Update()
    {
        float v = m_Slider.value;
        if (m_isInput)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                v -= m_ScroolSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
            {
                v += m_ScroolSpeed * Time.deltaTime;
            }
        }
        v = Mathf.Clamp(v, 0, 1);
        m_Slider.value = v;
    }


}
