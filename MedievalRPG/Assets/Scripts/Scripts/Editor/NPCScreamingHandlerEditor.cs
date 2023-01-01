using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCScreamingHandler))]
public class NPCScreamingHandlerEditor : Editor
{
    private void OnSceneGUI()
    {
        NPCScreamingHandler npcSH = (NPCScreamingHandler)target;

        DrawFieldOfView(npcSH, npcSH.viewRadius, npcSH.viewAngle, Color.red);
    }

    public void DrawFieldOfView(NPCScreamingHandler npcSH, float viewRadius, float viewAngle, Color wireColor)
    {
        Handles.color = wireColor;
        Handles.DrawWireArc(npcSH.transform.position, Vector3.up, Vector3.forward, 360, viewRadius);

        Vector3 viewAngleA = npcSH.DirFromAngles(-viewAngle / 2, false);
        Vector3 viewAngleB = npcSH.DirFromAngles(viewAngle / 2, false);

        Handles.DrawLine(npcSH.transform.position, npcSH.transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(npcSH.transform.position, npcSH.transform.position + viewAngleB * viewRadius);
    }
}
