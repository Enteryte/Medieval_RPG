#if HDPipeline && UNITY_2021_2_OR_NEWER
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if GAIA_PRO_PRESENT
using Gaia;
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

namespace ProceduralWorlds.HDRPTOD
{
    [System.Serializable]
    public class HDRPTimeOfDayComponents
    {
        //Global
        public GameObject m_componentsObject;
        //Lighting
        public GameObject m_sunRotationObject;
        public Light m_sunLight;
        public HDAdditionalLightData m_sunLightData;
        public Light m_moonLight;
        public HDAdditionalLightData m_moonLightData;
        public Volume m_timeOfDayVolume;
        public VolumeProfile m_timeOfDayVolumeProfile;
        public VisualEnvironment m_visualEnvironment;
        public PhysicallyBasedSky m_physicallyBasedSky;
        public CloudLayer m_cloudLayer;
        public VolumetricClouds m_volumetricClouds;
        public GlobalIllumination m_globalIllumination;
        public Fog m_fog;
        public LocalVolumetricFog m_localVolumetricFog;
        public Exposure m_exposure;
        public ScreenSpaceReflection m_screenSpaceReflection;
        public ScreenSpaceRefraction m_screenSpaceRefraction;
        public ContactShadows m_contactShadows;
        public MicroShadowing m_microShadowing;
        public IndirectLightingController m_indirectLightingController;
        public HDShadowSettings m_shadows;
        public HDRPTimeOfDayReflectionProbeManager m_reflectionProbeManager;
        //Post FX
        public Volume m_timeOfDayPostFXVolume;
        public VolumeProfile m_timeOfDayPostFXVolumeProfile;
        public ColorAdjustments m_colorAdjustments;
        public WhiteBalance m_whiteBalance;
        public Bloom m_bloom;
        public SplitToning m_splitToning;
        public Vignette m_vignette;
        public AmbientOcclusion m_ambientOcclusion;
        public DepthOfField m_depthOfField;
        //Advanced
        public LensFlareComponentSRP m_sunLensFlare;
        public LensFlareComponentSRP m_moonLensFlare;
        //Camera
        public Camera m_camera;
        public HDAdditionalCameraData m_cameraData;
        //Ray tracing
        public Volume m_rayTracingVolume;
#if RAY_TRACING_ENABLED
        public GlobalIllumination m_rayTracedGlobalIllumination;
        public ScreenSpaceReflection m_rayTracedScreenSpaceReflection;
        public AmbientOcclusion m_rayTracedAmbientOcclusion;
        public RayTracingSettings m_rayTracedSettings;
        public RecursiveRendering m_rayTracedRecursiveRendering;
        public SubSurfaceScattering m_rayTracedSubSurfaceScattering;
#endif

        public bool Validated(out string failedObject)
        {
            failedObject = "Unknown";
            if (m_camera == null)
            {
                failedObject = "m_camera";
                return false;
            }
            if (m_cameraData == null)
            {
                failedObject = "m_cameraData";
                return false;
            }
            if (m_reflectionProbeManager == null)
            {
                failedObject = "m_reflectionProbeManager";
                return false;
            }
            if (m_sunRotationObject == null)
            {
                failedObject = "m_sunRotationObject";
                return false;
            }
            if (m_sunLight == null)
            {
                failedObject = "m_sunLight";
                return false;
            }
            if (m_sunLightData == null)
            {
                failedObject = "m_sunLightData";
                return false;
            }
            if (m_moonLight == null)
            {
                failedObject = "m_moonLight";
                return false;
            }
            if (m_moonLightData == null)
            {
                failedObject = "m_moonLightData";
                return false;
            }
            if (m_timeOfDayVolume == null)
            {
                failedObject = "m_timeOfDayVolume";
                return false;
            }
            if (m_timeOfDayVolumeProfile == null)
            {
                failedObject = "m_timeOfDayVolumeProfile";
                return false;
            }
            if (m_visualEnvironment == null)
            {
                failedObject = "m_visualEnvironment";
                return false;
            }
            if (m_physicallyBasedSky == null)
            {
                failedObject = "m_physicallyBasedSky";
                return false;
            }
            if (m_cloudLayer == null)
            {
                failedObject = "m_cloudLayer";
                return false;
            }
            if (m_volumetricClouds == null)
            {
                failedObject = "m_volumetricClouds";
                return false;
            }
            if (m_globalIllumination == null)
            {
                failedObject = "m_globalIllumination";
                return false;
            }
            if (m_fog == null)
            {
                failedObject = "m_fog";
                return false;
            }
            if (m_localVolumetricFog == null)
            {
                failedObject = "m_localVolumetricFog";
                return false;
            }
            if (m_exposure == null)
            {
                failedObject = "m_exposure";
                return false;
            }
            if (m_screenSpaceReflection == null)
            {
                failedObject = "m_screenSpaceReflection";
                return false;
            }
            if (m_screenSpaceRefraction == null)
            {
                failedObject = "m_screenSpaceRefraction";
                return false;
            }
            if (m_contactShadows == null)
            {
                failedObject = "m_contactShadows";
                return false;
            }
            if (m_microShadowing == null)
            {
                failedObject = "m_microShadowing";
                return false;
            }
            if (m_shadows == null)
            {
                failedObject = "m_shadows";
                return false;
            }
            if (m_indirectLightingController == null)
            {
                failedObject = "m_indirectLightingController";
                return false;
            }
            if (m_timeOfDayPostFXVolume == null)
            {
                failedObject = "m_timeOfDayPostFXVolume";
                return false;
            }
            if (m_timeOfDayPostFXVolumeProfile == null)
            {
                failedObject = "m_timeOfDayPostFXVolumeProfile";
                return false;
            }
            if (m_colorAdjustments == null)
            {
                failedObject = "m_colorAdjustments";
                return false;
            }
            if (m_whiteBalance == null)
            {
                failedObject = "m_whiteBalance";
                return false;
            }
            if (m_bloom == null)
            {
                failedObject = "m_bloom";
                return false;
            }
            if (m_splitToning == null)
            {
                failedObject = "m_splitToning";
                return false;
            }
            if (m_vignette == null)
            {
                failedObject = "m_vignette";
                return false;
            }
            if (m_ambientOcclusion == null)
            {
                failedObject = "m_ambientOcclusion";
                return false;
            }

            if (m_depthOfField == null)
            {
                failedObject = "m_depthOfField";
                return false;
            }
            if (m_sunLensFlare == null)
            {
                failedObject = "m_sunLensFlare";
                return false;
            }
            if (m_moonLensFlare == null)
            {
                failedObject = "m_moonLensFlare";
                return false;
            }
#if RAY_TRACING_ENABLED
            if (m_rayTracingVolume == null)
            {
                failedObject = "m_rayTracingVolume";
                return false;
            }
            if (m_rayTracedGlobalIllumination == null)
            {
                failedObject = "m_rayTracedGlobalIllumination";
                return false;
            }
            if (m_rayTracedScreenSpaceReflection == null)
            {
                failedObject = "m_rayTracedScreenSpaceReflection";
                return false;
            }
            if (m_rayTracedAmbientOcclusion == null)
            {
                failedObject = "m_rayTracedAmbientOcclusion";
                return false;
            }
            if (m_rayTracedSettings == null)
            {
                failedObject = "m_rayTracedSettings";
                return false;
            }
            if (m_rayTracedRecursiveRendering == null)
            {
                failedObject = "m_rayTracedRecursiveRendering";
                return false;
            }
            if (m_rayTracedSubSurfaceScattering == null)
            {
                failedObject = "m_rayTracedSubSurfaceScattering";
                return false;
            }
#endif

            failedObject = null;
            return true;
        }
        /// <summary>
        /// Sets the sun/moon state based on isDay value
        /// </summary>
        /// <param name="isDay"></param>
        public void SetSunMoonState(bool isDay)
        {
            if (isDay)
            {
                m_sunLight.enabled = true;
                m_sunLightData.enabled = true;
                m_moonLight.enabled = false;
                m_moonLightData.enabled = false;
            }
            else
            {
                m_sunLight.enabled = false;
                m_sunLightData.enabled = false;
                m_moonLight.enabled = true;
                m_moonLightData.enabled = true;
            }
        }
    }

    [ExecuteAlways]
    public class HDRPTimeOfDay : MonoBehaviour
    {
        #region Properties

        public static HDRPTimeOfDay Instance
        {
            get { return m_instance; }
        }
        [SerializeField] private static HDRPTimeOfDay m_instance;

        public Transform Player
        {
            get { return m_player; }
            set
            {
                m_player = value;
                UpdatePlayerTransform();
            }
        }
        [SerializeField] private Transform m_player;

        public HDRPTimeOfDayProfile TimeOfDayProfile
        {
            get { return m_timeOfDayProfile; }
            set
            {
                if (m_timeOfDayProfile != value)
                {
                    m_timeOfDayProfile = value;
                    ProcessTimeOfDay();
                }
            }
        }
        [SerializeField] private HDRPTimeOfDayProfile m_timeOfDayProfile;

        public bool IncrementalUpdate
        {
            get { return m_incrementalUpdate; }
            set
            {
                if (m_incrementalUpdate != value)
                {
                    m_incrementalUpdate = value;
                    m_currentIncrementalFrameCount = 0;
                }
            }
        }
        [SerializeField] private bool m_incrementalUpdate = false;

        public int IncrementalFrameCount
        {
            get { return m_incrementalFrameCount; }
            set
            {
                m_incrementalFrameCount = Mathf.Clamp(value, 1, int.MaxValue);
            }
        }
        [SerializeField] private int m_incrementalFrameCount = 25;

        public bool UsePostFX
        {
            get { return m_usePostFX; }
            set
            {
                if (m_usePostFX != value)
                {
                    m_usePostFX = value;
                    ProcessTimeOfDay();
                    SetPostProcessingActive(value);
                }
            }
        }
        [SerializeField]
        private bool m_usePostFX = true;

        public HDRPTimeOfDayPostFXProfile TimeOfDayPostFxProfile
        {
            get { return m_timeOfDayPostFxProfile; }
            set
            {
                if (m_timeOfDayPostFxProfile != value)
                {
                    m_timeOfDayPostFxProfile = value;
                    ProcessTimeOfDay();
                }
            }
        }
        [SerializeField] private HDRPTimeOfDayPostFXProfile m_timeOfDayPostFxProfile;

        public TimeOfDayDebugSettings DebugSettings
        {
            get { return m_debugSettings; }
            set
            {
                if (m_debugSettings != value)
                {
                    m_debugSettings = value;
                }
            }
        }
        [SerializeField] private TimeOfDayDebugSettings m_debugSettings = new TimeOfDayDebugSettings();

        public HDRPTimeOfDayReflectionProbeProfile ReflectionProbeProfile
        {
            get { return m_reflectionProbeProfile; }
            set
            {
                if (m_reflectionProbeProfile != value)
                {
                    m_reflectionProbeProfile = value;
                    SetReflectionProbeProfile();
                }
            }
        }
        [SerializeField] private HDRPTimeOfDayReflectionProbeProfile m_reflectionProbeProfile;

        public float TimeOfDay
        {
            get { return m_timeOfDay; }
            set
            {
                if (m_timeOfDay != value)
                {
                    m_timeOfDay = Mathf.Clamp(value, 0f, 24f);
                    ProcessTimeOfDay();
                    if (!DebugSettings.m_simulate)
                    {
                        m_savedTODTime = value;
                    }
                }
            }
        }
        [SerializeField] private float m_timeOfDay = 11f;

        public float DirectionY
        {
            get { return m_directionY; }
            set
            {
                if (m_directionY != value)
                {
                    m_directionY = Mathf.Clamp(value, 0f, 360f);
                    ProcessTimeOfDay();
                }
            }
        }
        [SerializeField] private float m_directionY = 0f;

