using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObjectを作成する(editer画面のprojectから右クリック→createでこれにアクセスできるようになる
[CreateAssetMenu]
public class SimplexNoiseSettings : NoiseSettings {

    public int seed;
    [Range(1,6)]
    public int numLayers = 1;
    public float scale = 1;
    public float lacunarity = 2;
    public float persistence = .5f;
    public Vector2 offset;

    //override : 継承した親クラス(スーパークラス)の処理を書き直して新しい子クラス(サブクラス)を作る
    //
    public override System.Array GetDataArray () {
        //構造体の値を決定
        var data = new DataStruct () {
            seed = seed,
            numLayers = Mathf.Max (1, numLayers),
            scale = scale,
            lacunarity = lacunarity,
            persistence = persistence,
            offset = offset
        };

        return new DataStruct[] { data };
    }

    //構造体を宣言
    public struct DataStruct {
        public int seed;
        public int numLayers;
        public float scale;
        public float lacunarity;
        public float persistence;
        public Vector2 offset;
    }

    public override int Stride {
        get {
            return sizeof (float) * 7;
        }
    }
}