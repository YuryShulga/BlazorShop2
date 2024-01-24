using System.ComponentModel.DataAnnotations;

namespace BlazorShop;

public class Product
{
	public int Id { get; set; }

	[Required(ErrorMessage = "Название должно быть заполнено")]
	public string Name { get; set; }
	
	[Required(ErrorMessage = "Описание должно быть заполнено")]
	public string Description { get; set; }

	[Required(ErrorMessage = "Цена должна быть заполнена")]
	[Range(0, 1_000_000, ErrorMessage = "Цена должна быть от 0 до 1000000")]
	public decimal Price { get; set; }
}