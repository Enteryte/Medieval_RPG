using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interacting))]
public class InteractingEditor : Editor
{
    private void OnSceneGUI()
    {
        Interacting interacting = (Interacting)target;

        DrawFieldOfView(interacting, interacting.viewRadius, interacting.viewAngle, Color.red);
        DrawFieldOfView(interacting, interacting.viewRadius2, interacting.viewAngle2, Color.blue);
    }

    public void DrawFieldOfView(Interacting interacting, float viewRadius, float viewAngle, Color wireColor)
    {
        Handles.color = wireColor;
        Handles.DrawWireArc(interacting.transform.position, Vector3.up, Vector3.forward, 360, viewRadius);

        Vector3 viewAngleA = interacting.DirFromAngles(-viewAngle / 2, false);
        Vector3 viewAngleB = interacting.DirFromAngles(viewAngle / 2, false);

        Handles.DrawLine(interacting.transform.position, interacting.transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(interacting.transform.position, interacting.transform.position + viewAngleB * viewRadius);
    }
}
