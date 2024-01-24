using System.ComponentModel.DataAnnotations;

namespace BlazorShop.Models
{
    public class SendEmailDataModel
    {
        [Required(ErrorMessage = "Введите адрес Email - \"Кому(email-адрес)\"")]
        public string letterEmailAdress { get; set; }

        [Required(ErrorMessage = "Заполните поле: \"Тема письма\"")]
        public string letterSubject { get; set; }

        [Required(ErrorMessage = "Заполните поле: \"Содержимое письма:\"")]
        public string letterBody { get; set; }

    }
}
