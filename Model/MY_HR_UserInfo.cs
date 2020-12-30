using SqlSugar;

namespace Model
{
    [SugarTable("MY_HR_UserInfo")]//当和数据库名称不一样可以设置别名
    public class MY_HR_UserInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//通过特性设置主键和自增列 
        public string JobNo { get; set; }
        public string Name { get; set; }
    }
}
