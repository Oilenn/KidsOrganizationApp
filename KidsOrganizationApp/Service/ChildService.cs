using KidsOrganizationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service
{
    public interface IChildService
    {
        Child AddChild(string name,
                    string surname,
                    string patronomyc,
                    DateTime dateBirth);

        List<Child> GetAllChildren();
        List<Child> GetChildrenByCount(int start, int end);
        Child GetChildById(int id);
        Child GetChildByName(string name);
        Child GetChildBySurname(string surname);
        Child GetChildByPatronomyc(string patronomyc);
    }

    public class ChildService : IChildService
    {
        private AppDbContext _appDbContext;

        public ChildService(AppDbContext appDbContext) 
        { 
            _appDbContext = appDbContext;
        }

        public Child AddChild(
            string name, 
            string surname, 
            string patronomyc,
            DateTime dateBirth
            )
        {
            Child child = 
                new Child(name, surname, patronomyc, dateBirth);

            _appDbContext.Children.Add(child);

            _appDbContext.SaveChanges();
            return child;
        }

        public List<Child> GetAllChildren()
        {
            List<Child> children = new List<Child>();
            children = _appDbContext.Set<Child>().ToList();

            return children;
        }

        public Child GetChildById(int id)
        {
            throw new NotImplementedException();
        }

        public Child GetChildByName(string name)
        {
            throw new NotImplementedException();
        }

        public Child GetChildByPatronomyc(string patronomyc)
        {
            throw new NotImplementedException();
        }

        public Child GetChildBySurname(string surname)
        {
            throw new NotImplementedException();
        }

        public List<Child> GetChildrenByCount(int start, int end)
        {
            throw new NotImplementedException();
        }
    }
}
