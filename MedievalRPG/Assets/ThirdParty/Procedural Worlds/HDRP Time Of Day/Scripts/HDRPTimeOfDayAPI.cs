#if GAIA_PRO_PRESENT
using Gaia;
#endif
using UnityEngine;
#if HDPipeline
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
#endif

namespace ProceduralWorlds.HDRPTOD
{
    public enum TODAPISetFunctions { SSGI, SSR, ContactShadows, MicroShadows, SunShadows, MoonShadows, EnableClouds, SunLensFlare, MoonLensFlare, ProbeSystem }
    public enum DebugLogType { Log, Warning, Error }

    public class HDRPTimeOfDayAPI
    {

#if HDPipeline && UNITY_2021_2_OR_NEWER
        /// <summary>
        /// Gets the time of day system instance in the scene
        /// </summary>
        /// <returns></returns>
        public static HDRPTimeOfDay GetTimeOfDay()
        {
            return HDRPTimeOfDay.Instance;
        }
        /// <summary>
        /// Refreshes the time of day system by processing the time of day code
        /// </summary>
        public static void RefreshTimeOfDay()
        {
            if (IsPresent())
            {
                GetTimeOfDay().ProcessTimeOfDay();
            }
        }
        /// <summary>
        /// Sets the overall quality of HDRP Time Of Day
        /// Note setting this to high could decrease performance on lower end hardware
        /// </summary>
        /// <param name="overallQuality"></param>
        public static void SetOverallQuality(GeneralQuality overallQuality)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null && timeOfDay.TimeOfDayPostFxProfile != null)
                {
                    timeOfDay.TimeOfDayProfile.TimeOfDayData.m_fogQuality = overallQuality;
                    timeOfDay.TimeOfDayProfile.TimeOfDayData.m_denoisingQuality = overallQuality;
                    timeOfDay.TimeOfDayProfile.TimeOfDayData.m_ssgiQuality = overallQuality;
                    timeOfDay.TimeOfDayProfile.TimeOfDayData.m_ssrQuality = overallQuality;
                    timeOfDay.TimeOfDayPostFxProfile.TimeOfDayPostFXData.m_postProcessingQuality = overallQuality;
                }
            }
        }
        /// <summary>
        /// Allows you to assign a set enum function to anything that can be toggled on/off with a bool in the TOD system
        /// </summary>
        /// <param name="value"></param>
        /// <param name="functionCall"></param>
        /// <param name="enabledCloudType"></param>
        public static void SetTODBoolFunction(bool value, TODAPISetFunctions functionCall, GlobalCloudType enabledCloudType = GlobalCloudType.Both)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay tod = GetTimeOfDay();
                switch (functionCall)
                {
                    case TODAPISetFunctions.SSGI:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_useSSGI = value;
                        break;
                    }
                    case TODAPISetFunctions.SSR:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_useSSR = value;
                        break;
                    }
                    case TODAPISetFunctions.ContactShadows:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_useContactShadows = value;
                        break;
                    }
                    case TODAPISetFunctions.MicroShadows:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_useMicroShadows = value;
                        break;
                    }
                    case TODAPISetFunctions.SunShadows:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_enableSunShadows = value;
                        break;
                    }
                    case TODAPISetFunctions.MoonShadows:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_enableMoonShadows = value;
                        break;
                    }
                    case TODAPISetFunctions.EnableClouds:
                    {
                        if (value)
                        {
                            tod.TimeOfDayProfile.TimeOfDayData.m_globalCloudType = enabledCloudType;
                        }
                        else
                        {
                            tod.TimeOfDayProfile.TimeOfDayData.m_globalCloudType = GlobalCloudType.None;
                        }
                        break;
                    }
                    case TODAPISetFunctions.SunLensFlare:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_sunLensFlareProfile.m_useLensFlare = value;
                        break;
                    }
                    case TODAPISetFunctions.MoonLensFlare:
                    {
                        tod.TimeOfDayProfile.TimeOfDayData.m_moonLensFlareProfile.m_useLensFlare = value;
                        break;
                    }
                    case TODAPISetFunctions.ProbeSystem:
                    {
                        tod.EnableReflectionProbeSync = value;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Allows you to assign a set enum function to anything that can be toggled on/off with a bool in the TOD system
        /// </summary>
        /// <param name="value"></param>
        /// <param name="functionCall"></param>
        /// <param name="enabledCloudType"></param>
        public static bool GetTODBoolFunction(TODAPISetFunctions functionCall, GlobalCloudType enabledCloudType = GlobalCloudType.Both)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay tod = GetTimeOfDay();
                switch (functionCall)
                {
                    case TODAPISetFunctions.SSGI:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_useSSGI;
                    }
                    case TODAPISetFunctions.SSR:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_useSSR;
                    }
                    case TODAPISetFunctions.ContactShadows:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_useContactShadows;
                    }
                    case TODAPISetFunctions.MicroShadows:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_useMicroShadows;
                    }
                    case TODAPISetFunctions.SunShadows:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_enableSunShadows;
                    }
                    case TODAPISetFunctions.MoonShadows:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_enableMoonShadows;
                    }
                    case TODAPISetFunctions.EnableClouds:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_globalCloudType != GlobalCloudType.None;
                    }
                    case TODAPISetFunctions.SunLensFlare:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_sunLensFlareProfile.m_useLensFlare;
                    }
                    case TODAPISetFunctions.MoonLensFlare:
                    {
                        return tod.TimeOfDayProfile.TimeOfDayData.m_moonLensFlareProfile.m_useLensFlare;
                    }
                    case TODAPISetFunctions.ProbeSystem:
                    {
                        return tod.EnableReflectionProbeSync;
                    }
                }
            }

            return false;
        }
