using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace frontend.Model.Game
{
    public class MapModel
    {
        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        [JsonPropertyName("xmax")]
        public int XMax { get; set; }

        [JsonPropertyName("ymax")]
        public int YMax { get; set; }

        [JsonPropertyName("tiles")]
        public List<TileModel> Tiles { get; set; }
    }
}
