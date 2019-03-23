using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecraftChar : MonoBehaviour
{
    public Material material;

    public Mesh make_quad(Vector3 pos, Vector3 size, Vector3 angle, Vector2 uv_low_left, Vector2 uv_size, Vector2 texture_size)
    {
        Mesh m = new Mesh();
        var ver = new Vector3[4]
        {
            new Vector3(0 - size.x/2, 0 - size.y/2, 0),
            new Vector3(0 - size.x/2, 0 + size.y/2, 0),
            new Vector3(0 + size.x/2, 0 + size.y/2, 0),
            new Vector3(0 + size.x/2, 0 - size.y/2, 0)
        };

        Matrix4x4 mat = Matrix4x4.TRS(pos, Quaternion.Euler(angle), size);
        for (int i = 0; i < ver.Length; ++i)
        {
            ver[i] = mat.MultiplyPoint3x4(ver[i]);
        }

        m.vertices = ver;

        m.triangles = new int[6]
        {
            0,1,2,
            2,3,0
        };

        m.uv = new Vector2[4]
        {
            new Vector2(uv_low_left.x * (1/texture_size.x), uv_low_left.y * (1/texture_size.y)),
            new Vector2(uv_low_left.x * (1/texture_size.x), (uv_low_left.y + uv_size.y) * (1/texture_size.y)),
            new Vector2((uv_low_left.x + uv_size.x ) * (1/texture_size.x), (uv_low_left.y + uv_size.y )* (1/texture_size.y)),
            new Vector2((uv_low_left.x + uv_size.x) * (1/texture_size.x) , uv_low_left.y * (1/texture_size.y)),
        };


        return m;
    }

    public void make_box(Vector3 pos, Vector3 size, Vector3 angle, Vector2[] uv_lows, Vector2[] uv_sizes, Vector2 texture_size)
    {
		var tmp = new GameObject();
        GameObject f = Instantiate(tmp, Vector3.zero, Quaternion.identity, tmp.transform);
        f.AddComponent(typeof(MeshFilter));
        f.AddComponent(typeof(MeshRenderer));
        f.GetComponent<MeshRenderer>().material = material;
		Vector3 tmp_pos = pos;
		tmp_pos.z += size.z/2;
        f.GetComponent<MeshFilter>().mesh = make_quad(tmp_pos, size, new Vector3(0,180,0) + angle, uv_lows[0], uv_sizes[0], texture_size);

        GameObject b = Instantiate(tmp, Vector3.zero, Quaternion.identity, tmp.transform);
        b.AddComponent(typeof(MeshFilter));
        b.AddComponent(typeof(MeshRenderer));
        b.GetComponent<MeshRenderer>().material = material;
		tmp_pos = pos;
		tmp_pos.z -= size.z/2;
        b.GetComponent<MeshFilter>().mesh = make_quad(tmp_pos, size ,new Vector3(0,360,0) + angle, uv_lows[1], uv_sizes[1], texture_size);

        GameObject l = Instantiate(tmp, Vector3.zero, Quaternion.identity, tmp.transform);
        l.AddComponent(typeof(MeshFilter));
        l.AddComponent(typeof(MeshRenderer));
        l.GetComponent<MeshRenderer>().material = material;
		tmp_pos = pos;
		tmp_pos.x -= size.x/2;
        l.GetComponent<MeshFilter>().mesh = make_quad(tmp_pos, size, (Vector3.up * 90) + angle, uv_lows[2], uv_sizes[2], texture_size);

        GameObject r = Instantiate(tmp, Vector3.zero, Quaternion.identity, tmp.transform);
        r.AddComponent(typeof(MeshFilter));
        r.AddComponent(typeof(MeshRenderer));
        r.GetComponent<MeshRenderer>().material = material;
				tmp_pos = pos;
		tmp_pos.x += size.x/2;
        r.GetComponent<MeshFilter>().mesh = make_quad(tmp_pos, size, (Vector3.up * 270) + angle, uv_lows[3], uv_sizes[3], texture_size);

        GameObject t = Instantiate(tmp, Vector3.zero, Quaternion.identity, tmp.transform);
        t.AddComponent(typeof(MeshFilter));
        t.AddComponent(typeof(MeshRenderer));
        t.GetComponent<MeshRenderer>().material = material;
				tmp_pos = pos;
		tmp_pos.y += size.y/2;
        t.GetComponent<MeshFilter>().mesh = make_quad(tmp_pos, size, (Vector3.right * 90) + angle, uv_lows[4], uv_sizes[4], texture_size);

        GameObject bo = Instantiate(tmp, Vector3.zero, Quaternion.identity, tmp.transform);
        bo.AddComponent(typeof(MeshFilter));
        bo.AddComponent(typeof(MeshRenderer));
        bo.GetComponent<MeshRenderer>().material = material;
				tmp_pos = pos;
		tmp_pos.y -= size.y/2;
        bo.GetComponent<MeshFilter>().mesh = make_quad(tmp_pos, size, (Vector3.right * 270) +angle, uv_lows[5], uv_sizes[5], texture_size);
    }


    void Awake()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

		make_box(Vector3.zero, Vector3.one, Vector3.zero, 
		new Vector2[] 
		{
			new Vector2(24, 48),
			new Vector2(8, 48),
			new Vector2(0, 48),
			new Vector2(16, 48),
			new Vector2(8, 56),
			new Vector2(16, 56),
		}, 
		new Vector2[]
		{
			new Vector2(8, 8),
			new Vector2(8, 8),
			new Vector2(8, 8),
			new Vector2(8, 8),
			new Vector2(8, 8),
			new Vector2(8, 8),
		}, 
		new Vector2(64,64)
		);
		make_box(Vector3.down, Vector3.one, Vector3.zero, 
		new Vector2[] 
		{
			new Vector2(28, 32),
			new Vector2(20, 32),
			new Vector2(32, 32),
			new Vector2(36, 32),
			new Vector2(20, 44),
			new Vector2(28, 44),
		}, 
		new Vector2[]
		{
			new Vector2(8, 12),
			new Vector2(8, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(8, 4),
			new Vector2(8, 4),
		}, 
		new Vector2(64,64)
		);
		make_box(Vector3.left + Vector3.down, Vector3.one, Vector3.zero, 
		new Vector2[] 
		{
			new Vector2(40, 32),
			new Vector2(40, 32),
			new Vector2(40, 32),
			new Vector2(40, 32),
			new Vector2(44, 44),
			new Vector2(48, 44),
		}, 
		new Vector2[]
		{
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 4),
			new Vector2(4, 4),
		}, 
		new Vector2(64,64)
		);
		make_box(Vector3.right + Vector3.down, Vector3.one, Vector3.zero, 
		new Vector2[] 
		{
			new Vector2(40, 32),
			new Vector2(40, 32),
			new Vector2(40, 32),
			new Vector2(40, 32),
			new Vector2(44, 44),
			new Vector2(48, 44),
		}, 
		new Vector2[]
		{
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 4),
			new Vector2(4, 4),
		}, 
		new Vector2(64,64)
		);
		make_box(Vector3.left/2 + Vector3.down + Vector3.down, Vector3.one, Vector3.zero, 
		new Vector2[] 
		{
			new Vector2(12, 32),
			new Vector2(4, 32),
			new Vector2(8, 32),
			new Vector2(0, 32),
			new Vector2(4, 44),
			new Vector2(8, 44),
		}, 
		new Vector2[]
		{
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 4),
			new Vector2(4, 4),
		}, 
		new Vector2(64,64)
		);
		make_box(Vector3.right/2 + Vector3.down + Vector3.down, Vector3.one, Vector3.zero, 
		new Vector2[] 
		{
			new Vector2(12, 32),
			new Vector2(4, 32),
			new Vector2(8, 32),
			new Vector2(0, 32),
			new Vector2(4, 44),
			new Vector2(8, 44),
		}, 
		new Vector2[]
		{
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 12),
			new Vector2(4, 4),
			new Vector2(4, 4),
		}, 
		new Vector2(64,64)
		);
    }
}