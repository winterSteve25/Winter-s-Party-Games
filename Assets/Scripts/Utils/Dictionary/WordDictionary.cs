using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Utils.Dictionary
{
    public static class WordDictionary
    {
        private const string API_LINK = "https://api.dictionaryapi.dev/api/v2/entries/en/";
        
        public static IEnumerator DefinitionOf(string word, Action<string> callback)
        {
            var uwr = UnityWebRequest.Get(API_LINK + word);
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                yield break;
            }

            if (uwr.responseCode == 404)
            {
                yield break;
            }
            
            var dataStr = uwr.downloadHandler.text;
            if (string.IsNullOrEmpty(dataStr)) yield break;
            
            var data = JsonConvert.DeserializeObject<DefinitionResponse[]>(dataStr);
            callback(data[0].meanings[0].definitions[0].definition);
        }

        public static string GetRandomAdjective()
        {
            return GetRandomWordInFile("Adjectives");
        }

        public static string GetRandomNoun()
        {
            return GetRandomWordInFile("Nouns");
        }

        public static string GetRandomVerb()
        {
            return GetRandomWordInFile("Verbs");
        }
        
        public static string GetRandom(PartOfSpeech partOfSpeech)
        {
            return partOfSpeech switch
            {
                PartOfSpeech.Noun => GetRandomNoun(),
                PartOfSpeech.Adjective => GetRandomAdjective(),
                PartOfSpeech.Verb => GetRandomVerb(),
                _ => throw new ArgumentOutOfRangeException(nameof(partOfSpeech), partOfSpeech, null)
            };
        }

        private static string GetRandomWordInFile(string fileName)
        {
            var text = Resources.Load<TextAsset>("Data/" + fileName);
            var words = text.text.Split("\n");
            return words[Random.Range(0, words.Length)];
        }
    }
}