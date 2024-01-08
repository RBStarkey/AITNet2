using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AitNet2.Models
{
    public class DDL
    {
        [JsonProperty(PropertyName = "id")]
        [DisplayName("List ID")]
        [Required(ErrorMessage = "List Name is required")]
        [StringLength(50)]
        public string Id { get; set; }  //String to allow meaningful name

        [Required(ErrorMessage = "Document Type is required")]
        [DisplayName("Document Type")]
        public string DocumentType { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Date Amended is required")]
        [DisplayName("Date Amended")]
        public DateTime DateAmended { get; set; }

        [Required(ErrorMessage = "Amended By is required")]
        [DisplayName("Amended By")]
        public string AmendedBy { get; set; }

        [DisplayName("DDL List")]
        public List<string> ElementList { get; set; }
    }
}
