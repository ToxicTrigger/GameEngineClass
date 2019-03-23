using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Skinned : MonoBehaviour
{
    SkinnedMeshRenderer smr;
    public Transform[] bones;

    public Mesh MakeTriangle(Vector3[] pos, int BoneNumber)
    {
        Mesh m = new Mesh();
        m.vertices = pos;
        m.bindposes = new Matrix4x4[]
        {
            bones[BoneNumber].worldToLocalMatrix * transform.localToWorldMatrix,
        };
        m.boneWeights = new BoneWeight[]
        {
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
        };
        m.triangles = new int[]
        {
            0,1,2
        };
        return m;
    }

    public Mesh MakeQuad(Vector3 pos, Vector2 size, Vector3 angle, int BoneNumber, Vector4 uv, Vector2 textureSize)
    {
        Mesh m = new Mesh();
        var r = new Vector3[]
        {
            new Vector3(pos.x - size.x/2, pos.y - size.y/2, pos.z),
            new Vector3(pos.x - size.x/2, pos.y + size.y/2, pos.z ),
            new Vector3(pos.x + size.x/2, pos.y + size.y/2, pos.z ),
            new Vector3(pos.x - size.x/2, pos.y - size.y/2, pos.z ),
            new Vector3(pos.x + size.x/2, pos.y + size.y/2, pos.z ),
            new Vector3(pos.x + size.x/2, pos.y - size.y/2, pos.z ),
        };

        var v = new List<Vector3>();

        var mat = Matrix4x4.TRS(pos, Quaternion.Euler(angle), size);
        for(int i = 0; i < r.Length ; ++i)
        {
            v.Add(mat.MultiplyPoint3x4(r[i]));
        }

        m.vertices = v.ToArray();
        m.bindposes = new Matrix4x4[]
        {
            bones[BoneNumber].worldToLocalMatrix * transform.localToWorldMatrix,
        };
        m.boneWeights = new BoneWeight[]
        {
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
            new BoneWeight(){boneIndex0 = BoneNumber, weight0 = 1},
        };
        m.uv = new Vector2[] 
        {
            new Vector2(uv.x * (1/textureSize.x), uv.y * (1/textureSize.y)),
            new Vector2((uv.x) * (1/textureSize.x), (uv.y + uv.w) * (1/textureSize.y)),
            new Vector2((uv.x + uv.z) * (1/textureSize.x), (uv.y + uv.w) * (1/textureSize.y)),
            new Vector2((uv.x) * (1/textureSize.x), (uv.y) * (1/textureSize.y)),
            new Vector2((uv.x + uv.z) * (1/textureSize.x), (uv.y + uv.w) * (1/textureSize.y)),
            new Vector2((uv.x + uv.z) * (1/textureSize.x), (uv.y) * (1/textureSize.y)),
        };

        m.triangles = new int[]
        {
            0,1,2,
            3,4,5
        };
        return m;
    }

    public Mesh MergeMesh(Mesh[] targets, int[] BoneMatch)
    {
        Mesh origin = new Mesh();
        int verticesCount = 0;
        int indeices = 0;
        var vs = new List<Vector3>();
        var index = new List<int>();
        var bonePoses = new List<Matrix4x4>();
        var boneWeights = new List<BoneWeight>();
        var uvs = new List<Vector2>();

        foreach (var item in targets)
        {
            int poseCount = 0;
            foreach (var i in item.vertices)
            {
                vs.Add(i);
                index.Add(indeices);
                boneWeights.Add(item.boneWeights[poseCount]);
                uvs.Add(item.uv[poseCount]);
                verticesCount += 1;
                indeices += 1;
                poseCount += 1;

            }
            bonePoses.Add(item.bindposes[0]);
        }

        origin.vertices = vs.ToArray();
        origin.triangles = index.ToArray();
        origin.boneWeights = boneWeights.ToArray();
        origin.bindposes = bonePoses.ToArray();
        origin.uv = uvs.ToArray();
        return origin;
    }

    public Mesh MakeBox(Vector3 pos, Vector3 size, Vector3 angle, int BoneNumber, Vector4[] uv, Vector2 textureSize)
    {
        var box = new List<Mesh>();

        box.Add(MakeQuad(pos + new Vector3(0,0,size.z/2), size, new Vector3(angle.x, angle.y + 180 , angle.z), BoneNumber, uv[0],textureSize));
        box.Add(MakeQuad(pos + new Vector3(0,0,-(size.z/2)), size, angle, BoneNumber, uv[1],textureSize));
        
        box.Add(MakeQuad(pos + new Vector3(-(size.x / 2),0,-(size.z/2)), size, new Vector3(angle.x, angle.y+90, angle.z), BoneNumber, uv[2],textureSize));
        box.Add(MakeQuad(pos + new Vector3(size.x / 2,0,-(size.z/2)), size, new Vector3(angle.x, angle.y+270 , angle.z), BoneNumber, uv[3],textureSize));
        
        box.Add(MakeQuad(pos + new Vector3(0,(size.y) + size.y/2, -(size.z * 2) - (size.z/2)), size, new Vector3(angle.x + 90, angle.y, angle.z), BoneNumber, uv[4],textureSize));
        box.Add(MakeQuad(pos + new Vector3(0,(size.y/2),size.z + size.z/2), size, new Vector3(angle.x +270, angle.y, angle.z), BoneNumber, uv[5],textureSize));
        
        return MergeMesh(box.ToArray(), new int[] {0});
    }

    void Awake()
    {
        smr = GetComponent<SkinnedMeshRenderer>();
        smr.quality = SkinQuality.Bone1;
        Mesh steve = MergeMesh(new Mesh[]{
            MakeBox(Vector3.up, Vector3.one * 1, Vector3.zero, 1, 
                new Vector4[]  
                { 
                    new Vector4(8, 48, 8 , 8),
                    new Vector4(24, 48, 8 , 8),
                    new Vector4(16, 48, 8 , 8),
                    new Vector4(0, 48, 8 , 8),
                    new Vector4(8, 56, 8 , 8),
                    new Vector4(16, 56, 8 , 8),
                },
                new Vector2(64, 64)),
            MakeBox(Vector3.zero, new Vector3(1,1.5f, 0.5f), Vector3.zero, 0, 
                new Vector4[]  
                { 
                    new Vector4(8, 48, 8 , 8),
                    new Vector4(24, 48, 8 , 8),
                    new Vector4(16, 48, 8 , 8),
                    new Vector4(0, 48, 8 , 8),
                    new Vector4(8, 56, 8 , 8),
                    new Vector4(16, 56, 8 , 8),
                },
                new Vector2(64, 64)),
            
        },
        new int[] { 0 , 1,2}
        );

        smr.bones = this.bones;
        smr.sharedMesh = steve;
    }
}
