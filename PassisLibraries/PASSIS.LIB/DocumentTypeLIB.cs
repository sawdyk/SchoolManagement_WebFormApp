using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class DocumentTypeLIB
    {
        public void SaveDocumentType(DocumentType documentType)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.DocumentTypes.InsertOnSubmit(documentType);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DocumentType RetrieveDocumentType(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                DocumentType usr = context.DocumentTypes.SingleOrDefault(s => s.Id == Id);
                return usr;
            }
            catch (Exception ex) { throw ex; }
        }
        public DocumentType RetrieveDocumentTypeByCode(string code)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                DocumentType doc = context.DocumentTypes.SingleOrDefault(s => s.Code.Trim().ToLower().Equals(code.Trim().ToLower()));
                return doc;
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<DocumentType> getAllDocumentTypes(Int64 schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var results = from rs in context.DocumentTypes where (long)rs.SchoolId == schoolId select rs;
                return results.ToList<DocumentType>();
            }
            catch (Exception ex) { throw ex; }
        }
 


    }
}
