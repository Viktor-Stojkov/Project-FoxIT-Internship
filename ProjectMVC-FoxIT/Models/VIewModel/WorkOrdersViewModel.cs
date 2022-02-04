using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC_FoxIT.Models.VIewModel
{
    public class WorkOrdersViewModel
    {
        public int WorkOrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public string UserId { get; set; }

        //[DisplayFormat(DataFormatString = @"{0:MM\/dd\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }
        public string CustomerNote { get; set; }
        public string PerformedWorks { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public bool? IsActive { get; set; }

        public CustomerViewModel Customer { get; set; }
        public ProjectViewModel Project { get; set; }
        public List<WorkOrdersViewModel> WorkOrders { get; set; }

        public SelectList Customers { get; set; }
        public SelectList Projects { get; set; }
        public SelectList? Users { get; set; }
        public SelectList CustomerNotes { get; set; }
        public SelectList PerformedWorksModel { get; set; }
    }
}
