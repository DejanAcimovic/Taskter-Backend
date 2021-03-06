using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taskter.Api.Contracts
{
    public class ProjectTaskEntryInsertDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProjectTaskId { get; set; }
        [Required]
        public int DurationInMin { get; set; }
        [Required]
        public int Day {get; set;}
        [Required]
        public int Month{get; set;}
        [Required]
        public int Year{get; set;}
        public string Note { get; set; }

    }
}
