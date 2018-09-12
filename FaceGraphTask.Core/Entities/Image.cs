using System;
using Newtonsoft.Json;

namespace FaceGraphTask.Core.Entities
{
    public class Image : BaseEntity
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { set; get; }
        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { set; get; }
    }
}
