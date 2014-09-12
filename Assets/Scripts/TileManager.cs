using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{
	// Use this for initialization
	public Transform tile;
	void Start ()
	{
		for (float x = -5 * 0.4f; x < 5 * 0.4f; x += 0.4f) 
		{
			Instantiate(tile, new Vector3(x, 0, 0), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