        public bool UseRayTracing
        {
            get { return m_useRayTracing; }
            set
            {
                if (m_useRayTracing != value)
                {
                    m_useRayTracing = value;
                    m_rayTracingSetup = SetupRayTracing(value);
                    if (value)
                    {
                        HDRPTimeOfDayAPI.ValidateRayTracingEnabled();
                    }
                }
            }
        }
        [SerializeField] private bool m_useRayTracing = false;

        public bool RefreshOverrideVolume
        {
            get { return m_refreshOverrideVolume; }
            set
            {
                if (value)
                {
                    m_overrideVolumeData.m_isInVolue = CheckOverrideVolumes();
                    m_overrideVolumeData.m_transitionTime = 0f;
                    RefreshTimeOfDay();
                }
            }
        }
        [SerializeField] private bool m_refreshOverrideVolume = false;

        public bool UseOverrideVolumes
        {
            get { return m_useOverrideVolumes; }
            set
            {
                if (m_useOverrideVolumes != value)
                {
                    m_useOverrideVolumes = value;
                    if (value)
                    {
                        HDRPTimeOfDayOverrideVolumeController controller = GetComponent<HDRPTimeOfDayOverrideVolumeController>();
                        if (controller == null)
                        {
                            controller = gameObject.AddComponent<HDRPTimeOfDayOverrideVolumeController>();
                        }
                        RefreshTimeOfDay();
                        SetupAllOverrideVolumes();
                        controller.CheckState(true);
                    }
                    else
                    {
                        m_overrideVolumeData.m_transitionTime = 0f;
                        m_overrideVolumeData.m_isInVolue = false;
                        HDRPTimeOfDayOverrideVolume[] volumes = FindObjectsOfType<HDRPTimeOfDayOverrideVolume>();
                        if (volumes.Length > 0)
                        {
                            m_overrideVolumeData.m_isInVolue = false;
                            m_overrideVolumeData.m_settings = null;
                            for (int i = 0; i < volumes.Length; i++)
                            {
                                volumes[i].RemoveLocalFogVolume();
                            }
                        }
                    }
                }
            }
        }
        [SerializeField] private bool m_useOverrideVolumes = false;

        public bool AutoOrganizeOverrideVolumes
        {
            get { return m_autoOrganizeOverrideVolumes; }
            set
            {
                if (m_autoOrganizeOverrideVolumes != value)
                {
                    m_autoOrganizeOverrideVolumes = value;
                    if (value)
                    {
                        SortAllOverrideVolumes();
                    }
                }
            }
        }
        [SerializeField] private bool m_autoOrganizeOverrideVolumes = false;

        public bool UseWeatherFX
        {
            get { return m_useWeatherFX; }
            set
            {
                if (m_useWeatherFX != value)
                {
                    m_useWeatherFX = value;
                    if (value)
                    {
                        CheckCloudSettingsForWeather();
                    }
                    else
                    {
                        if (m_weatherIsActive)
                        {
                            if (m_selectedActiveWeatherProfile != -1 && m_selectedActiveWeatherProfile < WeatherProfiles.Count - 1)
                            {
                                StopCurrentWeatherVFX(IsDay, ConvertTimeOfDay(), true);
                            }
                        }
                    }
                }
            }
        }
        [SerializeField] private bool m_useWeatherFX = false;
        public List<HDRPTimeOfDayWeatherProfile> WeatherProfiles
        {
            get { return m_weatherProfiles; }
            set
            {
                if (m_weatherProfiles != value)
                {
                    m_weatherProfiles = value;
                }
            }
        }
        [SerializeField] private List<HDRPTimeOfDayWeatherProfile> m_weatherProfiles = new List<HDRPTimeOfDayWeatherProfile>();

        public bool UseAmbientAudio
        {
            get { return m_useAmbientAudio; }
            set
            {
                if (m_useAmbientAudio != value)
                {
                    m_useAmbientAudio = value;
                    SetupAmbientAudio();
                }
            }
        }
        [SerializeField] private bool m_useAmbientAudio = true;

        public HDRPTimeOfDayAmbientProfile AudioProfile
        {
            get { return m_audioProfile; }
            set
            {
                if (m_audioProfile != value)
                {
                    m_audioProfile = value;
                    SetupAmbientAudio();
                }
            }
        }
        [SerializeField] private HDRPTimeOfDayAmbientProfile m_audioProfile;

        public bool EnableReflectionProbeSync
        {
            get { return m_enableReflectionProbeSync; }
            set
            {
                if (m_enableReflectionProbeSync != value)
                {
                    m_enableReflectionProbeSync = value;
                    SetReflectionProbeSystem();
                }
            }
        }
        [SerializeField] private bool m_enableReflectionProbeSync = true;

        public ProbeRenderMode ProbeRenderMode
        {
            get { return m_probeRenderMode; }
            set
            {
                if (m_probeRenderMode != value)
                {
                    m_probeRenderMode = value;
                    SetReflectionProbeSystem();
                }
            }
        }
        [SerializeField] private ProbeRenderMode m_probeRenderMode = ProbeRenderMode.Sky;

        public float ProbeRenderDistance
        {
            get { return m_probeRenderDistance; }
            set
            {
                if (m_probeRenderDistance != value)
                {
                    m_probeRenderDistance = value;
                    SetReflectionProbeSystem();
                }
            }
        }
        [SerializeField] private float m_probeRenderDistance = 5000f;

        public HDRPTimeOfDaySeasonProfile SeasonProfile
        {
            get { return m_seasonProfile; }
            set
            {
                if (m_seasonProfile != value)
                {
                    m_seasonProfile = value;
                    UpdateSeasons();
                }
            }
        }
        [SerializeField] private HDRPTimeOfDaySeasonProfile m_seasonProfile;

        #endregion
        #region Variables

        public bool m_reflectionProbeSettings = false;
        public bool m_overrideVolumeSettings = false;
        public bool m_lightSourceOverride = false;
        public bool m_enableSeasons = true;
        public bool m_enableTimeOfDaySystem = false;
        public float m_timeOfDayMultiplier = 1f;
        public Vector2 m_randomWeatherTimer = new Vector2(120f, 400f);
        public Dictionary<int, HDRPTimeOfDayOverrideVolume> m_overrideVolumes = new Dictionary<int, HDRPTimeOfDayOverrideVolume>();
        public int m_overrideVolumeCount = -1;
        public bool m_probesRefreshing = false;
        public bool m_resetWeatherShaderProperty = true;
        public bool m_instantWeatherStop = true;
        public bool m_avoidSameRandomWeather = true;
        public CameraSettings m_cameraSettings = new CameraSettings();
        public bool m_ignoreRealtimeGICheck = false;

        //Interior
        public InteriorControllerManagerData m_interiorControllerData = new InteriorControllerManagerData();
        private HDRPTimeOfDayInteriorController m_currentInteriorController;

        [SerializeField] private float m_savedTODTime = 8f;
        [SerializeField] private List<GameObject> m_disableItems = new List<GameObject>();
        [SerializeField] private bool m_rayTracingSetup = false;
        [SerializeField] private HDRPTimeOfDayComponents Components = new HDRPTimeOfDayComponents();
        [SerializeField] private Color m_currentFogColor = Color.white;
        [SerializeField] private float m_currentLocalFogDistance = 100f;
        private int m_currentIncrementalFrameCount = 0;
        //Audio
        [SerializeField] private AudioSource m_ambientSourceA;
        [SerializeField] private AudioSource m_ambientSourceB;
        private bool m_hasBeenSetupCorrectly = false;
        private bool m_validating = false;
        private HDRPTimeOfDayOverrideVolume m_lastOverrideVolume;
        private OverrideDataInfo m_overrideVolumeData = new OverrideDataInfo();
        private bool m_audioInitilized = false;
        //Utils
        [SerializeField] private GameObject m_volumeMasterParent;
        [SerializeField] private GameObject m_volumeDayParent;
        [SerializeField] private GameObject m_volumeNightParent;
        //Weather VFX
        private bool m_weatherIsActive = false;
        private float m_currentRandomWeatherTimer;
        private float m_weatherDurationTimer;
        private float m_currentWeatherTransitionDuration;
        private int m_selectedActiveWeatherProfile = -1;
        private HDRPTimeOfDayWeatherProfile m_selectedWeatherProfileAsset;
        private int m_lastSelectedWeatherProfile = -1;
        private bool m_resetDuration = false;
        private IHDRPWeatherVFX m_weatherVFX;
        public List<IHDRPWeatherVFX> m_additionalWeatherVFX = new List<IHDRPWeatherVFX>();
        private float m_lastCloudLayerOpacity;
        private int m_currentValidateCheckerFrames = 0;
        private float m_audioBlendTimer;
        private bool m_weatherVFXStillPlaying = false;

        //Important Current Values
        [SerializeField] private float CurrentTime;
        [SerializeField] private bool IsDay;

        private const string ComponentsPrefabName = "Time Of Day Components.prefab";
        private const string TimeOfDayDefaultsProfileName = "Defaults Profile.asset";
        private const int ValidateCheckerFrameLimit = 1000;

        #endregion
        #region Unity Functions

        private void OnEnable()
        {
            m_currentWeatherTransitionDuration = 0f;
            m_hasBeenSetupCorrectly = false;
            m_instance = this;
            StopSimulate();
            FixCameraRenderDistance();
            SetupAmbientAudio();
            m_lightSourceOverride = false;
            m_hasBeenSetupCorrectly = SetupHDRPTimeOfDay();
            if (UseRayTracing)
            {
                m_rayTracingSetup = SetupRayTracing(UseRayTracing);
                ApplyRayTracingSettings();
            }
            else
            {
                DisableRayTraceVolume();
            }

            if (UseOverrideVolumes)
            {
                SetupAllOverrideVolumes();
            }

            if (m_hasBeenSetupCorrectly)
            {
                m_cameraSettings.ApplyCameraSettings(Components.m_cameraData, Components.m_camera);
                ResetStarsRotaion(m_timeOfDayProfile.TimeOfDayData);
                SetRefreshMode();
                ProcessTimeOfDay();
            }

            if (TimeOfDayPostFxProfile != null)
            {
                TimeOfDayPostFxProfile.TimeOfDayPostFXData.SetDepthOfFieldQuality(Components.m_depthOfField);
            }

            if (m_seasonProfile != null)
            {
                m_seasonProfile.Setup();
            }

            SetupInteriorSystems();

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
            else
            {
                EditorApplication.update -= EditorUpdate;
            }
#endif
        }
        private void OnDisable()
        {
            if (m_seasonProfile != null)
            {
                m_seasonProfile.Reset();
            }

            if (m_weatherProfiles.Count > 0 && m_resetWeatherShaderProperty)
            {
                foreach (HDRPTimeOfDayWeatherProfile profile in m_weatherProfiles)
                {
#if GTS_PRESENT
                    if (profile.WeatherData.m_weatherName.Contains("Sand"))
                    {
                        WeatherShaderManager.ResetAllWeatherPowerValues(profile.WeatherShaderData);
                    }
#else
                    WeatherShaderManager.ResetAllWeatherPowerValues(profile.WeatherShaderData);
#endif
                }
            }
        }
        private void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!m_hasBeenSetupCorrectly && !m_validating)
            {
                m_currentValidateCheckerFrames = 0;
                m_hasBeenSetupCorrectly = SetupHDRPTimeOfDay();
                StartCoroutine(ValidationChecker());
            }

            IsDay = IsDayTime();
            if (m_enableTimeOfDaySystem)
            {
                UpdateTime(IsDay, m_timeOfDayProfile.TimeOfDayData);
            }

            CurrentTime = ConvertTimeOfDay();
            if (UseAmbientAudio && m_audioProfile != null)
            {
                if (m_audioBlendTimer < 1f && m_audioInitilized)
                {
                    m_audioBlendTimer += Time.deltaTime / 3f;
                }
                else
                {
                    m_audioBlendTimer = 1f;
                }

                if (m_audioProfile.ProcessAmbientAudio(m_ambientSourceA, m_ambientSourceB, CurrentTime, m_audioBlendTimer, m_audioInitilized, this))
                {
                    m_audioBlendTimer = 0f;
                }

                m_audioInitilized = true;
            }

