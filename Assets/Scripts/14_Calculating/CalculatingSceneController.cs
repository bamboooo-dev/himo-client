using UnityEngine;
using UnityEngine.SceneManagement;

public class CalculatingSceneController : MonoBehaviour
{
  private float step_time;
  // Start is called before the first frame update
  void Start()
  {
    step_time = 0.0f;
  }

  // Update is called once per frame
  void Update()
  {
    // 経過時間をカウント
    step_time += Time.deltaTime;

    // 1秒後に画面遷移( Title へ移動)
    if (step_time >= 5.0f)
    {
      SceneManager.LoadScene("FinalResult");
    }

  }
}
