using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;
using System.Linq;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPagedSizes = [5, 10, 15, 30];
    private string[] allowedSortByColumnNames = [nameof(RestaurantDto.Name),
                    nameof(RestaurantDto.Category),
                    nameof(RestaurantDto.Description)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPagedSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", allowPagedSizes)}");

        RuleFor(r => r.SortBy)
            .Must(value => allowedSortByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional, must be in [{string.Join(",", allowedSortByColumnNames)}");
    }
}
