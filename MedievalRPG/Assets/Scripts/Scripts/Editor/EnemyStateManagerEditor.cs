using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStateManager))]
public class EnemyStateManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyStateManager stateManager = (EnemyStateManager)target;

        DrawFieldOfView(stateManager, stateManager.viewRadius, stateManager.viewAngle, Color.red);
    }

    public void DrawFieldOfView(EnemyStateManager stateManager, float viewRadius, float viewAngle, Color wireColor)
    {
        Handles.color = wireColor;
        Handles.DrawWireArc(stateManager.transform.position, Vector3.up, Vector3.forward, 360, viewRadius);

        Vector3 viewAngleA = stateManager.DirFromAngles(-viewAngle / 2, false);
        Vector3 viewAngleB = stateManager.DirFromAngles(viewAngle / 2, false);

        Handles.DrawLine(stateManager.transform.position, stateManager.transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(stateManager.transform.position, stateManager.transform.position + viewAngleB * viewRadius);
    }
}
