using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ProjectMVC_FoxIT.Models.VIewModel
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public WorkOrder WorkOrder { get; set; }
        public Customer Customer { get; set; }
        public List<ProjectViewModel> Projects { get; set; }

        public SelectList ProjectNames { get; set; }
        public SelectList Customers { get; set; }
    }
}
