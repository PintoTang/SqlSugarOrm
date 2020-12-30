namespace Service
{
    public interface IReceptionService
    {
        dynamic GetDataList();

        int UseTranExample();

        dynamic UseSqlText();

        dynamic UseStoredProcedure();

    }
}