#if GAIA_PRO_PRESENT
        /// <summary>
        /// Sets the seaon settings
        /// </summary>
        /// <param name="settings"></param>
        public static void SetSeasonSettings(PWSkySeason settings)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.SeasonProfile != null)
                {
                    timeOfDay.m_enableSeasons = settings.EnableSeasons;
                    timeOfDay.SeasonProfile.m_season = settings.Season;
                    timeOfDay.SeasonProfile.m_seasonTransitionDuration = settings.m_seasonTransitionDuration;
                    timeOfDay.SeasonProfile.m_seasonAutumnTint = settings.SeasonAutumnTint;
                    timeOfDay.SeasonProfile.m_seasonSpringTint = settings.SeasonSpringTint;
                    timeOfDay.SeasonProfile.m_seasonSummerTint = settings.SeasonSummerTint;
                    timeOfDay.SeasonProfile.m_seasonWinterTint = settings.SeasonWinterTint;
                }
            }
        }
        /// <summary>
        /// Gets the time of day season settings
        /// </summary>
        /// <returns></returns>
        public static PWSkySeason GetSeasonSettings()
        {
            PWSkySeason settings = new PWSkySeason();
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.SeasonProfile != null)
                {
                    settings.EnableSeasons = timeOfDay.m_enableSeasons;
                    settings.Season = timeOfDay.SeasonProfile.m_season;
                    settings.m_seasonTransitionDuration = timeOfDay.SeasonProfile.m_seasonTransitionDuration;
                    settings.SeasonAutumnTint = timeOfDay.SeasonProfile.m_seasonAutumnTint;
                    settings.SeasonSpringTint = timeOfDay.SeasonProfile.m_seasonSpringTint;
                    settings.SeasonSummerTint = timeOfDay.SeasonProfile.m_seasonSummerTint;
                    settings.SeasonWinterTint = timeOfDay.SeasonProfile.m_seasonWinterTint;
                }
            }

            return settings;
        }
