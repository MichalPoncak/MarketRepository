using Newtonsoft.Json;
using StarwarsWebPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarwarsWebPortal.ViewModels
{
    public class StarshipViewModel
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public int PreviousPageNumber { get; set; }
        public int CurrentPageNumber { get; set; }
        public int NextPageNumber { get; set; }
        [JsonProperty("results")]
        public List<StarshipModel> Starships { get; set; }
    }
}
