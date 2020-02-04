using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_creation : MonoBehaviour {

    public GameObject m_tile;
    public GameObject[,] game_map;

    [Range(3, 50)]
    public int size_x = 10;
    [Range(3, 50)]
    public int size_y = 10;

    // Start is called before the first frame update
    void Start() {
        game_map = new GameObject[size_x, size_y];
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                GameObject newObj = Instantiate(m_tile) as GameObject;
                newObj.transform.position = new Vector3(i, 0, j);
                newObj.transform.SetParent(this.transform);
                game_map[i, j] = newObj;
            }
        }

    }

    // Update is called once per frame
    void Update() {

    }
}
