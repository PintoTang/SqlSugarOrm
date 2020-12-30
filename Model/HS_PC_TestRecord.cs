using SqlSugar;
using System;

namespace Model
{

    [SugarTable("TestRecord")]//当和数据库名称不一样可以设置别名
    public class HS_PC_TestRecord
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//通过特性设置主键和自增列 
        public string No { get; set; }
        public string Sort { get; set; }
        public string Open { get; set; }

        [SugarColumn(ColumnName = "DefaultID")]//数据库列名取自定义
        public string ID { get; set; }

        [SugarColumn(ColumnName = "WriteID")]//数据库列名取自定义
        public string NID { get; set; }
        public string StdTemper { get; set; }
        public string StdHumi { get; set; }
        public string TestTemper { get; set; }
        public string TestHumi { get; set; }
        public string Result { get; set; }
        public string LotNo { get; set; }
        public DateTime WriteTime { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
        public string TempRange { get; set; }
        public string HumiRange { get; set; }
        public string Remark { get; set; }
    }
}
