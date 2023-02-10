using UnityEngine;
#if HDPipeline
using UnityEngine.Rendering;
#endif

namespace ProceduralWorlds.HDRPTOD
{
    public enum TimeOfDayComponentType { Lighting, PostProcessing, Sun, Moon, SunRotationObject, LocalFog, RayTracedVolume }

    [ExecuteAlways]
    public class HDRPTimeOfDayComponentType : MonoBehaviour
    {
        public TimeOfDayComponentType m_componentType = TimeOfDayComponentType.Lighting;
#if HDPipeline
        public Volume m_componentVolume;
        public VolumeProfile m_volumeProfile;
#endif

        private void OnEnable()
        {
#if HDPipeline
            if (m_componentVolume == null)
            {
                m_componentVolume = GetComponent<Volume>();
            }

            if (m_componentVolume != null)
            {
                m_volumeProfile = m_componentVolume.sharedProfile;
            }
#endif
        }
#if HDPipeline
        public Volume GetVolume()
        {
            if (m_componentVolume == null)
            {
                m_componentVolume = GetComponent<Volume>();
            }

            return m_componentVolume;
        }
#endif
    }
}