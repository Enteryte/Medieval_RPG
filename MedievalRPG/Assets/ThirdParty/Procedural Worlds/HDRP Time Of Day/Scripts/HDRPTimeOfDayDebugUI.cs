using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProceduralWorlds.HDRPTOD
{
    [ExecuteAlways]
    public class HDRPTimeOfDayDebugUI : MonoBehaviour
    {
#if HDPipeline && UNITY_2021_2_OR_NEWER
        public static HDRPTimeOfDayDebugUI Instance
        {
            get { return m_instance; }
        }
        [SerializeField] private static HDRPTimeOfDayDebugUI m_instance;

        [Header("Global Settings")]
        public HDRPTimeOfDay TimeOfDay;
        public EventSystem EventSystem;
        [Header("Time Of Day Settings")]
        public Slider m_time;
        public Text m_timeLabel;
        public Toggle m_autoUpdate;
        public Slider m_autoUpdateSpeed;
        public Text m_autoUpdateLabel;
        public Slider m_direction;
        public Text m_directionLabel;
        public Button m_startRain;
        [Header("Lighting Quality")]
        public Dropdown m_lightingRenderMode;

        private bool m_lastValuesSet = false;
        [SerializeField, HideInInspector] private bool m_lastSSGIValue;
        [SerializeField, HideInInspector] private bool m_lastSSRValue;
        [SerializeField, HideInInspector] private bool m_lastContactShadowsValue;

        private void OnEnable()
        {
            m_instance = this;
        }
        private void Awake()
        {
            CheckEventSystem();
        }
        private void Start()
        {
            if (TimeOfDay == null || !Application.isPlaying)
            {
                TimeOfDay = HDRPTimeOfDayAPI.GetTimeOfDay();
            }

            GetLastValues();
            BuildLightingOptions();
            BuildDefaultValues();
        }
        private void OnDisable()
        {
            SetLastValues();
        }
        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (m_time != null && m_timeLabel != null)
            {
                m_time.SetValueWithoutNotify(TimeOfDay.TimeOfDay);
                m_timeLabel.text = $"{m_time.value:0.00}";
            }
        }

        public void SetTimeOfDay()
        {
            if (m_time != null && m_timeLabel != null)
            {
                HDRPTimeOfDayAPI.SetCurrentTime(m_time.value);
                m_timeLabel.text = $"{m_time.value:0.00}";
            }
        }
        public void SetAutoUpdate()
        {
            if (m_autoUpdate != null && m_autoUpdateSpeed != null && m_autoUpdateLabel != null)
            {
                HDRPTimeOfDayAPI.SetAutoUpdateMultiplier(m_autoUpdate.isOn, m_autoUpdateSpeed.value);
                m_autoUpdateLabel.text = $"{m_autoUpdateSpeed.value:0.00}";
            }
        }
        public void SetDirection()
        {
            if (m_direction != null && m_directionLabel != null)
            {
                HDRPTimeOfDayAPI.SetDirection(m_direction.value);
                m_directionLabel.text = $"{m_direction.value:0.00}";
            }
        }
        public void StartStopRain()
        {
            if (HDRPTimeOfDay.Instance.WeatherActive())
            {
                HDRPTimeOfDayAPI.StopWeather(false);
            }
            else
            {
                HDRPTimeOfDayAPI.StartWeather(HDRPTimeOfDayAPI.GetWeatherIDByName("Rain"));
            }
        }
        public void StartStopSnow()
        {
            if (HDRPTimeOfDay.Instance.WeatherActive())
            {
                HDRPTimeOfDayAPI.StopWeather(false);
            }
            else
            {
                HDRPTimeOfDayAPI.StartWeather(HDRPTimeOfDayAPI.GetWeatherIDByName("Snow"));
            }
        }
        public void SetLightingRenderMode()
        {
            if (m_lightingRenderMode != null)
            {
                switch (m_lightingRenderMode.value)
                {
                    case 0:
                    {
                        HDRPTimeOfDayAPI.SetTODBoolFunction(false, TODAPISetFunctions.SSGI);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(false, TODAPISetFunctions.SSR);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(false, TODAPISetFunctions.ContactShadows);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(false, TODAPISetFunctions.ProbeSystem);
                        break;
                    }
                    case 1:
                    {
                        HDRPTimeOfDayAPI.SetTODBoolFunction(true, TODAPISetFunctions.SSGI);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(true, TODAPISetFunctions.SSR);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(true, TODAPISetFunctions.ContactShadows);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(false, TODAPISetFunctions.ProbeSystem);
                        break;
                    }
                    case 2:
                    {
                        HDRPTimeOfDayAPI.SetTODBoolFunction(true, TODAPISetFunctions.SSGI);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(true, TODAPISetFunctions.SSR);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(true, TODAPISetFunctions.ContactShadows);
                        HDRPTimeOfDayAPI.SetTODBoolFunction(true, TODAPISetFunctions.ProbeSystem);
                        break;
                    }
                }

                HDRPTimeOfDayAPI.RefreshTimeOfDay();
            }
        }

        private void BuildLightingOptions()
        {
            if (m_lightingRenderMode != null)
            {
                m_lightingRenderMode.options.Clear();
                List<string> options = new List<string>
                {
                    "Basic",
                    "Simple",
                    "Advanced"
                };
                m_lightingRenderMode.AddOptions(options);
            }
        }
        private void BuildDefaultValues()
        {
            if (TimeOfDay == null)
            {
                return;
            }

            if (m_time != null && m_timeLabel != null)
            {
                m_time.SetValueWithoutNotify(TimeOfDay.TimeOfDay);
                m_timeLabel.text = $"{m_time.value:0.00}";
            }

            HDRPTimeOfDayAPI.GetAutoUpdateMultiplier(out bool autoUpdate, out float updateValue);
            if (m_autoUpdate != null && m_autoUpdateSpeed != null && m_autoUpdateLabel != null)
            {
                m_autoUpdate.SetIsOnWithoutNotify(autoUpdate);
                m_autoUpdateSpeed.SetValueWithoutNotify(updateValue);
                m_autoUpdateLabel.text = $"{updateValue:0.00}";
            }

            if (m_direction != null && m_directionLabel != null)
            {
                m_direction.SetValueWithoutNotify(TimeOfDay.DirectionY);
                m_directionLabel.text = $"{m_direction.value:0.00}";
            }

            if (m_lightingRenderMode != null)
            {
                m_lightingRenderMode.SetValueWithoutNotify(2);
            }
        }
        private void CheckEventSystem()
        {
            if (EventSystem != null)
            {
                EventSystem[] events = FindObjectsOfType<EventSystem>();
                if (events.Length == 1)
                {
                    return;
                }
                EventSystem.enabled = EventSystem != EventSystem.current;
            }
        }
        private void GetLastValues()
        {
            m_lastValuesSet = true;
            m_lastSSGIValue = HDRPTimeOfDayAPI.GetTODBoolFunction(TODAPISetFunctions.SSGI);
            m_lastSSRValue = HDRPTimeOfDayAPI.GetTODBoolFunction(TODAPISetFunctions.SSR);
            m_lastContactShadowsValue = HDRPTimeOfDayAPI.GetTODBoolFunction(TODAPISetFunctions.ContactShadows);
        }
        private void SetLastValues()
        {
            if (!m_lastValuesSet)
            {
                return;
            }

            HDRPTimeOfDayAPI.SetTODBoolFunction(m_lastSSGIValue, TODAPISetFunctions.SSGI);
            HDRPTimeOfDayAPI.SetTODBoolFunction(m_lastSSRValue, TODAPISetFunctions.SSR);
            HDRPTimeOfDayAPI.SetTODBoolFunction(m_lastContactShadowsValue, TODAPISetFunctions.ContactShadows);
        }
#endif
    }
}