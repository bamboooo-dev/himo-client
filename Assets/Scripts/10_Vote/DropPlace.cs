using UnityEngine;
using UnityEngine.EventSystems;

// フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour, IDropHandler
{
  public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
  {
    ImageMovement card = eventData.pointerDrag.GetComponent<ImageMovement>(); // ドラッグしてきた情報からCardMovementを取得
    if (card != null) // もしカードがあれば、
    {
      card.transform.SetParent(this.transform); // カードの親要素を自分（アタッチされてるオブジェクト）にする
    }
  }
}
