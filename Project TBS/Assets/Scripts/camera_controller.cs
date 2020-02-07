using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour {

    public Camera cam;
    private Vector3 destination;
    private bool must_move = false;
    private float lerp_coef = 0;

    // Start is called before the first frame update
    void Start() {
        Vector3 pos = new Vector3(0, 10, 0);
        cam.transform.position = pos;
        cam.transform.LookAt(new Vector3(5, 0, 5));
    }

    // Update is called once per frame
    void Update() {
        if (must_move) {
            lerp_coef += 0.1f;

            GoToDestination(destination, lerp_coef);
            if (lerp_coef >= 1) {
                lerp_coef = 0;
                must_move = false;
            }
        }

        // SetCameraToCenter(m_variateurTestX, m_variateurTestY, m_variateurTestZ);
    }

    public void SetCameraToCenter(float x, float y, float z = 0) {
        Vector3 center_of_terrain = new Vector3(x, z, y);
        Vector3 camera_pos = new Vector3(x / 2, 10, y / 2);
        cam.transform.position = camera_pos;
        cam.transform.LookAt(center_of_terrain);
    }

    public void MoveTo(Vector3 destination) {
        this.destination = destination;
        must_move = true;
    }

    public void GoToDestination(Vector3 destination, float lerp_coef) {
        transform.position = Vector3.Lerp(transform.position, destination, lerp_coef);
    }
}
