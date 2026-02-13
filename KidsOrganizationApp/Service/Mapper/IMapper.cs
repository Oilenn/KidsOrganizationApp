using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.Mapper
{
    public interface IMapper<TDto, TDomain>
    {
        public TDto ToDTO(TDomain domain);
        public List<TDto> ToDTO(List<TDomain> domain);

        public TDomain ToNewDomain(TDto dto);
    }
}

