namespace Application.Interfaces;

  public interface IUnitOfWork
  {
      Task CommitChangesAsync();
  }

