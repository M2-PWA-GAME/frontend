using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace frontend.Model.Game
{
    public class GameGeneratorModel
    {
        [JsonPropertyName("ymax")]
        public int YMax { get; set; }

        [JsonPropertyName("xmax")]
        public int XMax { get; set; }

        [JsonPropertyName("seed")]
        public int Seed { get; set; }
    }
}
