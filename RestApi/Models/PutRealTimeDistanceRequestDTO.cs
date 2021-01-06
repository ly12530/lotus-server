using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models
{
    public class PutRealTimeDistanceRequestDTO 
    {
        [Required]
        [RegularExpression("^(\\d{2}):(\\d{2})$")]
        public string RealStartTime { get; set; }

        [Required]
        [RegularExpression("^(\\d{2}):(\\d{2})$")]
        public string RealEndTime { get; set; }

        [Required]
        public int DistanceTraveled { get; set; }
    }
}
