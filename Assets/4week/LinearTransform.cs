using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Matrix2x2
{
    public float m00;
    public float m01;
    public float m10;
    public float m11;

    public static Vector2 operator *(Matrix2x2 lhs, Vector2 vec)
    {
        return
        new Vector2
        ( 
            lhs.m00 * vec.x + lhs.m01 * vec.y, 
            lhs.m10 * vec.x + lhs.m11 * vec.y 
        );
    }

    //학문적으로는 레디안인데 1레디안..
    //우리는 디그리에 더 익숙함 0~360 도
    public static Vector2 rotation(Vector2 v, float angle)
    {
        var deg = Mathf.Deg2Rad * angle;
        var r = new Matrix2x2(
            new Vector2(Mathf.Cos(deg), Mathf.Sin(deg)), 
            new Vector2(-Mathf.Sin(deg), Mathf.Cos(deg)));
        return r * v;
    }

    public Matrix2x2(Vector2 column0, Vector2 column1)
    {
        m00 = column0.x;
        m10 = column0.y;
        m01 = column1.x;
        m11 = column1.y;
    }
}

[ExecuteAlways]
public class LinearTransform : MonoBehaviour
{
    public Matrix2x2 m = new Matrix2x2(new Vector2(1,0), new Vector2(0,1));
    public float angle;

    void DrawVector(Vector2 v)
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(Vector3.zero, v);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(v, Vector3.one * 0.1f);
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < 10; ++x)
        {
            for (int y = 0; y < 10; ++y)
            {
                Vector2 v = new Vector2(x, y);
                v = m * v;
                DrawVector( Matrix2x2.rotation(v, angle) );
            }
        }
    }
}
