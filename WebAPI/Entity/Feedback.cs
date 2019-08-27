using NoorCare;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Entity
{
    [Serializable]
    [Table("Feedback")]
    public class Feedback : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string FeedbackID { get; set; }
        [MaxLength(50)]
        public string DoctorID { get; set; }
        [MaxLength(50)]
        public string ClientID { get; set; }
        [MaxLength(1000)]
        public string FeedbackDetails { get; set; }

        public bool RecommendedDoctor { get; set; }
        [MaxLength(300)]
        public string Recommended { get; set; }

        public bool ILike { get; set; }

        public bool IsDeleted { get; set; }
        [MaxLength(128)]
        public string CreatedBy { get; set; }
        [MaxLength(128)]
        public string ModifiedBy { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime DateModified { get; set; }
    }
}