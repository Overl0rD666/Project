using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace УчетПлатежей.Models
{
    public class Payment
    {
        // ID платежа
        public int PaymentId { get; set; }
        // Дата
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        // ФИО
        public string FullName { get; set; }
        // Тип
        public string Type { get; set; }
        // Сумма
        [Required]
        public double Sum { get; set; }

        public List<Position> Positions { get; set; }

    }
}
