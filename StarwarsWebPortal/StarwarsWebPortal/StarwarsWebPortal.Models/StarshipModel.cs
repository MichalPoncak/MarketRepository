using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarwarsWebPortal.Models
{
    public class StarshipModel
    {
        // fields
        private string consumableRate;
        private int consumableAmount;

        public string Name { get; set; }
        [JsonProperty("MGLT")]
        public string Megalights { get; set; }
        [JsonProperty("consumables")]
        public string ConsumableRate
        {
            get
            {
                return consumableRate;
            }

            set
            {
                if (value != "unknown")
                {
                    // split the value from the API into 2 fields as it contains 2 values
                    consumableAmount = Convert.ToInt32(value.Substring(0, value.IndexOf(' ')));
                    consumableRate = value.Remove(0, value.IndexOf(' ') + 1);

                    // if the rate is in plural form make it singular
                    if (consumableRate[consumableRate.Length - 1] == 's')
                    {
                        consumableRate = consumableRate.TrimEnd(consumableRate[consumableRate.Length - 1]);
                    }
                }
                else
                {
                    consumableRate = "unknown";
                    consumableAmount = 0;
                }
            }
        }
        public int ConsumableAmount
        {
            get
            {
                return consumableAmount;
            }
        }
        public int RequiredResupplies { get; set; }
    }
}
