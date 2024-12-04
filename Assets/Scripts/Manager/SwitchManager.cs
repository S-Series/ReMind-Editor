using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public static List<Camera> s_cameras = new List<Camera>();
    [SerializeField] List<Camera> cameras = new List<Camera>(); //@ "List<Camera>[0]" will be default cam

    private void Awake()
    {
        if (cameras.Count < 1) { throw new System.Exception("Camera Setting Error!"); }
        s_cameras = cameras;
    }

    private void Start()
    {
        foreach (Camera obj in s_cameras) { obj.gameObject.SetActive(false); }
        s_cameras[0].gameObject.SetActive(true);
    }
    public static void ChangeCamera(Camera cam)
    {
        if (!s_cameras.Contains(cam)) {s_cameras.Add(cam); }

        foreach (Camera c in s_cameras) { c.gameObject.SetActive(false); }
        cam.gameObject.SetActive(true);
    }
    public static void ChangeCamera(string CamObjectName)
    {
        int camIndex;
        camIndex = s_cameras.FindIndex(item => item.gameObject.name == CamObjectName);

        if (camIndex == -1) { throw new System.Exception("Cannot Find Camera With Name"); }

        foreach (Camera c in s_cameras) { c.gameObject.SetActive(false); }
        s_cameras[camIndex].gameObject.SetActive(true);
    }

    [ContextMenu("Find All Cameras")]
    public void FindAllCamera()
    {
        cameras = new List<Camera>();
        cameras = GameManager.FindAllObjects<Camera>();
    }
}
