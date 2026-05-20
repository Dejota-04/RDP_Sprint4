using System.Collections.Generic;

namespace ReiDosPiratas.Application.DTOs
{
    public abstract class HateoasResource
    {
        public List<HateoasLink> Links { get; set; } = new List<HateoasLink>();
    }
}