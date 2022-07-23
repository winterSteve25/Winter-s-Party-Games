using System;

namespace Utils.Dictionary
{
    [Serializable]
    public struct DefinitionResponse
    {
        public Meaning[] meanings;
    }

    [Serializable]
    public struct Meaning
    {
        public string partOfSpeech;
        public Definition[] definitions;
    }

    [Serializable]
    public struct Definition
    {
        public string definition;
    }
}