using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service
{
    public interface IDocumentService
    {
        DocumentDTO Add(DocumentDTO dto);
        List<DocumentDTO> GetAll();
        DocumentDTO GetById(Guid id);
        void Update(DocumentDTO dto);
        void Delete(DocumentDTO dto);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper<DocumentDTO, Document> _mapper;

        public DocumentService(IDocumentRepository documentRepository, DocumentMapper mapper)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
        }

        public DocumentDTO Add(DocumentDTO dto)
        {
            _documentRepository.Add(_mapper.ToNewDomain(dto));
            return dto;
        }

        public List<DocumentDTO> GetAll()
        {
            return _documentRepository.GetAll()
                .Select(_mapper.ToDTO)
                .ToList();
        }

        public DocumentDTO GetById(Guid id)
        {
            return _mapper.ToDTO(_documentRepository.GetById(id));
        }

        public void Update(DocumentDTO dto)
        {
            var document = _documentRepository.GetById(dto.Id);
            _documentRepository.Update(document);
        }

        public void Delete(DocumentDTO dto)
        {
            _documentRepository.Remove(_documentRepository.GetById(dto.Id));
        }
    }
}

