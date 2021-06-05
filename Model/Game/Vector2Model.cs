using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace frontend.Model.Game
{
    public class Vector2Model
    {
        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("x")]
        public int X { get; set; }
    }
}
