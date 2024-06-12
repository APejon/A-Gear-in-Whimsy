using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class FollowScene : MonoBehaviour
{
    public AnimationClip animationClip;
    private bool isRecording = false;
    private float startTime;

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!isRecording || animationClip == null)
            return ;
        // Get the Scene view camera's transform
        Transform sceneCameraTransform = sceneView.camera.transform;
        // Update this camera's position and rotation
        transform.position = sceneCameraTransform.position;
        transform.rotation = sceneCameraTransform.rotation;
         // Calculate the time since recording started
        float time = Time.time - startTime;
        // Record position and rotation
        SetKeyframe(animationClip, "m_LocalPosition.x", time, transform.localPosition.x);
        SetKeyframe(animationClip, "m_LocalPosition.y", time, transform.localPosition.y);
        SetKeyframe(animationClip, "m_LocalPosition.z", time, transform.localPosition.z);
        SetKeyframe(animationClip, "m_LocalRotation.x", time, transform.localRotation.x);
        SetKeyframe(animationClip, "m_LocalRotation.y", time, transform.localRotation.y);
        SetKeyframe(animationClip, "m_LocalRotation.z", time, transform.localRotation.z);
        SetKeyframe(animationClip, "m_LocalRotation.w", time, transform.localRotation.w);
        // Repaint the Scene view to ensure the changes are reflected
        sceneView.Repaint();
    }

    void SetKeyframe(AnimationClip clip, string propertyName, float time, float value)
    {
        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), propertyName));
        if (curve == null)
        {
            curve = new AnimationCurve();
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), propertyName), curve);
        }
        curve.AddKey(new Keyframe(time, value));
    }
    public void StartRecording()
    {
        if (animationClip == null)
        {
            Debug.LogError("No animation clip assigned!");
            return;
        }
        animationClip.ClearCurves(); // Clear any existing curves
        startTime = Time.time;
        isRecording = true;
        Debug.Log("Recording started");
    }
    public void StopRecording()
    {
        isRecording = false;
        Debug.Log("Recording stopped");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FollowScene))]
public class CameraAnimationRecorderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FollowScene recorder = (FollowScene)target;

        if (GUILayout.Button("Start Recording"))
        {
            recorder.StartRecording();
        }

        if (GUILayout.Button("Stop Recording"))
        {
            recorder.StopRecording();
        }
    }
}
#endif
