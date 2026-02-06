using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace KidsOrganizationApp.Repository
{
    public class PersonInfoRepository : IPersonInfoRepository
    {
        private readonly AppDbContext _context;

        public PersonInfoRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(PersonInfo personInfo)
        {
            _context.PersonInfos.Add(personInfo);
            _context.SaveChanges();
        }

        public List<PersonInfo> GetAll()
        {
            return _context.PersonInfos
                .Include(p => p.Contact)
                .Include(p => p.Passport)
                .Include(p => p.SNILS)
                .Include(p => p.DiagnosisFile)
                .ToList();
        }

        public PersonInfo GetById(Guid id)
        {
            return _context.PersonInfos
                .Include(p => p.Contact)
                .Include(p => p.Passport)
                .Include(p => p.SNILS)
                .Include(p => p.DiagnosisFile)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Update(PersonInfo personInfo)
        {
            _context.PersonInfos.Update(personInfo);
            _context.SaveChanges();
        }

        public void Remove(PersonInfo personInfo)
        {
            _context.PersonInfos.Remove(personInfo);
            _context.SaveChanges();
        }
    }
}
