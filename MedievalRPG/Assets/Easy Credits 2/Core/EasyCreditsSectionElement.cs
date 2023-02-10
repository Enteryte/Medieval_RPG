using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onerat.EasyCredits {
    [CreateAssetMenu(fileName = "Easy Credits Section Element", menuName = "Easy Credits/Create Section Element", order = 1)]
    public class EasyCreditsSectionElement : ScriptableObject {
        public string SectionTitle;
        public List<SectionData> Content;
    }

    [System.Serializable] public class SectionData
    {
        public string Name;
    }

}

