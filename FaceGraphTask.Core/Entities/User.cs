using System.Collections.Generic;
using Newtonsoft.Json;

namespace FaceGraphTask.Core.Entities
{
    public class User : BaseEntity
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { set; get; }
        [JsonProperty(PropertyName = "email")]
        public string Email { set; get; }
        [JsonProperty(PropertyName = "password")]
        public string Password { set; get; }
        [JsonProperty(PropertyName = "role")]
        public string Role { set; get; }
        public List<Image> Images { set; get; }
    }
}
