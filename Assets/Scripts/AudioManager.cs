using UnityEngine;

public class AudioManager : MonoBehaviour
{
  [SerializeField] AudioClip[] seList;//BGMを読み込む
  [SerializeField] AudioSource audioSourceBGM;//BGMの音の大きさを調節するために読み込む

  [SerializeField] AudioClip[] bgmList;//SEを読み込む
  [SerializeField] AudioSource audioSourceSE;//SEの音の大きさを調節するために読み込む

  //BGMのボリューム調節する関数を作成
  public float BGMVolume
  {
    //audioSourceBGMのvolumeをいじることでBGMを調整
    get { return audioSourceBGM.volume; }
    set { audioSourceBGM.volume = value; }
  }

  //SEのボリュームを調節する関数を作成
  public float SEVolume
  {
    //audioSourceSEのvolumeをいじることでSEを調整
    get { return audioSourceSE.volume; }
    set { audioSourceSE.volume = value; }
  }

  //SecneをまたいでもObjectが破壊されないようにする
  static AudioManager Instance = null;

  public static AudioManager GetInstance()
  {
    if (Instance == null)
    {
      Instance = FindObjectOfType<AudioManager>();
    }
    return Instance;
  }

  private void Awake()
  {
    if (this != GetInstance())
    {
      Destroy(this.gameObject);
      return;
    }
    DontDestroyOnLoad(this.gameObject);
  }

  //BGMを再生する関数を作成
  public void PlayBGM(int index)
  {
    audioSourceBGM.clip = bgmList[index];
    audioSourceBGM.Play();
  }

  //SEを再生する関数を作成
  public void PlaySound(int index)
  {
    audioSourceSE.PlayOneShot(seList[index]);
  }

}
