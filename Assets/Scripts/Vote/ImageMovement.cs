using UnityEngine;
using UnityEngine.EventSystems;

public class ImageMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
  public Transform imageParent;

  public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
  {
    imageParent = transform.parent;
    transform.SetParent(imageParent.parent, false);
    GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
  }

  public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
  {
    transform.position = eventData.position;
  }

  public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
  {
    transform.SetParent(imageParent, false);
    GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
  }
}
