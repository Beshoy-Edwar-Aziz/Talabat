using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public class ProductWithBrandAndTypeSpec:BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpec(ProductSpec Params)
            :base(P=> 
            (string.IsNullOrEmpty(Params.Search)||P.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue||P.ProductBrandId==Params.BrandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            ) 
        {
            Includes.Add(P=>P.ProductType);
            Includes.Add(P=>P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        ApplyOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        ApplyOrderByDescending(P => P.Price);
                        break;
                    default:
                        ApplyOrderBy(P => P.Name);
                        break;
                }
            }
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
            
        }
        public ProductWithBrandAndTypeSpec(int id) : base(P=>P.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
