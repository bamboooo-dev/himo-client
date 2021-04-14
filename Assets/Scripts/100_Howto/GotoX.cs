using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoX : MonoBehaviour
{

   

    public void OnClickHowTo()
    {
        AudioManager.GetInstance().PlaySound(0);
        SceneManager.LoadScene("HowtoX");
    }

}
