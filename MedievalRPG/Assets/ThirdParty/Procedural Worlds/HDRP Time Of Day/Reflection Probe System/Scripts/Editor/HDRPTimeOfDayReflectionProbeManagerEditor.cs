using System.Collections.Generic;
using PWCommon5;
using UnityEditor;
using UnityEngine;

namespace ProceduralWorlds.HDRPTOD
{
#if HDPipeline
    [CustomEditor(typeof(HDRPTimeOfDayReflectionProbeManager))]
    public class HDRPTimeOfDayReflectionProbeManagerEditor : PWEditor
    {
        private EditorUtils m_editorUtils;
        private HDRPTimeOfDayReflectionProbeManager m_manager;
        private GUIStyle m_boxStyle;

        private void OnEnable()
        {
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
            m_manager = (HDRPTimeOfDayReflectionProbeManager)target;
            m_editorUtils.Initialize();

            //Set up the box style
            if (m_boxStyle == null)
            {
                m_boxStyle = new GUIStyle(GUI.skin.box)
                {
                    normal =
                    {
                        textColor = GUI.skin.label.normal.textColor
                    },
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.UpperLeft
                };
            }

            m_editorUtils.Panel("GlobalPanel", GlobalPanel, true);
        }

        private void GlobalPanel(bool helpEnabled)
        {
            HDRPTimeOfDayReflectionProbeProfile profile = m_manager.Profile;
            if (profile == null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginVertical(m_boxStyle);
                EditorGUILayout.LabelField("Global Settings", EditorStyles.boldLabel);
                profile = (HDRPTimeOfDayReflectionProbeProfile)m_editorUtils.ObjectField("ReflectionProbeProfile", profile, typeof(HDRPTimeOfDayReflectionProbeProfile), false, helpEnabled);
                EditorGUILayout.EndVertical();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_manager, "Probe Manager Change Made");
                    m_manager.Profile = profile;
                    EditorUtility.SetDirty(m_manager);
                }

                return;
            }
            ProbeRenderMode renderMode = m_manager.Profile.m_renderMode;
            ProbeTimeMode probeTimeMode = m_manager.Profile.m_probeTimeMode;
            bool followPlayer = m_manager.Profile.m_followPlayer;
            float renderDistance = m_manager.RenderDistance;
            Transform playerCamera = m_manager.m_playerCamera;
            ReflectionProbe probe = m_manager.m_globalProbe;


            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(m_boxStyle);
            EditorGUILayout.LabelField("Global Settings", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            profile = (HDRPTimeOfDayReflectionProbeProfile)m_editorUtils.ObjectField("ReflectionProbeProfile", profile, typeof(HDRPTimeOfDayReflectionProbeProfile), false, helpEnabled);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_manager, "Probe Manager Change Made");
                m_manager.Profile = profile;
                EditorUtility.SetDirty(m_manager);
                EditorGUIUtility.ExitGUI();
            }
            renderMode = (ProbeRenderMode)m_editorUtils.EnumPopup("RenderMode", renderMode, helpEnabled);
            if (renderMode == ProbeRenderMode.Sky)
            {
                EditorGUI.indentLevel++;
                probeTimeMode = (ProbeTimeMode)m_editorUtils.EnumPopup("ProbeTimeMode", probeTimeMode, helpEnabled);
                probe = (ReflectionProbe)m_editorUtils.ObjectField("GlobalProbe", probe, typeof(ReflectionProbe), true, helpEnabled);
                renderDistance = m_editorUtils.FloatField("RenderDistance", renderDistance, helpEnabled);
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(m_boxStyle);
                EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);
                followPlayer = m_editorUtils.Toggle("FollowPlayer", followPlayer, helpEnabled);
                if (followPlayer)
                {
                    EditorGUI.indentLevel++;
                    playerCamera = (Transform)m_editorUtils.ObjectField("PlayerCamera", playerCamera, typeof(Transform), true, helpEnabled);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();

                m_editorUtils.Panel("ProbeDataSettings", ProbeDataPanel);
            }
            else
            {
                EditorGUILayout.EndVertical();
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_manager, "Probe Manager Change Made");
                m_manager.Profile.m_renderMode = renderMode;
                m_manager.Profile.m_probeTimeMode = probeTimeMode;
                m_manager.Profile.m_followPlayer = followPlayer;
                m_manager.RenderDistance = renderDistance;
                m_manager.m_playerCamera = playerCamera;
                m_manager.m_globalProbe = probe;
                EditorUtility.SetDirty(m_manager);
                EditorUtility.SetDirty(m_manager.Profile);
