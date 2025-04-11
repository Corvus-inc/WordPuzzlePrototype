using System;
using System.Collections.Generic;

namespace Core
{
    [Serializable]
    public class LevelData
    {
        public int id;
        public List<string> words;
        public List<string> clusters;
    }
}