using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public class TitleManager : MonoBehaviour
{
  [SerializeField] private GameObject versionDialog = default;
  [SerializeField] private GameObject maintenanceDialog = default;

  async void Start()
  {
    StatusResponse response = await GetStatusAsync();

    checkMaintenance(response);

    checkVersion(response);
  }

  private void checkVersion(StatusResponse response)
  {
    string thisVersion = GameObject.Find("VersionText").GetComponent<Text>().text.Substring(1);
    if (!response.maintenanceStatus && thisVersion.CompareTo(response.latestVersion) == -1)
    {
      versionDialog.SetActive(true);
    }
  }

  private void checkMaintenance(StatusResponse response)
  {
    if (response.maintenanceStatus)
    {
      maintenanceDialog.SetActive(true);
      maintenanceDialog.transform.Find("TimeText").GetComponent<Text>().text = response.maintenanceFinishTime;
    }
  }

  private async UniTask<StatusResponse> GetStatusAsync()
  {
    UnityWebRequest request = UnityWebRequest.Get(Url.Status());
    request.SetRequestHeader("Accept", "text/json");
    await request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
    else
    {
      return JsonUtility.FromJson<StatusResponse>(request.downloadHandler.text);
    }
  }
}
