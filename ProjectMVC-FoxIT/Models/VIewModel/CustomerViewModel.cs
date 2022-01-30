using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ProjectMVC_FoxIT.Models.VIewModel
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string Edb { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public Project Project { get; set; }
        public WorkOrder WorkOrder { get; set; }
        public List<CustomerViewModel> Customers { get; set; }

        public SelectList CustomerNames { get; set; }
        public SelectList CustomerAddress { get; set; }
        public SelectList CustomerEdbs { get; set; }
    }
}
