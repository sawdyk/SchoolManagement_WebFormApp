using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class ResourceCenterLIB
    {
        public void SaveResourceCenter(ResourceCenter resourceCenter)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.ResourceCenters.InsertOnSubmit(resourceCenter);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ResourceCenter RetrieveResourceCenter(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                ResourceCenter usr = context.ResourceCenters.SingleOrDefault(s => s.Id == Id);
                return usr;
            }
            catch (Exception ex) { throw ex; }
        }
     
        public IList<ResourceCenter> getAllResourceCenters(Int64 schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var results = from rs in context.ResourceCenters where (long)rs.SchoolId == schoolId select rs;
                return results.ToList<ResourceCenter>();
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<ResourceCenter> getAllResourceCenters(Int64 schoolId, string filename, long docTypeId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var results = from rs in context.ResourceCenters select rs;
                if (schoolId > 0)
                    results = results.Where(s => s.SchoolId == schoolId);
                if (!string.IsNullOrEmpty(filename))
                    results = results.Where(f => f.FileName.Contains(filename));
                if (docTypeId > 0)
                    results = results.Where(d => d.DocumentType == docTypeId); 
                return results.ToList<ResourceCenter>();
            }
            catch (Exception ex) { throw ex; }
        }


    }
}
