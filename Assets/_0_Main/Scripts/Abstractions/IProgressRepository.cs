public interface IProgressRepository
{
    void Save(GameProgress p);
    GameProgress Load();
}