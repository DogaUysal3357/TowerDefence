using UnityEngine;
using System.Collections;


// http://forum.unity3d.com/threads/rts-camera-script.72045/

public class CameraController : MonoBehaviour
{
    [Header("Panning")]
    // WASDQE Panning
    public float minPanSpeed = 10f;    // Starting panning speed
    public float maxPanSpeed = 1000f;   // Max panning speed
    public float panTimeConstant = 20f; // Time to reach max panning speed

    [Header("Rotation")]
    // Mouse right-down rotation
    public float rotateSpeed = 10; // mouse down rotation speed about x and y axes
    public float zoomSpeed = 2;    // zoom speed
    
    [Header("Zoom")]
    public float scrollSpeed = 5f;
    public float minY = 20;
    public float maxY = 100;

    float panSpeed = 10;
    Vector3 panTranslation;
    bool wKeyDown = false;
    bool aKeyDown = false;
    bool sKeyDown = false;
    bool dKeyDown = false;
    bool qKeyDown = false;
    bool eKeyDown = false;
    bool shiftKeyDown = false;

    Vector3 lastMousePosition;
    //new Camera camera;

    void Start()
    {
       //camera = GetComponent<Camera>();
       maxPanSpeed = minPanSpeed * 4;
    }

    void Update()
    {
        //
        // WASDQE Panning

        // read key inputs
        wKeyDown = Input.GetKey(KeyCode.W);
        aKeyDown = Input.GetKey(KeyCode.A);
        sKeyDown = Input.GetKey(KeyCode.S);
        dKeyDown = Input.GetKey(KeyCode.D);
        qKeyDown = Input.GetKey(KeyCode.Q);
        eKeyDown = Input.GetKey(KeyCode.E);
        shiftKeyDown = Input.GetKey(KeyCode.LeftShift);

        // determine panTranslation
        panTranslation = Vector3.zero;
        if (dKeyDown && !aKeyDown)
            panTranslation += Vector3.right * Time.deltaTime * panSpeed;
        else if (aKeyDown && !dKeyDown)
            panTranslation += Vector3.left * Time.deltaTime * panSpeed;

        if (wKeyDown && !sKeyDown)
            panTranslation += Vector3.forward * Time.deltaTime * panSpeed;
        else if (sKeyDown && !wKeyDown)
            panTranslation += Vector3.back * Time.deltaTime * panSpeed;

        if (qKeyDown && !eKeyDown)
            panTranslation += Vector3.down * Time.deltaTime * panSpeed;
        else if (eKeyDown && !qKeyDown)
            panTranslation += Vector3.up * Time.deltaTime * panSpeed;
        transform.Translate(panTranslation, Space.Self);


        
        // Update panSpeed
        if (shiftKeyDown && (wKeyDown || aKeyDown || sKeyDown ||
            dKeyDown || qKeyDown || eKeyDown ))
        {
            panSpeed = maxPanSpeed;
        }
        else
        {
            panSpeed = minPanSpeed;
        }
        


        //
        // Mouse Rotation
        if (Input.GetMouseButton(1))
        {
            // if the game window is separate from the editor window and the editor
            // window is active then you go to right-click on the game window the
            // rotation jumps if  we don't ignore the mouseDelta for that frame.
            Vector3 mouseDelta;
            if (lastMousePosition.x >= 0 &&
                lastMousePosition.y >= 0 &&
                lastMousePosition.x <= Screen.width &&
                lastMousePosition.y <= Screen.height)
                mouseDelta = Input.mousePosition - lastMousePosition;
            else
                mouseDelta = Vector3.zero;

            var rotation = Vector3.up * Time.deltaTime * rotateSpeed * mouseDelta.x;
            rotation += Vector3.left * Time.deltaTime * rotateSpeed * mouseDelta.y;
            transform.Rotate(rotation, Space.Self);

            // Make sure z rotation stays locked
            rotation = transform.rotation.eulerAngles;
            rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }

        lastMousePosition = Input.mousePosition;

        //
        // Mouse Zoom

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;

        //camera.fieldOfView -= Input.mouseScrollDelta.y * zoomSpeed;
    }
}
