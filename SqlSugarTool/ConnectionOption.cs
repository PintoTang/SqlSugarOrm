namespace SqlSugarTool
{
    public class ConnectionOption
    {
        //配置节点名称
        public const string DbConfig = "DbConfig";

        public string ConfigName { set; get; }
        public string ConnectionString { set; get; }
        public SqlSugar.DbType DbType { set; get; }
        public bool IsAutoCloseConnection { set; get; }
        public bool Default { set; get; } = true;
    }
}
