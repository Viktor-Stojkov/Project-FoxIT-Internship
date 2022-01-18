using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectMVC_FoxIT.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Projects = new HashSet<Project>();
            WorkOrders = new HashSet<WorkOrder>();
        }

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

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
