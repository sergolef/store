using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithBrandsAndTypesSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandsAndTypesSpecification(string sort, int? brandId, int? typeId):
        base(x => 
            (!brandId.HasValue || x.ProductBrandId == brandId) &&
            (!typeId.HasValue || x.ProductTypeId == typeId)
        )
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);

            if(!string.IsNullOrEmpty(sort)){
                switch (sort) {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                    break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                    break;

                    default:
                        AddOrderBy(p => p.Name);
                    break;
                }
            }
        }

        public ProductWithBrandsAndTypesSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
        }
    }
}