#endif
        /// <summary>
        /// Starts the weather effect from the index selected. This is not an instant effect
        /// </summary>
        /// <param name="weatherProfileIndex"></param>
        public static void StartWeather(int weatherProfileIndex)
        {
            if (IsPresent() && weatherProfileIndex != -1)
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (weatherProfileIndex <= timeOfDay.WeatherProfiles.Count - 1 && weatherProfileIndex >= 0)
                {
                    timeOfDay.StartWeather(weatherProfileIndex);
                }
            }
        }
        /// <summary>
        /// Gets the weather profile ID by checking if it contains the 'name' string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetWeatherIDByName(string name)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                for (int i = 0; i < timeOfDay.WeatherProfiles.Count; i++)
                {
                    if (timeOfDay.WeatherProfiles[i].WeatherData.m_weatherName.Contains(name))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Stops the current active weather. This is an instant effect
        /// </summary>
        public static void StopWeather(bool instant = true)
        {
            if (IsPresent())
            {
                GetTimeOfDay().StopWeather(instant);
            }
        }
        /// <summary>
        /// Returns the weather active bool
        /// </summary>
        /// <returns></returns>
        public static bool WeatherActive()
        {
            if (IsPresent())
            {
                return GetTimeOfDay().WeatherActive();
            }
            return false;
        }
        /// <summary>
        /// Returns the time of day system, will return -1 if the time of day system is not present.
        /// </summary>
        /// <returns></returns>
        public static float GetCurrentTime()
        {
            if (IsPresent())
            {
                return GetTimeOfDay().TimeOfDay;
            }
            else
            {
                return -1f;
            }
        }
        /// <summary>
        /// Sets the time of day.
        /// If is0To1 is set to true you must provide a 0-1 value and not a 0-24 value the value will be multiplied by 24.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="is0To1"></param>
        public static void SetCurrentTime(float time, bool is0To1 = false)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (is0To1)
                {
                    timeOfDay.TimeOfDay = Mathf.Clamp(time * 24f, 0f, 24f);
                }
                else
                {
                    timeOfDay.TimeOfDay = Mathf.Clamp(time, 0f, 24f);
                }
            }
        }
        /// <summary>
        /// Sets the direction of the system on the y axis
        /// </summary>
        /// <param name="direction"></param>
        public static void SetDirection(float direction)
        {
            if (IsPresent())
            {
                GetTimeOfDay().DirectionY = direction;
            }
        }
        /// <summary>
        /// Sets if you want to use post processing based on the state bool
        /// </summary>
        /// <param name="state"></param>
        public static void SetPostProcessingState(bool state)
        {
            if (IsPresent())
            {
                GetTimeOfDay().UsePostFX = state;
            }
        }
        /// <summary>
        /// Sets if you want to use ambient audio based on the state bool
        /// </summary>
        /// <param name="state"></param>
        public static void SetAmbientAudioState(bool state)
        {
            if (IsPresent())
            {
                GetTimeOfDay().UseAmbientAudio = state;
            }
        }
        /// <summary>
        /// Sets if you want to use underwater overrides based on the state bool
        /// </summary>
        /// <param name="state"></param>
        /// <param name="mode"></param>
        public static void SetUnderwaterState(bool state, UnderwaterOverrideSystemType mode)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                timeOfDay.TimeOfDayProfile.UnderwaterOverrideData.m_useOverrides = state;
                timeOfDay.TimeOfDayProfile.UnderwaterOverrideData.m_systemType = mode;
            }
        }
        /// <summary>
        /// Sets if you want to use weather system based on the state bool
        /// </summary>
        /// <param name="state"></param>
        public static void SetWeatherState(bool state)
        {
            if (IsPresent())
            {
                GetTimeOfDay().UseWeatherFX = state;
            }
        }
        /// <summary>
        /// Sets the shadow distance multiplier
        /// </summary>
        /// <param name="value"></param>
        public static void SetGlobalShadowMultiplier(float value)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    timeOfDay.TimeOfDayProfile.TimeOfDayData.m_shadowDistanceMultiplier = value;
                    RefreshTimeOfDay();
                }
            }
        }
        /// <summary>
        /// Gets the shadow distance multiplier
        /// </summary>
        /// <returns></returns>
        public static float GetGlobalShadowMultiplier()
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    return timeOfDay.TimeOfDayProfile.TimeOfDayData.m_shadowDistanceMultiplier;
                }
            }

            return 1f;
        }
        /// <summary>
        /// Sets the fog distance multiplier
        /// </summary>
        /// <param name="value"></param>
        public static void SetGlobalFogMultiplier(float value)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    timeOfDay.TimeOfDayProfile.TimeOfDayData.m_globalFogMultiplier = value;
                    RefreshTimeOfDay();
                }
            }
        }
        /// <summary>
        /// Gets the fog distance multiplier
        /// </summary>
        /// <returns></returns>
        public static float GetGlobalFogMultiplier()
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    return timeOfDay.TimeOfDayProfile.TimeOfDayData.m_globalFogMultiplier;
                }
            }

            return 1f;
        }
        /// <summary>
        /// Sets the sun distance multiplier
        /// </summary>
        /// <param name="value"></param>
        public static void SetGlobalSunMultiplier(float value)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    timeOfDay.TimeOfDayProfile.TimeOfDayData.m_globalLightMultiplier = value;
                    RefreshTimeOfDay();
                }
            }
        }
        /// <summary>
        /// Gets the sun distance multiplier
        /// </summary>
        /// <returns></returns>
        public static float GetGlobalSunMultiplier()
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    return timeOfDay.TimeOfDayProfile.TimeOfDayData.m_globalLightMultiplier;
                }
            }

            return 1f;
        }
        /// <summary>
        /// Sets the auto update multiplier
        /// </summary>
        /// <param name="value"></param>
        public static void SetAutoUpdateMultiplier(bool autoUpdate, float value)
        {
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    timeOfDay.m_enableTimeOfDaySystem = autoUpdate;
                    timeOfDay.m_timeOfDayMultiplier = value;
                }
            }
        }
        /// <summary>
        /// Gets the auto update multiplier
        /// </summary>
        /// <returns></returns>
        public static void GetAutoUpdateMultiplier(out bool autoUpdate, out float value)
        {
            autoUpdate = false;
            value = 1f;
            if (IsPresent())
            {
                HDRPTimeOfDay timeOfDay = GetTimeOfDay();
                if (timeOfDay.TimeOfDayProfile != null)
                {
                    autoUpdate = timeOfDay.m_enableTimeOfDaySystem;
                    value = timeOfDay.m_timeOfDayMultiplier;
                }
            }
        }

        /// <summary>
        /// Retrns true if the system is present
        /// </summary>
        /// <returns></returns>
        private static bool IsPresent()
        {
            return HDRPTimeOfDay.Instance;
        }
        /// <summary>
        /// Validates if the render pipeline asset has ray tracing enabled.
        /// Logs warning if it's not
        /// </summary>
        public static void ValidateRayTracingEnabled()
        {
            if (GraphicsSettings.renderPipelineAsset != null)
            {
                HDRenderPipelineAsset asset = (HDRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
                if (asset != null)
                {
                    if (!asset.currentPlatformRenderPipelineSettings.supportRayTracing)
                    {
                        Debug.LogWarning("You have enabled ray tracing in time of day but the current render pipeline asset does not support ray tracing please enable it in the pipeline asset.");
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Gets the camera transform that is used for the player
        /// </summary>
        /// <returns></returns>
        public static Transform GetCamera()
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                return camera.transform;
            }

            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            if (cameras.Length > 0)
            {
                foreach (Camera cam in cameras)
                {
                    if (cam.isActiveAndEnabled)
                    {
                        return cam.transform;
                    }
                }
            }

            return null;
        }
    }
}