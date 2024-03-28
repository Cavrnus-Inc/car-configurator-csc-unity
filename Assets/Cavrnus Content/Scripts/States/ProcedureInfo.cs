using System;
using System.Collections.Generic;

namespace CavrnusDemo
{
    [Serializable]
    public class ProcedureInfo
    {
        public string ProcedureId;
        public string StepsId;
        
        public List<Step> Steps;
        public Dictionary<int, Step> StepLookup;
    }

    [Serializable]
    public class Step
    {
        public string Title;
    }
}