#if HDPipeline && UNITY_2021_2_OR_NEWER
                m_manager.Refresh();
#endif
            }
        }

        private void ProbeDataPanel(bool helpEnabled)
        {
            if (m_manager.m_currentData != null)
            {
                EditorGUILayout.LabelField("Current Selected Data: " + m_manager.m_currentData.m_name, EditorStyles.boldLabel);
            }

#if HDPipeline && UNITY_2021_2_OR_NEWER
            if (m_manager.Profile.m_probeTimeMode == ProbeTimeMode.CustomSetTime)
            {
                EditorGUILayout.BeginHorizontal();
                if (m_editorUtils.Button("-1 Hour"))
                {
                    float time = HDRPTimeOfDayAPI.GetCurrentTime();
                    time -= 1f;
                    if (time < 0f)
                    {
                        time = 24f;
                    }
                    HDRPTimeOfDayAPI.SetCurrentTime(time);
                }

                if (m_editorUtils.Button("+1 Hour"))
                {
                    float time = HDRPTimeOfDayAPI.GetCurrentTime();
                    time += 1f;
                    if (time > 24f)
                    {
                        time = 0f;
                    }
                    HDRPTimeOfDayAPI.SetCurrentTime(time);
                }

                if (m_editorUtils.Button("Round Hour"))
                {
                    HDRPTimeOfDayAPI.SetCurrentTime(Mathf.RoundToInt(HDRPTimeOfDayAPI.GetCurrentTime()));
                }

                EditorGUILayout.EndHorizontal();
            }
#endif

            List<ReflectionProbeTODData> todData = m_manager.Profile.m_probeTODData;
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < todData.Count; i++)
            {
                EditorGUILayout.BeginVertical(m_boxStyle);
                todData[i].m_showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(todData[i].m_showSettings, todData[i].m_name);
                if (todData[i].m_showSettings)
                {
                    todData[i].m_name = m_editorUtils.TextField("Name", todData[i].m_name, helpEnabled);
                    todData[i].m_probeCubeMap = (Cubemap)m_editorUtils.ObjectField("Cubemap", todData[i].m_probeCubeMap, typeof(Cubemap), false, helpEnabled, GUILayout.MaxHeight(16f));
                    todData[i].m_intensity = m_editorUtils.FloatField("Intensity", todData[i].m_intensity, helpEnabled);
                    todData[i].m_weatherIntensityMultiplier = m_editorUtils.FloatField("WeatherMultiplier", todData[i].m_weatherIntensityMultiplier, helpEnabled);
#if HDPipeline && UNITY_2021_2_OR_NEWER
                    todData[i].m_renderLayers = HDRPTimeOfDayEditor.LayerMaskField(m_editorUtils, "RenderLayers", todData[i].m_renderLayers, helpEnabled);
#endif
                    todData[i].m_isNightProbe = m_editorUtils.Toggle("IsNightProbe", todData[i].m_isNightProbe, helpEnabled);
                    todData[i].m_timeOfDayAcceptance = m_editorUtils.Vector2Field("TimeAcceptanceValue", todData[i].m_timeOfDayAcceptance, helpEnabled);
                    if (m_editorUtils.Button("Remove"))
                    {
                        todData.RemoveAt(i);
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.EndVertical();
            }

            if (m_editorUtils.Button("AddNewProbeData"))
            {
                todData.Add(new ReflectionProbeTODData());
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_manager, "Probe Manager Change Made");
                m_manager.Profile.m_probeTODData = todData;
                EditorUtility.SetDirty(m_manager);
                EditorUtility.SetDirty(m_manager.Profile);
            }
        }
    }
#endif
                }