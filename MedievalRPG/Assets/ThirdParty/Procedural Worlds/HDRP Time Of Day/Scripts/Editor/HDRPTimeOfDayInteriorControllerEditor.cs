using PWCommon5;
using UnityEditor;
using UnityEngine;

namespace ProceduralWorlds.HDRPTOD
{
    [CustomEditor(typeof(HDRPTimeOfDayInteriorController))]
    public class HDRPTimeOfDayInteriorControllerEditor : PWEditor
    {
        private HDRPTimeOfDayInteriorController m_controller;
        private GUIStyle m_boxStyle;
        private EditorUtils m_editorUtils;

        private Vector3 m_lastScale;
        private BoxCollider m_boxCollider;
        private Vector3 m_lastBoxColliderScale;
        private Vector3 m_lastPosition;
        private Vector3 m_lastRotation;

        private void OnEnable()
        {
            m_controller = (HDRPTimeOfDayInteriorController)target;
            if (m_controller != null)
            {
                CheckTransform(true);
                if (m_controller.Collider != null)
                {
                    m_boxCollider = (BoxCollider)m_controller.Collider;
                }
            }

            m_editorUtils = PWApp.GetEditorUtils(this);
        }
        private void OnDestroy()
        {
            if (m_editorUtils != null)
            {
                m_editorUtils.Dispose();
                m_editorUtils = null;
            }
        }
        public override void OnInspectorGUI()
        {
            m_controller = (HDRPTimeOfDayInteriorController)target;
            //Set up the box style
            if (m_boxStyle == null)
            {
                m_boxStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = {textColor = GUI.skin.label.normal.textColor},
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.UpperLeft
                };
            }
            m_editorUtils.Initialize();
            m_editorUtils.Panel("GlobalSettings", GlobalPanel, true);
            CheckTransform();
        }

        /// <summary>
        /// Global panel
        /// </summary>
        /// <param name="helpEnabled"></param>
        private void GlobalPanel(bool helpEnabled)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(m_boxStyle);
            m_controller.Collider = (Collider)m_editorUtils.ObjectField("Collider", m_controller.Collider, typeof(Collider), true, helpEnabled);
            m_controller.MainCamera = (Transform)m_editorUtils.ObjectField("MainCamera", m_controller.MainCamera, typeof(Transform), true, helpEnabled);
            m_controller.Priority = m_editorUtils.IntField("Priority", m_controller.Priority, helpEnabled);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(m_boxStyle);
            EditorGUILayout.LabelField("Controller Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            m_controller.m_controllerData.m_isEnabled = m_editorUtils.Toggle("Enabled", m_controller.m_controllerData.m_isEnabled, helpEnabled);
            if (m_controller.m_controllerData.m_isEnabled)
            {
                m_controller.m_controllerData.m_interiorReverbPreset = (AudioReverbPreset)m_editorUtils.EnumPopup("ReverbPreset", m_controller.m_controllerData.m_interiorReverbPreset, helpEnabled);
                if (m_controller.m_controllerData.m_interiorReverbPreset == AudioReverbPreset.User)
                {
                    EditorGUILayout.HelpBox(m_editorUtils.GetTextValue("UserReverbPresetHelp"), MessageType.Warning);
                }
                m_controller.m_controllerData.m_onEnterRenderMode = (InteriorParticleRenderMode)m_editorUtils.EnumPopup("RenderMode", m_controller.m_controllerData.m_onEnterRenderMode, helpEnabled);
                if (m_controller.m_controllerData.m_onEnterRenderMode == InteriorParticleRenderMode.BoxCollision)
                {
                    m_controller.m_controllerData.m_useBoundsSettings = m_editorUtils.Toggle("UseBoundsSettings", m_controller.m_controllerData.m_useBoundsSettings, helpEnabled);
                    if (m_controller.m_controllerData.m_useBoundsSettings)
                    {
                        EditorGUI.indentLevel++;
                        m_controller.m_controllerData.m_boundsMultiplier = m_editorUtils.FloatField("BoundsMultiplier", m_controller.m_controllerData.m_boundsMultiplier, helpEnabled);
                        EditorGUI.indentLevel--;
                    }
                    else
                    {
                        m_controller.m_controllerData.m_particleCollisionSettings = m_editorUtils.Vector4Field("CollisionScale", m_controller.m_controllerData.m_particleCollisionSettings, helpEnabled);
                        EditorGUI.indentLevel++;
                        m_controller.m_controllerData.m_boundsMultiplier = m_editorUtils.FloatField("BoundsMultiplier", m_controller.m_controllerData.m_boundsMultiplier, helpEnabled);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            if (EditorGUI.EndChangeCheck())
            {
                m_controller.Refresh(false);
                EditorUtility.SetDirty(m_controller);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(m_boxStyle);
            EditorGUILayout.LabelField("Gizmo Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            m_controller.m_gizmoSettings.m_drawGizmo = m_editorUtils.Toggle("DrawGizmo", m_controller.m_gizmoSettings.m_drawGizmo, helpEnabled);
            if (m_controller.m_gizmoSettings.m_drawGizmo)
            {
                EditorGUI.indentLevel++;
                m_controller.m_gizmoSettings.m_drawGizmoSelectedOnly = m_editorUtils.Toggle("SelectedOnly", m_controller.m_gizmoSettings.m_drawGizmoSelectedOnly, helpEnabled);
                m_controller.m_gizmoSettings.m_gizmoColor = m_editorUtils.ColorField("GizmoColor", m_controller.m_gizmoSettings.m_gizmoColor, helpEnabled);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                m_controller.m_gizmoSettings.SetGizmoRenderMode(m_controller.Collider);
                EditorUtility.SetDirty(m_controller);
            }
        }
        /// <summary>
        /// Checks the transform has been changed
        /// </summary>
        /// <param name="overrideSet"></param>
        private void CheckTransform(bool overrideSet = false)
        {
            if (!overrideSet)
            {
                bool set = false;
                if (m_controller.transform.localScale != m_lastScale)
                {
                    set = true;
                }
                else if (m_controller.transform.position != m_lastPosition)
                {
                    set = true;
                }
                else if (m_controller.transform.eulerAngles != m_lastRotation)
                {
                    set = true;
                }

                if (m_boxCollider != null)
                {
                    if (m_boxCollider.size != m_lastBoxColliderScale)
                    {
                        set = true;
                    }
                }

                if (set)
                {
                    m_lastScale = m_controller.transform.localScale;
                    m_lastPosition = m_controller.transform.position;
                    m_lastRotation = m_controller.transform.eulerAngles;
                    if (m_boxCollider != null)
                    {
                        m_lastBoxColliderScale = m_boxCollider.size;
                    }
                    m_controller.Refresh();
                }
            }
            else
            {
                m_lastScale = m_controller.transform.localScale;
                m_lastPosition = m_controller.transform.position;
                m_lastRotation = m_controller.transform.eulerAngles;
                if (m_boxCollider != null)
                {
                    m_lastBoxColliderScale = m_boxCollider.size;
                }
                m_controller.Refresh();
            }
        }
    }
}