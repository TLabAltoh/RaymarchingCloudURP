using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visualisation;

//通常MonoBehaviorはPlayModeの時だけ実行されるが、[ExecuteInEditMode]によってスクリプトをEditModeで実行できる
[ExecuteInEditMode]
public class RayStepVis : MonoBehaviour {
    public Transform container;
    public Transform eye;

    public float dstToBox;
    public float dstInsideBox;
    

    [Space ()]
    public bool useUV;
    public Vector2 uv;
    public float speed = .1f;
    public float inc = .1f;

    public bool showSamplePoints;
    public int numSamples = 5;
    public Color sampleCol;

    /// <summary>
    /// これがraymarchingの真髄(かもしれない)
    /// </summary>
    void Update () {
        if (useUV && Application.isPlaying) {

            /*uv.x += Time.deltaTime * speed;
            if (uv.x > 1) {
                uv.x = 0;
                uv.y += inc;
                if (uv.y > 1) {
                    uv.y = 1;
                    speed = 0;
                }
            }*/

            uv = new Vector2(0, 0);

            //画面のuvの向きにEyeのtransform.forwardを更新する
            transform.forward = RayDir (uv);
        }
        var col = Color.red;
        RayBox (container.position, container.localScale, eye.position, eye.forward);
        if (dstInsideBox == 0) {
            col = Color.grey;
            dstToBox = 999;
        }

        //通過点(2点)の手前側
        Debug.DrawRay (eye.position, eye.forward * dstToBox, col);
        //通過点(2点)の奥側
        Debug.DrawRay (eye.position + eye.forward * dstToBox, eye.forward * dstInsideBox, Color.white);
        if (dstInsideBox == 0) {
            dstToBox = 0;
        } else {
            //通過点(2点)の手前側
            Vis.DrawSphere (eye.position + eye.forward * dstToBox, .06f, col, Style.Unlit);
            //通過点(2点)の奥側
            Vis.DrawSphere (eye.position + eye.forward * (dstToBox + dstInsideBox), .06f, Color.white, Style.Unlit);
            if (showSamplePoints) {
                float step = dstInsideBox/numSamples;
                for (int i = 0; i < numSamples; i++)
                {
                    Vis.DrawSphere (eye.position + eye.forward * (dstToBox + step * i), .06f, sampleCol, Style.Unlit);
                }
            }
        }
    }

    Vector3 RayDir (Vector2 uv) {
        Camera cam = Camera.main;

        Matrix4x4 inv = cam.projectionMatrix.inverse;
        Matrix4x4 camToWorld = cam.cameraToWorldMatrix;
        //証明できたかもしれない
        Vector3 viewVector = inv * new Vector4 (uv.x * 2 - 1, uv.y * 2 - 1, 0, 1);

        /*Debug用のブロック文(この中で定義された変数はブロックの外では使うことが出来ない)
          使い方によってはコードの可読性が上がる
        {
            Debug.Log("uv = " + uv);
            Debug.Log("inv = " + inv);
            Debug.Log("projection = " + cam.projectionMatrix);
            Debug.Log("camToWorld = " + camToWorld);
            Debug.Log("worldToCameraMatrix =" + cam.worldToCameraMatrix);
            //Debug時に小数点切り捨てをさせず、5桁目までは表示する。
            Debug.Log("viewVector =" + viewVector.ToString("G5"));
            Debug.Log("camToWorld * viewVector = " + camToWorld * viewVector);
            Debug.Log("RayDir = " + (camToWorld * viewVector).normalized);
            Debug.Log("Camera.main.nearClipPlane =" + Camera.main.nearClipPlane);
            Debug.Log("Camera.main.farClipPlane =" + Camera.main.farClipPlane);
            Debug.Log((Camera.main.nearClipPlane + Camera.main.farClipPlane) /
                     (Camera.main.nearClipPlane - Camera.main.farClipPlane));
        }*/

        return (camToWorld * viewVector).normalized;
        //float3 viewVector = mul (unity_CameraInvProjection, float4 (v.uv * 2 - 1, 0, -1));
        //output.viewVector = mul (unity_CameraToWorld, float4 (viewVector, 0));
    }

    // Calculates dstToBox, dstInsideBox
    // If ray misses box, dstInsideBox will be zero, and dstToBox may have any value
    void RayBox (Vector3 centre, Vector3 size, Vector3 rayOrigin, Vector3 rayDir) {

        //boundsの範囲最大頂点と範囲最小頂点を定義(ray marching するオブジェクト(とそのtransform)を仮想的に定義する)
        Vector3 boundsMin = centre - size / 2;
        Vector3 boundsMax = centre + size / 2;

        //float.MaxValue = 340282346638528859811704183484516925440
        Vector3 invRayDirection = new Vector3 ((rayDir.x == 0) ? float.MaxValue : 1 / rayDir.x, (rayDir.y == 0) ? float.MaxValue : 1 / rayDir.y, (rayDir.z == 0) ? float.MaxValue : 1 / rayDir.z);

        //Vector3.Scale : ベクトルの各成分を乗算する
        //この式の仕組みがなんとなく理解できたので、ノートに残しておいた。
        Vector3 t0 = Vector3.Scale (boundsMin - rayOrigin, invRayDirection);
        Vector3 t1 = Vector3.Scale (boundsMax - rayOrigin, invRayDirection);
        Vector3 tmin = Vector3.Min (t0, t1);
        Vector3 tmax = Vector3.Max (t0, t1);

        float a = Mathf.Max (tmin.x, tmin.y, tmin.z);
        float b = Mathf.Min (tmax.x, tmax.y, tmax.z);

        // CASE 1: ray intersects box from outside (0 <= a <= b)
        // a is dst to nearest intersection, b dst to far intersection

        // CASE 2: ray intersects box from inside (a < 0 < b)
        // a is dst to intersection behind the ray, b is dst to forward intersection

        // CASE 3: ray misses box (a > b)

        dstToBox = Mathf.Max (0, a);
        dstInsideBox = Mathf.Max (0, b - dstToBox);

        //Debug用のブロック文
        /*
        {
            Debug.Log("boundsMin - rayOrigin =" +vecM);
            Debug.Log("rayDir =" + rayDir);
            Debug.Log("invRayDirection =" + invRayDirection);
            Debug.Log("t1 =" + t1);
            Debug.Log("t0 =" + t0);
            Debug.Log("dstToBox =" + dstToBox);
            Debug.Log("dstInsideBox =" + dstInsideBox);
            Debug.Log("size =" + size);
        }*/
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube (container.position, container.localScale);
    }
}