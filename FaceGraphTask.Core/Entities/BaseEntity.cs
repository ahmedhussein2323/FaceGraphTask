using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FaceGraphTask.Core.Entities
{
    public class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { set; get; }
    }
}
