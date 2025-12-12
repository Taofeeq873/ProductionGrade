namespace Application.Dtos;

public class BaseResponse
{
    public Guid? Id { get; set; }
    public string Message { get; set; } = default!;
    public string? DisplayValue { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}