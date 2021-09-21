using UnityEngine;

public class ReactionPrefab : MonoBehaviour
{
  float seconds;
  float startX;
  float startTime;

  void Start()
  {
    startX = this.transform.position.x;
    startTime = Time.time;
  }

  void Update()
  {
    Transform myTransform = this.transform;
    Vector3 pos = myTransform.position;
    pos.y += 1f;
    pos.x = startX + Mathf.PingPong((Time.time - startTime) * 50, 50f);
    myTransform.position = pos;
    seconds += Time.deltaTime;
    if (seconds >= 5)
    {
      Destroy(this.gameObject);
    }
  }
}