            if (m_weatherIsActive)
            {
                ProcessWeather();
                m_selectedWeatherProfileAsset.UnderwaterOverrideData.ApplySettings(CurrentTime);
                m_currentWeatherTransitionDuration = m_weatherVFX.GetCurrentDuration();
            }
            else
            {
                m_timeOfDayProfile.UnderwaterOverrideData.ApplySettings(CurrentTime);
                if (UseWeatherFX)
                {
                    CheckAutoWeather();
                }

                if (UseOverrideVolumes)
                {
                    m_overrideVolumeData.m_isInVolue = CheckOverrideVolumes();
                    if (m_overrideVolumeData.m_transitionTime < 1f)
                    {
                        if (m_lastOverrideVolume != null)
                        {
                            m_overrideVolumeData.m_transitionTime += Time.deltaTime / m_lastOverrideVolume.m_volumeSettings.m_blendTime;
                        }
                        else
                        {
                            m_overrideVolumeData.m_transitionTime += Time.deltaTime / 3f;
                        }
                    }
                }
                else
                {
                    if (m_overrideVolumeData.m_transitionTime < 1f)
                    {
                        m_overrideVolumeData.m_transitionTime += Time.deltaTime / 3f;
                    }
                }

                if (m_overrideVolumeData.m_transitionTime < 1f)
                {
                    ProcessTimeOfDay();
                }
            }

