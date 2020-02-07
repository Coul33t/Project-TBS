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

        for (int i = 0; i < size_x; i++) {
            for (int j = 0; j < size_y; j++) {
                GameObject new_tile = Instantiate(m_tile) as GameObject;

                float elevation = (float)((int)Random.Range(-2.0f, 2.0f));
                new_tile.transform.position = new Vector3(i, elevation, j);
                new_tile.transform.SetParent(this.transform);

                Renderer obj_renderer = new_tile.GetComponent<Renderer>();
                if (Random.Range(0.0f, 1.0f) < 0.5) {
                    obj_renderer.material.SetColor("_Color", Color.green);
                }

                else {
                    obj_renderer.material.SetColor("_Color", Color.grey);
                }


                game_map[i, j] = new_tile;
            }
        }

        Camera.main.GetComponent<camera_controller>().MoveTo(new Vector3(size_x / 2, 0, size_y / 2));
    }

    // Update is called once per frame
    void Update() {

    }
}
