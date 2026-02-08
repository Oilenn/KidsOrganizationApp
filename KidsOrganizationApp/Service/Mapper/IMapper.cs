using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.Mapper
{
    public interface IMapper<IDTO, IDomain>
    {
        //todo доделать абстракцию
        public IDTO ConvertToDTO();

    }
}
