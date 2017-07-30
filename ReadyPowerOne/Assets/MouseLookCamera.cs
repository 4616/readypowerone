using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookCamera : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0f, -7f, 0f);

    private Vector3 lastMousePos = Vector3.zero;

    // Update is called once per frame
    void Update() {
        if (target == null) {
            target = Player.GetPlayer().transform;
        }
        if (target != null) {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
            

            //transform.rotation = target.rotation;

            //float angle = AngleBetweenPoints(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f));

            Vector3 mouseDelta = lastMousePos - Input.mousePosition;

            

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle + 90f)), Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z + (mouseDelta.x/20f)));

            transform.position += offset;
            lastMousePos = Input.mousePosition;
        }
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
