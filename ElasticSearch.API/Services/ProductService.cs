using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using ElasticSearch.API.Repositories;
using System.Collections.Immutable;

namespace ElasticSearch.API.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
    {
        var response = await _productRepository.SaveAsync(request.CreateProduct());
        if (response == null)
        {
            return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasında bir hata ile karşılaşıldı." }, System.Net.HttpStatusCode.InternalServerError);
        }
        return ResponseDto<ProductDto>.Success(response.CreateDto(), System.Net.HttpStatusCode.Created);
    }

    public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
    {
        var result = await _productRepository.GetAllAsync();
        var productListDto = result.Select(x => new ProductDto(x.Id, x.Name,x.Price,x.Stock,
            new ProductFeatureDto(x.ProductFeature.Width, x.ProductFeature.Height, x.ProductFeature.Color))).ToList();

        return ResponseDto<List<ProductDto>>.Success(productListDto,System.Net.HttpStatusCode.OK);
    }
}