            UpdateStars(IsDay, CurrentTime, m_timeOfDayProfile.TimeOfDayData);
            UpdateSeasons();
            CheckInteriorControllers();
        }

        #endregion
        #region Public Functions

        /// <summary>
        /// Sets the current incrimental frame value this is useful when you want to override the frame value
        /// </summary>
        /// <param name="value"></param>
        public void SetCurrentIncrimentalFrameValue(int value)
        {
            m_currentIncrementalFrameCount = value;
        }
        /// <summary>
        /// Shows the debug log if it's enabled
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void ShowDebugLog(string message, DebugLogType type)
        {
            if (!DebugSettings.m_showDebugLogs)
            {
                return;
            }

            switch (type)
            {
                case DebugLogType.Log:
                {
                    Debug.Log(message);
                    break;
                }
                case DebugLogType.Warning:
                {
                    Debug.LogWarning(message);
                    break;
                }
                case DebugLogType.Error:
                {
                    Debug.LogError(message);
                    break;
                }
            }
        }
        /// <summary>
        /// Gets the TOD components data block
        /// </summary>
        /// <returns></returns>
        public HDRPTimeOfDayComponents GetTODComponents()
        {
            return Components;
        }
        /// <summary>
        /// Sets the probe system to refresh this checks to see if the baked probe needs to be changed
        /// </summary>
        public void UpdateReflectionProbeSystem()
        {
            Components.m_reflectionProbeManager.Refresh();
        }
        /// <summary>
        /// Returns the bool if weather is currently active
        /// </summary>
        /// <returns></returns>
        public bool WeatherActive()
        {
            return m_weatherIsActive;
        }
        /// <summary>
        /// Returns the bool if weather is currently active
        /// Also outs the active profile asset
        /// </summary>
        /// <param name="activeWeatherProfileAsset"></param>
        /// <returns></returns>
        public bool WeatherActive(out HDRPTimeOfDayWeatherProfile activeWeatherProfileAsset)
        {
            activeWeatherProfileAsset = null;
            if (m_weatherIsActive)
            {
                activeWeatherProfileAsset = m_selectedWeatherProfileAsset;
            }

            return m_weatherIsActive;
        }
        /// <summary>
        /// Starts weather effect with the weather profile index selected
        /// </summary>
        /// <param name="weatherProfile"></param>
        public void StartWeather(int weatherProfile)
        {
            m_selectedActiveWeatherProfile = weatherProfile;
            m_selectedWeatherProfileAsset = m_weatherProfiles[m_selectedActiveWeatherProfile];
            m_currentRandomWeatherTimer = 0f;
            if (weatherProfile < 0 || weatherProfile <= m_weatherProfiles.Count - 1)
            {
                if (m_selectedWeatherProfileAsset == null)
                {
                    ShowDebugLog("Weather profile at " + weatherProfile + " is null.", DebugLogType.Error);
                    return;
                }

                m_weatherVFXStillPlaying = true;
                m_weatherIsActive = true;
                m_weatherDurationTimer = UnityEngine.Random.Range(m_selectedWeatherProfileAsset.WeatherData.m_weatherDuration.x, m_selectedWeatherProfileAsset.WeatherData.m_weatherDuration.y);
                WeatherEffectsData data = m_selectedWeatherProfileAsset.WeatherFXData;
                m_weatherVFX = HDRPTimeOfDayWeatherProfile.GetInterface(Instantiate(data.m_weatherEffect));
                data.SetupAdditionalEffectsCopy();
                if (data.m_additionalEffectsCopy.Count > 0)
                {
                    m_additionalWeatherVFX.Clear();
                    if (data.m_randomizeAdditionalEffects)
                    {
                        data.RandomizeEffects();
                    }

                    foreach (HDRPWeatherAdditionalEffects additionalEffect in data.m_additionalEffectsCopy)
                    {
                        if (additionalEffect.m_active)
                        {
                            if (data.ValidateAdditionalEffect(additionalEffect))
                            {
                                GameObject additionalGameObject = Instantiate(additionalEffect.m_effect);
                                additionalEffect.ApplyGlobalAudioEffect(additionalGameObject);
                                IHDRPWeatherVFX additionalEffectInterface = HDRPTimeOfDayWeatherProfile.GetInterface(additionalGameObject);
                                if (additionalEffectInterface != null)
                                {
                                    additionalEffectInterface.StartWeatherFX(m_selectedWeatherProfileAsset);
                                    m_additionalWeatherVFX.Add(additionalEffectInterface);
                                }
                            }
                        }
                    }
                }

                if (m_weatherVFX != null)
                {
                    TimeOfDayProfileData todData = new TimeOfDayProfileData();
                    TimeOfDayProfileData.CopySettings(todData, m_timeOfDayProfile.TimeOfDayData);
                    m_selectedWeatherProfileAsset.WeatherData.SetupStartingSettings(todData, CurrentTime);
                    m_weatherVFX.StartWeatherFX(m_selectedWeatherProfileAsset);

                    if (m_timeOfDayProfile.UnderwaterOverrideData.m_systemType == UnderwaterOverrideSystemType.Gaia)
                    {
                        List<VisualEffect> vfxToAdd = m_weatherVFX.CanBeControlledByUnderwater();
                        foreach (IHDRPWeatherVFX weatherVfx in m_additionalWeatherVFX)
                        {
                            vfxToAdd.AddRange(weatherVfx.CanBeControlledByUnderwater());
                        }

                        m_interiorControllerData.AssignVisualEffects(vfxToAdd);
                        if (m_interiorControllerData.m_refreshControllerSystemsOnWeatherStart)
                        {
                            SetupInteriorSystems();
                        }
#if GAIA_PRO_PRESENT
                        Gaia.GaiaUnderwaterEffects underwaterEffects = GaiaUnderwaterEffects.Instance;
                        if (underwaterEffects != null)
                        {
                            underwaterEffects.m_surfaceVisualEffects.AddRange(vfxToAdd);
                        }
#endif
                    }
                }

                m_resetDuration = true;
            }
        }
        /// <summary>
        /// Stops the current weather effect
        /// </summary>
        public void StopWeather(bool instant = true)
        {
            if (instant)
            {
                StopCurrentWeatherVFX(IsDay, CurrentTime, instant);
            }
            else
            {
                m_weatherDurationTimer = -1f;
            }

#if GTS_PRESENT
            if (GTS.GTSTerrain.activeTerrain == null)
            {
                if (m_selectedWeatherProfileAsset != null && m_resetWeatherShaderProperty)
                {
                    WeatherShaderManager.ResetAllWeatherPowerValues(m_selectedWeatherProfileAsset.WeatherShaderData);
                }
            }
#endif
        }
        /// <summary>
        /// Refreshes gaia systems
        /// </summary>
        public void RefreshGaiaSystems()
        {
#if GAIA_PRO_PRESENT
            GaiaPlanarReflectionsHDRP reflectionsHdrp = GaiaPlanarReflectionsHDRP.Instance;
            if (reflectionsHdrp != null)
            {
                reflectionsHdrp.RequestRender = true;
            }
#endif
        }
        /// <summary>
        /// This will refresh all the override voluems and sort them according to being day or night;
        /// </summary>
        public void SortAllOverrideVolumes()
        {
            HDRPTimeOfDayOverrideVolume[] volumes = FindObjectsOfType<HDRPTimeOfDayOverrideVolume>();
            if (volumes.Length < 1)
            {
                return;
            }

            SetupOverrideVolumeOrganize();
            foreach (HDRPTimeOfDayOverrideVolume overrideVolume in volumes)
            {
                ParentOverrideVolume(overrideVolume, false);
            }
        }
        /// <summary>
        /// Parents the override volume to day or night gameobject based on it's type
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="checkSetup"></param>
        public void ParentOverrideVolume(HDRPTimeOfDayOverrideVolume volume, bool checkSetup = true)
        {
            if (checkSetup)
            {
                SetupOverrideVolumeOrganize();
            }

            if (volume != null)
            {
                switch (volume.m_volumeSettings.m_volumeType)
                {
                    case OverrideTODType.Day:
                    {
                        if (m_volumeDayParent != null)
                        {
                            volume.transform.SetParent(m_volumeDayParent.transform);
                        }
                        break;
                    }
                    case OverrideTODType.Night:
                    {
                        if (m_volumeNightParent != null)
                        {
                            volume.transform.SetParent(m_volumeNightParent.transform);
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Assigns the controller into the active controllers if it's not there
        /// </summary>
        /// <param name="controller"></param>
        public void AddInteriorController(HDRPTimeOfDayInteriorController controller)
        {
            if (controller != null)
            {
                m_interiorControllerData.AddController(controller);
            }
        }
        /// <summary>
        /// Removes the controller from the active controllers if it is there
        /// </summary>
        /// <param name="controller"></param>
        public void RemoveInteriorController(HDRPTimeOfDayInteriorController controller)
        {
            if (controller != null)
            {
                m_interiorControllerData.RemoveController(controller);
            }
        }
        /// <summary>
        /// Gets the current fog color value
        /// </summary>
        /// <returns></returns>
        public Color GetCurrentFogColor()
        {
            return m_currentFogColor;
        }
        /// <summary>
        /// Gets the current local fog distance value
        /// </summary>
        /// <returns></returns>
        public float GetCurrentLocalFogDistance()
        {
            return m_currentLocalFogDistance;
        }
        /// <summary>
        /// Resets the override volume blend time
        /// </summary>
        public void ResetOverrideVolumeBlendTime(bool resetIsInVolume)
        {
            m_overrideVolumeData.m_transitionTime = 0f;
            if (resetIsInVolume)
            {
                m_overrideVolumeData.m_isInVolue = false;
                m_overrideVolumeData.m_settings = null;
                m_lastOverrideVolume = null;
            }
        }
        /// <summary>
        /// Sets up time of day
        /// </summary>
        public bool SetupHDRPTimeOfDay()
        {
            m_currentRandomWeatherTimer = UnityEngine.Random.Range(m_randomWeatherTimer.x, m_randomWeatherTimer.y);

            if (m_timeOfDayProfile == null)
            {
                return false;
            }

            bool successful = BuildVolumesAndCollectComponents();
            UpdatePlayerTransform();
            SetupVisualEnvironment();
            return successful;
        }
        /// <summary>
        /// The function that processes the time of day
        /// </summary>
        /// <param name="hasBeenSetup"></param>
        public void ProcessTimeOfDay()
        {
            if (!m_hasBeenSetupCorrectly)
            {
                return;
            }

            if (!Application.isPlaying)
            {
                if (Components == null)
                {
                    ShowDebugLog("HDRP Time Of Day components is null.", DebugLogType.Error);
                    return;
                }
                else
                {
                    if (!Components.Validated(out string component))
                    {
                        if (!string.IsNullOrEmpty(component))
                        {
                            ShowDebugLog("HDRP Time Of Day components validate failed because " + component + " was null", DebugLogType.Error);
                        }
                        else
                        {
                            ShowDebugLog("HDRP Time Of Day components validate failed.", DebugLogType.Error);
                        }

                        return;
                    }
                }
            }
            else
            {
                if (IncrementalUpdate && m_enableTimeOfDaySystem)
                {
                    if (m_currentIncrementalFrameCount >= m_incrementalFrameCount)
                    {
                        m_currentIncrementalFrameCount = 0;
                    }
                    else
                    {
                        m_currentIncrementalFrameCount++;
                        return;
                    }
                }
            }

            CurrentTime = ConvertTimeOfDay();
            //This is used to evaluate systems that can range from 0-1
            UpdateSunRotation(CurrentTime);

            IsDay = IsDayTime();
            if (!m_weatherIsActive)
            {
                //Process TOD
                UpdateSun(CurrentTime, IsDay, m_lightSourceOverride, m_timeOfDayProfile.TimeOfDayData);
                UpdateAdvancedLighting(CurrentTime, IsDay, m_timeOfDayProfile.TimeOfDayData);
                UpdateFog(CurrentTime, m_timeOfDayProfile.TimeOfDayData);
                UpdateShadows(IsDay, CurrentTime, m_timeOfDayProfile.TimeOfDayData);
                UpdateClouds(CurrentTime, m_timeOfDayProfile.TimeOfDayData);
                UpdateLensFlare(CurrentTime, m_timeOfDayProfile.TimeOfDayData, IsDay);
            }
            else
            {
                ProcessWeather();
            }

            //Process Post FX
            if (UsePostFX)
            {
                if (TimeOfDayPostFxProfile != null)
                {
                    UpdateAmbientOcclusion(CurrentTime, TimeOfDayPostFxProfile.TimeOfDayPostFXData);
                    UpdateColorGrading(CurrentTime, TimeOfDayPostFxProfile.TimeOfDayPostFXData);
                    UpdateBloom(CurrentTime, TimeOfDayPostFxProfile.TimeOfDayPostFXData);
                    UpdateShadowToning(CurrentTime, TimeOfDayPostFxProfile.TimeOfDayPostFXData);
                    UpdateVignette(CurrentTime, TimeOfDayPostFxProfile.TimeOfDayPostFXData);
                    UpdateDepthOfField(CurrentTime, TimeOfDayPostFxProfile.TimeOfDayPostFXData);
                }
            }

            UpdateReflectionProbeSystem();
            RefreshGaiaSystems();
        }
        /// <summary>
        /// Processes the active weather profile
        /// </summary>
        public void ProcessWeather(bool overrideApply = false)
        {
            if (m_selectedWeatherProfileAsset == null || m_weatherVFX == null)
            {
                return;
            }

            if (m_weatherDurationTimer > 0f)
            {
                if (m_selectedWeatherProfileAsset.WeatherData.ApplyWeather(Components, IsDay, m_lightSourceOverride, CurrentTime, m_weatherVFX.GetCurrentDuration()))
                {
                    m_weatherDurationTimer -= Time.deltaTime;
                    if (m_enableTimeOfDaySystem || overrideApply)
                    {
                        TimeOfDayProfileData WeatherData = m_selectedWeatherProfileAsset.WeatherData.m_weatherData;
                        UpdateSun(CurrentTime, IsDay, m_lightSourceOverride, WeatherData);
                        UpdateAdvancedLighting(CurrentTime, IsDay, WeatherData);
                        UpdateFog(CurrentTime, WeatherData);
                        UpdateShadows(IsDay, CurrentTime, WeatherData);
                        UpdateClouds(CurrentTime, WeatherData);
                        UpdateLensFlare(CurrentTime, WeatherData, IsDay);
                        if (WeatherShaderManager.AllowGlobalShaderUpdates)
                        {
                            WeatherShaderManager.ApplyAllShaderValues(m_selectedWeatherProfileAsset.WeatherShaderData);
                        }
                    }
                }
            }
            else
            {
                WeatherShaderManager.AllowGlobalShaderUpdates = false;
                StopCurrentWeatherVFX(IsDay, CurrentTime);
            }
        }
        /// <summary>
        /// This function calls update all light values on either the sun or moon depending if it's day or night time
        /// </summary>
        public void RefreshLights(bool refreshIsDayCheck = false)
        {
            if (refreshIsDayCheck)
            {
                IsDay = IsDayTime();
            }
            StartCoroutine(RefreshSunSettings());
        }
        /// <summary>
        /// Returns true if the duration value for current vfx is >= 1
        /// </summary>
        /// <returns></returns>
        public bool HasWeatherTransitionCompleted()
        {
            if (m_weatherIsActive)
            {
                return m_weatherVFX.GetCurrentDuration() >= 1f;
            }
            return false;
        }
        /// <summary>
        /// Returns true if the duration value for current vfx is >= 1
        /// Returns 10 steps where you can add effects, apply settings in steps.
        /// Example of this is used in the reflection probe system to make a transition using weather intenisty multiplier
        /// </summary>
        /// <returns></returns>
        public int HasWeatherTransitionCompletedSteps(out bool isPlayingVFX)
        {
            isPlayingVFX = m_weatherVFXStillPlaying;
            int stepID = -1;
            if (m_weatherIsActive)
            {
                float value = m_weatherVFX.GetCurrentDuration();
                if (value >= 0f && value <= 0.1f)
                {
                    stepID = 0;
                }
                else if (value >= 0.1f && value <= 0.2f)
                {
                    stepID = 1;
                }
                else if (value >= 0.2f && value <= 0.3f)
                {
                    stepID = 2;
                }
                else if (value >= 0.3f && value <= 0.4f)
                {
                    stepID = 3;
                }
                else if (value >= 0.4f && value <= 0.5f)
                {
                    stepID = 4;
                }
                else if (value >= 0.5f && value <= 0.6f)
                {
                    stepID = 5;
                }
                else if (value >= 0.6f && value <= 0.7f)
                {
                    stepID = 6;
                }
                else if (value >= 0.7f && value <= 0.8f)
                {
                    stepID = 7;
                }
                else if (value >= 0.8f && value <= 0.9f)
                {
                    stepID = 8;
                }
                else
                {
                    stepID = 9;
                }
            }

            return stepID;
        }
        /// <summary>
        /// Logs what the time value is on animation curves and gradient fields
        /// This is to help you fine tune the times of day
        /// </summary>
        public void GetDebugInformation()
        {
            float currentTime = ConvertTimeOfDay();
            if (DebugSettings.m_roundUp)
            {
                Debug.Log("Animation Curve time is ranged from 0-1 and the current value at " + TimeOfDay +
                          " Time Of Day is " + currentTime.ToString("n2"));
                Debug.Log("Gradients time is ranged from 0-100% and the current value at " + TimeOfDay +
                          " Time Of Day is " + Mathf.FloorToInt(currentTime * 100f) + "%");
            }
            else
            {
                Debug.Log("Animation Curve time is ranged from 0-1 and the current value at " + TimeOfDay +
                          " Time Of Day is " + currentTime);
                Debug.Log("Gradients time is ranged from 0-100% and the current value at " + TimeOfDay +
                          " Time Of Day is " + currentTime * 100f + "%");
            }
        }
        /// <summary>
        /// Checks to see if the components and systems have been setup correctly
        /// </summary>
        /// <returns></returns>
        public bool HasBeenSetup()
        {
            return m_hasBeenSetupCorrectly;
        }
        /// <summary>
        /// Manually set the systems has been setup with the bool value
        /// </summary>
        /// <param name="value"></param>
        public void SetHasBeenSetup(bool value)
        {
            m_hasBeenSetupCorrectly = value;
        }
        /// <summary>
        /// Refreshes time of day settings and if you set check setup to true
        /// It will process all the components to make sure everythign is setup correctly
        /// </summary>
        /// <param name="checkSetup"></param>
        public void RefreshTimeOfDay(bool checkSetup = false)
        {
            if (checkSetup)
            {
                m_hasBeenSetupCorrectly = SetupHDRPTimeOfDay();
            }

            if (!m_hasBeenSetupCorrectly)
            {
                return;
            }

            ProcessTimeOfDay();
        }
        /// <summary>
        /// Checks to see if it's day time if retuns false then it's night time
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool IsDayTime()
        {
            return CalculateHorizon();
        }
        /// <summary>
        /// Applies visual environment settings
        /// </summary>
        public void SetupVisualEnvironment()
        {
            if (Components.m_visualEnvironment != null)
            {
                Components.m_visualEnvironment.skyAmbientMode.value = SkyAmbientMode.Dynamic;
                if (Components.m_volumetricClouds != null && Components.m_cloudLayer != null)
                {
                    switch (m_timeOfDayProfile.TimeOfDayData.m_globalCloudType)
                    {
                        case GlobalCloudType.Volumetric:
                        {
                            Components.m_visualEnvironment.cloudType.value = 0;
                            Components.m_volumetricClouds.enable.value = true;
                            Components.m_cloudLayer.opacity.value = 0f;
                            break;
                        }
                        case GlobalCloudType.Procedural:
                        {
                            Components.m_visualEnvironment.cloudType.value = (int) CloudType.CloudLayer;
                            Components.m_volumetricClouds.enable.value = false;
                            Components.m_cloudLayer.opacity.value = m_lastCloudLayerOpacity;
                            break;
                        }
                        case GlobalCloudType.Both:
                        {
                            Components.m_visualEnvironment.cloudType.value = (int) CloudType.CloudLayer;
                            Components.m_volumetricClouds.enable.value = true;
                            Components.m_cloudLayer.opacity.value = m_lastCloudLayerOpacity;
                            break;
                        }
                        case GlobalCloudType.None:
                        {
                            Components.m_visualEnvironment.cloudType.value = 0;
                            Components.m_volumetricClouds.enable.value = false;
                            Components.m_cloudLayer.opacity.value = 0f;
                            break;
                        }
                    }
                }

                if (Components.m_physicallyBasedSky != null)
                {
                    switch (m_timeOfDayProfile.TimeOfDayData.m_skyMode)
                    {
                        case TimeOfDaySkyMode.PhysicallyBased:
                        {
                            Components.m_visualEnvironment.skyType.value = (int)SkyType.PhysicallyBased;
                            Components.m_physicallyBasedSky.active = true;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Sets the skybox exposure value
        /// </summary>
        /// <param name="value"></param>
        public void SetSkySettings(float value, Color color)
        {
            if (Components.m_physicallyBasedSky != null)
            {
                Components.m_physicallyBasedSky.exposure.value = value;
                Components.m_physicallyBasedSky.groundTint.value = color;
            }
        }
        /// <summary>
        /// Refreshes the physically based or gradient sky to update the to the current lighting
        /// </summary>
        public void SetRefreshMode()
        {
            Components.m_physicallyBasedSky.updateMode.value = EnvironmentUpdateMode.OnChanged;
        }
        /// <summary>
        /// Sets the static singleton instance
        /// </summary>
        /// <param name="timeOfDay"></param>
        public void SetStaticInstance(HDRPTimeOfDay timeOfDay)
        {
            m_instance = timeOfDay;
        }
        /// <summary>
        /// Adds a gameobject item that has been disabled by this system
        /// </summary>
        /// <param name="disableObject"></param>
        public bool AddDisabledItem(GameObject disableObject)
        {
            if (!m_disableItems.Contains(disableObject))
            {
                if (IsNotTODVolume(disableObject))
                {
                    m_disableItems.Add(disableObject);
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Re-enables all the disable objects that have been added to the system
        /// </summary>
        public void EnableAllDisabledItems()
        {
            if (m_disableItems.Count > 0)
            {
                foreach (GameObject disableItem in m_disableItems)
                {
                    if (disableItem != null)
                    {
                        disableItem.SetActive(true);
                    }
                }
            }
        }
        /// <summary>
        /// Applies ray tracing settings
        /// </summary>
        public void ApplyRayTracingSettings()
        {
            if (m_rayTracingSetup)
            {
#if RAY_TRACING_ENABLED
                //SSGI
                Components.m_rayTracedGlobalIllumination.active = m_timeOfDayProfile.RayTracingSettings.m_rayTraceSSGI;
                switch (m_timeOfDayProfile.RayTracingSettings.m_ssgiRenderMode)
                {
                    case GeneralRenderMode.Performance:
                    {
                        Components.m_rayTracedGlobalIllumination.mode.value = RayTracingMode.Performance;
                        break;
                    }
                    case GeneralRenderMode.Quality:
                    {
                        Components.m_rayTracedGlobalIllumination.mode.value = RayTracingMode.Quality;
                        break;
                    }
                }
                Components.m_rayTracedGlobalIllumination.quality.value = (int)m_timeOfDayProfile.RayTracingSettings.m_ssgiQuality;
                //SSR
                Components.m_rayTracedScreenSpaceReflection.active = m_timeOfDayProfile.RayTracingSettings.m_rayTraceSSR;
                switch (m_timeOfDayProfile.RayTracingSettings.m_ssrRenderMode)
                {
                    case GeneralRenderMode.Performance:
                    {
                        Components.m_rayTracedScreenSpaceReflection.mode.value = RayTracingMode.Performance;
                        break;
                    }
                    case GeneralRenderMode.Quality:
                    {
                        Components.m_rayTracedScreenSpaceReflection.mode.value = RayTracingMode.Quality;
                        break;
                    }
                }
                Components.m_rayTracedScreenSpaceReflection.quality.value = (int)m_timeOfDayProfile.RayTracingSettings.m_ssrQuality;
                //AO
                Components.m_rayTracedAmbientOcclusion.active = m_timeOfDayProfile.RayTracingSettings.m_rayTraceAmbientOcclusion;
                Components.m_rayTracedAmbientOcclusion.quality.value = (int)m_timeOfDayProfile.RayTracingSettings.m_aoQuality;
                //Recursive Rendering
                Components.m_rayTracedRecursiveRendering.active = m_timeOfDayProfile.RayTracingSettings.m_recursiveRendering;
                //SSS
                Components.m_rayTracedSubSurfaceScattering.active = m_timeOfDayProfile.RayTracingSettings.m_rayTraceSubSurfaceScattering;
                Components.m_rayTracedSubSurfaceScattering.sampleCount.value = (int)m_timeOfDayProfile.RayTracingSettings.m_subSurfaceScatteringSampleCount;
#endif
            }
        }
        /// <summary>
        /// Adds an override volume
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        public bool AddOverrideVolume(int id, HDRPTimeOfDayOverrideVolume volume)
        {
            if (!m_overrideVolumes.ContainsKey(id))
            {
                m_overrideVolumes.Add(id, volume);
            }

            return true;
        }
        /// <summary>
        /// Removes an override volume
        /// </summary>
        /// <param name="id"></param>
        public bool RemoveOverrideVolume(int id)
        {
            if (m_overrideVolumes.ContainsKey(id))
            {
                m_overrideVolumes.Remove(id);
            }

            return false;
        }
        /// <summary>
        /// Sets up the override volumes in the scene
        /// </summary>
        public void SetupAllOverrideVolumes()
        {
            HDRPTimeOfDayOverrideVolume[] volumes = FindObjectsOfType<HDRPTimeOfDayOverrideVolume>();
            if (volumes.Length > 0)
            {
                m_overrideVolumeData.m_isInVolue = false;
                m_overrideVolumeData.m_settings = null;
                for (int i = 0; i < volumes.Length; i++)
                {
                    volumes[i].Setup(i);
                    volumes[i].SetupVolumeTypeToController();
                    volumes[i].ApplyLocalFogVolume(false);
                }
            }
        }
        /// <summary>
        /// Gets the current override data
        /// </summary>
        /// <returns></returns>
        public OverrideDataInfo GetCurrentOverrideData()
        {
            return m_overrideVolumeData;
        }
        /// <summary>
        /// Overrides the current light source settings this can be used to simulate lighting for example
        /// </summary>
        /// <param name="temperature"></param>
        /// <param name="tint"></param>
        /// <param name="intensity"></param>
        /// <param name="reset"></param>
        public void OverrideLightSource(float temperature, Color tint, float intensity, LightShadows shadows, bool reset = true, bool resetOnly = false)
        {
            if (resetOnly && reset)
            {
                m_lightSourceOverride = false;
                if (m_weatherIsActive)
                {
                    ProcessWeather();
                }
                else
                {
                    ProcessTimeOfDay();
                }
                return;
            }

            m_lightSourceOverride = true;
            bool isDay = IsDayTime();
            if (isDay)
            {
                if (Components.m_sunLightData != null)
                {
                    Components.m_sunLightData.SetColor(tint, temperature);
                    Components.m_sunLightData.SetIntensity(intensity);
                    Components.m_sunLight.shadows = shadows;
                }
            }
            else
            {
                if (Components.m_moonLightData != null)
                {
                    Components.m_moonLightData.SetColor(tint, temperature);
                    Components.m_moonLightData.SetIntensity(intensity);
                    Components.m_moonLight.shadows = shadows;
                }
            }

            if (reset)
            {
                m_lightSourceOverride = false;
                if (m_weatherIsActive)
                {
                    ProcessWeather();
                }
                else
                {
                    ProcessTimeOfDay();
                }
            }
        }
        /// <summary>
        /// Converts the time of day float from 24 = 0-1
        /// </summary>
        /// <returns></returns>
        public float ConvertTimeOfDay()
        {
            return TimeOfDay / 24f;
        }
        /// <summary>
        /// Plays an audio effect
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="source"></param>
        /// <param name="oneShot"></param>
        /// <param name="volume"></param>
        public void PlaySoundFX(AudioClip clip, AudioSource source, bool oneShot, float volume)
        {
            if (source != null && clip != null)
            {
                source.clip = clip;
                source.volume = volume;
                if (oneShot)
                {
                    source.PlayOneShot(clip, volume);
                }
                else
                {
                    if (!source.isPlaying)
                    {
                        source.Play();
                    }
                }
            }
        }
        /// <summary>
        /// Stops the weather effect
        /// If instant is set to true it will disable right away
        /// </summary>
        /// <param name="weatherProfile"></param>
        /// <param name="isDay"></param>
        /// <param name="currentTime"></param>
        /// <param name="instant"></param>
        public void StopCurrentWeatherVFX(bool isDay, float currentTime, bool instant = false)
        {
            if (m_weatherVFX == null)
            {
                ShowDebugLog("Weather VFX system could not be found", DebugLogType.Error);
                return;
            }

            m_weatherVFXStillPlaying = false;
            if (m_resetDuration)
            {
                if (m_currentWeatherTransitionDuration >= 1f)
                {
                    m_currentWeatherTransitionDuration = 0f;
                }
                else
                {
                    m_currentWeatherTransitionDuration = 1f - m_currentWeatherTransitionDuration;
                }
                m_resetDuration = false;
                TimeOfDayProfileData todData = new TimeOfDayProfileData();
                TimeOfDayProfileData.CopySettings(todData, m_selectedWeatherProfileAsset.WeatherData.m_weatherData);
                m_timeOfDayProfile.TimeOfDayData.SetupStartingSettings(todData, ConvertTimeOfDay());
                m_weatherVFX.StopWeatherFX(m_currentWeatherTransitionDuration);
                m_weatherVFX.SetVFXPlayState(false);
                if (m_additionalWeatherVFX.Count > 0)
                {
                    foreach (IHDRPWeatherVFX weatherVfx in m_additionalWeatherVFX)
                    {
                        if (weatherVfx != null)
                        {
                            weatherVfx.StopWeatherFX(m_currentWeatherTransitionDuration);
                            weatherVfx.SetVFXPlayState(false);
                        }
                    }
                }
            }

            bool destroyVFXInstant = false;
            if (instant)
            {
                m_weatherVFX.SetDuration(1f);
                destroyVFXInstant = true;
            }

            if (m_timeOfDayProfile.TimeOfDayData.ReturnFromWeather(Components, isDay, m_lightSourceOverride, currentTime, m_weatherVFX.GetCurrentDuration()))
            {
                m_selectedWeatherProfileAsset.WeatherData.Reset();
                if (destroyVFXInstant)
                {
                    m_weatherVFX.DestroyInstantly();
                    foreach (IHDRPWeatherVFX vfx in m_additionalWeatherVFX)
                    {
                        vfx.DestroyInstantly();
                    }
                }
                else
                {
                    m_weatherVFX.DestroyVFX();
                    foreach (IHDRPWeatherVFX vfx in m_additionalWeatherVFX)
                    {
                        vfx.DestroyVFX();
                    }
                }

                m_currentRandomWeatherTimer = UnityEngine.Random.Range(m_randomWeatherTimer.x, m_randomWeatherTimer.y);
                m_weatherIsActive = false;
                if (!m_enableTimeOfDaySystem)
                {
                    ProcessTimeOfDay();
                }
            }
        }
        /// <summary>
        /// Fixes the camera render distance
        /// Helpful when you are using local volumetric clouds
        /// </summary>
        public void FixCameraRenderDistance()
        {
            if (m_timeOfDayProfile != null)
            {
                if (!m_timeOfDayProfile.TimeOfDayData.m_useLocalClouds)
                {
                    return;
                }
            }
            bool hasBeenSet = false;
#if UNITY_EDITOR
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null)
            {
                if (sceneView.cameraSettings is { farClip: < 20000f })
                {
                    hasBeenSet = true;
                    sceneView.cameraSettings.farClip = m_cameraSettings.m_farClipPlane;
                    sceneView.camera.farClipPlane = m_cameraSettings.m_farClipPlane;
                }
            }
#endif
            if (hasBeenSet)
            {
                ShowDebugLog("Camera render distance was set to " + m_cameraSettings.m_farClipPlane + " as you are using local clouds", DebugLogType.Log);
            }
        }
        /// <summary>
        /// Function used to setup the time of day system and refreshes the components by removing them and re-adding the latest
        /// </summary>
        public void RefreshTimeOfDayComponents()
        {
            if (Components.m_componentsObject != null)
            {
                DestroyImmediate(Components.m_componentsObject);
                m_hasBeenSetupCorrectly = SetupHDRPTimeOfDay();
                ProcessTimeOfDay();
            }
        }
        /// <summary>
        /// Stops the simulation to see how the TOD transition looks within the editor
        /// </summary>
        /// <param name="stop"></param>
        public void StopSimulate()
        {
            TimeOfDay = m_savedTODTime;
            DebugSettings.m_simulate = false;
        }
        /// <summary>
        /// Starts the simulation by saving the TOD value and setting to bool to true
        /// </summary>
        public void StartSimulate()
        {
            m_savedTODTime = TimeOfDay;
            DebugSettings.m_simulate = true;
        }

        #endregion
        #region Private Functions

        /// <summary>
        /// Sets up the interior systems for time of day
        /// </summary>
        private void SetupInteriorSystems()
        {
            m_interiorControllerData.GetAllControllers();
            m_interiorControllerData.SetupAudioReverb(Player);
            m_interiorControllerData.SetReverbFilter(false, m_currentInteriorController);
        }
        /// <summary>
        /// Checks the interior controller state
        /// </summary>
        private bool CheckInteriorControllers()
        {
            if (m_weatherIsActive)
            {
                return m_interiorControllerData.Apply(m_interiorControllerData.CheckControllers(out bool isInMainBounds, out m_currentInteriorController), isInMainBounds, m_currentInteriorController);
            }

            return false;
        }
        /// <summary>
        /// Checks to see if the gameobject is not the components volume for TOD
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        private bool IsNotTODVolume(GameObject volume)
        {
            if (volume.GetComponent<HDRPTimeOfDayComponentType>())
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Updates the seasonal settings
        /// </summary>
        private void UpdateSeasons()
        {
            if (m_seasonProfile != null)
            {
                m_seasonProfile.Apply(m_enableSeasons, Application.isPlaying);
            }
        }
        /// <summary>
        /// Sets up the gameobjects that are used for parenting
        /// </summary>
        private void SetupOverrideVolumeOrganize()
        {
            if (m_volumeMasterParent == null)
            {
                m_volumeMasterParent = GameObject.Find("Override Volumes");
                if (m_volumeMasterParent == null)
                {
                    m_volumeMasterParent = new GameObject("Override Volumes");
                }
            }

            if (m_volumeDayParent == null)
            {
                m_volumeDayParent = GameObject.Find("Day Volumes");
                if (m_volumeDayParent == null)
                {
                    m_volumeDayParent = new GameObject("Day Volumes");
                }
            }

            if (m_volumeNightParent == null)
            {
                m_volumeNightParent = GameObject.Find("Night Volumes");
                if (m_volumeNightParent == null)
                {
                    m_volumeNightParent = new GameObject("Night Volumes");
                }
            }

            m_volumeDayParent.transform.SetParent(m_volumeMasterParent.transform);
            m_volumeNightParent.transform.SetParent(m_volumeMasterParent.transform);
        }
        /// <summary>
        /// Calculates if the sun is below the horizon + offset value
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="horizonMin"></param>
        /// <param name="horizonMax"></param>
        /// <returns></returns>
        private void SetReflectionProbeSystem()
        {
            if (Components.m_reflectionProbeManager.Profile != null)
            {
                if (m_enableReflectionProbeSync)
                {
                    Components.m_reflectionProbeManager.Profile.m_renderMode = ProbeRenderMode;
                    Components.m_reflectionProbeManager.RenderDistance = ProbeRenderDistance;
                }
                else
                {
                    Components.m_reflectionProbeManager.Profile.m_renderMode = ProbeRenderMode.None;
                }
            }
        }
        /// <summary>
        /// Sets the reflection probe profile to the probe manager
        /// </summary>
        private void SetReflectionProbeProfile()
        {
            if (Components.m_reflectionProbeManager != null)
            {
                Components.m_reflectionProbeManager.Profile = ReflectionProbeProfile;
            }
        }
        /// <summary>
        /// Calculates the horizon
        /// True = sun above the horizon, False = sun below the horizon +- the offset
        /// </summary>
        /// <returns></returns>
        private bool CalculateHorizon()
        {
            return Components.m_sunRotationObject.transform.up.y < m_timeOfDayProfile.TimeOfDayData.m_horizonOffset;
        }
        /// <summary>
        /// Setup the ambient audio system
        /// </summary>
        private void SetupAmbientAudio()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (UseAmbientAudio)
            {
                if (m_ambientSourceA == null)
                {
                    GameObject source = new GameObject("Ambient Audio Source A");
                    m_ambientSourceA = source.AddComponent<AudioSource>();
                    m_ambientSourceA.loop = true;
                    m_ambientSourceA.volume = 0f;
                    m_ambientSourceA.playOnAwake = false;
                    m_ambientSourceA.maxDistance = 10000f;
                    m_ambientSourceA.transform.SetParent(transform);
                }
                if (m_ambientSourceB == null)
                {
                    GameObject source = new GameObject("Ambient Audio Source B");
                    m_ambientSourceB = source.AddComponent<AudioSource>();
                    m_ambientSourceB.loop = true;
                    m_ambientSourceB.volume = 0f;
                    m_ambientSourceB.playOnAwake = false;
                    m_ambientSourceB.maxDistance = 10000f;
                    m_ambientSourceB.transform.SetParent(transform);
                }
            }
            else
            {
                if (m_ambientSourceA != null)
                {
                    DestroyImmediate(m_ambientSourceA.gameObject);
                }
                if (m_ambientSourceB != null)
                {
                    DestroyImmediate(m_ambientSourceB.gameObject);
                }
            }
        }
        /// <summary>
        /// Validation checker to check the TOD is setup correctly
        /// </summary>
        /// <returns></returns>
        private IEnumerator ValidationChecker()
        {
            while (true)
            {
                m_validating = true;
                yield return new WaitForEndOfFrame();
                m_currentValidateCheckerFrames++;
                if (m_hasBeenSetupCorrectly)
                {
                    m_validating = false;
                    StopAllCoroutines();
                }
                m_hasBeenSetupCorrectly = SetupHDRPTimeOfDay();
                if (m_currentValidateCheckerFrames >= ValidateCheckerFrameLimit || m_hasBeenSetupCorrectly)
                {
                    ProcessTimeOfDay();
                    m_validating = false;
                    StopAllCoroutines();
                }
            }
        }
        /// <summary>
        /// Editor update for scene view previewing
        /// </summary>
        private void EditorUpdate()
        {
            if (!m_hasBeenSetupCorrectly || Application.isPlaying)
            {
                return;
            }

#if UNITY_EDITOR
            if (EditorApplication.isUpdating)
            {
                return;
            }
#endif
            if (UseOverrideVolumes)
            {
                m_overrideVolumeData.m_isInVolue = CheckOverrideVolumes();
                if (m_overrideVolumeData.m_transitionTime < 1f && Components.Validated(out string failedObject))
                {
                    if (m_lastOverrideVolume != null)
                    {
                        m_overrideVolumeData.m_transitionTime += Time.deltaTime / m_lastOverrideVolume.m_volumeSettings.m_blendTime;
                    }
                    else
                    {
                        m_overrideVolumeData.m_transitionTime += Time.deltaTime / 3f;
                    }

                    ProcessTimeOfDay();
                }
            }
            else
            {
                if (m_overrideVolumeData.m_transitionTime < 1f && Components.Validated(out string failedObject))
                {
                    m_overrideVolumeData.m_transitionTime += Time.deltaTime / 3f;
                    ProcessTimeOfDay();
                }
            }

            m_timeOfDayProfile.UnderwaterOverrideData.ApplySettings(ConvertTimeOfDay());
            UpdateSeasons();
        }
        /// <summary>
        /// Updates the sun settings by rotating the sun and then back to refresh the light
        /// </summary>
        /// <returns></returns>
        private IEnumerator RefreshSunSettings()
        {
            Light light = Components.m_sunLight;
            HDAdditionalLightData data = Components.m_sunLightData;
            if (!IsDay)
            {
                light = Components.m_moonLight;
                data = Components.m_moonLightData;
            }

            Vector3 rotation = light.transform.eulerAngles;
            rotation.z += 1f;
            light.transform.eulerAngles = rotation;
            yield return new WaitForSeconds(0.1f);
            rotation.z -= 1f;
            light.transform.eulerAngles = rotation;
            data.UpdateAllLightValues();
            StopCoroutine(RefreshSunSettings());
        }
        /// <summary>
        /// Checks the setup for use of weather
        /// </summary>
        private void CheckCloudSettingsForWeather()
        {
            if (m_timeOfDayProfile != null)
            {
                if (!m_timeOfDayProfile.TimeOfDayData.m_useLocalClouds)
                {
#if UNITY_EDITOR
                    if (EditorUtility.DisplayDialog("Weather Enabled",
                        "You have enabled weather but local volumetric clouds is disabled. We highly recommend using local clouds for a better experience. Will also need to increase your camera far clip plane view distance, would you like us to set this up for you?",
                        "Yes", "No"))
                    {
                        FixCameraRenderDistance();
                        m_timeOfDayProfile.TimeOfDayData.m_useLocalClouds = true;
                        ProcessTimeOfDay();
                    }
#endif
                }
            }
        }
        /// <summary>
        /// Checks to see if you are in an override volume
        /// </summary>
        /// <returns></returns>
        private bool CheckOverrideVolumes()
        {
            if (m_overrideVolumes.Count < 1)
            {
                if (m_lastOverrideVolume != null)
                {
                    m_overrideVolumeData.m_transitionTime = 0f;
                    m_lastOverrideVolume = null;
                }
                return false;
            }
            else
            {
                HDRPTimeOfDayOverrideVolume volume = m_overrideVolumes.Last().Value;
                if (volume != null)
                {
                    if (!volume.enabled || !volume.gameObject.activeInHierarchy)
                    {
                        return false;
                    }
                    else if (WeatherActive() && !volume.m_volumeSettings.m_volumeActiveInWeather)
                    {
                        return false;
                    }

                    if (m_lastOverrideVolume == null || volume != m_lastOverrideVolume)
                    {
                        m_lastOverrideVolume = volume;
                        m_overrideVolumeData = new OverrideDataInfo
                        {
                            m_isInVolue = volume.m_volumeSettings.IsAnyOverrideEnabled(),
                            m_transitionTime = 0f,
                            m_settings = volume.m_volumeSettings
                        };

                        return true;
                    }

                    return volume.m_volumeSettings.IsAnyOverrideEnabled();
                }

                if (m_lastOverrideVolume != null)
                {
                    m_overrideVolumeData.m_transitionTime = 0f;
                    m_lastOverrideVolume = null;
                }
                return false;
            }
        }
        /// <summary>
        /// Sets up the ray tracing components
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool SetupRayTracing(bool value)
        {
#if RAY_TRACING_ENABLED
            HDRPTimeOfDayComponentType[] componentTypes = gameObject.GetComponentsInChildren<HDRPTimeOfDayComponentType>();
            if (componentTypes.Length > 0)
            {
                foreach (HDRPTimeOfDayComponentType type in componentTypes)
                {
                    if (type.m_componentType == TimeOfDayComponentType.RayTracedVolume)
                    {
                        Volume volume = type.GetComponent<Volume>();
                        if (volume != null)
                        {
                            Components.m_rayTracingVolume = volume;
                            Components.m_rayTracingVolume.weight = -1000f;
                            if (volume.sharedProfile != null)
                            {
                                volume.sharedProfile.TryGet(out Components.m_rayTracedGlobalIllumination);
                                volume.sharedProfile.TryGet(out Components.m_rayTracedScreenSpaceReflection);
                                volume.sharedProfile.TryGet(out Components.m_rayTracedAmbientOcclusion);
                                volume.sharedProfile.TryGet(out Components.m_rayTracedSettings);
                                volume.sharedProfile.TryGet(out Components.m_rayTracedRecursiveRendering);
                                volume.sharedProfile.TryGet(out Components.m_rayTracedSubSurfaceScattering);
                            }

                            if (Components.m_rayTracingVolume == null)
                            {
                                return false;
                            }
                            if (Components.m_rayTracedGlobalIllumination == null)
                            {
                                return false;
                            }
                            if (Components.m_rayTracedScreenSpaceReflection == null)
                            {
                                return false;
                            }
                            if (Components.m_rayTracedAmbientOcclusion == null)
                            {
                                return false;
                            }
                            if (Components.m_rayTracedSettings == null)
                            {
                                return false;
                            }
                            if (Components.m_rayTracedRecursiveRendering == null)
                            {
                                return false;
                            }
                            if (Components.m_rayTracedSubSurfaceScattering == null)
                            {
                                return false;
                            }

                            if (value)
                            {
                                Components.m_rayTracingVolume.weight = 1f;
                                return true;
                            }
                            else
                            {
                                Components.m_rayTracingVolume.weight = 0f;
                                return false;
                            }
                        }
                    }
                }
            }
#endif
            return false;
        }
        /// <summary>
        /// Disables the volume
        /// </summary>
        private void DisableRayTraceVolume()
        {
            if (Components.m_rayTracingVolume == null)
            {
                HDRPTimeOfDayComponentType[] componentTypes = gameObject.GetComponentsInChildren<HDRPTimeOfDayComponentType>();
                foreach (HDRPTimeOfDayComponentType type in componentTypes)
                {
                    if (type.m_componentType == TimeOfDayComponentType.RayTracedVolume)
                    {
                        Components.m_rayTracingVolume = type.GetVolume();
                    }
                }
            }

            if (Components.m_rayTracingVolume != null)
            {
                Components.m_rayTracingVolume.weight = -1000f;
            }
        }
        /// <summary>
        /// Updates the time of day by adding time to it if it's enabled
        /// </summary>
        /// <param name="isDay"></param>
        /// <param name="data"></param>
        private void UpdateTime(bool isDay, TimeOfDayProfileData data)
        {
            if (!Application.isPlaying || !m_enableTimeOfDaySystem)
            {
                return;
            }

            if (isDay)
            {
                TimeOfDay += (Time.deltaTime * m_timeOfDayMultiplier) / data.m_dayDuration;
            }
            else
            {
                TimeOfDay += (Time.deltaTime * m_timeOfDayMultiplier) / data.m_nightDuration;
            }
            if (TimeOfDay >= 24f)
            {
                TimeOfDay = 0f;
            }
        }
        /// <summary>
        /// Updates the sun/moon position and rotation
        /// </summary>
        /// <param name="time"></param>
        /// <param name="isDay"></param>
        /// <param name="data"></param>
        private void UpdateSun(float time, bool isDay, bool overrideSource, TimeOfDayProfileData data)
        {
            if (data.ValidateSun())
            {
                Components.SetSunMoonState(isDay);

                //Apply Settings
                HDAdditionalLightData lightData = Components.m_sunLightData;
                if (!isDay)
                {
                    lightData = Components.m_moonLightData;
                }

                data.ApplySunSettings(lightData, time, isDay, overrideSource, m_overrideVolumeData);
            }
        }
        /// <summary>
        /// Sets the sun rotation
        /// </summary>
        private void UpdateSunRotation(float time)
        {
            //Set rotation
            Components.m_sunRotationObject.transform.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(0f, 360f, time), DirectionY, 0f));
        }
        /// <summary>
        /// Sets the star intensity
        /// </summary>
        /// <param name="isDay"></param>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateStars(bool isDay, float time, TimeOfDayProfileData data)
        {
            if (!isDay && data.ValidateStars())
            {
                data.ApplyStarSettings(Components, time);
            }
        }
        /// <summary>
        /// Resets the stars rotation back to 0,0,0
        /// </summary>
        private void ResetStarsRotaion(TimeOfDayProfileData data)
        {
            if (data.m_resetStarsRotationOnEnable)
            {
                Components.m_physicallyBasedSky.spaceRotation.value = Vector3.zero;
            }
        }
        /// <summary>
        /// Updates the fog settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateFog(float time, TimeOfDayProfileData data)
        {
            if (data.ValidateFog())
            {
                data.ApplyFogSettings(Components, time, out m_currentFogColor, out m_currentLocalFogDistance);
            }
        }
        /// <summary>
        /// Updates the shadow settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateShadows(bool isDay, float time, TimeOfDayProfileData data)
        {
            if (data.ValidateShadows())
            {
                data.ApplyShadowSettings(Components, time, m_overrideVolumeData, isDay);
            }
        }
        /// <summary>
        /// Updates advanced lighting settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateAdvancedLighting(float time, bool isDay, TimeOfDayProfileData data)
        {
            if (data.ValidateAdvancedLighting())
            {
                data.ApplyAdvancedLighting(Components, isDay, time, m_overrideVolumeData);
            }
        }
        /// <summary>
        /// Updates cloud settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateClouds(float time, TimeOfDayProfileData data)
        {
            if (data.ValidateClouds())
            {
                m_lastCloudLayerOpacity = data.ApplyCloudSettings(Components, time);
            }
        }
        /// <summary>
        /// Updates ambient occlusion Settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateAmbientOcclusion(float time, TimeOfDayPostFXProfileData data)
        {
            if (data.ValidateAmbientOcclusion())
            {
                data.ApplyAmbientOcclusion(Components.m_ambientOcclusion, time);
            }
        }
        /// <summary>
        /// Updates color grading settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateColorGrading(float time, TimeOfDayPostFXProfileData data)
        {
            if (data.ValidateColorGrading())
            {
                data.ApplyColorGradingSettings(Components.m_colorAdjustments, Components.m_whiteBalance, time);
            }
        }
        /// <summary>
        /// Updates bloom settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateBloom(float time, TimeOfDayPostFXProfileData data)
        {
            if (data.ValidateBloom())
            {
                data.ApplyBloomSettings(Components.m_bloom, time);
            }
        }
        /// <summary>
        /// Updates shadow toning settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateShadowToning(float time, TimeOfDayPostFXProfileData data)
        {
            if (data.ValidateShadowToning())
            {
                data.ApplyShadowToningSettings(Components.m_splitToning, time);
            }
        }
        /// <summary>
        /// Updates vignette settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateVignette(float time, TimeOfDayPostFXProfileData data)
        {
            if (data.ValidateVignette())
            {
                data.ApplyVignetteSettings(Components.m_vignette, time);
            }
        }
        /// <summary>
        /// Updates depth of field settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        private void UpdateDepthOfField(float time, TimeOfDayPostFXProfileData data)
        {
            if (data.ValidateDepthOfField())
            {
                data.ApplyDepthOfField(Components.m_depthOfField, time);
            }
        }
        /// <summary>
        /// Updates lens flare settings
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data"></param>
        /// <param name="isDay"></param>
        private void UpdateLensFlare(float time, TimeOfDayProfileData data, bool isDay)
        {
            if (data.ValidateSunLensFlare())
            {
                data.ApplySunLensFlare(Components.m_sunLensFlare, time, isDay);
            }
            if (data.ValidateMoonLensFlare())
            {
                data.ApplyMoonLensFlare(Components.m_moonLensFlare, time, isDay);
            }
        }
        /// <summary>
        /// Sets post processing state
        /// </summary>
        /// <param name="isActive"></param>
        private void SetPostProcessingActive(bool isActive)
        {
            if (Components.m_timeOfDayPostFXVolume != null)
            {
                Components.m_timeOfDayPostFXVolume.gameObject.SetActive(isActive);
            }
        }
        /// <summary>
        /// Checks if a weather profile is going to be active
        /// </summary>
        private void CheckAutoWeather()
        {
            if (!m_useWeatherFX || m_weatherIsActive || !Application.isPlaying)
            {
                return;
            }

            m_currentRandomWeatherTimer -= Time.deltaTime;
            if (m_currentRandomWeatherTimer <= 0f)
            {
                m_selectedActiveWeatherProfile = UnityEngine.Random.Range(0, m_weatherProfiles.Count - 1);
                //Try avoid same profile twice
                if (m_avoidSameRandomWeather)
                {
                    if (m_lastSelectedWeatherProfile == -1)
                    {
                        m_lastSelectedWeatherProfile = m_selectedActiveWeatherProfile;
                    }
                    else
                    {
                        if (m_selectedActiveWeatherProfile == m_lastSelectedWeatherProfile)
                        {
                            m_selectedActiveWeatherProfile++;
                            if (m_selectedActiveWeatherProfile > m_weatherProfiles.Count - 1)
                            {
                                m_selectedActiveWeatherProfile = 0;
                            }
                        }

                        m_lastSelectedWeatherProfile = m_selectedActiveWeatherProfile;
                    }
                }

                StartWeather(m_selectedActiveWeatherProfile);
            }
        }
        /// <summary>
        /// Sets up the time of day prefab and volume
        /// </summary>
        private bool BuildVolumesAndCollectComponents()
        {
            if (!gameObject.activeInHierarchy)
            {
                return false;
            }

            if (!SetupComponentsPrefab())
            {
                return false;
            }

            if (Components.Validated(out string failedObject))
            {
                return true;
            }

            if (Components.m_reflectionProbeManager == null)
            {
                Components.m_reflectionProbeManager = HDRPTimeOfDayReflectionProbeManager.Instance;
                if (Components.m_reflectionProbeManager == null)
                {
                    Components.m_reflectionProbeManager = FindObjectOfType<HDRPTimeOfDayReflectionProbeManager>();
                }
            }

            if (Components.m_camera == null)
            {
                Transform cameraTransform = HDRPTimeOfDayAPI.GetCamera();
                Camera camera = null;
                HDAdditionalCameraData data = null;
                if (cameraTransform != null)
                {
                    camera = cameraTransform.GetComponent<Camera>();
                    if (camera != null)
                    {
                        data = camera.GetComponent<HDAdditionalCameraData>();
                        if (data == null)
                        {
                            data = camera.gameObject.AddComponent<HDAdditionalCameraData>();
                        }
                    }
                }

                Components.m_camera = camera;
                Components.m_cameraData = data;
            }

            HDRPTimeOfDayComponentType[] componentType = FindObjectsOfType<HDRPTimeOfDayComponentType>();
            if (componentType.Length > 0)
            {
                foreach (HDRPTimeOfDayComponentType type in componentType)
                {
                    if (type.m_componentType == TimeOfDayComponentType.PostProcessing)
                    {
                        if (Components.m_timeOfDayPostFXVolumeProfile == null || Components.m_timeOfDayPostFXVolume == null)
                        {
                            Components.m_timeOfDayPostFXVolume = type.GetComponent<Volume>();
                            if (Components.m_timeOfDayPostFXVolume != null)
                            {
                                Components.m_timeOfDayPostFXVolumeProfile = Components.m_timeOfDayPostFXVolume.sharedProfile;
                            }
                        }
                    }
                    else if (type.m_componentType == TimeOfDayComponentType.SunRotationObject)
                    {
                        Components.m_sunRotationObject = type.gameObject;
                    }
                }
            }

            if (m_player == null || !m_player.gameObject.activeSelf)
            {
                m_player = HDRPTimeOfDayAPI.GetCamera();
            }

            if (m_usePostFX && Components.m_timeOfDayPostFXVolume != null)
            {
                Components.m_timeOfDayPostFXVolume.isGlobal = true;
                Components.m_timeOfDayPostFXVolume.priority = 50;
                if (TimeOfDayPostFxProfile == null)
                {
                    Components.m_timeOfDayPostFXVolume.weight = 0f;
                }
                else
                {
                    Components.m_timeOfDayPostFXVolume.weight = 1f;
                }
            }

            if (UsePostFX)
            {
                if (Components.m_timeOfDayPostFXVolumeProfile == null)
                {
                    return false;
                }

                if (!Components.m_timeOfDayPostFXVolumeProfile.TryGet(out Components.m_colorAdjustments))
                {
                    return false;
                }
                if (!Components.m_timeOfDayPostFXVolumeProfile.TryGet(out Components.m_whiteBalance))
                {
                    return false;
                }
                if (!Components.m_timeOfDayPostFXVolumeProfile.TryGet(out Components.m_bloom))
                {
                    return false;
                }
                if (!Components.m_timeOfDayPostFXVolumeProfile.TryGet(out Components.m_splitToning))
                {
                    return false;
                }
                if (!Components.m_timeOfDayPostFXVolumeProfile.TryGet(out Components.m_vignette))
                {
                    return false;
                }
                if (!Components.m_timeOfDayPostFXVolumeProfile.TryGet(out Components.m_ambientOcclusion))
                {
                    return false;
                }
                if (!Components.m_timeOfDayPostFXVolumeProfile.TryGet(out Components.m_depthOfField))
                {
                    return false;
                }
            }

            if (Components.m_timeOfDayVolume == null)
            {
                return false;
            }

            Components.m_timeOfDayVolume.isGlobal = true;
            Components.m_timeOfDayVolume.priority = 50;
            Components.m_timeOfDayVolumeProfile = Components.m_timeOfDayVolume.sharedProfile;

            if (Components.m_timeOfDayVolumeProfile == null)
            {
                return false;
            }

            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_visualEnvironment))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_physicallyBasedSky))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_cloudLayer))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_volumetricClouds))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_globalIllumination))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_fog))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_exposure))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_screenSpaceReflection))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_screenSpaceRefraction))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_contactShadows))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_microShadowing))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_indirectLightingController))
            {
                return false;
            }
            if (!Components.m_timeOfDayVolumeProfile.TryGet(out Components.m_shadows))
            {
                return false;
            }

            Components.m_localVolumetricFog = Components.m_timeOfDayVolume.gameObject.GetComponentInChildren<LocalVolumetricFog>();
            if (Components.m_localVolumetricFog == null)
            {
                return false;
            }

            Light[] lights = Components.m_timeOfDayVolume.gameObject.GetComponentsInChildren<Light>();
            if (lights.Length < 1)
            {
                ShowDebugLog("Sun and moon light could not be found", DebugLogType.Error);
                return false;
            }
            else
            {
                foreach (Light light in lights)
                {
                    HDRPTimeOfDayComponentType lightType = light.GetComponent<HDRPTimeOfDayComponentType>();
                    if (lightType != null)
                    {
                        if (lightType.m_componentType == TimeOfDayComponentType.Sun)
                        {
                            Components.m_sunLight = light;
                        }
                        else if (lightType.m_componentType == TimeOfDayComponentType.Moon)
                        {
                            Components.m_moonLight = light;
                        }
                    }
                }
            }

            if (Components.m_sunLight == null)
            {
                return false;
            }
            else
            {
                if (Components.m_sunLightData == null)
                {
                    Components.m_sunLightData = Components.m_sunLight.GetComponent<HDAdditionalLightData>();
                    if (Components.m_sunLightData == null)
                    {
                        Components.m_sunLightData = Components.m_sunLight.gameObject.AddComponent<HDAdditionalLightData>();
                    }
                }
            }
            if (Components.m_moonLight == null)
            {
                return false;
            }
            else
            {
                if (Components.m_moonLightData == null)
                {
                    Components.m_moonLightData = Components.m_moonLight.GetComponent<HDAdditionalLightData>();
                    if (Components.m_moonLightData == null)
                    {
                        Components.m_moonLightData = Components.m_moonLight.gameObject.AddComponent<HDAdditionalLightData>();
                    }
                }
            }

            Light[] directionalLights = FindObjectsOfType<Light>();
            if (directionalLights.Length > 0)
            {
                foreach (Light directionalLight in directionalLights)
                {
                    if (directionalLight.type == LightType.Directional)
                    {
                        if (directionalLight != Components.m_sunLight && directionalLight != Components.m_moonLight)
                        {
                            if (directionalLight.enabled)
                            {
                                if (AddDisabledItem(directionalLight.gameObject))
                                {
                                    directionalLight.gameObject.SetActive(false);
                                }
                                Debug.Log(directionalLight.name + " was disabled as it conflicts with HDRP Time Of Day. If you ever remove time of day system you can just re-enable this light source.");
                            }
                        }
                    }
                }
            }

            //Sun lens flare
            Components.m_sunLensFlare = Components.m_sunLight.GetComponent<LensFlareComponentSRP>();
            if (Components.m_sunLensFlare == null)
            {
                Components.m_sunLensFlare = Components.m_sunLight.gameObject.AddComponent<LensFlareComponentSRP>();
            }
            if (Components.m_sunLensFlare == null)
            {
                return false;
            }

            //Moon lens flare
            Components.m_moonLensFlare = Components.m_moonLight.GetComponent<LensFlareComponentSRP>();
            if (Components.m_moonLensFlare == null)
            {
                Components.m_moonLensFlare = Components.m_moonLight.gameObject.AddComponent<LensFlareComponentSRP>();
            }
            if (Components.m_moonLensFlare == null)
            {
                return false;
            }

            if (Components.m_sunRotationObject == null)
            {
                return false;
            }

            SetSkySettings(m_timeOfDayProfile.TimeOfDayData.m_skyboxExposure, m_timeOfDayProfile.TimeOfDayData.m_skyboxGroundColor);

            return true;
        }
        /// <summary>
        /// Sets up the component prefab prefab and spawns if it's null/empty 
        /// </summary>
        /// <returns></returns>
        private bool SetupComponentsPrefab()
        {
            if (Components.m_timeOfDayVolume == null || Components.m_componentsObject == null)
            {
                GameObject timeOfDayVolume = GameObject.Find("Time Of Day Components");
                if (timeOfDayVolume == null)
                {
#if UNITY_EDITOR
                    GameObject componentsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GetAssetPath(ComponentsPrefabName));
                    if (componentsPrefab == null)
                    {
                        Debug.LogError("Time Of Day Components Prefab is missing and could not be found in the project. It's normally found in 'HDRP Time Of Day/Resources' folder");
                        return false;
                    }

                    timeOfDayVolume = Instantiate(componentsPrefab);
                    timeOfDayVolume.name = "Time Of Day Components";
#endif
                }

                timeOfDayVolume.transform.SetParent(transform);

                Components.m_timeOfDayVolume = timeOfDayVolume.GetComponent<Volume>();
                Components.m_componentsObject = timeOfDayVolume;
            }

            return true;
        }
        /// <summary>
        /// Sets the new player transform
        /// </summary>
        private void UpdatePlayerTransform()
        {
            if (Player == null)
            {
                return;
            }

            HDRPTimeOfDayTransformTracker[] tackers = FindObjectsOfType<HDRPTimeOfDayTransformTracker>();
            if (tackers.Length > 0)
            {
                foreach (HDRPTimeOfDayTransformTracker tracker in tackers)
                {
                    tracker.SetNewPlayer(Player);
                }
            }
        }

        #endregion
        #region Public Static Functions

        /// <summary>
        /// Adds time of day to the scene
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="selection"></param>
        public static void CreateTimeOfDay(GameObject parent, bool selection = true)
        {
            RemoveTimeOfDay(true);
            HDRPTimeOfDay timeOfDay = Instance;
            if (timeOfDay == null)
            {
                GameObject timeOfDayGameObject = new GameObject("HDRP Time Of Day");
                timeOfDay = timeOfDayGameObject.AddComponent<HDRPTimeOfDay>();
                HDRPDefaultsProfile defaults = null;
#if UNITY_EDITOR
                defaults = AssetDatabase.LoadAssetAtPath<HDRPDefaultsProfile>(GetAssetPath(TimeOfDayDefaultsProfileName));
                if (defaults != null)
                {
                    defaults.ApplyDefaultsToTimeOfDay(timeOfDay);
                }
#endif
                timeOfDay.SetHasBeenSetup(timeOfDay.SetupHDRPTimeOfDay());
                timeOfDay.SetStaticInstance(timeOfDay);
                SetupDefaultWeatherProfiles(timeOfDay);

                Volume[] localVolumes = FindObjectsOfType<Volume>();
                if (localVolumes.Length > 0)
                {
                    bool localFound = false;
                    List<Volume> allLocalVolumes = new List<Volume>();
                    foreach (Volume localVolume in localVolumes)
                    {
                        localFound = true;
                        allLocalVolumes.Add(localVolume);
                    }

#if GAIA_PRO_PRESENT
                    ProcessGaiaSetup(ref parent);
#endif
                    if (parent != null)
                    {
                        timeOfDay.transform.SetParent(parent.transform);
                    }
#if UNITY_EDITOR
                    if (localFound)
                    {
                        if (EditorUtility.DisplayDialog("Local/Global Volumes Found",
                                "We have detected local/global volumes in your scene, this could affect the lighting quality of the time of day system as it might override some important settings. Would you like us to disable these volumes?",
                                "Yes", "No"))
                        {
                            foreach (Volume volume in allLocalVolumes)
                            {
                                if (volume != null)
                                {
                                    if (timeOfDay.AddDisabledItem(volume.gameObject))
                                    {
                                        volume.gameObject.SetActive(false);
                                        Debug.Log(volume.name + " has been disabled");
                                    }
                                }
                            }
                        }
                    }

                    if (selection)
                    {
                        Selection.activeObject = timeOfDay;
                    }

                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
                }
            }

        }
        /// <summary>
        /// Removes time of day from the scene
        /// </summary>
        public static void RemoveTimeOfDay(bool isCreatingTOD = false)
        {
            HDRPTimeOfDay timeOfDay = Instance;
            if (timeOfDay != null)
            {
#if UNITY_EDITOR
                if (!isCreatingTOD)
                {
                    if (EditorUtility.DisplayDialog("Re-enable disabled gameobjects",
                            "HDRP Time Of Day has been removed would you like to activate all the disable objects that this system disabled?",
                            "Yes", "No"))
                    {
                        timeOfDay.EnableAllDisabledItems();
                    }
                }
#endif
                DestroyImmediate(timeOfDay.gameObject);
            }
            else
            {
                GameObject timeOfDayGameObject = GameObject.Find("HDRP Time Of Day");
                if (timeOfDayGameObject != null)
                {
                    DestroyImmediate(timeOfDayGameObject);
                }
            }
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
        }
        /// <summary>
        /// Get the asset path of the first thing that matches the name
        /// </summary>
        /// <param name="fileName">File name to search for</param>
        /// <returns></returns>
        public static string GetAssetPath(string fileName)
        {
#if UNITY_EDITOR
            string fName = Path.GetFileNameWithoutExtension(fileName);
            string[] assets = UnityEditor.AssetDatabase.FindAssets(fName, null);
            for (int idx = 0; idx < assets.Length; idx++)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[idx]);
                if (Path.GetFileName(path) == fileName)
                {
                    return path;
                }
            }
