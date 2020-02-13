using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input_handler : MonoBehaviour {

    public Camera cam;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        CameraInput();
    }

    void CameraInput() {
        int zoom_direction = 0;

        if (Input.GetKeyDown("[+]"))
            zoom_direction = -1;
        else if (Input.GetKeyDown("[-]"))
            zoom_direction = 1;
        else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            zoom_direction = (int) -Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));
        if (zoom_direction != 0)
            GameObject.Find("Engine").GetComponent<camera_controller>().ChangeOrthographicSize(zoom_direction);


        if (Input.GetKeyDown("a")) {
            if (!(GameObject.Find("Engine").GetComponent<camera_controller>().must_rotate || GameObject.Find("Engine").GetComponent<camera_controller>().must_move))
                GameObject.Find("Engine").GetComponent<camera_controller>().rotate(-1);
        }

        else if (Input.GetKeyDown("e")) {
            if (!(GameObject.Find("Engine").GetComponent<camera_controller>().must_rotate || GameObject.Find("Engine").GetComponent<camera_controller>().must_move))
                GameObject.Find("Engine").GetComponent<camera_controller>().rotate(1);
        }

    }
}
