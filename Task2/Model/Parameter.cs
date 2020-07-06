namespace Task2.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Parameter
    {
        public int ParameterId { get; set; }

        public int TestId { get; set; }

        [Required]
        [StringLength(200)]
        public string ParameterName { get; set; }

        public decimal RequiredValue { get; set; }

        public decimal MeasuredValue { get; set; }

        public virtual Test Test { get; set; }
    }
}
