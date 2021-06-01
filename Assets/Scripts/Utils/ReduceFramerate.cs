using UnityEngine;
public class ReduceFramerate : MonoBehaviour
{	void Start()
	{

		QualitySettings.vSyncCount = 0;

		Application.targetFrameRate = 15;
	}
}