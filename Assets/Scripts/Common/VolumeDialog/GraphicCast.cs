using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GraphicCast : Graphic {

  //頂点を生成する必要があるときのコールバック関数
  protected override void OnPopulateMesh(VertexHelper vh){
    base.OnPopulateMesh (vh);
    vh.Clear ();//頂点を全てクリアし、何も表示されないように
  }
}
