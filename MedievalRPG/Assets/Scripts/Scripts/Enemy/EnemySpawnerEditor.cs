using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = base.CreateInspectorGUI();
        inspector.Add(new Label("Diese Funktion benötigt entweder einen Collider oder kann auch als Manuell ausgeführt werden um Gegner zu Spawnen.\n" +
                                "Welche Art Collider ist Irellevant, er muss allerdings auf IsTrigger gesetzt sein."));
        return inspector;
    }

}
