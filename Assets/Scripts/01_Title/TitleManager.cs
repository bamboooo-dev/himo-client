using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public class TitleManager : MonoBehaviour
{
  [SerializeField] private GameObject dialog = default;
  private VersionResponse response;

  async void Start()
  {
    Debug.Log("Title scene started.");
    VersionResponse response = await GetVersionAsync();
    bool isLatest = CheckVersion(response.latestVersion);
    if (!isLatest) ShowDialog();
  }

  private void ShowDialog()
  {
    dialog.SetActive(true);
  }

  private bool CheckVersion(string latestVersion)
  {
    string thisVersion = GameObject.Find("VersionText").GetComponent<Text>().text.Substring(1);
    return thisVersion == latestVersion;
  }

  private async UniTask<VersionResponse> GetVersionAsync()
  {
    UnityWebRequest request = UnityWebRequest.Get(Url.Version());
    request.SetRequestHeader("Accept", "text/json");
    await request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
    else
    {
      response = JsonUtility.FromJson<VersionResponse>(request.downloadHandler.text);
      return response;
    }
  }
}
