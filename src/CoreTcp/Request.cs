using System;
using System.Collections.Generic;

namespace CoreTcp
{
    [Serializable]
    public class Request
    {
        public string Method { get; set; }
        public object[] Params { get; set; }
    };
}