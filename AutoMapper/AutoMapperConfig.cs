using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCatalog.MicroService.AutoMapper
{
    public class AutoMapperConfig
    {
        public static Type[] RegisterMappings()
        {
            return new Type[]
            {
                typeof(DomainToViewModelMappingProfile),
                typeof(ViewModelToDomainMappingProfile)
            };
        }
    }
}
