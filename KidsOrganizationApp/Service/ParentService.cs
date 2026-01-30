using KidsOrganizationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service
{
    public interface IParentService
    {
        List<Parent> GetAllParents();
        List<Parent> GetParentsByCount();
        Parent GetParentById(int id);
        Parent GetParentByName(string name);
        Parent GetParentBySurname(string surname);
        Parent GetParentByPatronomyc(string patronomyc);
    }

    public class ParentService : IParentService
    {
        private AppDbContext _appDbContext;

        public ParentService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Parent AddParent(
            string name,
            string surname,
            string patronomyc,
            DateTime dateBirth
            )
        {
            Parent parent =
                new Parent(name, surname, patronomyc, dateBirth);

            _appDbContext.SaveChanges();
            return parent;
        }

        public Parent GetParentById(int id)
        {
            throw new NotImplementedException();
        }

        public Parent GetParentByName(string name)
        {
            throw new NotImplementedException();
        }

        public Parent GetParentByPatronomyc(string patronomyc)
        {
            throw new NotImplementedException();
        }

        public Parent GetParentBySurname(string surname)
        {
            throw new NotImplementedException();
        }

        public List<Parent> GetAllParents()
        {
            List<Parent> parents = new List<Parent>();
            parents = _appDbContext.Set<Parent>().ToList();

            return parents;
        }

        public List<Parent> GetParentsByCount()
        {
            throw new NotImplementedException();
        }
    }
}
