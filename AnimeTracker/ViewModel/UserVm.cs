namespace AnimeTracker.ViewModel;

public class UserVm
{
    public int Id { get; set; }
    public string UserName { get; set; }
    
    public string UserFullName { get; set; }
    public string UserEmail { get; set; }
    public string UserPassword { get; set; }
    public string RepeatPassword { get; set; }
}