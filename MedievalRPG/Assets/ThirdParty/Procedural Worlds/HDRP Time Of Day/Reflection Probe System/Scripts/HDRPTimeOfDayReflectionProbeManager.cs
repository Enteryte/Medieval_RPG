using UnityEngine;
#if HDPipeline
using UnityEngine.Rendering.HighDefinition;
#endif

namespace ProceduralWorlds.HDRPTOD
{
    [ExecuteAlways]
    public class HDRPTimeOfDayReflectionProbeManager : MonoBehaviour
    {
        public static HDRPTimeOfDayReflectionProbeManager Instance
        {
            get { return m_instance; }
        }
        [SerializeField] private static HDRPTimeOfDayReflectionProbeManager m_instance;

        public float RenderDistance
        {
            get { return m_renderDistance; }
            set
            {
                if (m_renderDistance != value)
                {
                    m_renderDistance = value;
                    if (m_profile != null)
                    {
                        m_profile.m_renderDistance = value;
                    }
#if HDPipeline && UNITY_2021_2_OR_NEWER
                    m_globalHDProbeData.settingsRaw.influence.boxSize = new Vector3(value, value, value);
#endif
                }
            }
        }
        [SerializeField] private float m_renderDistance = 5000f;

        public HDRPTimeOfDayReflectionProbeProfile Profile
        {
            get { return m_profile; }
            set
            {
                if (m_profile != value)
                {
                    m_profile = value;
                    RenderDistance = value.m_renderDistance;
                }
            }
        }
        [SerializeField] private HDRPTimeOfDayReflectionProbeProfile m_profile;

        public Transform m_playerCamera;
        public ReflectionProbe m_globalProbe;
        public ReflectionProbeTODData m_currentData;

#if HDPipeline && UNITY_2021_2_OR_NEWER
        [SerializeField]
        private HDAdditionalReflectionData m_globalHDProbeData;

        #region Unity Functions

        private void OnEnable()
        {
            m_instance = this;
            if (m_globalProbe != null)
            {
                m_globalProbe.enabled = enabled;
            }

            if (m_playerCamera == null)
            {
                m_playerCamera = HDRPTimeOfDayAPI.GetCamera();
            }

            UpdateProbeSystem();
        }
        private void OnDisable()
        {
            if (m_globalProbe != null)
            {
                m_globalProbe.enabled = false;
            }
        }
        private void LateUpdate()
        {
            FollowPlayer();
        }

        #endregion
        #region Public Functions

        /// <summary>
        /// Refreshes the probe system
        /// </summary>
        public void Refresh()
        {
            UpdateProbeSystem();
        }

        #endregion
        #region Private Functions

        /// <summary>
        /// Applies new probe data to the global probe
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SetNewData(ReflectionProbeTODData data)
        {
            if (data.Validate(HDRPTimeOfDay.Instance, Profile.m_probeTimeMode))
            {
                m_currentData = data;
                m_globalHDProbeData.settingsRaw.lighting.multiplier = data.m_intensity * GetMultiplier(data);
                m_globalHDProbeData.settingsRaw.cameraSettings.probeLayerMask = data.m_renderLayers;
                m_globalHDProbeData.settingsRaw.cameraSettings.culling.cullingMask = data.m_renderLayers;
                m_globalHDProbeData.SetTexture(ProbeSettings.Mode.Custom, data.m_probeCubeMap);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Updates the probe systems
        /// </summary>
        private void UpdateProbeSystem()
        {
            if (m_profile == null)
            {
                return;
            }

            bool isEnabled = m_profile.m_renderMode == ProbeRenderMode.Sky;
            m_globalProbe.enabled = isEnabled;

            if (isEnabled && CanProcess())
            {
                foreach (ReflectionProbeTODData data in m_profile.m_probeTODData)
                {
                    if (SetNewData(data))
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Gets the active weather probe data weather intensity multiplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private float GetMultiplier(ReflectionProbeTODData data)
        {
            if (HDRPTimeOfDay.Instance != null)
            {
                if (HDRPTimeOfDay.Instance.WeatherActive())
                {
                    int step = HDRPTimeOfDay.Instance.HasWeatherTransitionCompletedSteps(out bool isPlaying);
                    //Treansition to multiplier if isPlaying is true, transition back to 1 if it's not
                    if (isPlaying)
                    {
                        switch (step)
                        {
                            case 0:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.1f);
                            }
                            case 1:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.2f);
                            }
                            case 2:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.3f);
                            }
                            case 3:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.4f);
                            }
                            case 4:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.5f);
                            }
                            case 5:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.6f);
                            }
                            case 6:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.7f);
                            }
                            case 7:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.8f);
                            }
                            case 8:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 0.9f);
                            }
                            default:
                            {
                                return Mathf.Lerp(1f, data.m_weatherIntensityMultiplier, 1f);
                            }
                        }
                    }
                    else
                    {
                        switch (step)
                        {
                            case 0:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.1f);
                            }
                            case 1:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.2f);
                            }
                            case 2:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.3f);
                            }
                            case 3:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.4f);
                            }
                            case 4:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.5f);
                            }
                            case 5:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.6f);
                            }
                            case 6:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.7f);
                            }
                            case 7:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.8f);
                            }
                            case 8:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 0.9f);
                            }
                            default:
                            {
                                return Mathf.Lerp(data.m_weatherIntensityMultiplier, 1f, 1f);
                            }
                        }
                    }
                }
            }
            return 1f;
        }
        /// <summary>
        /// Moves the probe position to the player position
        /// </summary>
        private void FollowPlayer()
        {
            if (m_profile.m_followPlayer)
            {
                if (m_playerCamera != null && m_globalProbe != null)
                {
                    m_globalProbe.transform.position = m_playerCamera.position;
                }
            }
        }
        /// <summary>
        /// Checks to see if it can be processed
        /// </summary>
        /// <returns></returns>
        private bool CanProcess()
        {
            if (m_profile.m_renderMode != ProbeRenderMode.Sky)
            {
                return false;
            }

            if (m_globalProbe == null)
            {
                m_globalProbe = GetComponent<ReflectionProbe>();
                if (m_globalProbe == null)
                {
                    return false;
                }
            }

            if (m_globalHDProbeData == null)
            {
                if (m_globalProbe != null)
                {
                    m_globalHDProbeData = m_globalProbe.GetComponent<HDAdditionalReflectionData>();
                    if (m_globalHDProbeData == null)
                    {
                        m_globalHDProbeData = m_globalProbe.gameObject.AddComponent<HDAdditionalReflectionData>();
                    }
                }
            }

            return true;
        }

        #endregion
#endif
    }
}