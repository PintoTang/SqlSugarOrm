using SqlSugar;

namespace Model
{
    [SugarTable("LogInfo")]
    public class LogInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public string ErrorInfo { get; set; }
    }
}
