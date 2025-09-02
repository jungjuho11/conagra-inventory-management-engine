namespace conagra_inventory_management_engine.Common;

public class QueryParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";
    public string? Search { get; set; }
}

public class StoreInventoryQueryParameters : QueryParameters
{
    public int? StoreId { get; set; }
    public int? ProductId { get; set; }
    public string? StoreName { get; set; }
    public string? ProductName { get; set; }
    public bool? BelowThreshold { get; set; }
}

public class InventoryThresholdQueryParameters : QueryParameters
{
    public int? StoreId { get; set; }
    public int? ProductId { get; set; }
    public string? StoreName { get; set; }
    public string? ProductName { get; set; }
}

public class ProductQueryParameters : QueryParameters
{
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
}

public class StoreQueryParameters : QueryParameters
{
    public int? StoreId { get; set; }
    public string? StoreName { get; set; }
    public string? StoreAddress { get; set; }
}

public class WarehouseQueryParameters : QueryParameters
{
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
}
