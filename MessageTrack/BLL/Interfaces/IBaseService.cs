namespace MessageTrack.BLL.Interfaces
{
    public interface IBaseService
    {
        void Commit();
        void Rollback();
    }
}
