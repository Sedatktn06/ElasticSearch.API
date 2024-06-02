using ElasticSearch.API.Models;
using Nest;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _elasticClient;
    private const string indexName = "products";
    public ProductRepository(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<Product?> SaveAsync(Product product)
    {
        product.CreatedAt = DateTime.Now;

        var response = await _elasticClient.IndexAsync(product, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));

        if (!response.IsValid) return null;

        product.Id = response.Id;

        return product;
    }

    public async Task<ImmutableList<Product>> GetAllAsync()
    {
        var result = await _elasticClient.SearchAsync<Product>(
            s => s.Index(indexName).Query(q => q.MatchAll()));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }
}
