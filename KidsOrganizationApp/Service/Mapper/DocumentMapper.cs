using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;

namespace KidsOrganizationApp.Service.Mapper
{
    public class DocumentMapper : IMapper<DocumentDTO, Document>
    {
        public DocumentDTO ToDTO(Document document)
        {
            return new DocumentDTO(
                document.Id,
                document.Type,
                document.Path);
        }

        public List<DocumentDTO> ToDTO(List<Document> documents)
        {
            var dto = new List<DocumentDTO>();

            foreach (var document in documents)
                dto.Add(ToDTO(document));

            return dto;
        }

        public Document ToNewDomain(DocumentDTO dto)
        {
            var document = new Document(
                dto.Type,
                dto.Path
            );

            return document;
        }

        public void UpdateDomain(Document domain, DocumentDTO dto)
        {
            domain.ChangePath(dto.Path);
            domain.ChangeType(dto.Type);
        }
    }
}
