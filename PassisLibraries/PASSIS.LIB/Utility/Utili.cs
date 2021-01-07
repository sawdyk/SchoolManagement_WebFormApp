using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.IO;
using System.Security.Cryptography;
using Net.SourceForge.Koogra.Excel;
using System.Web.UI.WebControls;
using System.Collections;
using PASSIS.LIB.CustomClasses;
using System.Data;

namespace PASSIS.LIB.Utility
{
    public  class Utili
    {
        public static IList<StudentScore> RetrieveStudentScoresFromScoreSheet(string filePath, Int64 termId, Int64 academicSessionId, Int64 subjectTeacherId, Int32 subjectId, Int64 schoolId, Int64 campusId)
        {
            IList<StudentScore> scoresFound = new List<StudentScore>();
            Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(filePath);
            Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];
            for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
            {
                try
                {
                    if (rowCount != workSheet.Rows.MinRow)// jump the first row
                    {
                        StudentScore score = new StudentScore();
                        //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                        Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                        //do validation when u hv time 
                        try { score.AdmissionNumber = row.Cells[2].Value.ToString(); }
                        catch { }
                        try { score.ExamScore = Convert.ToInt64(row.Cells[3].Value); }
                        catch { }
                        try { score.TermId = termId; }
                        catch { }
                        try { score.AcademicSessionID = academicSessionId; }
                        catch { }
                        try { score.SubjectTeacherId = subjectTeacherId; }
                        catch { }
                        try { score.SubjectId = subjectId; }
                        catch { }
                        try { score.SchoolId = schoolId; }
                        catch { }
                        try { score.CampusId = campusId; }
                        catch { }
                        scoresFound.Add(score);
                    }
                }
                catch (Exception ex) { throw ex; }

            }
            return scoresFound;
        }
        public static IList<StudentScoreRepository> RetrieveStudentTestAssignmentScoresFromScoresheet(string filePath, Int64 termId, Int64 academicSessionId, Int64 subjectTeacherId, Int32 subjectId, Int64 schoolId, Int64 campusId, int markObtainable, Int64 TemplateId)
        {
            IList<StudentScoreRepository> scoresFound = new List<StudentScoreRepository>();
            Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(filePath);
            Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];
            for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
            {
                try
                {
                    if (rowCount != workSheet.Rows.MinRow)// jump the first row
                    {
                        StudentScoreRepository score = new StudentScoreRepository();
                        //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                        Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                        //do validation when u hv time 
                        try
                        { score.AdmissionNo = row.Cells[2].Value.ToString(); }
                        catch { }
                        try { score.MarkObtained = Convert.ToInt64(row.Cells[3].Value); }
                        catch { }
                        score.TermId = termId;
                        score.SessionId = academicSessionId;
                        score.SubjectId = subjectId;
                        score.CampusId = campusId;
                        score.SchoolId = schoolId;
                        score.SubjectTeacherId = subjectTeacherId;
                        score.MarkObtainable = markObtainable;
                        score.TemplateId = TemplateId;
                        scoresFound.Add(score);
                    }
                }
                catch (Exception ex) { throw ex; }

            }
            return scoresFound;
        }


        public static IList<StudentScoreRepository> RetrieveStudentTestAssignmentScoresFromScoresheetBroad(string filePath, Int64 termId, Int64 academicSessionId, Int64 subjectTeacherId, Int32 subjectId, Int64 schoolId, Int64 campusId, int markObtainable, Int64 TestAssigenmentBroadSheetTemplateID)
        {
            IList<StudentScoreRepository> scoresFound = new List<StudentScoreRepository>();
            Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(filePath);
            Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];
            for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
            {
                try
                {
                    if (rowCount != workSheet.Rows.MinRow)// jump the first row
                    {
                        StudentScoreRepository score = new StudentScoreRepository();
                        //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                        Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                        //do validation when u hv time 
                        try
                        { score.AdmissionNo = row.Cells[2].Value.ToString(); }
                        catch { }
                        try { score.MarkObtained = Convert.ToInt64(row.Cells[3].Value); }
                        catch { }
                        score.TermId = termId;
                        score.SessionId = academicSessionId;
                        score.SubjectId = subjectId;
                        score.CampusId = campusId;
                        score.SchoolId = schoolId;
                        score.SubjectTeacherId = subjectTeacherId;
                        score.MarkObtainable = markObtainable;
                        score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                        scoresFound.Add(score);
                    }
                }
                catch (Exception ex) { throw ex; }

            }
            return scoresFound;
        }
        public static string[] XcelCommentSplitter(string newLineDelimitedString)
        {
            return newLineDelimitedString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        }
        #region  result processor
        public static string getAttainmentMessage(long code)
        {
            string message = string.Empty;
            switch (code)
            {
                case 1:
                    message = "Working towards the standard expected";
                    break;
                case 2:
                    message = "Working towards the standard expected";
                    break;
                case 3:
                    message = "Working at the standard expected";
                    break;
                case 4:
                    message = "Working above the standard expected";
                    break;
                case 5:
                    message = "Working above the standard expected";
                    break;
            }
            return message;
        }
        public static string getExamGradeLetter(decimal sc)
        {
            Int64 score = 0L;
            score = (Int64)sc;
            //Int64.TryParse(sc, out score);
            string result = string.Empty;
            if (score <= 49)
                result = "F";
            else if (score <= 59)
                result = "E";
            else if (score <= 69)
                result = "D";
            else if (score <= 79)
                result = "C";
            else if (score <= 89)
                result = "B";
            else if (score <= 100)
                result = "A";
            else
                result = "NA";
            return result;
        }

        public static string getExamGradeLetters(decimal sc, long schId, long classId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var score = context.ScoreGradeConfigurations.FirstOrDefault(s => s.LowestRange <= sc &&
                            s.HighestRange >= sc &&
                            s.SchoolId == schId &&
                            s.ClassId == classId);
            if (score != null)
            {
                return score.Grade.ToString();
            }
            else 
            {
                return "";
            }
        }

        public static string getExamGradeRemarks(decimal sc, long schId, long classId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            var score = context.ScoreGradeConfigurations.FirstOrDefault(s => s.LowestRange <= sc &&
                            s.HighestRange >= sc &&
                            s.SchoolId == schId &&
                            s.ClassId == classId);


            if (score != null)
            {
                return score.Remark.ToString();
            }
            else
            {
                return "";
            }
        }

        public static string GetSubjectPosition()
        {
           // int y;
            DataTable dt = new DataTable();
            DataView dv = new DataView(dt, "", "Total", DataViewRowState.CurrentRows);
            for (int x = 0; x < dv.Count; x++)
               dv[x].Row["Teacher's Signature"] = x + 1;


            return dv.ToString();
        }
        public static FormMode getFormMode(string str)
        {
            switch (str.ToLower())
            {
                case "edit":
                    return FormMode.Edit;
                case "view":
                    return FormMode.View;
                case "insert":
                    return FormMode.Insert;
                default:
                    return FormMode.View;
            }

        }

        public static string getExamGradeRemark(decimal sc)
        {
            Int64 score = 0L;
            score = (Int64)sc;
            //Int64.TryParse(sc, out score);
            string result = string.Empty;
            if (score <= 49)
                result = "Poor";
            else if (score <= 59)
                result = "Average";
            else if (score <= 69)
                result = "Fairly Good";
            else if (score <= 79)
                result = "Good";
            else if (score <= 89)
                result = "Very Good";
            else if (score <= 100)
                result = "Excellent";
            else
                result = "NA";
            return result;
        }

        public static string getTestGradeLetter(decimal sc)
        {
            Int64 score = 0L;
            score = (Int64)sc;
            string result = string.Empty;
            if (score <= 0)
                result = "";
            else
            {
                if (score <= 19)
                    result = "E";
                else if (score <= 23)
                    result = "D";
                else if (score <= 27)
                    result = "C";
                else if (score <= 31)
                    result = "B";
                else if (score <= 40)
                    result = "A";
            }
            //else
            //    result = "NA";
            return result;
        }
        public static decimal computeSubjectPercentageScore(Int64 test1, Int64 test2, Int64 test3, Int64 examScore)
        {
            decimal a, b, res;
            if (test1 <= 0)
            {
                a = decimal.Divide((test2 + test3), 2);
                b = decimal.Divide((examScore * 60), 100);

            }
            if (test2 <= 0)
            {
                a = decimal.Divide((test1 + test3), 2);
                b = decimal.Divide((examScore * 60), 100);

            }
            if (test3 <= 0)
            {
                a = decimal.Divide((test1 + test2), 2);
                b = decimal.Divide((examScore * 60), 100);

            }
            else
            {
                a = decimal.Divide((test1 + test2 + test3), 3);
                b = decimal.Divide((examScore * 60), 100);

            }
            res = Math.Round(a + b, 1);
            return res;
        }
        public static decimal computeCurrentGPA(Int64 test1, Int64 test2, Int64 test3, Int64 examScore)
        {
            decimal a = 0m;// 
            if (test1 > 0 && test2 > 0 && test3 > 0)
                a = decimal.Divide((test1 + test2 + test3), 3);
            if (test3 == 0)
                a = decimal.Divide((test1 + test2), 2);
            else
                if (test2 == 0 && test3 == 0)
                    a = test1;
            decimal b = decimal.Divide((examScore * 60), 100);
            decimal res = decimal.Divide((a + b), 20);
            return Math.Round(res, 2);
        }
        public static decimal computeCurrentGPA(Int64 test1, Int64 test2, Int64 examScore)
        {
            decimal a = decimal.Divide((test1 + test2), 2);
            decimal b = decimal.Divide((examScore * 60), 100);
            decimal res = decimal.Divide((a + b), 20);
            return Math.Round(res, 2);
        }
        public static ResultSummaryStatistics getResultSummaryForASubject(IList<ResultProperties> resultList)
        {
            ResultSummaryStatistics resSum = new ResultSummaryStatistics();
            resSum.TotalNumberOfStudents = resultList.Count;
            decimal femalTotalScore = 0m, maleTotalScore = 0m;
            long femaleCount = 0L, maleCount = 0L;

            resSum.StudentWithA = resSum.StudentWithB = resSum.StudentWithC = resSum.StudentWithD = resSum.StudentWithE = 0L;
            foreach (ResultProperties propt in resultList)
            {
                switch (GenderNumberToAlphabet((Int32)propt.gender))
                {
                    case "F":
                        femalTotalScore += propt.score;
                        ++femaleCount;
                        break;
                    case "M":
                        maleTotalScore += propt.score;
                        ++maleCount;
                        break;
                }

                switch (getExamGradeLetter(propt.score))
                {
                    case "A":
                        resSum.StudentWithA += 1;
                        break;
                    case "B":
                        resSum.StudentWithB += 1;
                        break;
                    case "C":
                        resSum.StudentWithC += 1;
                        break;
                    case "D":
                        resSum.StudentWithD += 1;
                        break;
                    case "E":
                        resSum.StudentWithE += 1;
                        break;
                }
            }
            resSum.MaleAverageScore = Math.Round(Convert.ToDecimal(1) / Convert.ToDecimal(1), 1);
            resSum.FemaleAverageScore = Math.Round(Convert.ToDecimal(1) / Convert.ToDecimal(1), 1);
            resSum.SubjectAverageScore = Math.Round((Convert.ToDecimal(1) / Convert.ToDecimal(1) / Convert.ToDecimal(1) / Convert.ToDecimal(1)), 1);

            // prepare other stuff 
            return resSum;
        }
        public static Dictionary<Int64, ResultSummaryStatistics> ComputeResultStatistics(Dictionary<Int64, IList<ResultProperties>> resultProp)
        {
            Dictionary<Int64, ResultSummaryStatistics> summaryStat = new Dictionary<long, ResultSummaryStatistics>();
            foreach (var pair in resultProp)
            {
                //insert key in dictionary, computer the various  object
                ResultSummaryStatistics summary = new ResultSummaryStatistics();
                summary = getResultSummaryForASubject(pair.Value);
                summaryStat.Add(pair.Key, summary);
            }
            return summaryStat;
        }
        #endregion
        public static string GenderNumberToAlphabet(Int32 gender)
        {
            string res = string.Empty;
            switch (gender)
            {
                case 1:
                    res = "M";
                    break;
                case 2:
                    res = "F";
                    break;
            }
            return res;
        }
        public static string getAccommodationType(string accType)
        {
            string result = string.Empty;
            switch (accType.ToLower())
            {
                case "d":
                    result = "Day Student";
                    break;
                case "b":
                    result = "Boarder";
                    break;
                default:
                    result = "Day Student";
                    break;
            }
            return result;
        }
        public static string getCellFromExcelRow(Net.SourceForge.Koogra.Excel2007.Row row, uint column)
        {
            try { return row.GetCell(column).Value.ToString().Trim(); }
            catch { return ""; }
        }
        public static string getCellFromExcelRow(Net.SourceForge.Koogra.Excel2007.Row row, uint column1, uint column2)
        {
            try { return row.GetCell(column1).Value.ToString().Trim() + row.GetCell(column2).Value.ToString().Trim(); }
            catch { return ""; }
        }
        public static Gender stringToGenderEnum(string genderString)
        {
            Gender enumObj = Gender.Male;
            switch (genderString.Trim().ToLower())
            {
                case "f":
                    enumObj = Gender.Female;
                    break;
                case "m":
                    enumObj = Gender.Male;
                    break;
            }
            return enumObj;
        }
        public static IList<User> RetrieveStudentsFromExcelFile(String filePath, long schlId, long campusId)
        {
            IList<User> studentsFound = new List<User>();

                Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(filePath);
                Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];

                for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
                {
                    //Net.SourceForge.Koogra.Excel2007.Row row = workSheet.GetRow(rowCount);
                    try
                    {
                        if (rowCount != workSheet.Rows.MinRow) //jump the first row
                        {
                            //if (!string.IsNullOrEmpty(getCellFromExcelRow(row, 5)) || !string.IsNullOrEmpty(getCellFromExcelRow(row, 6)))
                            //if (!string.IsNullOrEmpty(getCellFromExcelRow(row, 6)))
                            Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];

                            if (true)
                            {
                                User stdnt = new User();
                                //Net.SourceForge.Koogra.Excel2007.Row row = workSheet.GetRow(rowCount);
                                stdnt.AccomodationType = getAccommodationType(row.Cells[1].Value.ToString());
                                stdnt.AdmissionNumber = row.Cells[2].Value.ToString();
                                stdnt.Username = stdnt.LastName = row.Cells[3].Value.ToString();
                                stdnt.Password = "password";
                                stdnt.FirstName = row.Cells[4].Value.ToString();
                                stdnt.MiddleName = row.Cells[5].Value.ToString();
                                stdnt.Gender = (int)stringToGenderEnum(row.Cells[6].Value.ToString());
                                //grade ommitted
                                //stdnt.GradeString = RemoveSpecialCharacters(getCellFromExcelRow(row, 7));
                                //stdnt.HomeRoomTutor = getCellFromExcelRow(row, 8);
                                stdnt.SchoolId = schlId;
                                stdnt.SchoolCampusId = campusId;
                                stdnt.BusRoute = row.Cells[9].Value.ToString();

                                try { Double d = Double.Parse(row.Cells[10].Value.ToString()); stdnt.DateOfBirth = DateTime.FromOADate(d); }
                                catch { stdnt.DateOfBirth = SqlDateTime.MinValue.Value; }
                                stdnt.SchoolHouse = row.Cells[11].Value.ToString();
                                stdnt.SchoolHouseParentName = row.Cells[12].Value.ToString();
                                stdnt.LastSchoolAttended = row.Cells[13].Value.ToString();
                                stdnt.PhoneNumber = row.Cells[14].Value.ToString();
                                stdnt.StreetAddress = row.Cells[15].Value.ToString();
                                stdnt.City = row.Cells[16].Value.ToString();
                                stdnt.State = row.Cells[17].Value.ToString();
                                stdnt.FathersTitle = row.Cells[18].Value.ToString();
                                stdnt.FathersName = row.Cells[19].Value.ToString();
                                stdnt.FathersPhoneNumber = row.Cells[20].Value.ToString();
                                stdnt.FathersWorkAddress = row.Cells[21].Value.ToString();
                                stdnt.FathersEmail = row.Cells[22].Value.ToString();
                                stdnt.FathersNationality = row.Cells[23].Value.ToString();
                                stdnt.MothersName = row.Cells[24].Value.ToString();
                                stdnt.MothersOtherName = row.Cells[25].Value.ToString();
                                stdnt.MothersWorkAddress = row.Cells[26].Value.ToString();
                                stdnt.MothersPhoneNumber = row.Cells[27].Value.ToString();
                                stdnt.MothersEmail = row.Cells[28].Value.ToString();
                                stdnt.MothersNationality = row.Cells[29].Value.ToString();
                                stdnt.GuardianDetails = row.Cells[30].Value.ToString();
                                stdnt.GuardianPhoneNumber = row.Cells[31].Value.ToString();
                                stdnt.GuardianEmail = row.Cells[32].Value.ToString();
                                stdnt.GuardianRelationship = row.Cells[33].Value.ToString();
                                stdnt.SpecialNoteAlert = row.Cells[34].Value.ToString();
                                stdnt.Siblings = row.Cells[35].Value.ToString();
                                stdnt.DateCreated = stdnt.LastLoginDate = DateTime.Now;
                                stdnt.IsLearningSupport = 0;//not leaning support
                                studentsFound.Add(stdnt);

                            }
                        }
                    }
                    catch (Exception ex) { throw ex; }
                }
            return studentsFound;
        }
        protected static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9%._]", string.Empty);

        }
        public static string ProcessRetrievedStudentInfo(IList<User> userList, Int64 schlId, Int64 campsId, log4net.ILog logger)
        {
            //variables to change 
            string campusCode = new AcademicSessionLIB().RetrieveSchoolCampus(campsId).Code;
            Int64 userIdTemp = 1; // class grade usersiD meant for admin

            StringBuilder log = new StringBuilder();
            log.Append("Total numbers of Students found = ").Append(userList.Count).AppendLine();
            UsersLIB usrDAL = new UsersLIB();
            ClassGradeLIB clsGrdDal = new ClassGradeLIB();
            //matric number processing not yet done 
            IList<User> LsStudentsToBeTreatedLatter = new List<User>();
            IList<User> IbStudentsToBeTreatedLatter = new List<User>();
            logger.InfoFormat("about starting.....{0} users to be processed", userList.Count);
            Int32 studentsCreated = 0;
            foreach (User usr in userList)
            {  //save the users info
                try
                {
                    //logger.InfoFormat("processing  {0} {1} {2}  {3}  {4}", usr.FirstName, usr.LastName, usr.GradeString, usr.HomeRoomTutor, usr.FullName);

                            //usr.MatricNumber = GenerateMatricNumber(campusCode, usr.GradeString);
                            if (new UsersLIB().RetrieveUsersByName(usr.Username).Count > 0)
                            {
                                usr.Username = string.Format("{0}{1}", usr.Username, DateTime.Now.ToString("mmss"));
                            }

                            new UsersLIB().SaveUser(usr);
                            //save student role
                            UserRole usrRol = new UserRole();
                            usrRol.RoleId = (long)Utility.roles.student; //student
                            usrRol.UserId = usr.Id;
                            new UsersLIB().SaveUserRole(usrRol);
                            ++studentsCreated;

                            ////save student save grade class teacher 
                            //GradeStudent grdStudent = new GradeStudent();
                            //grdStudent.ClassId = gradeFound.ClassId;
                            //grdStudent.GradeId = gradeFound.Id;
                            //grdStudent.GradeTeacherId = (long)gradeFound.GradeTeacherId;
                            //grdStudent.StudentId = usr.Id;
                            //grdStudent.SessionId = usr.Id;///invalid , use real values
                            //grdStudent.DateCreated = DateTime.Now;
                            //new ClassGradeDAL().SaveGradeStudent(grdStudent);

                            //logger.InfoFormat("processing completed for  {0} {1} {2}  {3}  {4}", usr.FirstName, usr.LastName, usr.GradeString, usr.HomeRoomTutor, usr.FullName);

                            // 

                            //persist studentID, GradeClassTeacherInfoId,AcademicSessionId, 


                }
                catch (Exception ex)
                {
                    //logger.InfoFormat("Exception occurrred while processing this  {0} {1} {2}  {3}  {4}   eXCEPTION ::::{5}", usr.FirstName, usr.LastName, usr.GradeString, usr.HomeRoomTutor, usr.FullName, ex.Message);

                }

            }
            //treat student to be treated latter Ilist LS
            log.Append("Total numbers of LS users found = ").Append(LsStudentsToBeTreatedLatter.Count).AppendLine();
            foreach (User std2btrtedl8r in LsStudentsToBeTreatedLatter)
            {

            }
            log.Append("Total numbers of IB users found = ").Append(IbStudentsToBeTreatedLatter.Count).AppendLine();

            //treat student to be treated latter Ilist IB
            foreach (User std2btrtedl8r in IbStudentsToBeTreatedLatter)
            {
                //get matric number, 
            }
            log.Append("Total numbers of users inserted into DB  = ").Append(userList.Count - LsStudentsToBeTreatedLatter.Count - IbStudentsToBeTreatedLatter.Count).AppendLine();

            logger.InfoFormat("summary {0} ===={1}", log.ToString(), log.ToString());
            return string.Format("{0} Students have been created.", studentsCreated);
        }
        public static void migrateParents(log4net.ILog logger, IList<PASSIS.LIB.User> allStudents)
        {
            /*retrieve all students from userrroles 
            //foreach parents
                   move from users tp [parentdetails 
             * check if the fatherseail address exists 
             * if yes
             *     create the parent using emailaddress as its username
             *      update the users parentid 
             *      update the userrole
             *    no 
             *       update the users parentid 
             * 
             * 
             * 
             
             */

            logger.InfoFormat("....found {0} students", allStudents.Count);
            foreach (User usr in allStudents)
            {
                try
                {
                    //check to know whether the fathers email is null
                    string searchObj = string.Empty;
                    searchObj = usr.FathersEmail.Trim().ToLower();

                    if (string.IsNullOrEmpty(searchObj))
                    {
                        if (!string.IsNullOrEmpty(usr.MothersEmail))//look for other string to uniquely identify the father
                            searchObj = usr.MothersEmail.Trim().ToLower();
                        else
                        {
                            searchObj = getRandomUsername();
                        }
                    }
                    if (searchObj.Contains("@"))
                    {
                        if (searchObj.Split('@').Length > 2)// it has many email address
                        {
                            //get the first email address from the list 
                            searchObj = RetrieveEmailAddressesFromAnyString(searchObj)[0];
                        }
                    }

                    IList<User> father = new UsersLIB().RetrieveUsersByName(searchObj);
                    if (father.Count == 0)
                    {//
                        logger.InfoFormat("parent not yet created ");
                        ParentDetail pd = new ParentDetail();
                        pd.FathersEmail = usr.FathersEmail;
                        pd.FathersName = usr.FathersName;
                        pd.FathersNationality = usr.FathersNationality;
                        pd.FathersPhoneNumber = usr.FathersPhoneNumber;
                        pd.FathersTitle = usr.FathersTitle;
                        pd.FathersWorkAddress = usr.FathersWorkAddress;
                        pd.MothersEmail = usr.MothersEmail;
                        pd.MothersName = usr.MothersName;
                        pd.MothersNationality = usr.MothersNationality;
                        pd.MothersOtherName = usr.MothersOtherName;
                        pd.MothersWorkAddress = usr.MothersWorkAddress;
                        pd.Siblings = usr.Siblings;
                        pd.GuardianDetails = usr.GuardianDetails;
                        pd.GuardianEmail = usr.GuardianEmail;
                        pd.GuardianPhoneNumber = usr.GuardianPhoneNumber;
                        pd.GuardianRelationship = usr.GuardianRelationship;
                        new UsersLIB().SaveParentDetail(pd);

                        User newParent = new User();
                        newParent.Username = searchObj;/// could be father/mother's email 
                        newParent.DateOfBirth = newParent.LastLoginDate = (DateTime)DateTime.Now;
                        newParent.DateCreated = (DateTime)DateTime.Now;
                        newParent.Password = Utility.Utili.getDefaultPassword();
                        newParent.Gender = usr.FathersEmail == "" ? (Int32)Utility.Gender.Female : (Int32)Utility.Gender.Male;
                        newParent.SchoolCampusId = usr.SchoolCampusId;
                        newParent.ParentId = pd.Id;
                        new UsersLIB().SaveUser(newParent);

                        UserRole usrRol = new UserRole();
                        usrRol.RoleId = (long)Utility.roles.parent;
                        usrRol.UserId = newParent.Id;
                        new UsersLIB().SaveUserRole(usrRol);
                        //tell d kid his parent 
                        ParentStudentMap psm = new ParentStudentMap();
                        psm.ParentId = pd.Id;
                        psm.ParentUserId = newParent.Id;
                        psm.StudentId = usr.Id;
                        new ParentStudentMapLIB().SaveParentStudentMap(psm);


                        usr.ParentId = pd.Id;
                        new UsersLIB().UpdateUser(usr);
                        logger.InfoFormat("just created new parent for  student with Id {0}.....fullname {1}", usr.Id, usr.StudentFullName);
                    }
                    else
                    {//
                        logger.InfoFormat("already exist , tell himm is parent");
                        usr.ParentId = father[0].ParentId;
                        new UsersLIB().UpdateUser(usr);

                        ParentStudentMap psm = new ParentStudentMap();
                        psm.ParentId = father[0].ParentId;
                        psm.StudentId = usr.Id;
                        psm.ParentUserId = father[0].Id;
                        new ParentStudentMapLIB().SaveParentStudentMap(psm);
                        logger.InfoFormat("has existing parent:::: student with Id {0}.....fullname {1}", usr.Id, usr.StudentFullName);

                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    logger.InfoFormat("error occurred on userId = {0} , message = {1}", usr.Id, ex.Message);
                }
            }
        }
        public static string getRandomUsername()
        {
            return string.Format("Identity{0}", Utili.GetUniqueRandomNumber(10));
        }
        public static string[] RetrieveEmailAddressesFromAnyString(string text)
        {
            const string MatchEmailPattern =
           @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
             + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // Find matches.
            MatchCollection matches = rx.Matches(text);
            // Report the number of matches found.
            int noOfMatches = matches.Count;
            // Report on each match.
            string[] results = new string[noOfMatches];
            int c = 0;
            foreach (Match match in matches)
            {
                results[c] = match.Value.ToString();
                c++;
            }
            return results;
        }
        public static string GetUniqueRandomNumber(object objlength)
        {
            int length = Convert.ToInt32(objlength);
            var number = rng.NextDouble().ToString("0.000000000000").Substring(2, length);
            return number;
        }
        #region password
        /// <summary>
        /// Under construction 
        /// </summary>
        /// <returns></returns>
        public static string getDefaultPassword()
        {
            return "password";
        }

        #endregion
        private static Random rng = new Random(Environment.TickCount);
        public static string GenerateAdmissionNumber(string schoolCode, Int64 schoolId)
        {
            //prefix the lenght to be 7 temporarily   [LK,AT] [ 09,12,..year- current year] [5 digit  number based on db count]
            return new StringBuilder().Append(schoolCode.ToUpper()).Append(AppendZeroToNumber(new UsersLIB().getAllUsersPerCampus(roles.student, schoolId).Count + 1, 6)).ToString();
        }
        public static string AppendZeroToNumber(Int64 ActualNumber, int requiredLenght)
        {
            //ActualNumber.ToString().
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < (requiredLenght - ActualNumber.ToString().Length); ++i)
            {
                str.Append("0");
            }
            return str.Append(ActualNumber.ToString()).ToString();
        }
    }
}
