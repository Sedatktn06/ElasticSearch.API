using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs;

public record ProductCreateDto(string Name,decimal Price,int Stock,ProductFeatureDto ProductFeatureDto)
{
    public Product CreateProduct()
    {
        return new Product
        {
            Name = Name,
            Price = Price,
            Stock = Stock,
            ProductFeature = new ProductFeature
            {
                Color = ProductFeatureDto.Color,
                Height = ProductFeatureDto.Height,
                Width = ProductFeatureDto.Width,
            },
        };
    }
}
