using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
   public class ParentStudentMapLIB : UsersLIB
    {
       public void SaveParentStudentMap(ParentStudentMap par)
       {
           try
           {
               PASSISLIBDataContext context = new PASSISLIBDataContext();
               context.ParentStudentMaps.InsertOnSubmit(par);
               context.SubmitChanges();
           }
           catch (Exception ex)
           { throw ex; }
       }
       public ParentStudentMap RetrieveParentStudentMap(long parentId)
       {

           PASSISLIBDataContext context = new PASSISLIBDataContext();
           ParentStudentMap psm = context.ParentStudentMaps.FirstOrDefault(p => p.ParentId == parentId);
               return psm;
       
       }
       public ParentStudentMap RetrieveStudentParent(long studentId)
       {

           PASSISLIBDataContext context = new PASSISLIBDataContext();
           ParentStudentMap psm = context.ParentStudentMaps.FirstOrDefault(p => p.StudentId == studentId);
           return psm;

       }
       public IList<ParentStudentMap> GetParentStudentMap(long studentId)
       {

           PASSISLIBDataContext context = new PASSISLIBDataContext();
           var result = from p in context.ParentStudentMaps where p.StudentId == studentId select p;
           //ParentStudentMap psm = context.ParentStudentMaps.FirstOrDefault(p => p.StudentId == studentId);
           return result.ToList<ParentStudentMap>();

       }
       public IList<User> GetUsers(long studentId)
       {

           PASSISLIBDataContext context = new PASSISLIBDataContext();
           var result = from p in context.Users where p.Id == studentId select p;
           //ParentStudentMap psm = context.ParentStudentMaps.FirstOrDefault(p => p.StudentId == studentId);
           return result.ToList<User>();

       }
    }
}
