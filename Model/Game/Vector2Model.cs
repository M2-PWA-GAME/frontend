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

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
        public bool Equals(Vector2Model obj)
        {
            return Y == obj.Y && X == obj.X;
        }
    }
}