#endif
            return "";
        }
        /// <summary>
        /// Sets up the default weather profiles
        /// </summary>
        /// <param name="timeOfDay"></param>
        public static void SetupDefaultWeatherProfiles(HDRPTimeOfDay timeOfDay)
        {
            if (timeOfDay != null)
            {
#if UNITY_EDITOR
                if (EditorUtility.DisplayDialog("Enable Weather Effects", "Would you like to enable weather effects?", "Yes", "No"))
                {
                    timeOfDay.UseWeatherFX = true;
                }
                else
                {
                    timeOfDay.UseWeatherFX = false;
                }
#endif
            }
        }
        /// <summary>
        /// Cleans the HDRP components that Gaia creates
        /// </summary>
        private static void CleanGaiaHDRP()
        {
#if GAIA_PRO_PRESENT
            GameObject hdVolume = GameObject.Find("HD Environment Volume");
            if (hdVolume != null)
            {
                DestroyImmediate(hdVolume);
            }
            GameObject hdPostVolume = GameObject.Find("HD Post Processing Environment Volume");
            if (hdPostVolume != null)
            {
                DestroyImmediate(hdPostVolume);
            }
            GameObject legacyTOD = GameObject.Find("Gaia Weather");
            if (legacyTOD != null)
            {
                DestroyImmediate(legacyTOD);
            }
            HDRPDensityVolumeController hdLocalFog = FindObjectOfType<HDRPDensityVolumeController>();
            if (hdLocalFog != null)
            {
                DestroyImmediate(hdLocalFog.gameObject);
            }
            ReflectionProbeManager probeManager = FindObjectOfType<ReflectionProbeManager>();
            if (probeManager != null)
            {
                DestroyImmediate(probeManager.gameObject);
            }
#endif
        }
        /// <summary>
        /// When adding time of day calling this will cleanup and setup gaia to use the new system
        /// </summary>
        private static void ProcessGaiaSetup(ref GameObject parent)
        {
#if GAIA_PRO_PRESENT
            if (GaiaUtils.CheckIfSceneProfileExists(out SceneProfile profile))
            {
                profile.m_lightSystemMode = GaiaConstants.GlobalSystemMode.Gaia;
                for (int i = 0; i < profile.m_lightingProfiles.Count; i++)
                {
                    GaiaLightingProfileValues lightingProfile = profile.m_lightingProfiles[i];
                    if (lightingProfile.m_profileType ==
                        GaiaConstants.GaiaLightingProfileType.ProceduralWorldsSky)
                    {
                        profile.m_selectedLightingProfileValuesIndex = i;
                        break;
                    }
                }

                CleanGaiaHDRP();

                GameObject gaiaParent = GameObject.Find("Gaia Lighting");
                if (gaiaParent != null)
                {
                    parent = gaiaParent;
                }
            }
#endif
        }

        #endregion
    }
}
#endif