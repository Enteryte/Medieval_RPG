using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProceduralWorlds.HDRPTOD
{
    public enum ProbeRenderMode
    {
        None,
        Sky
    }

    public enum ProbeTimeMode
    {
        OneNightProbeOnly,
        CustomSetTime
    }

    [Serializable]
    public class ReflectionProbeTODData
    {
        public string m_name;
        public Cubemap m_probeCubeMap;
        public float m_intensity = 1f;
        public float m_weatherIntensityMultiplier = 0.5f;
        public LayerMask m_renderLayers = -1;
        public bool m_isNightProbe = false;
        public Vector2 m_timeOfDayAcceptance = new Vector2(6f, 11f);
        public bool m_showSettings = false;

#if HDPipeline && UNITY_2021_2_OR_NEWER
        /// <summary>
        /// Validates the data and if true it will apply the data
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <param name="timeMode"></param>
        /// <returns></returns>
        public bool Validate(HDRPTimeOfDay timeOfDay, ProbeTimeMode timeMode)
        {
            if (m_probeCubeMap != null && timeOfDay != null)
            {
                if (timeMode == ProbeTimeMode.OneNightProbeOnly)
                {
                    if (m_isNightProbe)
                    {
                        return !timeOfDay.IsDayTime();
                    }
                    else
                    {
                        if (timeOfDay.TimeOfDay >= m_timeOfDayAcceptance.x &&
                            timeOfDay.TimeOfDay <= m_timeOfDayAcceptance.y)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (timeOfDay.TimeOfDay >= m_timeOfDayAcceptance.x &&
                        timeOfDay.TimeOfDay <= m_timeOfDayAcceptance.y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
#endif
    }

    public class HDRPTimeOfDayReflectionProbeProfile : ScriptableObject
    {
        public List<ReflectionProbeTODData> m_probeTODData = new List<ReflectionProbeTODData>();
        public ProbeTimeMode m_probeTimeMode = ProbeTimeMode.CustomSetTime;
        public ProbeRenderMode m_renderMode = ProbeRenderMode.Sky;
        public bool m_followPlayer = true;
        public float m_renderDistance = 5000f;
    }
}