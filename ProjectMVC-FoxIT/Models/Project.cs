using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ProjectMVC_FoxIT.Models
{
    public partial class Project
    {
        public Project()
        {
            WorkOrders = new HashSet<WorkOrder>();
        }


        public int ProjectId { get; set; }
        
        [Required]
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
