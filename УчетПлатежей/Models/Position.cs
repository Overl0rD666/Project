using System.ComponentModel.DataAnnotations;

namespace УчетПлатежей.Models
{
    public class Position
    {
        // ID позиции
        public int PositionId { get; set; }

        // Заявленная сумма
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Недопустимый символ.")]
        [Required]
        public double Claim { get; set; }

        // Комментарий
        public string Text { get; set; }

        // Вложенные файлы
        public byte[] FileByte { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        

        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        
    }
}
