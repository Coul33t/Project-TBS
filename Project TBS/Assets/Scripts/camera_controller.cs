using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour {

    public Camera cam;
    public bool must_rotate = false;
    public bool must_move = false;

    private Vector3 destination;
    private Quaternion rotation;
    private int camera_elevation = 15;

    private float lerp_coef_mov = 0;
    private float lerp_coef_rot = 0;
    private float mov_duration = 2;
    private float rot_duration = 2;
    private float mov_start_time = -1;
    private float rot_start_time = -1;
    private float mov_progress = 0;
    private float rot_progress = 0;

    private Vector3 focus_point = new Vector3(5, 0, 5);
    private List<Vector3> camera_rotation = new List<Vector3>();
    private int camera_rotation_index = 0;

    // Start is called before the first frame update
    void Start() {
        cam.orthographicSize = 5;
        Vector3 pos = new Vector3(0, camera_elevation, 0);
        cam.transform.position = pos;
        //cam.transform.LookAt(focus_point);
        cam.transform.rotation = Quaternion.Euler(new Vector3(65, 45, 0));
        this.rotation = cam.transform.rotation;

        camera_rotation.Add(new Vector3(0, 1, 0));
        camera_rotation.Add(new Vector3(1, 1, 0));
        camera_rotation.Add(new Vector3(1, 1, 1));
        camera_rotation.Add(new Vector3(0, 1, 1));
    }

    // Update is called once per frame
    void Update() {
        if (must_move) {
            mov_progress = (Time.time - mov_start_time) / mov_duration;

            if (mov_progress > 1) {
                mov_progress = 0;
                must_move = false;
            }

            else
                GoToDestination();
        }

        if (must_rotate) {
            rot_progress = (Time.time - rot_start_time) / rot_duration;

            if (rot_progress > 1) {
                rot_progress = 0;
                must_rotate = false;
            }

            else
                RotateToDestination();
        }
    }

    public void MoveTo(Vector3 destination) {
        this.destination = destination;
        must_move = true;
        mov_start_time = Time.time;
    }

    public void RotateTo(Quaternion rotation) {
        this.rotation = rotation;
        must_rotate = true;
        rot_start_time = Time.time;
    }

    public void SetFocusPoint(Vector3 focus_point) {
        this.focus_point = focus_point;
    }

    public void RotateToDestination(bool linear=false) {
        if (linear)
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, this.rotation, rot_progress);
        else
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, this.rotation, rot_progress);
    }

    public void GoToDestination(bool linear=true) {
        if (linear)
            cam.transform.position = Vector3.Lerp(cam.transform.position, this.destination, mov_progress);
        else
            cam.transform.position = Vector3.Slerp(cam.transform.position, this.destination, mov_progress);

    }

    public void ChangeOrthographicSize(int direction) {
        if ((direction < 0 && cam.orthographicSize > 5) || (direction > 0 && cam.orthographicSize < 20))
            cam.orthographicSize += direction * 5;
    }

    public void rotate(int direction) {
        if (direction > 0) {
            camera_rotation_index++;
            if (camera_rotation_index > 3)
                camera_rotation_index = 0;
        }

        else {
            camera_rotation_index--;
            if (camera_rotation_index < 0)
                camera_rotation_index = camera_rotation.Count - 1;
        }

        Vector3 new_coordinates = camera_rotation[camera_rotation_index];
        Vector3 size = new Vector3(GameObject.Find("Map").GetComponent<map_creation>().size_x,
                                   camera_elevation,
                                   GameObject.Find("Map").GetComponent<map_creation>().size_y);

        Vector3 final_position = Vector3.Scale(new_coordinates, size);
        Vector3 final_rotation = this.rotation.eulerAngles;

        final_rotation.y  -= 90;
        if (direction < 0)
            final_rotation.y += 2 * 90;

        MoveTo(final_position);
        RotateTo(Quaternion.Euler(final_rotation));
    }
}
