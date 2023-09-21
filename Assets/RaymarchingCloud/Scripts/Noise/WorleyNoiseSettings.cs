using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorleyNoiseSettings : ScriptableObject {

    // 乱数生成のseed値
    public int seed;

    // 立体空間の軸ごとに設定する平均化ポイントの数
    [Range (1, 100)]
    public int numDivisionsA = 5;
    [Range (1, 100)]
    public int numDivisionsB = 10;
    [Range (1, 100)]
    public int numDivisionsC = 15;


    public float persistence = .5f;
    public int tile = 1;
    public bool invert = true;

}