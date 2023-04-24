using WebApiMaxine.DTO;

namespace WebApiMaxine.Services
{

    public class FilesSortService
    {
        public void DefinationSort(FileInfo[] fileList)
        {
            var sortedList = new List<string>()
            {
                "ORG_DEPART",
                "EMP_EMPLOYEE",
                "EMP_JOB_CODE",
                "ORG_DEP_MGR",
                "EMP_PROFILE",
                "SAL_PROFILE",
                "EMP_EXPERIENCE",
                "EMP_EXP_PCC",
                "PER_CERT_CODE",
                "TSF_RESERVE",
                "TSF_LEAVE"
            };

            var fileInfo = new List<FilesInfoDTO>();
            foreach (FileInfo file in fileList)
            {
                string fullName = file.FullName;
                string dataName = file.Name.Substring(0, file.Name.IndexOf($"_CDIB"));
                int id = sortedList.FindIndex(c => c == dataName);
                fileInfo.Add(new FilesInfoDTO() { Id = id, Name = file.Name, DataName = dataName, FullName = fullName });
            }
            fileInfo.Sort();

            //return fileInfo;
        }
    }
}
