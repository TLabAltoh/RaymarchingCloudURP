using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightConeTest : MonoBehaviour {

    public int seed;
    public int numStepsLight = 8;
    public float lightConeRadius = 1;

    public Transform target;

    void Update () {

        //world spaceの原点からsphereに向かって伸びる線を描画
        Debug.DrawLine (Vector3.zero, target.position, Color.yellow);

        //原点からsphereへのベクトルを単位ベクトルに変換する
        Vector3 dir = target.position.normalized;

        //方向ベクトルとVector3.upの外積をとる
        Vector3 localX = Vector3.Cross (Vector3.up, dir).normalized;

        //localXと方向ベクトルとの外積をとる
        Vector3 localZ = Vector3.Cross (localX, dir).normalized;

        //計算した外積を描画
        Debug.DrawRay (Vector3.zero, localX, Color.red);
        Debug.DrawRay (Vector3.zero, localZ, Color.cyan);

        Vector2[] lightConeOffsets = new Vector2[numStepsLight];
        var prng = new System.Random (seed);
        for (int i = 0; i < numStepsLight; i++) {
            float p = i / (numStepsLight - 1f);

            //ランダムなVector2を生成(ここで、.NextDouble ()は0~1の値しか生成しないので、
            //値 * 2 -1 をして、全方位に対してランダムなVector2を生成する
            var offset = new Vector2 ((float) prng.NextDouble (), (float) prng.NextDouble ()) * 2 - Vector2.one;
            
            //pを掛けて配列の最初のVector2程、伸びの小さいVector2になるようにしている
            lightConeOffsets[i] = offset.normalized * p * lightConeRadius;

            //Vector2のそれぞれの成分をlocalXとlocalZに掛けることで、localXとlocalZに対して空間的に平行なベクトルを生成する
            Vector3 localPos = target.position * p + lightConeOffsets[i].x * localX + lightConeOffsets[i].y * localZ;
            
            //描画
            Debug.DrawLine(target.position * p, localPos, Color.yellow);
        }
    }
}