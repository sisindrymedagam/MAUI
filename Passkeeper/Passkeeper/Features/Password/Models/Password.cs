using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Passkeeper.Features.Password.Models;

[Table("Passwords")]
public class Password : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    private string url = string.Empty;
    [Column("Url")]
    public string Url
    {
        get => url;
        set { if (url != value) { url = value; OnPropertyChanged(); } }
    }

    private string username = string.Empty;
    [Column("Username")]
    public string Username
    {
        get => username;
        set { if (username != value) { username = value; OnPropertyChanged(); } }
    }

    private string passwordEncrypted = string.Empty;
    [Column("PasswordEncrypted")]
    public string PasswordEncrypted
    {
        get => passwordEncrypted;
        set { if (passwordEncrypted != value) { passwordEncrypted = value; OnPropertyChanged(); } }
    }

    private string? category;
    [Column("Category")]
    public string? Category
    {
        get => category;
        set { if (category != value) { category = value; OnPropertyChanged(); } }
    }

    private string? note;
    [Column("Note")]
    public string? Note
    {
        get => note;
        set { if (note != value) { note = value; OnPropertyChanged(); } }
    }

    private string? company;
    [Column("Company")]
    public string? Company
    {
        get => company;
        set { if (company != value) { company = value; OnPropertyChanged(); } }
    }

    private string? companyIcon;
    [Column("CompanyIcon")]
    public string? CompanyIcon
    {
        get => companyIcon;
        set { if (companyIcon != value) { companyIcon = value; OnPropertyChanged(); } }
    }

    private string? categoryIcon;
    [Column("CategoryIcon")]
    public string? CategoryIcon
    {
        get => categoryIcon;
        set { if (categoryIcon != value) { categoryIcon = value; OnPropertyChanged(); } }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}