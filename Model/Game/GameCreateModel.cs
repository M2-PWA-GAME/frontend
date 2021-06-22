using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace frontend.Model.Game
{
    public class GameCreateModel
    {
        [JsonPropertyName("xmax")]
        public int XMax { get; set; }

        [JsonPropertyName("ymax")]
        public int YMax { get; set; }

        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        [JsonPropertyName("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
