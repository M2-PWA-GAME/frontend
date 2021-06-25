using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace frontend.Model.Game
{
    public class WhosTurnModel
    {
        [JsonPropertyName("whoseTurn")]
        public string WhoseTurn { get; set; }
    }
}
