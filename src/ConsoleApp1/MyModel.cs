using System.Text.Json.Serialization;
using System.Text.Json;

namespace ConsoleApp1
{
    internal class MyModel
    {
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        public string Id { get; set; }
        public string Name { get; set; }

        //[JsonExtensionData]
        public Dictionary<string, string> Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                // Deserialize the dictionary from the provided JsonElement dictionary
                _settings = new Dictionary<string, string>();
                foreach (var kvp in value)
                {
                    _settings[kvp.Key] = kvp.Value;
                }
            }
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static MyModel FromJson(string json)
        {
            return JsonSerializer.Deserialize<MyModel>(json);
        }
    }
}
