using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories;

 
public class CreateCategoryRequest : Request
{
    [Required(ErrorMessage = "Invalid Title")]
    [MaxLength(80, ErrorMessage = "Title must contain a maximum 80 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Invalid Description")]
    public string Description { get; set; } = string.Empty;
}