using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectMVC_FoxIT.Models
{
    public partial class WorkOrder
    {
        public int WorkOrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public string CustomerNote { get; set; }
        public string PerformedWorks { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Project Project { get; set; }
    }
}
