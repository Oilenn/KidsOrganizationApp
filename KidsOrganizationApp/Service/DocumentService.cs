using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;

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

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public DocumentDTO Add(DocumentDTO dto)
        {
            var document = new Document
            {
                DocumentType = dto.DocumentType,
                Path = dto.Path
            };

            _documentRepository.Add(document);
            return ConvertToDTO(document);
        }

        public List<DocumentDTO> GetAll()
        {
            return _documentRepository.GetAll()
                .Select(ConvertToDTO)
                .ToList();
        }

        public DocumentDTO GetById(Guid id)
        {
            return ConvertToDTO(_documentRepository.GetById(id));
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

        private DocumentDTO ConvertToDTO(Document document)
        {
            return new DocumentDTO
            {
                Id = document.Id,
                DocumentType = document.DocumentType,
                Path = document.Path
            };
        }
    }
}

