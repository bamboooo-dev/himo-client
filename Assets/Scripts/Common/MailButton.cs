using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Google.Play.Review;
using UnityEngine.iOS;

public class MailButton : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
  private ReviewManager _reviewManager;
  private PlayReviewInfo _playReviewInfo;
#endif

  void Start()
  {
#if UNITY_ANDROID && !UNITY_EDITOR
    _reviewManager = new ReviewManager();
#else
    Debug.Log("Start");
    StartCoroutine(RequestPlayReviewInfoObject());
#endif
  }

  public void OnClickMailButton()
  {
#if UNITY_ANDROID && !UNITY_EDITOR
    StartCoroutine(StartInAppReviewFlow());
#else
    Device.RequestStoreReview();
#endif
  }

  private IEnumerator RequestPlayReviewInfoObject()
  {
#if UNITY_ANDROID && !UNITY_EDITOR
    var requestFlowOperation = _reviewManager.RequestReviewFlow();
    yield return requestFlowOperation;
    if (requestFlowOperation.Error != ReviewErrorCode.NoError)
    {
      Debug.Log(requestFlowOperation.Error.ToString());
      yield break;
    }
    _playReviewInfo = requestFlowOperation.GetResult();
#else
    Debug.Log("Request PlayReviewInfo.");
    yield break;
#endif
  }

  private IEnumerator StartInAppReviewFlow()
  {
#if UNITY_ANDROID && !UNITY_EDITOR
    var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
    yield return launchFlowOperation;
    _playReviewInfo = null; // Reset the object
    if (launchFlowOperation.Error != ReviewErrorCode.NoError)
    {
      Debug.Log(requestFlowOperation.Error.ToString());
      yield break;
    }
    // The flow has finished. The API does not indicate whether the user
    // reviewed or not, or even whether the review dialog was shown. Thus, no
    // matter the result, we continue our app flow.
#else
    Debug.Log("Start InAppReviewFlow.");
    yield break;
#endif

  }
}
