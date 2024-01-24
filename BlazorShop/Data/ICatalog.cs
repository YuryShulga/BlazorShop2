namespace BlazorShop.Models
{
	public interface ICatalog
	{
		public Task<IReadOnlyCollection<Product>> GetProducts();
		public Task<Product?> GetProductById(int id);
	}
}