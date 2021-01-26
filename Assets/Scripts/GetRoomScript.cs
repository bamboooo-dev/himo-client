using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRoomScript : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  // Ran when button is clicked
  public void RunGetRoom(Text text)
  {
    ulong id = 1;
    var response = GetRoomTest.GetRoom(id);
    text.text = "Table: " + response.Table.Name;
  }
}
