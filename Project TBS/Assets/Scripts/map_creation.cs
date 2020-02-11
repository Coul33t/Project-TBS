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
    [Range(-10,0)]
    public int min_size_z = 0;
    [Range(0, 10)]
    public int max_size_z = 0;
    [Range(0, 1)]
    public float simple_coef_0 = 0.1f;
    [Range(0, 1)]
    public float simple_coef_1 = 0.9f;
    [Range(0, 1)]
    public float multiple_coef_0 = 0.1f;
    [Range(0, 1)]
    public float multiple_coef_1 = 0.2f;
    [Range(0, 1)]
    public float multiple_coef_2 = 0.3f;
    [Range(0, 1)]
    public float multiple_coef_3 = 0.4f;
    [Range(0, 1)]
    public float multiple_coef_4 = 0.7f;



    void Awake() {
        CreateMap();
    }

    // Start is called before the first frame update
    void Start() {
        GameObject.Find("Engine").GetComponent<camera_controller>().MoveTo(new Vector3(-size_x / 4, 0, -size_y / 4));
    }

    // Update is called once per frame
    void Update() {

    }

    public void CreateMap() {
        game_map = new GameObject[size_x, size_y];
        int[,] elevation_grid = GetSimplexElevation(size_x, size_y, min_size_z, max_size_z);

        for (int i = 0; i < size_x; i++) {
            for (int j = 0; j < size_y; j++) {
                GameObject new_tile = Instantiate(m_tile) as GameObject;

                new_tile.transform.position = new Vector3(i, elevation_grid[i, j], j);
                new_tile.transform.SetParent(this.transform);

                Renderer obj_renderer = new_tile.GetComponent<Renderer>();

                if (i == j) {
                    obj_renderer.material.color = new Color32(255, 150, 150, 0);
                }

                if (elevation_grid[i, j] >= 0)
                    obj_renderer.material.color = new Color32(150, 255, 155, 0);

                else if (elevation_grid[i, j] == -1)
                    obj_renderer.material.color = new Color32(139, 69, 19, 0);

                else if (elevation_grid[i, j] == -2)
                    obj_renderer.material.color = new Color32(255, 255, 150, 0);

                else
                    obj_renderer.material.color = new Color32(150, 150, 150, 0);


                game_map[i, j] = new_tile;
            }
        }
    }

    private int[,] GetSimplexElevation(int size_x, int size_y, int limit_z_low, int limit_z_high) {
        int[,] elevation_grid = new int[size_x, size_y];
        int last_single_elevation;
        int[] last_elevations = new int[2];
        float rand;
        /*float[] coefs_simple = new float[] {0.1f, 0.9f};
        float[] coefs_multiple = new float[] {0.05f, 0.10f, 0.15f, 0.20f, 0.60f};*/
        float[] coefs_simple = new float[] {simple_coef_0, simple_coef_1};
        float[] coefs_multiple = new float[] {multiple_coef_0,
                                              multiple_coef_1,
                                              multiple_coef_2,
                                              multiple_coef_3,
                                              multiple_coef_4};
        for (int i = 0; i < size_x; i++) {
            for (int j = 0; j < size_y; j++) {
                if (i == 0 && j == 0) {
                    elevation_grid[i, j] = (int) Mathf.Round(Random.Range(min_size_z, max_size_z + 1));
                }

                else {
                    rand = Random.value;

                    if (i == 0) {
                        last_single_elevation = elevation_grid[i, j-1];

                        if (rand < coefs_simple[0] && last_single_elevation < limit_z_high) {
                            elevation_grid[i, j] = last_single_elevation + 1;
                        } else if (rand > coefs_simple[1] && last_single_elevation > limit_z_low) {
                            elevation_grid[i, j] = last_single_elevation - 1;
                        } else {
                            elevation_grid[i, j] = last_single_elevation;
                        }
                    }

                    else if (j == 0) {
                        last_single_elevation = elevation_grid[i-1, j];

                        if (rand < coefs_simple[0] && last_single_elevation < limit_z_high) {
                            elevation_grid[i, j] = last_single_elevation + 1;
                        } else if (rand > coefs_simple[1] && last_single_elevation > limit_z_low) {
                            elevation_grid[i, j] = last_single_elevation - 1;
                        } else {
                            elevation_grid[i, j] = last_single_elevation;
                        }
                    }

                    else {
                        last_elevations[0] = elevation_grid[i-1, j];
                        last_elevations[1] = elevation_grid[i, j-1];

                        if (last_elevations[0] == last_elevations[1]) {
                            if (rand < coefs_simple[0] && last_elevations[0] < limit_z_high) {
                                elevation_grid[i, j] = last_elevations[0] + 1;
                            } else if (rand > coefs_simple[1] && last_elevations[0] > limit_z_low) {
                                elevation_grid[i, j] = last_elevations[0] - 1;
                            } else {
                                elevation_grid[i, j] = last_elevations[0];
                            }
                        }

                        else {
                            if (rand < coefs_multiple[0] && last_elevations[0] < limit_z_high) {
                                elevation_grid[i, j] = last_elevations[0] + 1;
                            } else if (rand < coefs_multiple[1] && last_elevations[0] > limit_z_low) {
                                elevation_grid[i, j] = last_elevations[0] - 1;
                            } else if (rand < coefs_multiple[2] && last_elevations[1] < limit_z_high) {
                                elevation_grid[i, j] = last_elevations[1] + 1;
                            } else if (rand < coefs_multiple[3] && last_elevations[1] > limit_z_low) {
                                elevation_grid[i, j] = last_elevations[1] - 1;
                            } else if (rand < coefs_multiple[4]){
                                elevation_grid[i, j] = last_elevations[0];
                            } else {
                                elevation_grid[i, j] = last_elevations[1];
                            }
                        }
                    }

                }

            }
        }

        return elevation_grid;
    }
}
