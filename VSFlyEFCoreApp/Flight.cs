using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VSFlyEFCoreApp
{
    public class Flight
    {
        [Key]
        public int FlightNo { get; set; }
        [StringLength(50), MinLength(3)]
        public string Departure { get; set; }
        [StringLength(50), MinLength(3)]
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public short? Seats { get; set; }

        [Required]
        public double BasePrice { get; set; }

        [ForeignKey("PilotId")]
        public virtual Pilot Pilot { get; set; }
        public int PilotId { get; set; }

        public virtual ICollection<Booking> BookingSet { get; set; }

        public Flight() { 
        }
    }
}
