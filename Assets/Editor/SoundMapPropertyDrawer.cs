using UnityEditor;
using Utils.Audio;

namespace Editor
{
    [CustomPropertyDrawer(typeof(SoundMap))]
    public class SoundMapPropertyDrawer : SerializableDictionaryPropertyDrawer
    {
        
    }
}